using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Account
{
    public class AccountInfoModel : BaseAccountModel
    {
        public override long Id { get; set; }

        [JsonIgnore]
        public override string Email { get; set; }

        [JsonIgnore]
        public override string Password { get; set; }


    }
}
