using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
	public class UpdateStudentModel : StudentModel
	{
		[JsonIgnore]
		public override long Id { get; set; }

		[Required]
		public override string Password { get => base.Password; set => base.Password = value; }

		[Required]
		[JsonPropertyName("first_name")]
		public override string FirstName { get; set; }

		[Required]
		[JsonPropertyName("last_name")]
		public override string LastName { get; set; }
	}
}
