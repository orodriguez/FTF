using System.Data.Entity.Migrations;

namespace FTF.Core.Migrations
{
    public partial class AddUserToTags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "User_Id", c => c.Int());
            CreateIndex("dbo.Tags", "User_Id");
            AddForeignKey("dbo.Tags", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "User_Id", "dbo.Users");
            DropIndex("dbo.Tags", new[] { "User_Id" });
            DropColumn("dbo.Tags", "User_Id");
        }
    }
}
