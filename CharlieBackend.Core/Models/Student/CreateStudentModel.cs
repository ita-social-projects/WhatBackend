using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
	public class CreateStudentModel:StudentModel
	{
		[JsonIgnore]
		public override long Id { get; set; }
	}
}
