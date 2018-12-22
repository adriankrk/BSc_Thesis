namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "ExpirationDate");
            DropColumn("dbo.ServiceProviders", "Newsletter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceProviders", "Newsletter", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "ExpirationDate", c => c.DateTime(nullable: false));
        }
    }
}
