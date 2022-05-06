using Sitecore.Data;
using System.Collections.Generic;

namespace CGP.Feature.Search.Models
{
    public class AutoSuggestResult
    {
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        public List<Results> results { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsSpellCheck.
        /// </summary>
        public bool IsSpellCheck { get; set; } = false;

        /// <summary>
        /// Gets or sets the Suggestion.
        /// </summary>
        public string Suggestion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSuggestResult"/> class.
        /// </summary>
        public AutoSuggestResult()
        {
            this.results = new List<Results>();
        }
    }

    public class Results
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public ID Id { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the MobileImageSrc.
        /// </summary>
        public string BannerImageSrc { get; set; }

        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
       // public string Category { get; set; }

        /// <summary>
        /// Gets or sets the Link.
        /// </summary>
        public string Link { get; set; }

    }

    public enum Category
    {
        /// <summary>
        /// Defines the Product.
        /// </summary>
        Product,

        /// <summary>
        /// Defines the Article.
        /// </summary>
        Article,

        /// <summary>
        /// Defines the Others.
        /// </summary>
        Others,
    }
}