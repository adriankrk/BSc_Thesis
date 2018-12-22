namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Comments", "ServiceId", "dbo.Services");
            DropIndex("dbo.Services", new[] { "CategoryId" });
            DropIndex("dbo.Comments", new[] { "ServiceId" });
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
                        LokalizacjaId = c.Int(nullable: false),
                        Lokalizacja = c.String(),
                        IPAddress = c.String(),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            AddColumn("dbo.Comments", "Companies_Id", c => c.Int());
            AddForeignKey("dbo.Comments", "Companies_Id", "dbo.Company_Model", "Id");
            CreateIndex("dbo.Comments", "Companies_Id");
            DropTable("dbo.Services");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                        Adres = c.String(nullable: false),
                        Telefon = c.String(nullable: false),
                        StronaWWW = c.String(),
                        LokalizacjaId = c.Int(nullable: false),
                        Lokalizacja = c.String(),
                        IPAddress = c.String(),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Comments", new[] { "Companies_Id" });
            DropIndex("dbo.Company_Model", new[] { "CategoryId" });
            DropForeignKey("dbo.Comments", "Companies_Id", "dbo.Company_Model");
            DropForeignKey("dbo.Company_Model", "CategoryId", "dbo.Categories");
            DropColumn("dbo.Comments", "Companies_Id");
            DropTable("dbo.Company_Model");
            CreateIndex("dbo.Comments", "ServiceId");
            CreateIndex("dbo.Services", "CategoryId");
            AddForeignKey("dbo.Comments", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Services", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
