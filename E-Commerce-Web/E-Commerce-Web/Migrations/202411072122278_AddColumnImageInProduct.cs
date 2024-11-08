namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnImageInProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ImagePath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ImagePath");
        }
    }
}
