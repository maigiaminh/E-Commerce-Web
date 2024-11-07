namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveColumnInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "VerificationToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "VerificationToken");
            DropColumn("dbo.Users", "Active");
        }
    }
}
