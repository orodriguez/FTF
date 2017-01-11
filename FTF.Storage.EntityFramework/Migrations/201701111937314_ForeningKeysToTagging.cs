namespace FTF.Storage.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeningKeysToTagging : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.Taggings", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.Taggings", new[] { "Note_Id" });
            DropIndex("dbo.Taggings", new[] { "Tag_Id" });
            RenameColumn(table: "dbo.Taggings", name: "Note_Id", newName: "NoteId");
            RenameColumn(table: "dbo.Taggings", name: "Tag_Id", newName: "TagId");
            AlterColumn("dbo.Taggings", "NoteId", c => c.Int(nullable: false));
            AlterColumn("dbo.Taggings", "TagId", c => c.Int(nullable: false));
            CreateIndex("dbo.Taggings", "NoteId");
            CreateIndex("dbo.Taggings", "TagId");
            AddForeignKey("dbo.Taggings", "NoteId", "dbo.Notes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Taggings", "TagId", "dbo.Tags", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Taggings", "TagId", "dbo.Tags");
            DropForeignKey("dbo.Taggings", "NoteId", "dbo.Notes");
            DropIndex("dbo.Taggings", new[] { "TagId" });
            DropIndex("dbo.Taggings", new[] { "NoteId" });
            AlterColumn("dbo.Taggings", "TagId", c => c.Int());
            AlterColumn("dbo.Taggings", "NoteId", c => c.Int());
            RenameColumn(table: "dbo.Taggings", name: "TagId", newName: "Tag_Id");
            RenameColumn(table: "dbo.Taggings", name: "NoteId", newName: "Note_Id");
            CreateIndex("dbo.Taggings", "Tag_Id");
            CreateIndex("dbo.Taggings", "Note_Id");
            AddForeignKey("dbo.Taggings", "Tag_Id", "dbo.Tags", "Id");
            AddForeignKey("dbo.Taggings", "Note_Id", "dbo.Notes", "Id");
        }
    }
}
