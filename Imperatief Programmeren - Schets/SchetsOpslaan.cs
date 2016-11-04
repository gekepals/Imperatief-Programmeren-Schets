using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SchetsEditor
{
    public class SchetsOpslaan
    {
        Schets schets;
        Bitmap bmp;
         
        public SchetsOpslaan()
        {
            this.schets = new Schets();
            bmp = schets.bitmap;
        }

        public void opslaan(object o, EventArgs ea)
        {
            if (bmp.Text == "")
                opslaanAls(o, ea);
            else schrijfNaarFile();
        }

        public void opslaanAls(object o, EventArgs ea)
        {
            SaveFileDialog dialoog = new SaveFileDialog();
            dialoog.Filter = "JPG|*.jpg|BMP|*.bmp|PNG|*.png|Alle files|*.*";
            dialoog.Title = "Afbeelding opslaan als...";
            if (dialoog.ShowDialog() == DialogResult.OK)
            {
                bmp.Text = dialoog.FileName;
                bmp.Save()
            }
        }
    }
}
