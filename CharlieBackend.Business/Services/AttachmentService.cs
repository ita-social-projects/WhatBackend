using System;
using System.IO;
using AutoMapper;
using CharlieBackend.Core;
using Azure.Storage.Blobs;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Linq;

namespace CharlieBackend.Business.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AzureStorageBlobAccount  _blobAccount;
        private readonly ILogger<AttachmentService> _logger;

        public AttachmentService( 
                             IUnitOfWork unitOfWork,
                             IMapper mapper,
                             AzureStorageBlobAccount blobAccount,
                             ILogger<AttachmentService> logger
                                )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobAccount = blobAccount;
            _logger = logger;
        }

        public async Task<Result<IList<AttachmentDto>>> AddAttachmentsAsync(
                    IFormFileCollection fileCollection,
                    ClaimsPrincipal claimsContext
                    )
        {
            try
            {
                IList<AttachmentDto> attachments = new List<AttachmentDto>();
                
                foreach (var file in fileCollection) 
                {

                    string containerName = Guid.NewGuid().ToString("N");

                    BlobContainerClient container = 
                                new BlobContainerClient(_blobAccount.connectionString, containerName);

                    await container.CreateIfNotExistsAsync();

                    BlobClient blob = container.GetBlobClient(file.FileName);


                    _logger.LogInformation("FileName: " + file.FileName);
                    _logger.LogInformation("Uri: " + blob.Uri);

                    using Stream uploadFileStream = file.OpenReadStream();

                    await blob.UploadAsync(uploadFileStream);

                    Attachment attachment = new Attachment()
                    {
                        UserId  = Convert.ToInt64(claimsContext.Claims
                                .First(claim => claim.Type == "Id").Value),
                        UserRole = (UserRole) Enum.Parse(typeof(UserRole), 
                                claimsContext.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value),
                        ContainerName = containerName,
                        FileName = file.FileName
                    };

                    _unitOfWork.AttachmentRepository.Add(attachment);

                    await _unitOfWork.CommitAsync();

                    attachments.Add(_mapper.Map<AttachmentDto>(attachment));
                }
                    
                return Result<IList<AttachmentDto>>.GetSuccess(attachments);
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<IList<AttachmentDto>>.GetError(ErrorCode.InternalServerError,
                     "Cannot add attachments");
            }          
        }

        public async Task<Result<IList<AttachmentDto>>> GetAttachmentsListAsync()
        {
            var attachments = _mapper.Map<IList<AttachmentDto>>(await _unitOfWork.AttachmentRepository.GetAllAsync());

            return Result<IList<AttachmentDto>>.GetSuccess(attachments);
        }

        public async Task<Result<DownloadAttachmentDto>> DownloadAttachmentAsync(long attachmentId)
        {
            var attachment = await _unitOfWork.AttachmentRepository.GetByIdAsync(attachmentId);

            if (attachment == null)
            {
                return Result<DownloadAttachmentDto>.GetError(ErrorCode.ValidationError,
                     "Attachement with id: " + attachmentId + " is not found");
            }

            BlobClient blob = new BlobClient
                        (
                        _blobAccount.connectionString,
                        attachment.ContainerName,
                        attachment.FileName
                        );

            BlobDownloadInfo download = await blob.DownloadAsync();

            DownloadAttachmentDto downloadedAttachment = new DownloadAttachmentDto()
                        { 
                           DownloadInfo = download,
                           FileName = attachment.FileName
                        };

            return Result<DownloadAttachmentDto>.GetSuccess(downloadedAttachment);
        }

        public async Task<Result<AttachmentDto>> DeleteAttachmentAsync(long attachmentId)
        {
            var attachment = await _unitOfWork.AttachmentRepository.GetByIdAsync(attachmentId);

            if (attachment == null)
            {
                return Result<AttachmentDto>.GetError(ErrorCode.ValidationError,
                     "Attachement with id: " + attachmentId + " is not found");
            }

            BlobContainerClient container = new BlobContainerClient
                        (
                        _blobAccount.connectionString, 
                        attachment.ContainerName
                        );

            await container.DeleteIfExistsAsync();

            await _unitOfWork.AttachmentRepository.DeleteAsync(attachmentId);

            await _unitOfWork.CommitAsync();

            return Result<AttachmentDto>.GetSuccess(_mapper.Map<AttachmentDto>(attachment));
        }
    }
}
