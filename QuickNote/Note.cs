using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNote
{
    public class Note
    {
        private int id;

        public int Id { get => this.id; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Color Color { get; set; }
        public string Content { get; set; }
        public System.Drawing.Size Size { get; set; }

        public Note()
        {
            this.id = FileManager.GenerateNoteID();
            this.Location = new System.Drawing.Point(100, 100);
            this.Color = System.Drawing.Color.Beige;
            this.Size = new System.Drawing.Size(290, 270);
        }

        public Note(int id, System.Drawing.Point location, System.Drawing.Color color, System.Drawing.Size size, string content)
        {
            this.id = id;
            this.Location = location;
            this.Color = color;
            this.Content = content;
            this.Size = size;
        }
    }
}
