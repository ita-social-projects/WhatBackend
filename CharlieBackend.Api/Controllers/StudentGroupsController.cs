﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/student_groups")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {

        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupsController(IStudentGroupService studentGroupService)
        {
            _studentGroupService = studentGroupService;
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteStudentGroup(long id)
        //{
        //    var x = await _studentGroupService.SearchStudentGroup(id);
        //    if (x == null)
        //        return Ok("Not Found");
        //    else
        //    {
        //        _studentGroupService.DeleteStudentGrop(id);
        //        return Ok("Done");
        //    }
        //}

        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPost]
        public async Task<ActionResult<StudentGroupDto>> PostStudentGroup(CreateStudentGroupDto studentGroup)
        {
            
            var isStudentGroupNameExist = await _studentGroupService
                    .IsGroupNameExistAsync(studentGroup.Name);

            if (isStudentGroupNameExist.Data)
            {
                return Result<StudentGroupDto>.GetError(ErrorCode.UnprocessableEntity, "Group name already exists").ToActionResult();
            }

            var resStudentGroup = await _studentGroupService.CreateStudentGroupAsync(studentGroup);
          
            return resStudentGroup.ToActionResult();
        }

        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateStudentGroupDto>> PutStudentGroup(long id, UpdateStudentGroupDto studentGroupDto)
        {
            var isStudentGroupNameExist = await _studentGroupService
                   .IsGroupNameExistAsync(studentGroupDto.Name);

            if (isStudentGroupNameExist.Data)
            {
                return Result<StudentGroupDto>.GetError(ErrorCode.UnprocessableEntity, "Group name already exists").ToActionResult();
            }

            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentGroupAsync(id, studentGroupDto);

            return updatedStudentGroup.ToActionResult();
        }

        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPut("{id}/students")]
        public async Task<ActionResult<UpdateStudentsForStudentGroup>> PutStudentsOfStudentGroup(long id, UpdateStudentsForStudentGroup studentGroupDto) 
        {
            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentsForStudentGroupAsync(id, studentGroupDto);

            return updatedStudentGroup.ToActionResult();
        }

        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<StudentGroupDto>>> GetAllStudentGroups()
        {
            return Ok(await _studentGroupService.GetAllStudentGroupsAsync());
        }

        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroupDto>> GetStudentGroupById(long id)
        {
            var foundStudentGroup = await _studentGroupService
                    .GetStudentGroupByIdAsync(id);

            return foundStudentGroup.ToActionResult();
        }
    }
}
