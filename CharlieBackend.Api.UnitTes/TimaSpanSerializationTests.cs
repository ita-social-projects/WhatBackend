using CharlieBackend.Core.DTO.Schedule;
using Newtonsoft.Json;
using System;
using Xunit;

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
        public void Serialize_NetonsoftJson_TimeSpanIsSerializedWithExpectedFormat()
        {
            // Act
            var result = JsonConvert.SerializeObject(_dto);

            // Assert
            Assert.Equal(_expectedResult, result);
        }
    }
}
