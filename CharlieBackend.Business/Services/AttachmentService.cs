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
        private readonly IBlobService _blobService;

        public AttachmentService( 
                             IUnitOfWork unitOfWork,
                             IMapper mapper,
                             IBlobService blobService
                                )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
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

                    using Stream uploadFileStream = file.OpenReadStream();

                    var blob = await _blobService.UploadAsync(file.FileName, uploadFileStream);

                    Attachment attachment = new Attachment()
                    {
                        UserId  = Convert.ToInt64(claimsContext.Claims
                                .First(claim => claim.Type == "Id").Value),
                        UserRole = (UserRole) Enum.Parse(typeof(UserRole), 
                                claimsContext.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value),
                        ContainerName = blob.Data.BlobContainerName,
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

            var download = await _blobService.DownloadAsync(attachment.ContainerName, attachment.FileName);

            DownloadAttachmentDto downloadedAttachment = new DownloadAttachmentDto()
                        { 
                           DownloadInfo = download.Data,
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

            await _blobService.DeleteAsync(attachment.ContainerName);

            await _unitOfWork.AttachmentRepository.DeleteAsync(attachmentId);

            await _unitOfWork.CommitAsync();

            return Result<AttachmentDto>.GetSuccess(_mapper.Map<AttachmentDto>(attachment));
        }
    }
}
