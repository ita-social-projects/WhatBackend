namespace CharlieBackend.Core.Models.Visit
{
    public class VisitModel
    {
        public virtual long StudentId { get; set; }
        public virtual long StudentMark { get; set; }
        public virtual bool Presence { get; set; }
        public virtual string Comments { get; set; }

    }
}
