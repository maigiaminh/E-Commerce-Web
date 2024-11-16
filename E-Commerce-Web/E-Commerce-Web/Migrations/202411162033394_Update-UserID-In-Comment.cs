namespace E_Commerce_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserIDInComment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "User_UserID", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "User_UserID" });
            DropColumn("dbo.Comments", "UserId");
            RenameColumn(table: "dbo.Comments", name: "User_UserID", newName: "UserId");
            AlterColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "UserId");
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserId" });
            AlterColumn("dbo.Comments", "UserId", c => c.Int());
            AlterColumn("dbo.Comments", "UserId", c => c.String());
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "User_UserID");
            AddColumn("dbo.Comments", "UserId", c => c.String());
            CreateIndex("dbo.Comments", "User_UserID");
            AddForeignKey("dbo.Comments", "User_UserID", "dbo.Users", "UserID");
        }
    }
}
