using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class Attachment : BaseEntity
    {
        public string ContainerName { get; set; }

        public string FileName { get; set; }
    }
}
