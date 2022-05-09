using CharlieBackend.Panel.Models.EventColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IEventColorService
    {
        Task<IList<EventColorViewModel>> GetAllEventColorsAsync();
    }
}
