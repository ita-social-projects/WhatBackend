using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
	public class UpdateStudentModel : StudentModel
	{
		[JsonIgnore]
		public override long Id { get; set; }
		public override string Password { get => base.Password; set => base.Password = value; }
	}
}
