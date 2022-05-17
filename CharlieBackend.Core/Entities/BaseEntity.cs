using System;

namespace CharlieBackend.Core.Entities
{
    [Serializable]
    public class BaseEntity : IBaseEntity
    {
        public long Id { get; set; }
    }
}
