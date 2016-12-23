using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs
{
    [Binding]
    public class Steps
    {
        private Note _note;
        private Func<DateTime> _getCurrentDate;

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _getCurrentDate = () => DateTime.Parse(date);

        [Given(@"I created a note \#(.*) with text '(.*)'")]
        public void CreateNote(int id, string text) => 
            new Notes(generateId: () => id, getCurrentDate: _getCurrentDate).Create(id, text);

        [When(@"I retrieve the note \#(.*)")]
        public void RetrieveNote(int id) => 
            _note = Notes.RetrieveNote(id);

        [Then(@"the note should match:")]
        public void NoteShouldMatch(Table table) => table.CompareToInstance(_note);
    }

    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }
    }

    public class Notes
    {
        private readonly Func<int> _generateId;

        private readonly Func<DateTime> _getCurrentDate;

        public Notes(Func<int> generateId, Func<DateTime> getCurrentDate)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
        }

        public void Create(int id, string text)
        {
            using (var db = new FtfDbContext())
            {
                db.Notes.Add(new Note
                {
                    Id = _generateId(),
                    Text = text,
                    CreationDate = _getCurrentDate()
                });

                db.SaveChanges();
            }
        }

        public static Note RetrieveNote(int id)
        {
            using (var db = new FtfDbContext())
            {
                return db.Notes.First(note => note.Id == id);
            }
        }
    }

    public class FtfDbContext : DbContext
    {
        public FtfDbContext() : base("FTF.Tests")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<FtfDbContext>());
        }

        public IDbSet<Note> Notes { get; set; }
    }
}
