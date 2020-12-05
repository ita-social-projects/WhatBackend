using System;
using System.IO;
using AutoMapper;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.DTO.Attachment;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl.Interfaces;


namespace CharlieBackend.Business.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AttachmentService> _logger;

        public AttachmentService( IUnitOfWork unitOfWork,
                             IMapper mapper,
                             BlobServiceClient blobServiceClient,
                             ILogger<AttachmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobServiceClient = blobServiceClient;
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

                    BlobContainerClient cloudBlobContainerClient = await _blobServiceClient.
                                CreateBlobContainerAsync(containerName);

                    BlobClient blobClient = cloudBlobContainerClient.GetBlobClient(file.FileName);


                    _logger.LogInformation("FileName: " + file.FileName);

                    _logger.LogInformation("Uri: " + blobClient.Uri);

                    using Stream uploadFileStream = file.OpenReadStream();

                    await blobClient.UploadAsync(uploadFileStream);

                    Attachment attachment = new Attachment()
                    {
                        Uri = blobClient.Uri.ToString()
                    };

                    _unitOfWork.AttachmentRepository.Add(attachment);

                    attachments.Add(_mapper.Map<AttachmentDto>(attachment));
                }
                    await _unitOfWork.CommitAsync();

                return Result<IList<AttachmentDto>>.GetSuccess(attachments);
            }
            catch
            {
                _unitOfWork.Rollback();

                return Result<IList<AttachmentDto>>.GetError(ErrorCode.InternalServerError,
                     "Cannot add attachments");
            }
            
        }
    }
}
