using System.IO;
using AutoMapper;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
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
        private readonly IBlobService _blobService;
        private readonly ICurrentUserService _currentUserService;

        public AttachmentService( 
                             IUnitOfWork unitOfWork,
                             IMapper mapper,
                             IBlobService blobService,
                             ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<IList<AttachmentDto>>> AddAttachmentsAsync(
                    IFormFileCollection fileCollection,
                    ClaimsPrincipal claimsContext
                    )
        {
            if (!AttachmentsSizeValidation(fileCollection))
            {
                return Result<IList<AttachmentDto>>.GetError(ErrorCode.ValidationError,
                            "Files are too big, max size is 50 MB");
            }

            if (!AttachmentsExtentionValidation(fileCollection))
            {
                return Result<IList<AttachmentDto>>.GetError(ErrorCode.ValidationError,
                           "Some of files have dengerous extention");
            }

            IList<Attachment> attachments = new List<Attachment>();

            foreach (var file in fileCollection)
            {

                using Stream uploadFileStream = file.OpenReadStream();

                var blob = await _blobService.UploadAsync(file.FileName, uploadFileStream);

                Attachment attachment = new Attachment()
                {
                    CreatedByAccountId = _currentUserService.AccountId,
                    ContainerName = blob.BlobContainerName,
                    FileName = file.FileName
                };

                _unitOfWork.AttachmentRepository.Add(attachment);

                attachments.Add(attachment);
            }

            await _unitOfWork.CommitAsync();

            return Result<IList<AttachmentDto>>.GetSuccess(_mapper.
                        Map<IList<AttachmentDto>>(attachments));
                
        }

        public async Task<Result<IList<AttachmentDto>>> GetAttachmentsListAsync(AttachmentRequestDto request)
        {
            string error = ValidateAttachmentRequest(request);

            if (error != null)
            {
                return Result<IList<AttachmentDto>>.GetError(ErrorCode.ValidationError, error);
            }

            long accountId = _currentUserService.AccountId;
            UserRole userRole = _currentUserService.Role;

            var result = new List<AttachmentDto>();

            if (userRole == UserRole.Student)
            {
                result = await _unitOfWork.AttachmentRepository
                    .GetAttachmentList(accountId, null, null, accountId, request.StartDate, request.FinishDate);
            }
            else
            {
                result = await _unitOfWork.AttachmentRepository
                    .GetAttachmentList(accountId, request.CourseID, request.GroupID, accountId, request.StartDate, request.FinishDate);
            }

            return Result<IList<AttachmentDto>>.GetSuccess(result);
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

            await _blobService.DeleteAsync(attachment.ContainerName);

            await _unitOfWork.AttachmentRepository.DeleteAsync(attachmentId);

            await _unitOfWork.CommitAsync();

            return Result<AttachmentDto>.GetSuccess(_mapper.Map<AttachmentDto>(attachment));
        }

        public bool AttachmentsExtentionValidation(IFormFileCollection fileCollection)
        {
            string[] dangerousExtentions = 
            {
                ".exe",".pif",".application",".gadget",".msi",".msp",".com",
                ".scr",".hta",".cpl",".msc",".jar",".bat",".cmd",".vb",".vbs",
                ".vbe",".js",".jse",".ws",".wsf",".wsc",".wsh",".ps1",".ps1xml",
                ".ps2",".ps2xml",".psc1",".psc2",".msh",".msh1",".msh2",".mshxml",
                ".msh1xml",".msh2xml",".scf",".lnk",".inf",".reg",".doc",".xls",
                ".ppt",".docm",".dotm",".xlsm",".xltm",".xlam",".pptm",".potm",
                ".ppam",".ppsm",".sldm",".dll"
            };

            foreach (var file in fileCollection)
            {
                foreach (var extention in dangerousExtentions)
                {
                    if (file.FileName.ToLower().EndsWith(extention))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool AttachmentsSizeValidation(IFormFileCollection fileCollection)
        {
            const int maxSize = 52428800;
            long currentSize = 0;

            foreach (var file in fileCollection)
            {
                currentSize += file.Length;
            }

            if (currentSize <= maxSize)
            {
                return true;
            }

            return false;
        }

        private string ValidateAttachmentRequest(AttachmentRequestDto request)
        {

            if (request == default)
            {
                return "RequestDto is null";
            }

            return null;
        }
    }
}
