using System.Data.Entity.Migrations;

namespace FTF.Core.Migrations
{
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
