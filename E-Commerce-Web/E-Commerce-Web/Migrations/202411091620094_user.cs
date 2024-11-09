namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Province", c => c.String());
            AddColumn("dbo.Users", "District", c => c.String());
            DropColumn("dbo.Users", "City");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "City", c => c.String());
            DropColumn("dbo.Users", "District");
            DropColumn("dbo.Users", "Province");
        }
    }
}
