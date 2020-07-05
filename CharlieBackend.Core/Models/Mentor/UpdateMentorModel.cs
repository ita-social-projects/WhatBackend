using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Mentor
{
    public class UpdateMentorModel : MentorModel
    {
        [JsonIgnore]
        public override long Id { get; set; }
        public override string Password { get => base.Password; set => base.Password = value; }
    }
}
