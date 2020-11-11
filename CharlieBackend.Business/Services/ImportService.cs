using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.File;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.FileModels;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class ImportService : IImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
    }
}
