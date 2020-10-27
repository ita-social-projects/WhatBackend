using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Account
{
    public class AccountInfoModel : BaseAccountModel
    {
        [Required]
        public override long Id { get; set; }

        [Required]
        [JsonIgnore] //ask expert
        [EmailAddress]
        [StringLength(50)]
        public override string Email { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(65)]
        public override string Password { get; set; }


    }
}
