using System.Collections.Generic;
using System.Linq;

namespace CGP.Feature.Search.Models
{
    public class InputParameters
    {
        public InputParameters()
        {
            Filters = new List<InputFilter>();
        }
        public string CurrentItemId { get; set; }
        public string SearchTerm { get; set; }
        public List<InputFilter> Filters { get; set; }
        public string FilterString { get; set; }
        public int SearchCount { get; set; }
        public int SkipCount { get; set; }
    }

    public class InputFilter
    {
        public string FilterName { get; set; }
        public string FilterKey { get; set; }
        public List<string> FilterValues { get; set; }
        public bool IsRangeFilter { get; set; }

        public int FilterValueAsInt()
        {
            var intValue = 0;
            var value = FilterValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out intValue);
            }

            return intValue;
        }
    }
}