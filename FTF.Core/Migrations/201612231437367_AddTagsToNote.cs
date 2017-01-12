using System.Data.Entity.Migrations;

namespace FTF.Core.Migrations
{
    public partial class AddTagsToNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notes", t => t.Note_Id)
                .Index(t => t.Note_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Note_Id", "dbo.Notes");
            DropIndex("dbo.Tags", new[] { "Note_Id" });
            DropTable("dbo.Tags");
        }
    }
}
