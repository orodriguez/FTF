namespace FTF.Storage.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutogenerateId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes");
            DropPrimaryKey("dbo.Notes");
            AlterColumn("dbo.Notes", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Notes", "Id");
            AddForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes");
            DropPrimaryKey("dbo.Notes");
            AlterColumn("dbo.Notes", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Notes", "Id");
            AddForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes", "Id");
        }
    }
}
