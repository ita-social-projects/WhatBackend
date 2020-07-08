using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
	public class CreateStudentModel: StudentModel
	{
		[JsonIgnore]
		public override long Id { get; set; }

		[Required]
		public override string FirstName { get; set; }

		[Required]
		public override string LastName { get; set; }
	}
}
