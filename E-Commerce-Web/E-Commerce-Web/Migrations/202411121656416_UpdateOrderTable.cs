namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "CouponID", newName: "Coupon_CouponID");
            RenameIndex(table: "dbo.Orders", name: "IX_CouponID", newName: "IX_Coupon_CouponID");
            AddColumn("dbo.OrderDetails", "SizeID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "ShippingFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "RecipientName", c => c.String());
            AddColumn("dbo.Orders", "RecipientPhone", c => c.String());
            AddColumn("dbo.Orders", "RecipientAddress", c => c.String());
            CreateIndex("dbo.OrderDetails", "SizeID");
            AddForeignKey("dbo.OrderDetails", "SizeID", "dbo.Sizes", "SizeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "SizeID", "dbo.Sizes");
            DropIndex("dbo.OrderDetails", new[] { "SizeID" });
            DropColumn("dbo.Orders", "RecipientAddress");
            DropColumn("dbo.Orders", "RecipientPhone");
            DropColumn("dbo.Orders", "RecipientName");
            DropColumn("dbo.Orders", "ShippingFee");
            DropColumn("dbo.Orders", "Discount");
            DropColumn("dbo.OrderDetails", "SizeID");
            RenameIndex(table: "dbo.Orders", name: "IX_Coupon_CouponID", newName: "IX_CouponID");
            RenameColumn(table: "dbo.Orders", name: "Coupon_CouponID", newName: "CouponID");
        }
    }
}
