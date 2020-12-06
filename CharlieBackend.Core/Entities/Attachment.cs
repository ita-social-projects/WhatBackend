using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class Attachment : BaseEntity
    {
        public string containerName { get; set; }

        public string fileName { get; set; }
    }
}
