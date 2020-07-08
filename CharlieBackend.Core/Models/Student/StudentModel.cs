using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.Models.Student
{
	public class StudentModel : BaseAccountModel
	{
		[JsonIgnore]
		public override string Password { get; set; }

		[JsonIgnore]
		public override int Role { get; set; }
		//public List<long> Groups_id { get; set; }

	}
}
