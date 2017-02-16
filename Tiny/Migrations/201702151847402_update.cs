namespace Tiny.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikerId = c.String(maxLength: 128),
                        LinkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.LikerId)
                .ForeignKey("dbo.Links", t => t.LinkId, cascadeDelete: true)
                .Index(t => t.LikerId)
                .Index(t => t.LinkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "LinkId", "dbo.Links");
            DropForeignKey("dbo.Likes", "LikerId", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "LinkId" });
            DropIndex("dbo.Likes", new[] { "LikerId" });
            DropTable("dbo.Likes");
        }
    }
}
