namespace CharlieBackend.Core.Entities
{
    public partial class Attachment : BaseEntity
    {
        public string ContainerName { get; set; }

        public string FileName { get; set; }
    }
}
