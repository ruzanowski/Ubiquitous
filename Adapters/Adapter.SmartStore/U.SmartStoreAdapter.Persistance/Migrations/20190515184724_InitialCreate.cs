using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SmartStore.Persistance.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaStorage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Data = table.Column<byte[]>(maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PictureBinary = table.Column<byte[]>(nullable: true),
                    MimeType = table.Column<string>(maxLength: 40, nullable: false),
                    Width = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true),
                    SeoFilename = table.Column<string>(nullable: true),
                    IsNew = table.Column<bool>(nullable: false),
                    IsTransient = table.Column<bool>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: false),
                    MediaStorageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ProductTypeId = table.Column<int>(nullable: false),
                    ParentGroupedProductId = table.Column<int>(nullable: false),
                    VisibleIndividually = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 400, nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    FullDescription = table.Column<string>(nullable: true),
                    AdminComment = table.Column<string>(nullable: true),
                    ShowOnHomePage = table.Column<bool>(nullable: false),
                    HomePageDisplayOrder = table.Column<int>(nullable: false),
                    MetaKeywords = table.Column<string>(maxLength: 400, nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaTitle = table.Column<string>(maxLength: 400, nullable: true),
                    AllowCustomerReviews = table.Column<bool>(nullable: false),
                    ApprovedRatingSum = table.Column<int>(nullable: false),
                    NotApprovedRatingSum = table.Column<int>(nullable: false),
                    ApprovedTotalReviews = table.Column<int>(nullable: false),
                    NotApprovedTotalReviews = table.Column<int>(nullable: false),
                    SubjectToAcl = table.Column<bool>(nullable: false),
                    LimitedToStores = table.Column<bool>(nullable: false),
                    Sku = table.Column<string>(maxLength: 400, nullable: true),
                    ManufacturerPartNumber = table.Column<string>(maxLength: 400, nullable: true),
                    Gtin = table.Column<string>(nullable: true),
                    IsShipEnabled = table.Column<bool>(nullable: false),
                    IsFreeShipping = table.Column<bool>(nullable: false),
                    AdditionalShippingCharge = table.Column<decimal>(nullable: false),
                    IsTaxExempt = table.Column<bool>(nullable: false),
                    IsEsd = table.Column<bool>(nullable: false),
                    TaxCategoryId = table.Column<int>(nullable: false),
                    StockQuantity = table.Column<int>(nullable: false),
                    DisplayStockAvailability = table.Column<bool>(nullable: false),
                    DisplayStockQuantity = table.Column<bool>(nullable: false),
                    MinStockQuantity = table.Column<int>(nullable: false),
                    LowStockActivityId = table.Column<int>(nullable: false),
                    NotifyAdminForQuantityBelow = table.Column<int>(nullable: false),
                    OrderMinimumQuantity = table.Column<int>(nullable: false),
                    OrderMaximumQuantity = table.Column<int>(nullable: false),
                    QuantityStep = table.Column<int>(nullable: false),
                    AllowedQuantities = table.Column<string>(maxLength: 1000, nullable: true),
                    DisableBuyButton = table.Column<bool>(nullable: false),
                    DisableWishlistButton = table.Column<bool>(nullable: false),
                    AvailableForPreOrder = table.Column<bool>(nullable: false),
                    CallForPrice = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SpecialPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    SpecialPriceStartDateTimeUtc = table.Column<DateTime>(nullable: true),
                    SpecialPriceEndDateTimeUtc = table.Column<DateTime>(nullable: true),
                    CustomerEntersPrice = table.Column<bool>(nullable: false),
                    MinimumCustomerEnteredPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MaximumCustomerEnteredPrice = table.Column<decimal>(nullable: false),
                    HasTierPrices = table.Column<bool>(nullable: false),
                    HasDiscountsApplied = table.Column<bool>(nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AvailableStartDateTimeUtc = table.Column<DateTime>(nullable: true),
                    AvailableEndDateTimeUtc = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    SystemName = table.Column<string>(maxLength: 400, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: false),
                    BasePriceEnabled = table.Column<bool>(nullable: false),
                    BasePriceMeasureUnit = table.Column<string>(nullable: true),
                    BasePriceAmount = table.Column<decimal>(nullable: true),
                    BasePriceBaseAmount = table.Column<int>(nullable: true),
                    MainPictureId = table.Column<int>(nullable: true),
                    HasPreviewPicture = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 400, nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlRecord",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EntityId = table.Column<int>(nullable: false),
                    EntityName = table.Column<string>(maxLength: 400, nullable: false),
                    Slug = table.Column<string>(maxLength: 400, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 400, nullable: true),
                    FullName = table.Column<string>(maxLength: 400, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BottomDescription = table.Column<string>(maxLength: 400, nullable: true),
                    BadgeText = table.Column<string>(nullable: true),
                    BadgeStyle = table.Column<int>(nullable: false),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    CategoryTemplateId = table.Column<int>(nullable: false),
                    MetaKeywords = table.Column<string>(maxLength: 400, nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaTitle = table.Column<string>(maxLength: 400, nullable: true),
                    ParentCategoryId = table.Column<int>(nullable: false),
                    PictureId = table.Column<int>(nullable: true),
                    PageSize = table.Column<int>(nullable: true),
                    AllowCustomersToSelectPageSize = table.Column<bool>(nullable: true),
                    PageSizeOptions = table.Column<string>(maxLength: 200, nullable: true),
                    PriceRanges = table.Column<string>(maxLength: 400, nullable: true),
                    ShowOnHomePage = table.Column<bool>(nullable: false),
                    HasDiscountsApplied = table.Column<bool>(nullable: false),
                    SubjectToAcl = table.Column<bool>(nullable: false),
                    LimitedToStores = table.Column<bool>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: false),
                    DefaultViewMode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 400, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ManufacturerTemplateId = table.Column<int>(nullable: false),
                    MetaKeywords = table.Column<string>(maxLength: 400, nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaTitle = table.Column<string>(maxLength: 400, nullable: true),
                    PictureId = table.Column<int>(nullable: true),
                    PageSize = table.Column<int>(nullable: true),
                    AllowCustomersToSelectPageSize = table.Column<bool>(nullable: true),
                    PageSizeOptions = table.Column<string>(maxLength: 200, nullable: true),
                    PriceRanges = table.Column<string>(maxLength: 400, nullable: true),
                    LimitedToStores = table.Column<bool>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: false),
                    HasDiscountsApplied = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manufacturer_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product_Picture_Mapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ProductId = table.Column<int>(nullable: false),
                    PictureId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Picture_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Picture_Mapping_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Picture_Mapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Category_Mapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ProductId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    IsFeaturedProduct = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Category_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_Mapping_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Category_Mapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Manufacturer_Mapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ProductId = table.Column<int>(nullable: false),
                    ManufacturerId = table.Column<int>(nullable: false),
                    IsFeaturedProduct = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Manufacturer_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Manufacturer_Mapping_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Manufacturer_Mapping_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_PictureId",
                table: "Category",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturer_PictureId",
                table: "Manufacturer",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_Mapping_CategoryId",
                table: "Product_Category_Mapping",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_Mapping_ProductId",
                table: "Product_Category_Mapping",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Manufacturer_Mapping_ManufacturerId",
                table: "Product_Manufacturer_Mapping",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Manufacturer_Mapping_ProductId",
                table: "Product_Manufacturer_Mapping",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Picture_Mapping_PictureId",
                table: "Product_Picture_Mapping",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Picture_Mapping_ProductId",
                table: "Product_Picture_Mapping",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaStorage");

            migrationBuilder.DropTable(
                name: "Product_Category_Mapping");

            migrationBuilder.DropTable(
                name: "Product_Manufacturer_Mapping");

            migrationBuilder.DropTable(
                name: "Product_Picture_Mapping");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "TaxCategory");

            migrationBuilder.DropTable(
                name: "UrlRecord");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Picture");
        }
    }
}
