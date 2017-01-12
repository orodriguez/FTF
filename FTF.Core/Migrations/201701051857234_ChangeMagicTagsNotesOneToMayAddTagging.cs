using System.Data.Entity.Migrations;

namespace FTF.Core.Migrations
{
    public partial class ChangeMagicTagsNotesOneToMayAddTagging : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagNotes", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagNotes", "Note_Id", "dbo.Notes");
            DropIndex("dbo.TagNotes", new[] { "Tag_Id" });
            DropIndex("dbo.TagNotes", new[] { "Note_Id" });
            CreateTable(
                "dbo.Taggings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note_Id = c.Int(),
                        Tag_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notes", t => t.Note_Id)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .Index(t => t.Note_Id)
                .Index(t => t.Tag_Id);
            
            DropTable("dbo.TagNotes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagNotes",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Note_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Note_Id });
            
            DropForeignKey("dbo.Taggings", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes");
            DropIndex("dbo.Taggings", new[] { "Tag_Id" });
            DropIndex("dbo.Taggings", new[] { "Note_Id" });
            DropTable("dbo.Taggings");
            CreateIndex("dbo.TagNotes", "Note_Id");
            CreateIndex("dbo.TagNotes", "Tag_Id");
            AddForeignKey("dbo.TagNotes", "Note_Id", "dbo.Notes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagNotes", "Tag_Id", "dbo.Tags", "Id", cascadeDelete: true);
        }
    }
}
