using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class SchetsOpslaan
    {
        Schets schets;
        Bitmap bmp;
        string bmptitle;
        SaveFileDialog dialoog = new SaveFileDialog();

        public SchetsOpslaan()
        {
            this.schets = new Schets();
            bmp = schets.bitmap;
        }

        public void opslaan(object o, EventArgs ea)
        {
            if (bmptitle == "")
                opslaanAls(o, ea);
            else schrijfNaarFile(bmptitle);
        }

        public void opslaanAls(object o, EventArgs ea)
        {
            
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
        }

 
    }
}
