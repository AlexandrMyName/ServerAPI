using System.Collections.Generic;
using System;


namespace ServerService.Models
{
    public class GetingTreeItemsByTreeInfo
    {

        public Guid TreeKey { get; set; }
        public IReadOnlyCollection<Guid>? ExpandedTreeItemIds { get; set; }
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
    }
}
