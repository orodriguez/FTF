namespace FTF.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreationDateToTagging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Taggings", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Taggings", "CreationDate");
        }
    }
}
