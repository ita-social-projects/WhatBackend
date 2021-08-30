using CharlieBackend.Business.Helpers;
using Newtonsoft.Json;
using System;
using Xunit;

namespace CharlieBackend.Api.UnitTest
{
    public class JsonContractResolverTests
    {
        [Fact]
        public void Deserialize_BothFieldsArePassed_SerializationIsOk()
        {
            // Arrange
            var jsonDto = "{ \"requiredField\" : 5, \"nullableField\" : 8}";
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new JsonContractResolver() };

            // Act
            var result = JsonConvert.DeserializeObject<TestDto>(jsonDto, serializerSettings);

            // Assert
            Assert.Equal(5, result.RequiredField);
            Assert.Equal(8, result.NullableField);
        }

        [Fact]
        public void Deserialize_RequiredFieldAreNotPassed_SerializationIsFailed()
        {
            // Arrange
            var jsonDto = "{\"nullableField\" : 8}";
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new JsonContractResolver() };
            Action action = () => JsonConvert.DeserializeObject<TestDto>(jsonDto, serializerSettings);

            // Assert
            var exception = Assert.Throws<JsonSerializationException>(action);
            Assert.Equal("Required property 'requiredField' not found in JSON. Path '', line 1, position 21.", exception.Message);
        }

        [Fact]
        public void Deserialize_NullableFieldAreNotPassed_SerializationIsOk()
        {
            // Arrange
            var jsonDto = "{ \"requiredField\" : 5 }";
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new JsonContractResolver() };

            // Act
            var result = JsonConvert.DeserializeObject<TestDto>(jsonDto, serializerSettings);

            // Assert
            Assert.Equal(5, result.RequiredField);
            Assert.Null(result.NullableField);
        }

        [Fact]
        public void Serialize_FieldsInCamelCase()
        {
            // Arrange
            var dto = new TestDto() { NullableField = 8, RequiredField = 5 };
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new JsonContractResolver() };

            // Act
            var result = JsonConvert.SerializeObject(dto, serializerSettings);

            // Assert
            Assert.Equal("{\"requiredField\":5,\"nullableField\":8}", result);
        }

        [Fact]
        public void Deserialize_ReferenceFieldIsNotPassed_FieldIsNull()
        {
            // Arrange
            var jsonDto = "{}";
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new JsonContractResolver() };

            // Act
            var result = JsonConvert.DeserializeObject<TestReferenceDto>(jsonDto, serializerSettings);

            // Assert
            Assert.Null(result.Field);
        }

        [Fact]
        public void Deserialize_ReferenceFieldIsPassed_FieldIsSet()
        {
            // Arrange
            var jsonDto = "{ \"field\":\"someTest\"}";
            var serializerSettings = new JsonSerializerSettings() { ContractResolver = new JsonContractResolver() };

            // Act
            var result = JsonConvert.DeserializeObject<TestReferenceDto>(jsonDto, serializerSettings);

            // Assert
            Assert.Equal("someTest", result.Field);
        }

        private class TestDto
        {
            public int RequiredField { get; set; }
            public int? NullableField { get; set; }
        }

        private class TestReferenceDto
        {
            public string Field { get; set; }
        }
    }
}
