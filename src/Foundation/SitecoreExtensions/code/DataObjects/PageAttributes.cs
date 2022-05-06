using CGP.Foundation.SitecoreExtensions.DataItems;
using Sitecore.Data.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.SitecoreExtensions.DataObjects
{
    public class PageAttributes : IEnumerable<PageAttribute>
    {
        private readonly List<PageAttribute> _data = new List<PageAttribute>();

        /// <summary>
        /// Loads Data 
        /// </summary>
        public void Init(Item page)
        {
            var pageitem = new ProductBaseItem(page);
            if (pageitem.AttributeField == null)
                return;

            var values = pageitem.AttributeField.GetItems();

            if (!values.Any())
                return;

            foreach (var value in values)
            {
                AddAttribute(value);
            }
        }

        private void AddAttribute(Item attributeValue)
        {
            var attribute = PageAttribute.FromAttributeValueItem(attributeValue);

            var existingAttribute = _data.FirstOrDefault(t => t.Id == attribute.Id);

            if (existingAttribute == null)
                _data.Add(attribute);
            else
            {
                existingAttribute.Merge(attribute);
            }
        }

        /// <summary>
        /// Only string representation for titles
        /// Does not include Ids  
        /// </summary>
        public virtual IDictionary<string, IEnumerable<string>> AttributeValues
        {
            get
            {
                return this.GroupBy(t => t.Title)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToArray().SelectMany(t => t.Values.Select(d => d.Title))
                    );
            }
        }

        #region IEnumerable

        public IEnumerator<PageAttribute> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}