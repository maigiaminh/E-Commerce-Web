namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnCityInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "City");
        }
    }
}
