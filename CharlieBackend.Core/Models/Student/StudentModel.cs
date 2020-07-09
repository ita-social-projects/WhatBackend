using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
    public class StudentModel : BaseAccountModel
    {
        [JsonIgnore]
        public override string Password { get; set; }

        [JsonIgnore]
        public override int Role { get; set; }

        [JsonIgnore]
        public override bool IsActive { get; set; }
        //public List<long> Groups_id { get; set; }

    }
}
