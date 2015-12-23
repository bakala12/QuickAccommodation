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
                "dbo.HistoricalOffers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferInfoId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        IsBooked = c.Boolean(nullable: false),
                        RoomId = c.Int(nullable: false),
                        OriginalOfferId = c.Int(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.OfferInfoes", t => t.OfferInfoId, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.OriginalOfferId)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.OfferInfoId)
                .Index(t => t.VendorId)
                .Index(t => t.CustomerId)
                .Index(t => t.RoomId)
                .Index(t => t.OriginalOfferId);
            
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
                        Rank_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ranks", t => t.Rank_Id)
                .ForeignKey("dbo.UserDatas", t => t.UserDataId, cascadeDelete: true)
                .Index(t => t.UserDataId)
                .Index(t => t.Rank_Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferInfoId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        CustomerId = c.Int(),
                        IsBooked = c.Boolean(nullable: false),
                        RoomId = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.OfferInfoes", t => t.OfferInfoId, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.OfferInfoId)
                .Index(t => t.VendorId)
                .Index(t => t.CustomerId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.OfferInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferStartTime = c.DateTime(nullable: false),
                        OfferEndTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        OfferPublishTime = c.DateTime(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Capacity = c.Int(nullable: false),
                        Number = c.String(),
                        PlaceId = c.Int(nullable: false),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .Index(t => t.PlaceId);
            
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
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistoricalOffers", "VendorId", "dbo.Users");
            DropForeignKey("dbo.HistoricalOffers", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.HistoricalOffers", "OriginalOfferId", "dbo.Offers");
            DropForeignKey("dbo.HistoricalOffers", "OfferInfoId", "dbo.OfferInfoes");
            DropForeignKey("dbo.HistoricalOffers", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserDataId", "dbo.UserDatas");
            DropForeignKey("dbo.UserDatas", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Users", "Rank_Id", "dbo.Ranks");
            DropForeignKey("dbo.Offers", "VendorId", "dbo.Users");
            DropForeignKey("dbo.Offers", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Places", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Offers", "OfferInfoId", "dbo.OfferInfoes");
            DropForeignKey("dbo.Offers", "CustomerId", "dbo.Users");
            DropIndex("dbo.UserDatas", new[] { "Address_Id" });
            DropIndex("dbo.Places", new[] { "AddressId" });
            DropIndex("dbo.Rooms", new[] { "PlaceId" });
            DropIndex("dbo.Offers", new[] { "RoomId" });
            DropIndex("dbo.Offers", new[] { "CustomerId" });
            DropIndex("dbo.Offers", new[] { "VendorId" });
            DropIndex("dbo.Offers", new[] { "OfferInfoId" });
            DropIndex("dbo.Users", new[] { "Rank_Id" });
            DropIndex("dbo.Users", new[] { "UserDataId" });
            DropIndex("dbo.HistoricalOffers", new[] { "OriginalOfferId" });
            DropIndex("dbo.HistoricalOffers", new[] { "RoomId" });
            DropIndex("dbo.HistoricalOffers", new[] { "CustomerId" });
            DropIndex("dbo.HistoricalOffers", new[] { "VendorId" });
            DropIndex("dbo.HistoricalOffers", new[] { "OfferInfoId" });
            DropTable("dbo.UserDatas");
            DropTable("dbo.Ranks");
            DropTable("dbo.Places");
            DropTable("dbo.Rooms");
            DropTable("dbo.OfferInfoes");
            DropTable("dbo.Offers");
            DropTable("dbo.Users");
            DropTable("dbo.HistoricalOffers");
            DropTable("dbo.Addresses");
        }
    }
}
