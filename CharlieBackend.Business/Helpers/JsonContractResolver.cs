using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace CharlieBackend.Business.Helpers
{
    public class JsonContractResolver : DefaultContractResolver
    {
        public JsonContractResolver()
        {
            this.NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            JsonObjectContract contract = base.CreateObjectContract(objectType);

            foreach (var property in contract.Properties)
            {
                if (!IsNullableValueProperty(property))
                {
                    property.Required = Required.Always;
                }
            }

            return contract;
        }

        private static bool IsNullableValueProperty(JsonProperty property)
        {
            if (property.PropertyType.IsValueType)
            {
                return Nullable.GetUnderlyingType(property.PropertyType) != null;
            }

            return true;
        }
    }
}
