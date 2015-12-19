namespace AccommodationDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingrank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Rank_Id", c => c.Int());
            CreateIndex("dbo.Users", "Rank_Id");
            AddForeignKey("dbo.Users", "Rank_Id", "dbo.Ranks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Rank_Id", "dbo.Ranks");
            DropIndex("dbo.Users", new[] { "Rank_Id" });
            DropColumn("dbo.Users", "Rank_Id");
            DropTable("dbo.Ranks");
        }
    }
}
