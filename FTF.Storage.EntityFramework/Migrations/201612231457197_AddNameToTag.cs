namespace FTF.Storage.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "Name");
        }
    }
}
