using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace HoloImmitation
{
    public class PicBoxContainer
    {
        protected PictureBox pic;
        protected Label lbl;
        protected Bitmap img;
        protected Graphics graph;
        protected Bitmap canvas;

        protected Point shift, zeroPt;
        bool stretch;

        public PicBoxContainer(PictureBox pic, Label lbl)
        {
            this.pic = pic;
            this.lbl = lbl;
            pic.SizeChanged += pic_SizeChanged;
            pic.PreviewKeyDown += pic_PreviewKeyDown;
            
            pic.MouseDoubleClick += pic_MouseDoubleClick;

            pic.MouseDown += pic_MouseDown;
            pic.MouseUp += pic_MouseUp;
            pic.MouseMove += pic_MouseMove;

            canvas = new Bitmap(pic.Width, pic.Height);
            pic.Image = canvas;
            graph = Graphics.FromImage(canvas);
        }

        


        bool dragging;
        Point startPos;
        void pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
               /* zeroPt = shift;
                shift = Point.Empty;*/
            }
        }
        void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                shift.X = shift.X + (e.X - startPos.X) ;
                shift.Y = shift.Y + (e.Y - startPos.Y) ;
                startPos = e.Location;
                Refresh();
            }
        }
        void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                startPos = e.Location;
            }
        }

        void pic_Paint(object sender, PaintEventArgs e)
        {
            Refresh();
        }

        void pic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            stretch = !stretch;
            Refresh();
        }

        void pic_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
        }

        void pic_SizeChanged(object sender, EventArgs e)
        {
            RedravCanvas();
        }

        public void Refresh()
        {
            RedravCanvas();          
            pic.Refresh();
        }

        private void RedravCanvas()
        {
            

            pic.Image = null;
            canvas = new Bitmap(pic.Width, pic.Height);
            pic.Image = canvas;

            graph = Graphics.FromImage(canvas);

            if (img == null)
                return;

            if (stretch)
            {
                double kw, kh;
                kw = ((double)canvas.Width) / img.Width;
                kh = ((double)canvas.Height) / img.Height;
                double k = kw;                
                if (kh < kw)
                    k = kh;
                int nw = (int)(img.Width * k), nh = (int)(img.Height * k);
                graph.DrawImage(img, new Rectangle(canvas.Width / 2 - nw/ 2, canvas.Height / 2 - nh / 2, nw, nh), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            }
            else
            {
                graph.DrawImage(img, zeroPt.X + shift.X, zeroPt.Y + shift.Y);
            }
        }

        public void SetImage(Bitmap bmp)
        {
            img = bmp;           
            RedravCanvas();
            pic.Refresh();
        }

        
    }
}
