namespace AccommodationDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        LocalNumber = c.String(),
                        PostalCode = c.String(),
                        City = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OfferInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferStartTime = c.DateTime(nullable: false),
                        OfferEndTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        AvailableVacanciesNumber = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        OfferPublishTime = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaceId = c.Int(nullable: false),
                        OfferInfoId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.OfferInfoes", t => t.OfferInfoId, cascadeDelete: true)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.PlaceId)
                .Index(t => t.OfferInfoId)
                .Index(t => t.VendorId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        HashedPassword = c.String(),
                        Salt = c.String(),
                        UserDataId = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDatas", t => t.UserDataId, cascadeDelete: true)
                .Index(t => t.UserDataId);
            
            CreateTable(
                "dbo.UserDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CompanyName = c.String(),
                        Email = c.String(),
                        AdrressId = c.Int(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaceName = c.String(),
                        AddressId = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "VendorId", "dbo.Users");
            DropForeignKey("dbo.Offers", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Places", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Offers", "OfferInfoId", "dbo.OfferInfoes");
            DropForeignKey("dbo.Offers", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.UserDatas", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Places", new[] { "AddressId" });
            DropIndex("dbo.UserDatas", new[] { "Address_Id" });
            DropIndex("dbo.Users", new[] { "UserDataId" });
            DropIndex("dbo.Offers", new[] { "CustomerId" });
            DropIndex("dbo.Offers", new[] { "VendorId" });
            DropIndex("dbo.Offers", new[] { "OfferInfoId" });
            DropIndex("dbo.Offers", new[] { "PlaceId" });
            DropTable("dbo.Places");
            DropTable("dbo.UserDatas");
            DropTable("dbo.Users");
            DropTable("dbo.Offers");
            DropTable("dbo.OfferInfoes");
            DropTable("dbo.Addresses");
        }
    }
}
