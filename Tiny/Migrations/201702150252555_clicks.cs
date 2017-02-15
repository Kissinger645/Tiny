namespace Tiny.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clicks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clicks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Clicks = c.Int(nullable: false),
                        LinkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Links", t => t.LinkId, cascadeDelete: true)
                .Index(t => t.LinkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clicks", "LinkId", "dbo.Links");
            DropIndex("dbo.Clicks", new[] { "LinkId" });
            DropTable("dbo.Clicks");
        }
    }
}
