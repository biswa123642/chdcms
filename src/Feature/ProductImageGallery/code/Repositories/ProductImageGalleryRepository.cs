using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using CGP.Feature.ProductImageGallery.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore.Data.Fields;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using FieldUtil = CGP.Foundation.SitecoreExtensions.Utilities.FieldUtil;
using ItemUtil = CGP.Foundation.SitecoreExtensions.Utilities.ItemUtil;

namespace CGP.Feature.ProductImageGallery.Repositories
{
    public class ProductImageGalleryRepository : VariantsRepository, IProductImageGalleryRepository
    {
        private readonly ILogger logger;
        public ProductImageGalleryRepository(ILogger logger)
        {
            this.logger = logger;
        }
        public ProductImageGalleryViewModel GetProductImageGalleryModel()
        {
            ProductImageGalleryViewModel productImageGalleryViewModel = new ProductImageGalleryViewModel {
                ProductImageGalleryModel = new ProductImageGalleryModel()
            };
            FillBaseProperties(productImageGalleryViewModel);

            try
            {
                List<Item> productVariant = new List<Item>();
                var variantGroupingItem = HelperExtension.GetChildItem(Sitecore.Context.Item, Template.Templates.VariantGroupingTemplateId);

                MultilistField selectedProductVariant = variantGroupingItem.Fields[Constants.ProductImageGallery.ChooseVariant];
                productImageGalleryViewModel.ProductImageGalleryModel.IsCarousel = FieldUtil.IsChecked(variantGroupingItem, Constants.ProductImageGallery.IsCarousel);

                bool isVideoFirst = FieldUtil.IsChecked(variantGroupingItem, Constants.ProductImageGallery.ViewVideosFirst);
                productImageGalleryViewModel.ProductImageGalleryModel.ProductVariantList = new List<ProductVariant>();

                var productVariantItemList = HelperExtension.GetProductVariants(variantGroupingItem);

                if (productVariantItemList.Count() > 0)
                {
                    foreach (Item variantItem in productVariantItemList)
                    {
                        if (!isVideoFirst)
                        {
                            productImageGalleryViewModel.ProductImageGalleryModel.ProductVariantList.Add(this.ProductVariantList(variantItem));
                        }
                        else
                        {
                            productImageGalleryViewModel.ProductImageGalleryModel.ProductVariantList.Add(this.ProductVariantVideoFirstList(variantItem));
                        }
                    }
                }
                productImageGalleryViewModel.JSONData = Newtonsoft.Json.JsonConvert.SerializeObject(productImageGalleryViewModel.ProductImageGalleryModel);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in ProductImageGalleryRepository.GetModel()", ex);
            }
            return productImageGalleryViewModel;
        }

        private ProductVariant ProductVariantList(Item variantItem)
        {
            ProductVariant productVariant = new ProductVariant
            {
                ProductMediaList = new List<ProductMedia>()
            };

            try
            {
                MultilistField mediaList = GetMediaList(variantItem, productVariant);

                foreach (Item mediaItem in mediaList.GetItems())
                {
                    ProductMedia productMedia = new ProductMedia();
                    if (mediaItem.TemplateID != Template.Templates.VideoTemplateId)
                    {
                        productMedia.MediaURL = ItemUtil.GetMediaUrl(mediaItem);
                        productMedia.MediaType = Constants.ProductImageGallery.Image;
                    }
                    else
                    {
                        var youtubeVideo = mediaItem[Constants.ProductImageGallery.YouTubeID];
                        if (!(string.IsNullOrEmpty(youtubeVideo)))
                        {
                            productMedia.YoutubeId = youtubeVideo;
                            productMedia.MediaType = Constants.ProductImageGallery.Youtube;
                        }
                        else
                        {
                            Sitecore.Data.Fields.LinkField linkField = mediaItem.Fields[Constants.ProductImageGallery.GalleryVideo];
                            productMedia.MediaURL = SitecoreUtil.GetVideoLink(linkField);
                            productMedia.MediaType = Constants.ProductImageGallery.Video;
                        }
                    }
                    productVariant.ProductMediaList.Add(productMedia);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in ProductImageGalleryRepository.ProductVariantList()", ex);
            }
            return productVariant;
        }

        private static MultilistField GetMediaList(Item variantItem, ProductVariant productVariant)
        {
            productVariant.VariantSKU = variantItem[Constants.ProductImageGallery.VariantSKU];
            MultilistField mediaList = variantItem.Fields[Constants.ProductImageGallery.MediaList];
            return mediaList;
        }

        private ProductVariant ProductVariantVideoFirstList(Item variantItem)
        {
            ProductVariant productVariant = new ProductVariant
            {
                ProductMediaList = new List<ProductMedia>()
            };

            List<ProductMedia> productImages = new List<ProductMedia>();
            List<ProductMedia> productVideos = new List<ProductMedia>();

            MultilistField mediaList = GetMediaList(variantItem, productVariant);
            foreach (Item mediaItems in mediaList.GetItems())
            {
                ProductMedia productMedia = new ProductMedia();

                if (mediaItems.TemplateID != Template.Templates.VideoTemplateId)
                {
                    productMedia.MediaURL = ItemUtil.GetMediaUrl(mediaItems);
                    productImages.Add(productMedia);
                    productMedia.MediaType = Constants.ProductImageGallery.Image;
                }
                else
                {
                    var video = mediaItems[Constants.ProductImageGallery.YouTubeID];
                    if (!(string.IsNullOrEmpty(video)))
                    {
                        productMedia.YoutubeId = video;
                        productMedia.MediaType = Constants.ProductImageGallery.Youtube;
                    }
                    else
                    {
                        Sitecore.Data.Fields.LinkField linkField = mediaItems.Fields[Constants.ProductImageGallery.GalleryVideo];
                        productMedia.MediaURL = SitecoreUtil.GetVideoLink(linkField);
                        productMedia.MediaType = Constants.ProductImageGallery.Video;
                    }
                    productVideos.Add(productMedia);
                }
            }
            productVariant.ProductMediaList.AddRange(productVideos);
            productVariant.ProductMediaList.AddRange(productImages);
            return productVariant;
        }
    }
}