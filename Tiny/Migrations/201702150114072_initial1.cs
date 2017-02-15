namespace Tiny.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        ShortUrl = c.String(),
                        Created = c.DateTime(nullable: false),
                        Public = c.Boolean(nullable: false),
                        UserName = c.String(),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Links", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Links", new[] { "OwnerId" });
            DropTable("dbo.Links");
        }
    }
}
