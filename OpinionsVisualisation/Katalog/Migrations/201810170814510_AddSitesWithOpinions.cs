namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSitesWithOpinions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Company_Model", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Comments", "Companies_Id", "dbo.Company_Model");
            DropIndex("dbo.Company_Model", new[] { "CategoryId" });
            DropIndex("dbo.Comments", new[] { "Companies_Id" });
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        WebPage = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        AbsolventWebPage = c.String(),
                        GoldenLineWebPage = c.String(),
                        GoworkWebPage = c.String(),
                        PostedDate = c.DateTime(nullable: false),
                        IPAddress = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            AddForeignKey("dbo.Comments", "Companies_Id", "dbo.Companies", "Id");
            CreateIndex("dbo.Comments", "Companies_Id");
            DropTable("dbo.Company_Model");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Company_Model",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                        Adres = c.String(nullable: false),
                        Telefon = c.String(nullable: false),
                        StronaWWW = c.String(),
                        IPAddress = c.String(),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Comments", new[] { "Companies_Id" });
            DropIndex("dbo.Companies", new[] { "CategoryId" });
            DropForeignKey("dbo.Comments", "Companies_Id", "dbo.Companies");
            DropForeignKey("dbo.Companies", "CategoryId", "dbo.Categories");
            DropTable("dbo.Companies");
            CreateIndex("dbo.Comments", "Companies_Id");
            CreateIndex("dbo.Company_Model", "CategoryId");
            AddForeignKey("dbo.Comments", "Companies_Id", "dbo.Company_Model", "Id");
            AddForeignKey("dbo.Company_Model", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
