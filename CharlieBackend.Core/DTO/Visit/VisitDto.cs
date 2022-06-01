using System;

namespace CharlieBackend.Core.DTO.Visit
{
    public class VisitDto
    {
        #nullable enable

        public long StudentId { get; set; }

        public long? MarkId { get; set; }

        public bool Presence { get; set; }

        public sbyte? Mark { get; set; }

        public string? Comment { get; set; }

        #nullable disable

        public override bool Equals(Object obj)
        {
            if (obj is VisitDto)
            {
                var that = obj as VisitDto;
                return this.StudentId == that.StudentId &&
                    this.MarkId == that.MarkId &&
                    this.Presence == that.Presence && 
                    this.Mark == that.Mark &&
                    this.Comment == that.Comment;
            }

            return false;
        }
    }
}
