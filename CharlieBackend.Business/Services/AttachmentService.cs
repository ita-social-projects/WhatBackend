using System;
using System.IO;
using AutoMapper;
using CharlieBackend.Core;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Azure.Storage.Blobs.Models;
using Azure.Storage;

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

        public async Task<Result<IList<AttachmentDto>>> AddAttachmentsAsync(IFormFileCollection fileCollection)
        {
            try
            {
                IList<AttachmentDto> attachments = new List<AttachmentDto>();
                
                foreach (var file in fileCollection) 
                {
                    // Container names must start or end with a letter or number, 
                    // and can contain only letters, numbers, and the dash (-) character.
                    // All letters in a container name must be lowercase.
                    string containerName = "what-attachments-" + Guid.NewGuid().ToString();

                    BlobContainerClient cloudBlobContainerClient = 
                                new BlobContainerClient(_blobAccount.connectionString, containerName);

                    await cloudBlobContainerClient.CreateIfNotExistsAsync();

                    BlobClient blobClient = cloudBlobContainerClient.GetBlobClient(file.FileName);


                    _logger.LogInformation("FileName: " + file.FileName);
                    _logger.LogInformation("Uri: " + blobClient.Uri);

                    using Stream uploadFileStream = file.OpenReadStream();

                    await blobClient.UploadAsync(uploadFileStream);

                    Attachment attachment = new Attachment()
                    {
                        containerName = containerName,
                        fileName = file.FileName
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

            BlobClient blobClient = new BlobClient
                        (
                        _blobAccount.connectionString,
                        attachment.containerName,
                        attachment.fileName
                        );

            BlobDownloadInfo download = await blobClient.DownloadAsync();

            DownloadAttachmentDto downloadedAttachment = new DownloadAttachmentDto()
                        { 
                           downloadInfo = download,
                           fileName = attachment.fileName
                        };

            return Result<DownloadAttachmentDto>.GetSuccess(downloadedAttachment);
        }
    }
}
