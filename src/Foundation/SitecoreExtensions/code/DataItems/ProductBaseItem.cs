using CGP.Foundation.SitecoreExtensions.DataObjects;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CGP.Foundation.SitecoreExtensions.DataItems
{
    public class ProductBaseItem
    {
        public static ID TemplateId = ID.Parse("{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}");
        public static ID AttributesFieldId = ID.Parse("{CAAA6C5B-798C-4D26-8919-FE66D620F798}");
        public const string AttributesFieldName = "Page Attributes";
        private PageAttributes _attributes;
        private Item _currentItem;
        public ProductBaseItem(Item currentItem)
        {
            _currentItem = currentItem;
        }
        public PageAttributes Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    var attributes = new PageAttributes();
                    attributes.Init(_currentItem);
                    _attributes = attributes;
                }
                return _attributes;
            }
        }

        public MultilistField AttributeField
        {
            get { return this._currentItem.Fields[AttributesFieldName]; }
        }

    }
}