namespace CharlieBackend.Core.DTO.Visit
{
    public class VisitDto
    {
        #nullable enable
        public long StudentId { get; set; }

        public sbyte? StudentMark { get; set; }

        public bool Presence { get; set; }

        public string? Comment { get; set; }

        #nullable disable
    }
}
