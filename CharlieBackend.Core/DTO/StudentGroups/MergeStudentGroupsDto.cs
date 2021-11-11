using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.StudentGroups
{
    public class MergeStudentGroupsDto
    {
        public long ResultingStudentGroupId { get; set; }

        public IList<long> IdsOfStudentGroupsToMerge { get; set; }
    }
}
