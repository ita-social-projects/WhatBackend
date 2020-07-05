using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Mentor
{
    public class CreateMentorModel : MentorModel
    {
        [JsonIgnore]
        public override long Id { get; set; }
    }
}
