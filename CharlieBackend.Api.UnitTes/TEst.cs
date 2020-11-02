using AutoMapper;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class TEst
    {
        [Fact]
        public void CheckDto()
        { 
        var configuration = new MapperConfiguration(cfg =>
  cfg.CreateMap<Lesson, CreateLessonDto>()
                .ForMember(destination => destination.LessonVisits, conf => conf.MapFrom(x => x.Visits.Select(x => x.Lesson.Visits))));

        configuration.AssertConfigurationIsValid();
        }
    }
}
