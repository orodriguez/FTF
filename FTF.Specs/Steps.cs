using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace FTF.Specs
{
    [Binding]
    public class Steps
    {
        private Note _note;

        private Func<DateTime> _getCurrentDate;

        private Exception _error;

        private FtfDbContext _db;

        private DbContextTransaction _transation;

        [Given(@"today is '(.*)'")]
        public void TodayIs(string date) => _getCurrentDate = () => DateTime.Parse(date);

        [Given(@"I created a note \#(.*) with text '(.*)'")]
        public void CreateNote(int id, string text) =>
            new Notes(generateId: () => id, getCurrentDate: _getCurrentDate, db: _db).Create(id, text);

        [When(@"I retrieve the note \#(.*)")]
        public void RetrieveNote(int id)
        {
            try
            {
                _note = new Notes(generateId: () => id, getCurrentDate: _getCurrentDate, db: _db).Retrieve(id);
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        [Then(@"the note should match:")]
        public void NoteShouldMatch(Table table) => table.CompareToInstance(_note);

        [Then(@"it should show the error '(.*)'")]
        public void ShouldShowError(string message)
        {
            Assert.NotNull(_error, "No error was produced");
            Assert.AreEqual(message, _error.Message);
        }

        [BeforeScenario()]
        public void BeforeScenario()
        {
            _db = new FtfDbContext();
            _transation = _db.Database.BeginTransaction();
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            _transation.Rollback();
            _db.Dispose();
            _db = null;
        }
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

        private readonly FtfDbContext _db;

        public Notes(Func<int> generateId, Func<DateTime> getCurrentDate, FtfDbContext db)
        {
            _generateId = generateId;
            _getCurrentDate = getCurrentDate;
            _db = db;
        }

        public void Create(int id, string text)
        {
            _db.Notes.Add(new Note
            {
                Id = _generateId(),
                Text = text,
                CreationDate = _getCurrentDate()
            });

            _db.SaveChanges();
        }

        public Note Retrieve(int id)
        {
            var note = _db.Notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
                throw new Exception($"Note #{id} does not exist");

            return note;
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
