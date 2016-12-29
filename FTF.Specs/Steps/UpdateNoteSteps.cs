﻿using FTF.Api.Actions.Notes;
using FTF.Core.Notes.Update;
using TechTalk.SpecFlow;

namespace FTF.Specs.Steps
{
    [Binding]
    public class UpdateNoteSteps
    {
        private readonly Context _context;

        public UpdateNoteSteps(Context context)
        {
            _context = context;
        }

        [Given(@"I updated the note number (.*) with text '(.*)'")]
        public void UpdateNote(int id, string text)
        {
            Update update = new Handler(
                notes: _context.Db.Notes, 
                saveChanges: () => _context.Db.SaveChanges()
            ).Update;

            update(id, text);
        }
    }
}