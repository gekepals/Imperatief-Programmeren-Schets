using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.IO;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class Schets
    {
        public Bitmap bitmap;
        public Control control;
        
        public Schets()
        {
            bitmap = new Bitmap(1, 1);
        }
        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }
        public void VeranderAfmeting(Size sz)
        {
            if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
            {
                Bitmap nieuw = new Bitmap( Math.Max(sz.Width,  bitmap.Size.Width)
                                         , Math.Max(sz.Height, bitmap.Size.Height)
                                         );
                Graphics gr = Graphics.FromImage(nieuw);
                gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
                gr.DrawImage(bitmap, 0, 0);
                bitmap = nieuw;
            }
        }
        public void Teken(Graphics gr)
        {
            gr.DrawImage(bitmap, 0, 0);
        }
        public void Schoon()
        {
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public void leesFile(string filenaam)
        {
            control = new Control();
            Graphics g = control.CreateGraphics();
            int nr = 0;
            StreamReader sr = new StreamReader(filenaam);
            string regel;
            string[] woorden;
            char[] separators = { ' ' };
            while ((regel = sr.ReadLine()) != null)
            {
                nr++;
                woorden = regel.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                if (woorden.Length == 6)
                {
                    switch (woorden[0])
                    {
                        case "pen":
                            return;
                            break;
                        case "lijn":
                            LijnTool lijn = new LijnTool();
                            lijn.Bezig(g,
                                new Point(int.Parse(woorden[1]), int.Parse(woorden[2])),
                                new Point(int.Parse(woorden[3]), int.Parse(woorden[4])));
                            break;
                        case "rechthoek":
                            RechthoekTool rechthoek = new RechthoekTool();
                            rechthoek.Bezig(g,
                                new Point(int.Parse(woorden[1]), int.Parse(woorden[2])),
                                new Point(int.Parse(woorden[3]), int.Parse(woorden[4])));
                            RechthoekTool.MaakPen(Brushes.Red, 3);
                            break;
                        case "grechthoek":
                            VolRechthoekTool grechthoek = new VolRechthoekTool();
                            grechthoek.Compleet(g,
                                new Point(int.Parse(woorden[1]), int.Parse(woorden[2])),
                                new Point(int.Parse(woorden[3]), int.Parse(woorden[4])));
                            VolRechthoekTool.MaakPen(Brushes.Yellow, 3);
                            break;
                        default:
                            return;

                    }
                }
            }
        }
    }
}
