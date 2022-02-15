﻿using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Panel.Models.StudentGroups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services.Interfaces
{
    public interface IStudentGroupService
    {
        Task<IList<StudentGroupViewModel>> GetAllStudentGroupsAsync(bool isAllGroups = true);

        Task<StudentGroupEditViewModel> PrepareStudentGroupUpdateAsync(long id);

        Task<StudentGroupEditViewModel> PrepareStudentGroupAddAsync();

        Task<StudentGroupDto> UpdateStudentGroupAsync(long id, StudentGroupDto updateDto);

        Task<CreateStudentGroupDto> AddStudentGroupAsync(long id, CreateStudentGroupDto addDto);
    }
}
