using System.Collections.Generic;

namespace CGP.Feature.Search.Models
{
    public class GlobalSearchGroup
    {
        public int GroupCount { get; set; }
        public string GroupIdentifier { get; set; }
        public List<GlobalResultItem> GroupItems { get; set; }
    }
}