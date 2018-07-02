using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickNote
{
    public partial class NoteForm : Form
    {
        //Viewed note
        private Note note;

        //Controls
        private TableLayoutPanel panel;
        private CustomTitlebar titleBar;
        private RichTextBox textBox;
        private PictureBox resizer;

        //Timer for autosaving after editing text
        private Timer txtTimer;

        //To control resizing
        private bool resizeState = false;
        private Point mouseDownResizeLocation = new Point();

        public NoteForm(Note note)
        {
            this.note = note;
            InitializeComponent();
            
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Beige;
            this.Size = note.Size;
            this.panel = new TableLayoutPanel();
            this.panel.Dock = DockStyle.Fill;
            this.Controls.Add(this.panel);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = note.Location;

            //Hides the application from the windows taskbar
            this.ShowInTaskbar = false;

            //Timer ticks after 1 seconds elapsed.
            this.txtTimer = new Timer();
            this.txtTimer.Interval = 1000;
            this.txtTimer.Tick += TxtTimer_Tick;

            
        }

        //When the timer ticks the note is saved
        private void TxtTimer_Tick(object sender, EventArgs e)
        {
            this.txtTimer.Stop();
            FileManager.SaveNote(this.note);
        }

        //Deletes the note and closes form
        public void Quit()
        {
            FileManager.DeleteNote(this.note);
            this.Close();
        }

        private void InstantiateTitleBar()
        {
            titleBar = new CustomTitlebar(this);
            titleBar.Show();
        }

        private void InstantiateTextBox()
        {
            textBox = new RichTextBox
            {
                Dock = DockStyle.Fill,

                Height = this.panel.Height - 30,
                BackColor = this.BackColor,
                BorderStyle = BorderStyle.None,
                Font = new Font(FontFamily.GenericMonospace, 12f),
                AllowDrop = true
            };

            textBox.TextChanged += textBox_TextChanged;
            textBox.Text = note.Content;
            textBox.DragDrop += TextBox_DragDrop;
            this.panel.Controls.Add(textBox);

        }

        private void TextBox_DragDrop(object sender, DragEventArgs e)
        {
            //System.Diagnostics.Debug.Write(e.Data.GetData(typeof(System.IO.File).ToString()));
        }

        //When the text changes the timer is restarted
        //This is to prevent redundant saving to file while user is typing
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.txtTimer.Stop();
            this.note.Content = this.textBox.Text;
            this.txtTimer.Start();
        }

        private void InstantiateResizer()
        {
            this.resizer = new PictureBox()
            {
                Anchor = (AnchorStyles.Bottom | AnchorStyles.Right),
                Image = QuickNote.Properties.Resources.resizer,
                Size = new Size(15, 15),
                
            };
            this.panel.Controls.Add(resizer);
            resizer.SendToBack();

            resizer.MouseDown += Resizer_MouseDown;
            resizer.MouseMove += Resizer_MouseMove;
            resizer.MouseUp += Resizer_MouseUp;

        }

        #region resizing movements
        private void Resizer_MouseUp(object sender, MouseEventArgs e)
        {
            resizeState = false;
        }

        private void Resizer_MouseMove(object sender, MouseEventArgs e)
        {
            if (resizeState)
            {
                int width = this.Size.Width;
                int height = this.Size.Height;

                width += e.Location.X - mouseDownResizeLocation.X;
                height += e.Location.Y - mouseDownResizeLocation.Y;

                if(width < 150)
                {
                    width = 150;
                }

                if(height < 150)
                {
                    height = 150;
                }

                this.Size = new Size(width, height);
                this.titleBar.Width = width;
                this.note.Size = this.Size;
                this.textBox.Height = this.panel.Height - 30;
                FileManager.SaveNote(this.note);

            }
        }

        private void Resizer_MouseDown(object sender, MouseEventArgs e)
        {
            this.resizeState = true;
            this.mouseDownResizeLocation = e.Location;
        }
        #endregion


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.InstantiateTitleBar();
            
            this.InstantiateTextBox();
            this.InstantiateResizer();
        }

        //Saves note in order to load correct location upon startup
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.note.Location = this.Location;
            FileManager.SaveNote(this.note);
        }

    }
}
