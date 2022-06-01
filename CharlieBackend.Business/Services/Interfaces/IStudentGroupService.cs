﻿using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.ResultModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.Interfaces
{
    public interface IStudentGroupService
    {
        Task<Result<StudentGroupDto>> CreateStudentGroupAsync(CreateStudentGroupDto studentGroupModel);

        Task<Result<IList<StudentGroupDto>>> GetAllStudentGroupsAsync(DateTime? startDate, DateTime? finishDate);

        Task<Result<bool>> IsGroupNameExistAsync(string groupName);

        Task<bool> DeleteStudentGroupAsync(long StudentGroupId);

        Task<Result<StudentGroupDto>> UpdateStudentGroupAsync(long id, UpdateStudentGroupDto studentGroupModel);

        Task<Result<StudentGroupDto>> GetStudentGroupByIdAsync(long id);

        Task<Result<IList<StudentGroupDto>>> GetStudentGroupsByDateAsync(DateTime? startDate, DateTime? finishDate);

        Task<Result<StudentGroupDto>> MergeStudentGroupsAsync(MergeStudentGroupsDto groupsToMerge);
    }
}
