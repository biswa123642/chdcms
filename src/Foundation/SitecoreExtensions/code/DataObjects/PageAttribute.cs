using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.SitecoreExtensions.DataObjects
{
    public class PageAttribute
    {
        public const string ValueNameFieldName = "Value";
        public const string AttributeNameFieldName = "Key";
        public static readonly ID ListItemTemplateId = ID.Parse("{62E96C2C-0949-4EEA-997A-D9408A414D7E}");

        private List<AttributeValue> _values = new List<AttributeValue>();

        [JsonConverter(typeof(string))]
        public ID Id { get; private set; }

        public string Title { get; private set; }

        public string Value { get; private set; }

        public IEnumerable<AttributeValue> Values { get { return this._values; } }

        public static PageAttribute FromAttributeItem(Item attributeItem)
        {
            if (attributeItem == null)
                throw new ArgumentNullException("attributeItem");

            var attr = new PageAttribute();
            attr.Title = attributeItem[AttributeNameFieldName];
            attr.Id = attributeItem.ID;
            attr.Value = attributeItem[ValueNameFieldName];

            if (attributeItem.HasChildren)
            {
                foreach (var child in attributeItem.Children.Where(x => x.TemplateID == ListItemTemplateId))
                {
                    attr._values.Add(new AttributeValue { Id = child.ID, Title = child[AttributeNameFieldName], Value = child[ValueNameFieldName] });
                }
            }

            return attr;
        }

        public static PageAttribute FromAttributeValueItem(Item attributeValueItem)
        {
            if (attributeValueItem == null)
                throw new ArgumentNullException("attributeValueItem");

            var attr = new PageAttribute();
            var value = attributeValueItem[ValueNameFieldName];
            var title = attributeValueItem[AttributeNameFieldName];

            attr._values.Add(new AttributeValue
            {
                Id = attributeValueItem.ID,
                Title = title,
                Value = value
            });

            if (attributeValueItem.Parent != null)
            {
                var attributeItem = attributeValueItem.Parent;
                attr.Title = attributeItem[AttributeNameFieldName];
                attr.Id = attributeItem.ID;
                attr.Value = attributeItem[ValueNameFieldName];
            }

            return attr;
        }

        /// <summary>
        /// Adds value of attribute to current object
        /// </summary>
        /// <param name="attribute"></param>
        public void Merge(PageAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            if (attribute.Id != Id)
                throw new ArgumentException("Cannot Merge Two different Attributes");

            foreach (var value in attribute.Values)
            {
                if (Values.Any(t => t.Id == value.Id) == false)
                {
                    this._values.Add(value);
                }
            }
        }
    }
}