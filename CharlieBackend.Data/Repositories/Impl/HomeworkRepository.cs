﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Data.Helpers;

namespace CharlieBackend.Data.Repositories.Impl
{
    public class HomeworkRepository : Repository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(ApplicationContext applicationContext)
        : base(applicationContext)
        {
        }

        public async Task<IList<Homework>> GetHomeworksByCourseId(long courseId)
        {
            return await (from task in _applicationContext.Homeworks
                          .Include(x => x.AttachmentsOfHomework)
                          from lessons in task.Theme.Lessons
                          where lessons.StudentGroup.CourseId == courseId
                          select task).ToListAsync();
        }

        public async override Task<Homework> GetByIdAsync(long homeworkId)
        {
            return await _applicationContext.Homeworks
                        .Include(x => x.AttachmentsOfHomework)
                        .FirstOrDefaultAsync(x => x.Id == homeworkId);
        }

        public void UpdateManyToMany(IEnumerable<AttachmentOfHomework> currentHomeworkAttachments,
                             IEnumerable<AttachmentOfHomework> newHomeworkAttachments)
        {
            _applicationContext.AttachmentsOfHomework.
                    TryUpdateManyToMany(currentHomeworkAttachments, newHomeworkAttachments);
        }

        public async Task<IList<Homework>> GetHomeworksByThemeId(long themeId)
        {

        }
    }
}
