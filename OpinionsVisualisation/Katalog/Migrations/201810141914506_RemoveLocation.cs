namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLocation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Company_Model", "LokalizacjaId");
            DropColumn("dbo.Company_Model", "Lokalizacja");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Company_Model", "Lokalizacja", c => c.String());
            AddColumn("dbo.Company_Model", "LokalizacjaId", c => c.Int(nullable: false));
        }
    }
}
