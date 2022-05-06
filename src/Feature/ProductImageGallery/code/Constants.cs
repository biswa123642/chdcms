using Sitecore.Data;

namespace CGP.Feature.ProductImageGallery
{
    public class Template
    {
        public static class Templates
        {
            public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
            public static readonly ID VideoTemplateId = new ID("{1E9F56B7-FD80-4EAE-9FD4-8AB6242C6DB8}");
            public static readonly ID ImageId = new ID("{F1828A2C-7E5D-4BBD-98CA-320474871548}");
            public static readonly ID JpegId = new ID("{DAF085E8-602E-43A6-8299-038FF171349F}");

        }
    }

    public class Constants
    {
        public class ProductImageGallery
        {
            public static readonly string YouTubeID = "YouTubeID";
            public static readonly string GalleryVideo = "GalleryVideo";
            public static readonly string ViewVideosFirst = "ViewVideosFirst";
            public static readonly string IsCarousel = "IsCarousel";
            public static readonly string ChooseVariant = "ChooseVariant";
            public static readonly string MediaList = "MediaList";
            public static readonly string VariantSKU = "VariantSKU";
            public static readonly string Image = "image";
            public static readonly string Video = "video";
            public static readonly string Youtube = "youtube";
        }
    }
}