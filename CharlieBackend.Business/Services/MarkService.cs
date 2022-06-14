using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Mark;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class MarkService : IMarkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MarkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MarkDto>> GetMarkByIdAsync(long id)
        {
            var foundMark = await _unitOfWork.MarkRepository.GetByIdAsync(id);

            if (foundMark == null)
            {
                return Result<MarkDto>.GetError(ErrorCode.NotFound, "Mark is not found");
            }

            return Result<MarkDto>.GetSuccess(_mapper.Map<MarkDto>(foundMark));
        }
    }
}
