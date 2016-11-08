using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.IO;
using SchetsEditor;

namespace SchetsEditor
{
    public class SchetsWin : Form
    {
        string bmptitle;
        Schets schets;
        Bitmap bmp;
        MenuStrip menuStrip;
        SchetsControl schetscontrol;
        ISchetsTool huidigeTool;
        Panel paneel;
        bool vast;
        bool opgeslagen;
        public int lijndikte;

        ResourceManager resourcemanager
            = new ResourceManager("SchetsEditor.Properties.Resources"
                                 , Assembly.GetExecutingAssembly()
                                 );

        private void veranderAfmeting(object o, EventArgs ea)
        {
            schetscontrol.Size = new Size ( this.ClientSize.Width  - 70
                                          , this.ClientSize.Height - 50);
            paneel.Location = new Point(64, this.ClientSize.Height - 30);
        }

        private void klikToolMenu(object obj, EventArgs ea)
        {
            this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
        }

        private void klikToolButton(object obj, EventArgs ea)
        {
            this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
        }

        private void afsluiten(object obj, EventArgs ea)
        {
            if (opgeslagen == false)
                nietOpgeslagen(obj, ea);
            else
                this.Close();
        }

        public SchetsWin()
        {
            this.schets = new Schets();
            bmp = schets.bitmap;
            schets.leesFile("../../Tekenelementen.txt");

            ISchetsTool[] deTools = { new PenTool()         
                                    , new LijnTool()
                                    , new RechthoekTool()
                                    , new VolRechthoekTool()
                                    , new CirkelTool()
                                    , new VolCirkelTool()
                                    , new TekstTool()
                                    , new GumTool()
                                    };
            String[] deKleuren = { "Black", "Red", "Green", "Blue"
                                 , "Yellow", "Magenta", "Cyan" 
                                 };

            this.ClientSize = new Size(700, 500);
            huidigeTool = deTools[0];

            schetscontrol = new SchetsControl();
            schetscontrol.Location = new Point(64, 10);
            schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                       {   vast=true;
                                           opgeslagen = false;  
                                           huidigeTool.MuisVast(schetscontrol, mea.Location); 
                                       };
            schetscontrol.MouseMove += (object o, MouseEventArgs mea) =>
                                       {   if (vast)
                                           huidigeTool.MuisDrag(schetscontrol, mea.Location); 
                                       };
            schetscontrol.MouseUp   += (object o, MouseEventArgs mea) =>
                                       {   if (vast)
                                           huidigeTool.MuisLos (schetscontrol, mea.Location);
                                           vast = false; 
                                       };
            schetscontrol.KeyPress +=  (object o, KeyPressEventArgs kpea) => 
                                       {   huidigeTool.Letter  (schetscontrol, kpea.KeyChar); 
                                       };
            this.Controls.Add(schetscontrol);

            menuStrip = new MenuStrip();
            menuStrip.Visible = false;
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakToolMenu(deTools);
            this.maakAktieMenu(deKleuren);
            this.maakToolButtons(deTools);
            this.maakAktieButtons(deKleuren);
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }

        private void maakFileMenu()
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("File");
            menu.MergeAction = MergeAction.MatchOnly;
            menu.DropDownItems.Add("Open", null, this.openen);
            menu.DropDownItems.Add("Opslaan", null, this.opslaan);
            menu.DropDownItems.Add("Opslaan als", null, this.opslaanAls);
            menu.DropDownItems.Add("Sluiten", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }

        private void maakToolMenu(ICollection<ISchetsTool> tools)
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
            foreach (ISchetsTool tool in tools)
            {   ToolStripItem item = new ToolStripMenuItem();
                item.Tag = tool;
                item.Text = tool.ToString();
                item.Image = (Image)resourcemanager.GetObject(tool.ToString());
                item.Click += this.klikToolMenu;
                menu.DropDownItems.Add(item);
            }
            menuStrip.Items.Add(menu);
        }

        private void maakAktieMenu(String[] kleuren)
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("Aktie");
            menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon );
            menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer );
            ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
            foreach (string k in kleuren)
                submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
            menu.DropDownItems.Add(submenu);
            menuStrip.Items.Add(menu);
        }

        private void maakToolButtons(ICollection<ISchetsTool> tools)
        {
            int t = 0;
            foreach (ISchetsTool tool in tools)
            {
                RadioButton b = new RadioButton();
                b.Appearance = Appearance.Button;
                b.Size = new Size(45, 62);
                b.Location = new Point(10, 10 + t * 62);
                b.Tag = tool;
                b.Text = tool.ToString();
                b.Image = (Image)resourcemanager.GetObject(tool.ToString());
                b.TextAlign = ContentAlignment.TopCenter;
                b.ImageAlign = ContentAlignment.BottomCenter;
                b.Click += this.klikToolButton;
                this.Controls.Add(b);
                if (t == 0) b.Select();
                t++;
            }
        }

        private void maakAktieButtons(String[] kleuren)
        {   
            paneel = new Panel();
            paneel.Size = new Size(600, 24);
            this.Controls.Add(paneel);
            
            Button b; Label l; ComboBox cbb; TextBox t;
            b = new Button(); 
            b.Text = "Clear";  
            b.Location = new Point(  0, 0); 
            b.Click += schetscontrol.Schoon; 
            paneel.Controls.Add(b);
            
            b = new Button(); 
            b.Text = "Rotate"; 
            b.Location = new Point( 80, 0); 
            b.Click += schetscontrol.Roteer; 
            paneel.Controls.Add(b);
            
            l = new Label();  
            l.Text = "Penkleur:"; 
            l.Location = new Point(180, 3); 
            l.AutoSize = true;               
            paneel.Controls.Add(l);
            
            cbb = new ComboBox(); cbb.Location = new Point(240, 0); 
            cbb.DropDownStyle = ComboBoxStyle.DropDownList; 
            cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
            foreach (string k in kleuren)
                cbb.Items.Add(k);
            cbb.SelectedIndex = 0;
            paneel.Controls.Add(cbb);

            l = new Label();
            l.Text = "Lijndikte:";
            l.Location = new Point(370, 3);
            l.AutoSize = true;
            paneel.Controls.Add(l);

            t = new TextBox();
            t.Location = new Point(430, 0);
            t.Size = new Size(40, 10);
            t.AutoSize = true;
            if (int.TryParse(t.Text, out lijndikte))
                lijndikte = int.Parse(t.Text);
            paneel.Controls.Add(t);

            b = new Button();
            b.Text = "OK";
            b.Location = new Point(480, 0);
            b.Size = new Size(40, 20);
            b.Click += schetscontrol.VeranderLijndikte;
            paneel.Controls.Add(b);
        }

        public void opslaan(object o, EventArgs ea)
        {
            if (bmptitle == null)
                opslaanAls(o, ea);
            else schrijfNaarFile(bmptitle);
        }

        public void opslaanAls(object o, EventArgs ea)
        {
            SaveFileDialog dialoog = new SaveFileDialog();
            dialoog.Filter = "JPG|*.jpg|BMP|*.bmp|PNG|*.png|Alle files|*.*";
            dialoog.Title = "Afbeelding opslaan als...";
            if (dialoog.ShowDialog() == DialogResult.OK)
            {
                bmptitle = dialoog.FileName;
                this.schrijfNaarFile(bmptitle);
            }
        }

        public void schrijfNaarFile(string s)
        {
            bmp.Save(s);
            opgeslagen = true;
        }

        private void openen(object o, EventArgs ea)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "JPG|*.jpg|BMP|*.bmp|PNG|*.png|Alle files|*.*";
            open.Title = "Open...";
            if (open.ShowDialog() == DialogResult.OK)
            {
                schets.bitmap = (Bitmap)Bitmap.FromFile(open.FileName);
            }
        }

        private void nietOpgeslagen(object o, EventArgs ea)
        {
            DialogResult dialoog = MessageBox.Show("Weet je zeker dat je wilt afsluiten? Er zijn niet-opgeslagen veranderingen.", "Exit", MessageBoxButtons.YesNo);
            if (dialoog == DialogResult.Yes)
                this.Close();
            else if (dialoog == DialogResult.No)
                return;
        }
    }
}
