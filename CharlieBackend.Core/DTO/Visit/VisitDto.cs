using System;

namespace CharlieBackend.Core.DTO.Visit
{
    public class VisitDto
    {
        #nullable enable

        public long StudentId { get; set; }

        public bool Presence { get; set; }

        public sbyte? StudentMark { get; set; }

        public string? Comment { get; set; }

        #nullable disable

        public override bool Equals(object obj)
        {
            if (obj is VisitDto)
            {
                var that = obj as VisitDto;
                return this.StudentId == that.StudentId &&
                    this.Presence == that.Presence && 
                    this.StudentMark == that.StudentMark && 
                    this.Comment == that.Comment;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StudentId, Presence, Comment, StudentMark);
        }
    }
}
