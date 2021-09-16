namespace CharlieBackend.Core.DTO.Student
{
    public class StudentDetailsDto : StudentDto
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarUrl { get; set; }

    }
}
