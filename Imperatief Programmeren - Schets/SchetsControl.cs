using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace SchetsEditor
{   public class SchetsControl : UserControl
    {   public Schets schets;
        private Color penkleur;
        public int lijndikte;
        public string filenaam = "../../Tekenelementen.txt";

        public Color PenKleur
        { get { return penkleur; }
        }
        public Schets Schets
        { get { return schets;   }
        }
        public SchetsControl()
        {   this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {   schets.Teken(pea.Graphics);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {   Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }
        public void VeranderKleur(object obj, EventArgs ea)
        {   string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        public void VeranderLijndikte(object obj, EventArgs ea)
        {
            SchetsWin schetswin = new SchetsWin();
            lijndikte = 3;
            if (schetswin.lijndikte > 0 && schetswin.lijndikte < 20)
                lijndikte = schetswin.lijndikte;
            else
                MessageBox.Show("Voer een geldige waarde in!");
        }

        List<RechthoekTool> rechthoeken = new List<RechthoekTool>();

        public void tekenRechthoek(string s, Point p1, Point p2)
        {
            rechthoeken.Add(new RechthoekTool(s, p1, p2));
        }

        public void schrijfNaarFileString(object o, EventArgs ea)
        {
            StreamWriter sw = new StreamWriter(filenaam);
            foreach (RechthoekTool R in rechthoeken)
                sw.WriteLine(R.ToString());
            sw.Close();
        }
        /*
        public void inlezen(object o, EventArgs ea)
        {
            rechthoeken.Clear();
            StreamReader sr = new StreamReader(filenaam);
            string regel;
            while ((regel = sr.ReadLine()) != null)
                rechthoeken.Add(new RechthoekTool());
            sr.Close();
            this.Invalidate();
        }
        */
    }
}
