﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Student;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        #region
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;
        #endregion

        public StudentsController(IStudentService studentService, 
            IAccountService accountService)
        {
            _studentService = studentService;
            _accountService = accountService;
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<ActionResult> PostStudent(CreateStudentModel studentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var foundStudent = await _studentService
                    .GetStudentByEmailAsync(studentModel.Email);

            if (foundStudent != null)
            {
                return Ok(foundStudent.Id);
            }

            var isEmailTaken = await _accountService.IsEmailTakenAsync(studentModel.Email);

            if (isEmailTaken)
            {
                return StatusCode(409, "Account with this email already exists!");
            }

            var createdStudentModel = await _studentService
                    .CreateStudentAsync(studentModel);

            if (createdStudentModel == null)
            {
                return StatusCode(422, "Cannot create student.");
            }

            return Ok(new { createdStudentModel.Id });
        }

        [Authorize(Roles = "2")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<UpdateStudentModel>>> GetStudentById(long id)
        {
            try
            {
                var studentModel = await _studentService.GetStudentByIdAsync(id);

                if (studentModel != null)
                {
                    return Ok(new { 
                            first_name = studentModel.FirstName, 
                            last_name = studentModel.LastName, 
                            student_group_ids = studentModel.StudentGroupIds, 
                            email = studentModel.Email 
                    });
                }
                else
                {
                    return StatusCode(409, "Cannot find student with such id.");
                }
            }
            catch 
            { 
                return StatusCode(500); 
            }
        }

        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<List<StudentModel>>> GetAllStudents()
        {
            try
            {
                var studentsModels = await _studentService.GetAllStudentsAsync();

                return Ok(studentsModels);
            }
            catch
            { 
                return StatusCode(500); 
            }
        }

        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, UpdateStudentModel mentorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var isEmailChangableTo = await _accountService
                        .IsEmailChangableToAsync(mentorModel.Email);

                if (!isEmailChangableTo)
                {
                    return StatusCode(409, "Email is already taken!");
                }

                mentorModel.Id = id;

                var updatedCourse = await _studentService.UpdateStudentAsync(mentorModel);

                if (updatedCourse != null)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(409, "Cannot update.");
                }

            }
            catch 
            { 
                return StatusCode(500); 
            }
        }

        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DisableStudent(long id)
        {
            try
            {
                var accountId = await _studentService.GetAccountId(id);

                if (accountId == null)
                {
                    return BadRequest("Unknown student id.");
                }

                var isDisabled = await _accountService
                        .DisableAccountAsync((long)accountId);

                if (isDisabled)
                {
                    return NoContent();
                }

                return StatusCode(500, "Error occurred while trying to disable student account.");

            }
            catch 
            { 
                return StatusCode(400, "Bad token."); 
            }
        }
    }
}