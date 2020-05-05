using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
    /// <summary>
    /// Represents a product
    /// </summary>
    [DataContract]
    public class Product : BaseEntity
    {
        private ICollection<ProductCategory> _productCategories;
        private ICollection<ProductManufacturer> _productManufacturers;

        private int _stockQuantity;
        private string _sku;
        private string _manufacturerPartNumber;
        private decimal _price;
        private decimal _length;
        private decimal _width;
        private decimal _height;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

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
        /// Gets or sets the SKU
        /// </summary>
        [DataMember]
        public string Sku
        {
            [DebuggerStepThrough] get => _sku;
            set => _sku = value;
        }

        /// <summary>
        /// Gets or sets the manufacturer part number
        /// </summary>
        [DataMember]
        public string ManufacturerPartNumber
        {
            [DebuggerStepThrough] get => _manufacturerPartNumber;
            set => _manufacturerPartNumber = value;
        }

        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        [DataMember]
        public int StockQuantity
        {
            [DebuggerStepThrough] get => _stockQuantity;
            set => _stockQuantity = value;
        }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        [DataMember]
        public decimal Price
        {
            [DebuggerStepThrough] get => _price;
            set => _price = value;
        }

        /// <summary>
        /// Gets or sets the product cost
        /// </summary>
        [DataMember]
        public decimal ProductCost { get; set; }

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
            [DebuggerStepThrough] get => _length;
            set => _length = value;
        }

        /// <summary>
        /// Gets or sets the width
        /// </summary>
        [DataMember]
        public decimal Width
        {
            [DebuggerStepThrough] get => _width;
            set => _width = value;
        }

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        [DataMember]
        public decimal Height
        {
            [DebuggerStepThrough] get => _height;
            set => _height = value;
        }

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
        /// Measure unit for the base price (e.g. "kg", "g", "qm²" etc.)
        /// </summary>
        [DataMember]
        public string BasePriceMeasureUnit { get; set; }

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

    }
}