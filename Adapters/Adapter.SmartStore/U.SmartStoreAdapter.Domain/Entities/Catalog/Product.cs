using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using U.SmartStoreAdapter.Domain.Entities.Media;
using Newtonsoft.Json;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
    /// <summary>
    /// Represents a product
    /// </summary>
    [DataContract]
	public class Product : BaseEntity
    {
		#region static

		private static readonly HashSet<string> _visibilityAffectingProductProps = new HashSet<string>
		{
			nameof(AvailableEndDateTimeUtc),
			nameof(AvailableStartDateTimeUtc),
			nameof(Deleted),
			nameof(LimitedToStores),
			nameof(MinStockQuantity),
			nameof(Published),
			nameof(SubjectToAcl),
			nameof(VisibleIndividually)
		};

		public static IReadOnlyCollection<string> GetVisibilityAffectingPropertyNames()
		{
			return _visibilityAffectingProductProps;
		}

		#endregion

		private ICollection<ProductCategory> _productCategories;
        private ICollection<ProductManufacturer> _productManufacturers;
        private ICollection<ProductPicture> _productPictures;
        
        private int _stockQuantity;
		private string _sku;
		private string _gtin;
		private string _manufacturerPartNumber;
		private decimal _price;
		private decimal _length;
		private decimal _width;
		private decimal _height;
		private decimal? _basePriceAmount;
		private int? _basePriceBaseAmount;

		public bool MergedDataIgnore { get; set; }
		public Dictionary<string, object> MergedDataValues { get; set; }

		/// <summary>
		/// Gets or sets the product type identifier
		/// </summary>
		[DataMember]
		public int ProductTypeId { get; set; }

		/// <summary>
		/// Gets or sets the parent product identifier. It's used to identify associated products (only with "grouped" products)
		/// </summary>
		[DataMember]
		public int ParentGroupedProductId { get; set; }

		/// <summary>
		/// Gets or sets the values indicating whether this product is visible in catalog or search results.
		/// It's used when this product is associated to some "grouped" one
		/// This way associated products could be accessed/added/etc only from a grouped product details page
		/// </summary>
		[DataMember]
		public bool VisibleIndividually { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        [DataMember]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        [DataMember]
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
		[DataMember]
		public string AdminComment { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
		[DataMember]
		public bool ShowOnHomePage { get; set; }

		/// <summary>
		/// Gets or sets the display order for homepage products
		/// </summary>
		[DataMember]
		public int HomePageDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
		[DataMember]
		public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
		[DataMember]
		public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
		[DataMember]
		public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product allows customer reviews
        /// </summary>
		[DataMember]
		public bool AllowCustomerReviews { get; set; }

        /// <summary>
        /// Gets or sets the rating sum (approved reviews)
        /// </summary>
		[DataMember]
		public int ApprovedRatingSum { get; set; }

        /// <summary>
        /// Gets or sets the rating sum (not approved reviews)
        /// </summary>
		[DataMember]
		public int NotApprovedRatingSum { get; set; }

        /// <summary>
        /// Gets or sets the total rating votes (approved reviews)
        /// </summary>
		[DataMember]
		public int ApprovedTotalReviews { get; set; }

        /// <summary>
        /// Gets or sets the total rating votes (not approved reviews)
        /// </summary>
		[DataMember]
		public int NotApprovedTotalReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
		[DataMember]
		public bool SubjectToAcl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
		/// </summary>
		[DataMember]
		public bool LimitedToStores { get; set; }

		/// <summary>
		/// Gets or sets the SKU
		/// </summary>
		[DataMember]
		public string Sku
		{
			[DebuggerStepThrough]
			get => _sku;
			set => _sku = value;
		}

		/// <summary>
		/// Gets or sets the manufacturer part number
		/// </summary>
		[DataMember]
		public string ManufacturerPartNumber
		{
			[DebuggerStepThrough]
			get => _manufacturerPartNumber;
			set => _manufacturerPartNumber = value;
		}

		/// <summary>
		/// Gets or sets the Global Trade Item Number (GTIN). These identifiers include UPC (in North America), EAN (in Europe), JAN (in Japan), and ISBN (for books).
		/// </summary>
		[DataMember]
		public string Gtin
		{
			[DebuggerStepThrough]
			get => _gtin;
			set => _gtin = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the entity is ship enabled
		/// </summary>
		[DataMember]
		public bool IsShipEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is free shipping
		/// </summary>
		[DataMember]
		public bool IsFreeShipping { get; set; }

		/// <summary>
		/// Gets or sets the additional shipping charge
		/// </summary>
		[DataMember]
		public decimal AdditionalShippingCharge { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the product is marked as tax exempt
		/// </summary>
		[DataMember]
		public bool IsTaxExempt { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the product is an electronic service
		/// bound to EU VAT regulations for digital goods.
		/// </summary>
		[DataMember]
		public bool IsEsd { get; set; }

		/// <summary>
		/// Gets or sets the tax category identifier
		/// </summary>
		[DataMember]
		public int TaxCategoryId { get; set; }
		

		/// <summary>
		/// Gets or sets the stock quantity
		/// </summary>
		[DataMember]
		public int StockQuantity
		{
			[DebuggerStepThrough]
			get => _stockQuantity;
			set => _stockQuantity = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to display stock availability
		/// </summary>
		[DataMember]
		public bool DisplayStockAvailability { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to display stock quantity
		/// </summary>
		[DataMember]
		public bool DisplayStockQuantity { get; set; }

		/// <summary>
		/// Gets or sets the minimum stock quantity
		/// </summary>
		[DataMember]
		public int MinStockQuantity { get; set; }

		/// <summary>
		/// Gets or sets the low stock activity identifier
		/// </summary>
		[DataMember]
		public int LowStockActivityId { get; set; }

		/// <summary>
		/// Gets or sets the quantity when admin should be notified
		/// </summary>
		[DataMember]
		public int NotifyAdminForQuantityBelow { get; set; }

		/// <summary>
		/// Gets or sets the order minimum quantity
		/// </summary>
		[DataMember]
		public int OrderMinimumQuantity { get; set; }

		/// <summary>
		/// Gets or sets the order maximum quantity
		/// </summary>
		[DataMember]
		public int OrderMaximumQuantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity step
        /// </summary>
        [DataMember]
        public int QuantityStep { get; set; }

        /// <summary>
        /// Gets or sets the comma seperated list of allowed quantities. null or empty if any quantity is allowed
        /// </summary>
        [DataMember]
		public string AllowedQuantities { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to disable buy (Add to cart) button
		/// </summary>
		[DataMember]
		public bool DisableBuyButton { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to disable "Add to wishlist" button
		/// </summary>
		[DataMember]
		public bool DisableWishlistButton { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this item is available for Pre-Order
		/// </summary>
		[DataMember]
		public bool AvailableForPreOrder { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show "Call for Pricing" or "Call for quote" instead of price
		/// </summary>
		[DataMember]
		public bool CallForPrice { get; set; }

		/// <summary>
		/// Gets or sets the price
		/// </summary>
		[DataMember]
		public decimal Price
		{
			[DebuggerStepThrough]
			get => _price;
			set => _price = value;
		}

		/// <summary>
		/// Gets or sets the old price
		/// </summary>
		[DataMember]
		public decimal OldPrice { get; set; }

		/// <summary>
		/// Gets or sets the product cost
		/// </summary>
		[DataMember]
		public decimal ProductCost { get; set; }

		/// <summary>
		/// Gets or sets the product special price
		/// </summary>
		[DataMember]
		public decimal? SpecialPrice { get; set; }

		/// <summary>
		/// Gets or sets the start date and time of the special price
		/// </summary>
		[DataMember]
		public DateTime? SpecialPriceStartDateTimeUtc { get; set; }

		/// <summary>
		/// Gets or sets the end date and time of the special price
		/// </summary>
		[DataMember]
		public DateTime? SpecialPriceEndDateTimeUtc { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a customer enters price
		/// </summary>
		[DataMember]
		public bool CustomerEntersPrice { get; set; }

		/// <summary>
		/// Gets or sets the minimum price entered by a customer
		/// </summary>
		[DataMember]
		public decimal MinimumCustomerEnteredPrice { get; set; }

		/// <summary>
		/// Gets or sets the maximum price entered by a customer
		/// </summary>
		[DataMember]
		public decimal MaximumCustomerEnteredPrice { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this product has tier prices configured
		/// <remarks>The same as if we run this.TierPrices.Count > 0
		/// We use this property for performance optimization:
		/// if this property is set to false, then we do not need to load tier prices navigation property
		/// </remarks>
		/// </summary>
		[DataMember]
		public bool HasTierPrices { get; set; }
		
		
		/// <summary>
		/// Gets or sets a value indicating whether this product has discounts applied
		/// <remarks>The same as if we run this.AppliedDiscounts.Count > 0
		/// We use this property for performance optimization:
		/// if this property is set to false, then we do not need to load Applied Discounts navifation property
		/// </remarks>
		/// </summary>
		[DataMember]
		public bool HasDiscountsApplied { get; set; }

		/// <summary>
		/// Gets or sets the weight
		/// </summary>
		[DataMember]
		public decimal Weight { get; set; }

		/// <summary>
		/// Gets or sets the length
		/// </summary>
		[DataMember]
		public decimal Length
		{
			[DebuggerStepThrough]
			get => _length;
			set => _length = value;
		}

		/// <summary>
		/// Gets or sets the width
		/// </summary>
		[DataMember]
		public decimal Width
		{
			[DebuggerStepThrough]
			get => _width;
			set => _width = value;
		}

		/// <summary>
		/// Gets or sets the height
		/// </summary>
		[DataMember]
		public decimal Height
		{
			[DebuggerStepThrough]
			get => _height;
			set => _height = value;
		}

		/// <summary>
		/// Gets or sets the available start date and time
		/// </summary>
		[DataMember]
		public DateTime? AvailableStartDateTimeUtc { get; set; }

		/// <summary>
		/// Gets or sets the available end date and time
		/// </summary>
		[DataMember]
		public DateTime? AvailableEndDateTimeUtc { get; set; }

		/// <summary>
		/// Gets or sets a display order. This value is used when sorting associated products (used with "grouped" products)
		/// </summary>
		[DataMember]
		public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
		[DataMember]
		public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
		/// Gets or sets the product system name.
		/// </summary>
		[DataMember]
		public string SystemName { get; set; }

		/// <summary>
		/// Gets or sets the date and time of product creation
		/// </summary>
		[DataMember]
		public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of product update
        /// </summary>
		[DataMember]
		public DateTime UpdatedOnUtc { get; set; }


        /// <summary>
		/// Gets or sets if base price quotation (PAnGV) is enabled
		/// </summary>
		[DataMember]
		public bool BasePriceEnabled { get; set; }

		/// <summary>
		/// Measure unit for the base price (e.g. "kg", "g", "qm²" etc.)
		/// </summary>
		[DataMember]
		public string BasePriceMeasureUnit { get; set; }

		/// <summary>
		/// Amount of product per packing unit in the given measure unit 
        /// (e.g. 250 ml shower gel: "0.25" if MeasureUnit = "liter" and BaseAmount = 1)
		/// </summary>
		[DataMember]
		public decimal? BasePriceAmount
		{
			[DebuggerStepThrough]
			get => _basePriceAmount;
			set => _basePriceAmount = value;
		}

		/// <summary>
		/// Reference value for the given measure unit 
		/// (e.g. "1" liter. Formula: [BaseAmount] [MeasureUnit] = [SellingPrice] / [Amount])
		/// </summary>
		[DataMember]
		public int? BasePriceBaseAmount
		{
			[DebuggerStepThrough]
			get => _basePriceBaseAmount;
			set => _basePriceBaseAmount = value;
		}

		[DataMember]
		public bool BasePriceHasValue => BasePriceEnabled && BasePriceAmount.GetValueOrDefault() > 0 && BasePriceBaseAmount.GetValueOrDefault() > 0 && BasePriceMeasureUnit!=null;

		/// <summary>
		/// Gets or sets the main picture id
		/// </summary>
		[DataMember]
		public int? MainPictureId { get; set; }

        /// <summary>
		/// Gets or sets a value that indictaes whether the product has a preview picture
		/// </summary>
		[DataMember]
        public bool HasPreviewPicture { get; set; }

        /// <summary>
        /// Gets or sets the product type
        /// </summary>
        [DataMember]
		public ProductType ProductType
		{
			get => (ProductType)ProductTypeId;
			set => ProductTypeId = (int)value;
        }

		public string ProductTypeLabelHint
		{
			get
			{
				switch (ProductType)
				{
					case ProductType.SimpleProduct:
						return "secondary d-none";
					case ProductType.GroupedProduct:
						return "success";
					case ProductType.BundledProduct:
						return "info";
					default:
						return "";
				}
			}
		}


		/// <summary>
        /// Gets or sets the collection of ProductCategory
        /// </summary>
        [DataMember]
        public virtual ICollection<ProductCategory> ProductCategories
        {
			get => _productCategories ?? (_productCategories = new HashSet<ProductCategory>());
			protected set => _productCategories = value;
		}

        /// <summary>
        /// Gets or sets the collection of ProductManufacturer
        /// </summary>
		[DataMember]
		public virtual ICollection<ProductManufacturer> ProductManufacturers
        {
			get => _productManufacturers ?? (_productManufacturers = new HashSet<ProductManufacturer>());
			protected set => _productManufacturers = value;
        }

        /// <summary>
        /// Gets or sets the collection of ProductPicture
        /// </summary>
		[DataMember]
		public virtual ICollection<ProductPicture> ProductPictures
        {
            get => _productPictures ?? (_productPictures = new HashSet<ProductPicture>());
            protected set => _productPictures = value;
        }
    }
}
