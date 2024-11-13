namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubtotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Subtotal");
        }
    }
}
