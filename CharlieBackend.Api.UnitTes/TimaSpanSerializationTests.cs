using CharlieBackend.Core.DTO.Schedule;
//using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CharlieBackend.Core;

namespace CharlieBackend.Api.UnitTest
{
    public class TimaSpanSerializationTests
    {
        private readonly UpdateScheduleDto _dto;
        private readonly string _expectedResult;

        public TimaSpanSerializationTests()
        {
            _dto = new UpdateScheduleDto() { LessonStart = new TimeSpan(12, 11, 10, 9) };
            _expectedResult = "{\"LessonStart\":\"12.11:10:09\",\"LessonEnd\":\"00:00:00\",\"RepeatRate\":0,\"DayNumber\":null}";
        }

        [Fact]
        public void Serialize_SystemTextJson_TimeSpanIsSerializedWithExpectedFormat()
        {
            // Arrange
            var serializeOptions = new JsonSerializerOptions() { Converters = { new TimeSpanConverter() } }; 
            
            // Act
            var result = JsonSerializer.Serialize(_dto , serializeOptions);

            // Assert
            Assert.Equal(_expectedResult, result);
        }

        [Fact]
        public void Serialize_NetonsoftJson_TimeSpanIsSerializedWithExpectedFormat()
        {
            // Act
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(_dto);

            // Assert
            Assert.Equal(_expectedResult, result);
        }
    }
}
