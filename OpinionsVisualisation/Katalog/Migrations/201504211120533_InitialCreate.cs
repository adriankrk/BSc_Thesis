namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        IPAddress = c.String(),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Content = c.String(nullable: false, maxLength: 200),
                        IPAddress = c.String(),
                        UserId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        CommentCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.CommentCategories", t => t.CommentCategoryId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.CommentCategoryId);
            
            CreateTable(
                "dbo.CommentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        Email = c.String(),
                        City = c.String(nullable: false, maxLength: 40),
                        ZipCode = c.String(nullable: false),
                        Street = c.String(nullable: false, maxLength: 40),
                        IsConfirmed = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Newsletter = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceProviders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(nullable: false),
                        City = c.String(nullable: false, maxLength: 40),
                        ZipCode = c.String(nullable: false),
                        Street = c.String(nullable: false, maxLength: 40),
                        PhoneNumber = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Newsletter = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "CommentCategoryId" });
            DropIndex("dbo.Comments", new[] { "ServiceId" });
            DropIndex("dbo.Services", new[] { "CategoryId" });
            DropForeignKey("dbo.Comments", "CommentCategoryId", "dbo.CommentCategories");
            DropForeignKey("dbo.Comments", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Services", "CategoryId", "dbo.Categories");
            DropTable("dbo.ServiceProviders");
            DropTable("dbo.Customers");
            DropTable("dbo.CommentCategories");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Services");
        }
    }
}
