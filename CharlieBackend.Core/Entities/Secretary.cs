using CharlieBackend.Core.Interfaces;

namespace CharlieBackend.Core.Entities
{
    public class Secretary : BaseEntity, ICloneable<Secretary>
    {
        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }

        public Secretary Clone()
        {
            Account account = null;
            if (Account != null)
                account = this.Account.Clone();

            return new Secretary()
            {
                AccountId = this.AccountId,
                Id = this.Id,
                Account = account
            };
        }
    }
}
