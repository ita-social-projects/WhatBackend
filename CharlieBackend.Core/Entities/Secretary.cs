namespace CharlieBackend.Core.Entities
{
    public class Secretary : BaseEntity
    {
        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
