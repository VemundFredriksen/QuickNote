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
    public partial class CustomTitlebar : Form
    {
        //For moving the form
        private Point mouseDownPoint = new Point();
        private bool moveState = false;

        //Keeping track if form is pinned to front
        private bool topMostState = false;

        //Controllers on to titlebar
        Button delBtn;
        Button plusBtn;

        public CustomTitlebar(Form owner)
        {
            InitializeComponent();
            this.Owner = owner;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(owner.Size.Width, 20);
            this.BackColor = ColorTranslator.FromHtml("#FFE86B");
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(owner.Location.X, owner.Location.Y - 39);

            this.ShowInTaskbar = false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            InstantiatePinButton();
            InstantiateDeleteButton();
            InstantiatePlusButton();
            
        }

        private void InstantiatePinButton()
        {
            Button pinButton = new Button
            {
                Image = QuickNote.Properties.Resources.pin,
                Size = new Size(36, 36),
                Padding = new Padding(9),
                Margin = new Padding(5),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };

            pinButton.FlatAppearance.BorderSize = 0;
            pinButton.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            pinButton.MouseClick += PinButton_MouseClick;

            this.Controls.Add(pinButton);
        }

        private void PinButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.Owner.TopMost = !this.topMostState;
            this.topMostState = !this.topMostState;

        }

        private void InstantiatePlusButton()
        {
            plusBtn = new Button
            {
                Image = QuickNote.Properties.Resources.plus,
                Size = new Size(36, 36),
                Padding = new Padding(9),
                Margin = new Padding(5),
                Dock = DockStyle.Left,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
        };
            plusBtn.FlatAppearance.BorderSize = 0;
            plusBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            plusBtn.MouseClick += PlusBtn_MouseClick;

            this.Controls.Add(plusBtn);
        }

        private void PlusBtn_MouseClick(object sender, MouseEventArgs e)
        {
            (new NoteForm(new Note())).Show();
        }

        private void InstantiateDeleteButton()
        {
            delBtn = new Button();
            delBtn.Image = QuickNote.Properties.Resources.delete;
            delBtn.Size = new Size(36, 36);
            delBtn.Padding = new Padding(9);
            delBtn.Margin = new Padding(5);
            delBtn.Dock = DockStyle.Right;
            delBtn.FlatAppearance.BorderSize = 0;
            delBtn.FlatStyle = FlatStyle.Flat;
            delBtn.TabStop = false;
            delBtn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            this.Controls.Add(delBtn);
            delBtn.MouseClick += DelBtn_MouseClick;
        }

        private void DelBtn_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure u want to delete this note?",
                "Delete Note",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
            if(result == DialogResult.Yes)
            {
                ((NoteForm)this.Owner).Quit();
            }
        }

        //Mouse Events for moving the Noteform
        #region Mouse Events - Form movement
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.mouseDownPoint = e.Location;
            this.moveState = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.moveState = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (moveState)
            {
                Point pos = new Point(this.Location.X + (e.X - mouseDownPoint.X), this.Location.Y + (e.Y - mouseDownPoint.Y));
                this.Location = pos;
                this.Owner.Location = new Point(Owner.Location.X + (e.X - mouseDownPoint.X), Owner.Location.Y + (e.Y - mouseDownPoint.Y));
            }
        }
        #endregion
    }

}
