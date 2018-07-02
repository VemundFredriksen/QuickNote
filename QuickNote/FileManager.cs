using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QuickNote
{
    static class FileManager
    {
        //Directory for storing note-files and config-files
        static string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\QuickNote";

        public static void Init()
        {
            InitializeDirectory();
            InitializeConfig();
        }

        //Validates and sets up directory
        private static void InitializeDirectory()
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        //Validates and sets up config files
        private static void InitializeConfig()
        {
            string configPath = directory + "\\config.txt";
            if (!File.Exists(configPath))
                File.Create(configPath);
        }

        public static string[] GetContentFiles()
        {
            return Directory.GetFiles(directory, "note*");
        }

        //Generates a new random ID
        public static int GenerateNoteID()
        {
            int generated = (new Random()).Next(Int16.MaxValue);
            while(File.Exists(directory + "\\note" + generated + ".txt"))
            {
                generated = (new Random()).Next(Int16.MaxValue);
            }

            return generated;
        }

        //Reads content in file specified
        public static Note LoadNote(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            try
            {
                string[] configs = File.ReadAllLines(path)[0].Split(';');
                string content = File.ReadAllText(path);
                content = content.Substring(content.IndexOf("\n") + 1);
                Note note = new Note(int.Parse(configs[0]), 
                    new System.Drawing.Point(int.Parse(configs[1].Split(',')[0]), int.Parse(configs[1].Split(',')[1])),
                    System.Drawing.ColorTranslator.FromHtml(configs[2]), 
                    new System.Drawing.Size(int.Parse(configs[3].Split(',')[0]), int.Parse(configs[3].Split(',')[1])),
                    content);
                return note;
            }
            catch(Exception)
            {
                throw new Exception("Note was on wrong format, check or delete note");
            }
        }

        public static void SaveNote(Note note)
        {
            if (String.IsNullOrEmpty(note.Content))
            {
                DeleteNote(note);
            }
            else
            {
                string content = "" + note.Id + ";" + note.Location.X + "," + note.Location.Y + ";" +
                    System.Drawing.ColorTranslator.ToHtml(note.Color) + ";" + note.Size.Width + "," +
                    note.Size.Height + ";" + Environment.NewLine + note.Content;

                File.WriteAllText(directory + "\\note" + note.Id + ".txt", content);
            }
        }

        public static void DeleteNote(Note note)
        {
            File.Delete(directory + "\\note" + note.Id + ".txt");
        }

    }
}
