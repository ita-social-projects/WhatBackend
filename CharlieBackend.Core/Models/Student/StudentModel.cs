using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
	public class StudentModel : BaseAccountModel
	{
		[JsonIgnore]
		public override string Password { get; set; }

		public override string Email { get; set; }

		[JsonIgnore]
		public override int Role { get; set; }
		public long groups_id { get; set; }

	}
}
