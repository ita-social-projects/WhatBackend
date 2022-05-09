using AutoMapper;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services
{
    public class EventColorService : IEventColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventColorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IList<EventColorDTO>>> GetAllEventColorsAsync()
        {
            var colors = await _unitOfWork.EventColorRepository.GetAllAsync();

            return Result<IList<EventColorDTO>>
                .GetSuccess(_mapper.Map<IList<EventColorDTO>>(colors));
        }

        public async Task<Result<EventColorDTO>> GetEventColorByIdAsync(long id)
        {
            var color = await _unitOfWork.EventColorRepository.GetByIdAsync(id);

            return Result<EventColorDTO>.GetSuccess(_mapper.Map<EventColorDTO>(color));
        }
    }
}
