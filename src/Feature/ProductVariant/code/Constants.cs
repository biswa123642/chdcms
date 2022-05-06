using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.ProductVariant
{
    public static class Template
    {
        public static class Templates
        {
            public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
            public static readonly ID ProductVariantTemplateId = new ID("{6659B02D-BD4B-4821-B452-77FCA1DA6B43}");
        }
    }
    public static class Constants
    {
        public static class ProductVariant
        {
            public static readonly string ChooseVariant = "ChooseVariant";

            public static readonly ID PrimaryVariationTitleId = new ID("{AD4BC59B-2B83-45B1-A2CE-5A5BEF26F459}");
            public static readonly ID SecondaryVariationTitleId = new ID("{2E934EE7-64DD-4B50-90DC-013F157EA150}");
            public static readonly ID TertiaryVariationTitleId = new ID("{C973C857-C930-4F40-993D-398BD49252E7}");

            public static readonly ID PrimaryVariationValueId = new ID("{22CAFC35-3F72-476D-AAED-34FBF9F40DF6}");
            public static readonly ID SecondaryVariationValueId = new ID("{6951D151-0A70-4A96-9AE7-9AD5E8881216}");
            public static readonly ID TertiaryVariationValueId = new ID("{9303C8ED-CB7A-4D6F-9BCB-05E0AF55FE4B}");
            public static readonly ID VariantSKU = new ID("{915344F6-4F20-4302-9772-BEF16CC60BE7}");
            public static readonly ID VariantDescription = new ID("{17CDAD4B-B760-4682-B72D-FDA00DD2E27D}");
        }
    }
}