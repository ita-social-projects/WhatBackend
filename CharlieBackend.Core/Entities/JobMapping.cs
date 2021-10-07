using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public partial class JobMapping : BaseEntity
    {
        /// <summary>
        /// This property contains understandable to human ID of the job
        /// </summary>
        /// <example> 
        /// Imagine there is homework with id 7.
        /// This way, CustomJobID will be "homework7"
        /// </example>
        public string CustomJobID { get; set; }

        /// <summary>
        /// This property contains ID of the job at hangfire server
        /// </summary>
        public string HangfireJobID { get; set; }
    }
}
