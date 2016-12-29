namespace FTF.Storage.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "User_Id", c => c.Int());
            AddColumn("dbo.Users", "Name", c => c.String());
            CreateIndex("dbo.Notes", "User_Id");
            AddForeignKey("dbo.Notes", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Users", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Username", c => c.String());
            DropForeignKey("dbo.Notes", "User_Id", "dbo.Users");
            DropIndex("dbo.Notes", new[] { "User_Id" });
            DropColumn("dbo.Users", "Name");
            DropColumn("dbo.Notes", "User_Id");
        }
    }
}
