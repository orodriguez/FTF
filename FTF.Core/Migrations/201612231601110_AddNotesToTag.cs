using System.Data.Entity.Migrations;

namespace FTF.Core.Migrations
{
    public partial class AddNotesToTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Note_Id", "dbo.Notes");
            DropIndex("dbo.Tags", new[] { "Note_Id" });
            CreateTable(
                "dbo.TagNotes",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Note_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Note_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Notes", t => t.Note_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Note_Id);
            
            DropColumn("dbo.Tags", "Note_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Note_Id", c => c.Int());
            DropForeignKey("dbo.TagNotes", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.TagNotes", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagNotes", new[] { "Note_Id" });
            DropIndex("dbo.TagNotes", new[] { "Tag_Id" });
            DropTable("dbo.TagNotes");
            CreateIndex("dbo.Tags", "Note_Id");
            AddForeignKey("dbo.Tags", "Note_Id", "dbo.Notes", "Id");
        }
    }
}
