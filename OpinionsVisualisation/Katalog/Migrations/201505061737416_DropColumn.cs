namespace Katalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "Newsletter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Newsletter", c => c.Boolean(nullable: false));
        }
    }
}
