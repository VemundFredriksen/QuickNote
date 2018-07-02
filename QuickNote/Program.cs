using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickNote
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Sets up directories and config files
            FileManager.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<Note> notes = LoadAllNotes();
            if(notes.Count < 1)
            {
                Application.Run(new NoteForm(new Note()));
            }
            else
            {
                foreach(Note n in notes)
                {
                    System.Threading.Thread thread;
                    thread = new System.Threading.Thread(InflateNote);
                    thread.SetApartmentState(System.Threading.ApartmentState.STA);
                    thread.Start(new NoteForm(n));
                }
            }
            
        }

        //Loads all notes stored in directory
        private static List<Note> LoadAllNotes()
        {
            string[] contentFiles = FileManager.GetContentFiles();

            List<Note> savedNotes = new List<Note>();
            foreach (string s in contentFiles)
            {
                savedNotes.Add(FileManager.LoadNote(s));
            }
            return savedNotes;
        }

        //Inflates the form. All forms run on dedicated thread
        private static void InflateNote(Object note)
        {
            Application.Run((Form)note);
        }

    }
}
