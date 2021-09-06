using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace CharlieBackend.Core.Extensions
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

            foreach (var contractProperty in contract.Properties)
            {
                if (!IsOfNullableType(contractProperty))
                {
                    contractProperty.Required = Required.Always;
                }
            }

            return contract;
        }

        private static bool IsOfNullableType(JsonProperty property)
        {
            if (property.PropertyType.IsValueType)
            {
                return Nullable.GetUnderlyingType(property.PropertyType) != null;
            }

            return true;
        }
    }
}
