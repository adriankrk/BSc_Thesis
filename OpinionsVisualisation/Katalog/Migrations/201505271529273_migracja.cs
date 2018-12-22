namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Adres", c => c.String(nullable: false));
            AddColumn("dbo.Services", "Telefon", c => c.String(nullable: false));
            AddColumn("dbo.Services", "StronaWWW", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "StronaWWW");
            DropColumn("dbo.Services", "Telefon");
            DropColumn("dbo.Services", "Adres");
        }
    }
}
