using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class MergeStudentGroupsDto
    {
        public long ResultingStudentGroupId { get; set; }

        public IList<long> IdsOfStudentGroupsToMerge { get; set; }
    }
}
