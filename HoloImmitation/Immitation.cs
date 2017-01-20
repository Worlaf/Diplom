#define SAFE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Math;
using AForge.Imaging;
using System.Diagnostics;
using System.ComponentModel;


namespace HoloImmitation
{



    enum FilterType
    {
        None,
        Laplasian,
        PhaseShift
    }

    class APImage
    {
        public struct APData
        {
            public double Amplitude;
            public double Phase;
        }

        protected Bitmap origin;
        public Bitmap Origin { get { return origin; } }
        public APData this[int x, int y]
        {
            get
            {

#if SAFE
                if (x < 0)
                    x = 0;
                if (x > origin.Width)
                    x = origin.Width - 1;
                if (y < 0)
                    y = 0;
                if (y > origin.Height)
                    y = origin.Height - 1;
#endif

                APData data;
                data.Amplitude = this.amplitude[x, y];
                data.Phase = this.phase[x, y];
                return data;

            }

        }


        protected double[,] amplitude;

        protected double[,] phase;

        /// <summary>
        /// Создание АФ изображения с заданной фазой
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="phase"></param>
        public APImage(Bitmap origin, double phase)
        {
            
                this.origin = origin;
            
            this.amplitude = new double[origin.Width, origin.Height];
            this.phase = new double[origin.Width, origin.Height];

            System.Drawing.Imaging.BitmapData data = this.origin.LockBits(new Rectangle(System.Drawing.Point.Empty, origin.Size),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j, width = origin.Width, height = origin.Height;
            
            unsafe
            {

                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        this.amplitude[i, j] = (double)(idx0[0] * .3 + idx0[1] * .59 + idx0[2] * .11) * idx0[3] / 255.0;
                        this.phase[i, j] = phase;
                        idx0+=4;
                    }
                }

            }
            origin.UnlockBits(data);
        }

        /// <summary>
        /// Создание АФ изображение со случайной фазой
        /// </summary>
        /// <param name="origin"></param>
        public APImage(Bitmap origin, bool originAsAmplitude = false)
        {
           
            this.origin = origin;
            this.amplitude = new double[origin.Width, origin.Height];
            this.phase = new double[origin.Width, origin.Height];
            int i, j, width = origin.Width, height = origin.Height;
            System.Drawing.Imaging.BitmapData data = this.origin.LockBits(new Rectangle(System.Drawing.Point.Empty, origin.Size),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            unsafe
            {

                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        if (originAsAmplitude)
                        {
                            this.amplitude[i, j] = (double)(idx0[0] * .3 + idx0[1] * .59 + idx0[2] * .11) * idx0[3] / 255.0 / 255.0;
                            this.phase[i, j] = 0;
                        }
                        else
                        {
                            this.phase[i, j] = (double)(idx0[0] * .3 + idx0[1] * .59 + idx0[2] * .11) * idx0[3] / 255.0 / 255.0 * 2 * Math.PI;
                            this.amplitude[i, j] = 1;
                        }                        
                        idx0+=4;
                    }
                }


            }
            origin.UnlockBits(data);
        }

        public APImage(double[,] amplitude)
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            this.amplitude = amplitude;
            this.phase = new double[width, height];
            this.origin = new Bitmap(width, height);
            Random rnd = new Random();
            System.Drawing.Imaging.BitmapData data = origin.LockBits(new Rectangle(System.Drawing.Point.Empty, origin.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        idx0[0] = idx0[1] =  idx0[2] = (byte)amplitude[i, j];

                        idx0[3] = 255;
                        this.phase[i, j] = rnd.NextDouble() * 2 * Math.PI;
                        idx0+=4;
                    }
                }

            }
            origin.UnlockBits(data);


        }

        public APImage(double[,] amplitude, double[,] phase)
        {
#if SAFE
            if (amplitude.GetLength(0) != phase.GetLength(0) || amplitude.GetLength(1) != phase.GetLength(1))
            {
                throw new Exception("Размеры массивов не совпадают");
            }
#endif
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            this.amplitude = amplitude;
            this.phase = phase;

            this.origin = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = origin.LockBits(new Rectangle(System.Drawing.Point.Empty, origin.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        idx0[0] = idx0[1] = idx0[2] = (byte)amplitude[i, j];

                        idx0[3] = 255;
                        idx0+=4;
                    }
                }

            }
            origin.UnlockBits(data);

        }

        public APImage(Complex[,] c)
        {
            int width = c.GetLength(0), height = c.GetLength(1);
            this.amplitude = new double[width, height];
            this.phase = new double[width, height]; 
           
            int i, j;

            unsafe
            {
                      
               
                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        amplitude[i, j] = c[i, j].Magnitude;
                        phase[i, j] = c[i, j].Phase;                       
                    }
                }

            }

            origin = GetAmplitudeImage();
        }

        public APImage(int size)
        {
            origin = new Bitmap(size, size);
            amplitude = new double[size, size];
            phase = new double[size, size];
            FillAmplitudes(0);
            FillPhases(0);
        }

        public void RandomizeAmplitudes()
        {
            Random rnd = new Random();
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    amplitude[i, j] = rnd.NextDouble();
                }
            }
        }
        public void RandomizePhases()
        {
            Random rnd = new Random();
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    phase[i, j] = rnd.NextDouble()*Math.PI*2;
                }
            }
        }
       
        public void FillAmplitudes(double value)
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);            
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    amplitude[i, j] = value;
                }
            }
        }
        public void FillPhases(double value)
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    phase[i, j] = value;
                }
            }
        }

        public double[,] GetAmplitudes(bool amplify = true)
        {
            if (!amplify)
                return amplitude;

            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            double max = double.MinValue;
            double min = double.MaxValue;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (amplitude[i, j] > max)
                    {
                        max = amplitude[i, j];
                    }
                    if (amplitude[i, j] < min)
                    {
                        min = amplitude[i, j];
                    }
                }
            }
            double d = -min;
            double w = (max - min);
            if (w == 0)
                return amplitude;

            double[,] result = new double[width, height];
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    result[i, j] = (amplitude[i, j] + d) / w;
                }
            }
            return result;
        }

        public double[,] GetAmplitudes(double maxpcnt, bool amplify = true)
        {
            if (!amplify)
                return amplitude;

            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            double max = double.MinValue;
            double min = double.MaxValue;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (amplitude[i, j] > max)
                    {
                        max = amplitude[i, j];
                    }
                    if (amplitude[i, j] < min)
                    {
                        min = amplitude[i, j];
                    }
                }
            }
            max = max * maxpcnt;
            double d = -min;
            double w = (max - min);
            if (w == 0)
                return amplitude;
            double val;
            double[,] result = new double[width, height];
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    val = amplitude[i, j];
                    if (val > max)
                        val = max;
                    result[i, j] = (val + d) / w;
                }
            }
            return result;
        }

        public double[,] GetPhases(double maxpcnt, bool amplify = true)
        {
            if (!amplify)
                return phase;

            int width = phase.GetLength(0), height = phase.GetLength(1);
            double max = double.MinValue;
            double min = double.MaxValue;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (phase[i, j] > max)
                    {
                        max = phase[i, j];
                    }
                    if (phase[i, j] < min)
                    {
                        min = phase[i, j];
                    }
                }
            }
            max = max * maxpcnt;
            double d = -min;
            double w = (max - min);
            double val;
            double[,] result = new double[width, height];
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    val = phase[i, j];
                    if (val > max)
                        val = max;
                    val = (val + d) / w;
                    if (min < 0 && val > 0.5)
                    {
                        val = val - 0.5;
                    }
                    else if (min < 0 && val <= 0.5)
                    {
                        val = val + 0.5;
                    }
                    result[i, j] = val;
                }
            }
            return result;
        }

        public double[,] GetReal()
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            double[,] real = new double[width, height];
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    real[i, j] = amplitude[i, j] * Math.Cos(phase[i, j]);
                }
            }
            return real;
        }

        public double[,] GetImag()
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            double[,] imag = new double[width, height];
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    imag[i, j] = amplitude[i, j] * Math.Sin(phase[i, j]);
                }
            }
            return imag;
        }

        public double[,] GetReal(double maxpcnt, bool amplify = true)
        {
            if (!amplify)
                return GetReal();

            double[,] real = GetReal();

            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);


            double max = double.MinValue;
            double min = double.MaxValue;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (real[i, j] > max)
                    {
                        max = real[i, j];
                    }
                    if (real[i, j] < min)
                    {
                        min = real[i, j];
                    }
                }
            }
            max = max * maxpcnt;
            double d = -min;
            double w = (max - min);
            double val;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    val = real[i, j];
                    if (val > max)
                        val = max;
                    real[i, j] = (val + d) / w;
                }
            }
            return real;
        }

        public double[,] GetImag(double maxpcnt, bool amplify = true)
        {
            if (!amplify)
                return GetImag();

            double[,] imag = GetImag();

            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);


            double max = double.MinValue;
            double min = double.MaxValue;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (imag[i, j] > max)
                    {
                        max = imag[i, j];
                    }
                    if (imag[i, j] < min)
                    {
                        min = imag[i, j];
                    }
                }
            }
            max = max * maxpcnt;
            double d = -min;
            double w = (max - min);
            double val;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    val = imag[i, j];
                    if (val > max)
                        val = max;
                    imag[i, j] = (val + d) / w;
                }
            }
            return imag;
        }

        public double[] GetAmpliduteBounds()
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            double max = double.MinValue;
            double min = double.MaxValue;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    if (amplitude[i, j] > max)
                    {
                        max = amplitude[i, j];
                    }
                    if (amplitude[i, j] < min)
                    {
                        min = amplitude[i, j];
                    }
                }
            }

            return new double[] { min, max };
        }
        
        public Bitmap GetAmplitudeRangeImage(double ampl, double range)
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            Bitmap newImage = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;
                

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        if (amplitude[i, j] < ampl + range && amplitude[i, j] > ampl - range)
                        {
                            idx0[0] = idx0[1] = idx0[2] = 255;
                        }
                        else 
                        {
                            idx0[0] = idx0[1] = idx0[2] = 0;
                        }
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }
        
        public Bitmap GetAmplitudeImage(bool amplify = false)
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            Bitmap newImage = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;
                double d, tmp;
                double[,] amplitudes;

                if (amplify)
                {
                    amplitudes = this.GetAmplitudes(true);
                    d = 255;
                }
                else
                {
                    amplitudes = amplitude;
                    d = 1;
                }
                byte b;


                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        tmp = amplitudes[i,j] * d;
                        if (tmp < 255)
                            b = (byte)(tmp);
                        else
                            b = 255;

                        idx0[0] = idx0[1] = idx0[2] = b;
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }

        public Bitmap GetAmplitudeImage(double max, bool amplify = false)
        {
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            Bitmap newImage = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;
                double d, tmp;
                double[,] amplitudes;

                if (amplify)
                {
                    amplitudes = this.GetAmplitudes(max, true);
                    d = 255;
                }
                else
                {
                    amplitudes = amplitude;
                    d = 1;
                }
                byte b;


                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        tmp = amplitudes[i, j] * d;
                        if (tmp < 255)
                            b = (byte)(tmp);
                        else
                            b = 255;

                        idx0[0] = idx0[1] = idx0[2] = b;
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }

        public Bitmap GetPhaseImage()
        {
            int width = origin.Width, height = origin.Height;
            Bitmap newImage = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        idx0[0] = idx0[1] = idx0[2] = (byte)phase[i, j];
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }

        public Bitmap GetRealImage()
        {
            int width = origin.Width, height = origin.Height;
            Bitmap newImage = new Bitmap(width, height);
            Complex[,] cpx = GetComplex();
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        idx0[0] = idx0[1] = idx0[2] = (byte)phase[i, j];
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }

        public Bitmap GetImagImage()
        {
            int width = origin.Width, height = origin.Height;
            Bitmap newImage = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        idx0[0] = idx0[1] = idx0[2] = (byte)phase[i, j];
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }

        public Complex[,] GetComplex()
        {
            int width = Origin.Width, height = Origin.Height, i, j;
            Complex[,] c = new Complex[width, height];
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    c[i, j] = Utility.Exp2Cpx(amplitude[i, j], phase[i, j]);
                   /* c[i, j].Re = amplitude[i, j] * Math.Cos(phase[i, j]);
                    c[i, j].Im = amplitude[i, j] * Math.Sin(phase[i, j]);*/
                }
            }

            return c;
        }

        public void SwapAmplitudePhase()
        {
            
            
            int width = amplitude.GetLength(0), height = amplitude.GetLength(1);
            double tmp;
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    tmp = amplitude[i, j];
                    amplitude[i, j] = phase[i, j] / (2 * Math.PI);
                    phase[i, j] = tmp * 2 * Math.PI;
                }
            }
        }

        public double[,] GetPhases()
        {
            return phase;
        }

        public void LoadAmplitudeImage(Bitmap bmp, LoadImageProperties props)
        {
            Bitmap result = GetAmplitudeImage(true);
            Graphics graph = Graphics.FromImage(result);
            int size, w, h;
            int x, y;
            size = result.Width;
            w = bmp.Width;
            h = bmp.Height;
            if (props.PasteSizeOptions == LoadImageProperties.SizeOptions.None)
            {
                if (props.PlaseAtCenter)
                {
                    x = y = size / 2;
                    x -= w / 2;
                    y -= h / 2;
                    graph.DrawImage(bmp, x, y);
                }
                else
                {
                    graph.DrawImage(bmp, 0, 0);
                }
            }
            else if (props.PasteSizeOptions == LoadImageProperties.SizeOptions.Stretch)
            {
                graph.DrawImage(bmp, new Rectangle(0, 0, size, size), new Rectangle(0, 0, w, h), GraphicsUnit.Pixel);
            }
            else
            {
                double f = w;
                if (h > w)
                    f = h;
                double k = size / f;

                w = (int)(w * k);
                h = (int)(h * k);
                graph.DrawImage(bmp, new Rectangle(size / 2 - w / 2, size / 2 - h / 2, w, h), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

            }

            System.Drawing.Imaging.BitmapData data = result.LockBits(new Rectangle(System.Drawing.Point.Empty, result.Size),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j, width = origin.Width, height = origin.Height;

            unsafe
            {

                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        this.amplitude[i, j] = (double)(idx0[0] * .3 + idx0[1] * .59 + idx0[2] * .11) * idx0[3] / 255.0;                        
                        idx0 += 4;
                    }
                }

            }
            result.UnlockBits(data);
        }

        public void LoadPhaseImage(Bitmap bmp, LoadImageProperties props)
        {
            Bitmap result = GetPhaseImage();
            Graphics graph = Graphics.FromImage(result);
            int size, w, h;
            int x, y;
            size = result.Width;
            w = bmp.Width;
            h = bmp.Height;
            if (props.PasteSizeOptions == LoadImageProperties.SizeOptions.None)
            {
                if (props.PlaseAtCenter)
                {
                    x = y = size / 2;
                    x -= w / 2;
                    y -= h / 2;
                    graph.DrawImage(bmp, x, y);
                }
                else
                {
                    graph.DrawImage(bmp, 0, 0);
                }
            }
            else if (props.PasteSizeOptions == LoadImageProperties.SizeOptions.Stretch)
            {
                graph.DrawImage(bmp, new Rectangle(0, 0, size, size), new Rectangle(0, 0, w, h), GraphicsUnit.Pixel);
            }
            else
            {
                double f = w;
                if (h > w)
                    f = h;
                double k = size / f;

                w = (int)(w * k);
                h = (int)(h * k);
                graph.DrawImage(bmp, new Rectangle(size / 2 - w / 2, size / 2 - h / 2, w, h), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

            }

            System.Drawing.Imaging.BitmapData data = result.LockBits(new Rectangle(System.Drawing.Point.Empty, result.Size),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j, width = origin.Width, height = origin.Height;

            unsafe
            {

                byte* idx0 = (byte*)data.Scan0;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        this.phase[i, j] = (double)(idx0[0] * .3 + idx0[1] * .59 + idx0[2] * .11) * idx0[3] / 255.0 / 255.0 * 2 * Math.PI;
                        idx0 += 4;
                    }
                }

            }
            result.UnlockBits(data);
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            int i, j, a;
            Color o;

            for (i = 0; i < original.Width; i++)
            {
                for (j = 0; j < original.Height; j++)
                {
                    o = original.GetPixel(i, j);
                    a = (int)(o.R * .3 + o.G * .59 + o.B * .11) * o.A;

                    newBitmap.SetPixel(i, j, Color.FromArgb(a, a, a));
                }
            }

            return newBitmap;
        }
    }

    class Utility
    {
        public static Complex Exp2Cpx(double r, double phi)
        {
            Complex cpx;
            cpx.Re = r * Math.Cos(phi);
            cpx.Im = r * Math.Sin(phi);
            return cpx;
        }

        public static Complex[,] Shift2D(Complex[,] array)
        {
           
            int w = array.GetLength(0), h = array.GetLength(1);

            /* if (w % 2 != 0 || h % 2 != 0){
                 throw new Exception("Error");
             }
            */
            int cntX = w/2;
            int cntY = h/2;
            int i, j;
            Complex[,] o = new Complex[w, h];
            unsafe
            {

                for (i = 0; i < cntX; i++)
                {
                    for (j = 0; j < cntY; j++)
                    {
                        o[cntX + i, cntY + j] = array[i, j];
                        o[i, cntY + j] = array[cntX + i, j];
                        o[i, j] = array[cntX + i, cntY + j];
                        o[cntX + i, j] = array[i, cntY + j];
                    }
                }
                
            }
                
            return o;            
        }

        public static double[,] Laplasian(double[,] h_in)
        {
            
            double hh;
            int x, y;
            int width = h_in.GetLength(0);
            int height = h_in.GetLength(1);
            double[,] h_out = new double[width, height];

            

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    hh = h_in[x, y];
                    //if (x > 0 && y > 0) hh += h_in[x - 1, y - 1];
                    if (x > 0) hh += h_in[x - 1, y];
                    //if (x > 0 && y < height - 1) hh += h_in[x - 1, y + 1];
                    if (y > 0) hh += h_in[x, y - 1];
                    if (y < height - 1) hh += h_in[x, y + 1];
                    //if (x < width - 1 && y < height - 1) hh += h_in[x + 1, y + 1];
                    if (x < width - 1) hh += h_in[x + 1, y];
                    //if (x < width - 1 && y > 0) hh += h_in[x + 1, y - 1];
                    hh /= 5;

                    h_out[x, y] = h_in[x, y] - hh;                   
                }
            }           
            return h_out;
        }
        public static Complex[,] Laplasian(Complex[,] h_in)
        {

            double hh;
            int x, y;
            int width = h_in.GetLength(0);
            int height = h_in.GetLength(1);
            Complex[,] h_out = new Complex[width, height];



            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    hh = h_in[x, y].Re;
                    //if (x > 0 && y > 0) hh += h_in[x - 1, y - 1];
                    if (x > 0) hh += h_in[x - 1, y].Re;
                    //if (x > 0 && y < height - 1) hh += h_in[x - 1, y + 1];
                    if (y > 0) hh += h_in[x, y - 1].Re;
                    if (y < height - 1) hh += h_in[x, y + 1].Re;
                    //if (x < width - 1 && y < height - 1) hh += h_in[x + 1, y + 1];
                    if (x < width - 1) hh += h_in[x + 1, y].Re;
                    //if (x < width - 1 && y > 0) hh += h_in[x + 1, y - 1];
                    hh /= 5;

                    h_out[x, y].Re = h_in[x, y].Re - hh;
                    h_out[x,y].Im = h_in[x,y].Im;
                }
            }
            return h_out;
        }
        public static Bitmap MakeBitmap(double[,] normalized_data, double multiplier = 1)
        {
            int width = normalized_data.GetLength(0), height = normalized_data.GetLength(1);
            Bitmap newImage = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data = newImage.LockBits(new Rectangle(System.Drawing.Point.Empty, newImage.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int i, j;
            unsafe
            {
                byte* idx0 = (byte*)data.Scan0;
                double tmp;                
                byte b;

                for (i = 0; i < width; i++)
                {
                    for (j = 0; j < height; j++)
                    {
                        tmp = normalized_data[i, j] / multiplier * 255 ;
                        if (tmp < 255)
                            b = (byte)(tmp);
                        else
                            b = 255;

                        idx0[0] = idx0[1] = idx0[2] = b;
                        idx0[3] = 255;
                        idx0 += 4;
                    }
                }

            }
            newImage.UnlockBits(data);
            return newImage;
        }

        /// <summary>
        /// Комплексно сопряженное число
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Complex CpxConj(Complex num)
        {
            num.Im = -num.Im;
            return num;
        }

        public static Complex[,] CpxPhaseShiftFilter(double amp, double[,] I0, double[,] IPi2, double[,] IPi)
        {
            int size = I0.GetLength(0);
            Complex[,] result = new Complex[size, size];
            Complex c1, c2;
            c1.Re = 1 / (4 * amp);
            c1.Im = 1 / (4 * amp);
            int x, y;
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    c2.Re = I0[x, y] - IPi2[x, y];
                    c2.Im = IPi[x, y] - IPi2[x, y];
                    result[x, y] = c1 * c2;
                }
            }
            return result;
        }

        public static Complex[,] PhaseShiftFilter(double amp, double[,] I0, double[,] IPi2, double[,] IPi)
        {
            int size = I0.GetLength(0);
            Complex[,] result = new Complex[size, size];
            Complex c1, c2;
            c1.Re = 1 / (4 * amp);
            c1.Im = 1 / (4 * amp);
            int x, y;
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    c2.Re = I0[x, y] - IPi2[x, y];
                    c2.Im = IPi[x, y] - IPi2[x, y];
                    result[x, y] = (c1 * c2);
                }
            }
            return result;
        }
        public static double[,] PhaseShiftFilter(double amp, double[,] I0, double[,] IPi2, double[,] IPi, double[,] I3Pi2)
        {
            int size = I0.GetLength(0);
            double[,] result = new double[size, size];
            Complex c1, c2;
            c1.Re = 1 / (4 * amp);
            c1.Im = 1 / (4 * amp);
            int x, y;
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    c2.Re = I0[x, y] - IPi[x, y];
                    c2.Im = IPi2[x, y] - I3Pi2[x, y];
                    result[x, y] = (c1 * c2).Magnitude;
                }
            }
            return result;
        }
    }

    class HologrammGenerator
    {
        public static APImage Construct(APImage source, HologrammParametres parametres, bool onlyIntensity = true)
        {
            return Construct(source, parametres.WaveLength / 1000000000, parametres.Angle, parametres.Amplitude, parametres.Distance / 1000, parametres.CCDPixelDistance / 1000000, parametres.CCDPixelDistance / 1000000, parametres.RefBeamPhase, onlyIntensity);
        }
        public static APImage Construct(APImage source, double lambda, double angle, double amplitude, double zDistance, double srcPixelDist, double dstPixelDist, double refBeamPhase, bool onlyIntensity = true)
        {
            if (source == null)
                return null;
            int size = source.Origin.Width;
            double[,] srcAmplitudes = source.GetAmplitudes(false);
            double[,] srcPhases = source.GetPhases();
            Complex[,] srcCpx = source.GetComplex();

            Complex[,] cpx1 = new Complex[size, size];
            
            //Первый этап, вычисляем преобразование Фурье
            int k, l;
            double kdx, ldy;
            double dst_half = size * dstPixelDist / 2;
            double const1 = -Math.PI / (lambda * zDistance);

            for (k = 0; k < size; k++)
            {
                kdx = k * dstPixelDist - dst_half;
                kdx = kdx * kdx;
                for (l = 0; l < size; l++)
                {
                    ldy = l * dstPixelDist - dst_half;
                    ldy = ldy * ldy;
                    cpx1[k, l] = Utility.Exp2Cpx(srcAmplitudes[k, l], const1 * (kdx + ldy) + srcPhases[k, l]);
                    //cpx1[k, l] = srcCpx[k, l] * Utility.Exp2Cpx(1, const1 * (kdx + ldy));
                }
            }
            FourierTransform.FFT2(cpx1, FourierTransform.Direction.Forward);
            cpx1 = Utility.Shift2D(cpx1);


            //
            Complex[,] field = ReferenceBeam.GenerateFlatField(dstPixelDist, size, amplitude, lambda, angle, refBeamPhase);

            Complex tcpx1;
            tcpx1.Re = 0;
            tcpx1.Im = 1 / (lambda * zDistance);
            tcpx1 *= Utility.Exp2Cpx(1, -2 * Math.PI * zDistance / lambda);
            const1 = -Math.PI * lambda * zDistance;
            int m, n;
            double mdx, ndy, Ndx = 1 / (size * dstPixelDist);
            Complex tcpx2, tcpx3;
            double max_amp = double.MinValue;

            for (m = 0; m < size; m++)
            {
                mdx = m * Ndx;// -dst_half;
                mdx = mdx * mdx;
                for (n = 0; n < size; n++)
                {
                    ndy = n * Ndx;// -dst_half;
                    ndy = ndy * ndy;
                   
                    cpx1[m, n] = cpx1[m, n] * tcpx1 * Utility.Exp2Cpx(1, const1 * (mdx + ndy));
                   // cpx1[m, n] = tcpx2.SquaredMagnitude + tcpx3.SquaredMagnitude + tcpx2 * Utility.CpxConj(tcpx3) +
                   //     Utility.CpxConj(tcpx2) * tcpx3;

                    if (cpx1[m, n].Magnitude > max_amp)
                        max_amp = cpx1[m, n].Magnitude;
                    

                }
            }
            
            max_amp /= 10;
            if (max_amp == 0)
            {
                max_amp = 1;
            }
            for (m = 0; m < size; m++)
            {                
                for (n = 0; n < size; n++)
                {
                    if (onlyIntensity)
                    {
                        //Условие убрать после тестирований
                        if (max_amp > 0)
                            tcpx2 = cpx1[m, n] / max_amp;
                        else
                            tcpx2 = cpx1[m, n];
                        tcpx3 = field[m, n];
                        cpx1[m, n] = tcpx2.SquaredMagnitude + tcpx3.SquaredMagnitude + tcpx2 * Utility.CpxConj(tcpx3) +
                            Utility.CpxConj(tcpx2) * tcpx3;
                    }
                    else
                    {
                        cpx1[m, n] = cpx1[m, n] / max_amp + field[m, n];
                    }
                }
            }


            return new APImage(cpx1);

        }

        public static APImage Reconstruct(Complex[,] source, HologrammParametres parametres, bool filter, bool no_refBeam, bool only_refBeam)
        {
            return Reconstruct(source, parametres.WaveLength / 1000000000, parametres.Angle, parametres.Amplitude, parametres.Distance / 1000, parametres.CCDPixelDistance / 1000000, filter, no_refBeam, only_refBeam);
        }

        public static APImage Reconstruct(Complex[,] source, double lambda, double angle, double amplitude, double zDistance, double PixelDist, bool filter, bool no_refBeam, bool only_refBeam)
        {
            if (source == null)
                return null;
            int size = source.GetLength(0);

            if (filter)
            {
                source = Utility.Laplasian(source);
            }


            Complex[,] field = ReferenceBeam.GenerateFlatField(PixelDist, size, amplitude, lambda, angle);
            Complex[,] cpx1 = new Complex[size, size];
            double const1 = -Math.PI / (lambda * zDistance);
            int k, l;
            double kdx,ldy;
            double half_len = (size - 1) * PixelDist / 2;
            double len = (size - 1) * PixelDist;

            Complex ctmp1, ctmp2;
            for (k = 0; k < size; k++)
            {
                kdx = k * PixelDist - half_len;
                kdx *= kdx;
                for (l = 0; l < size; l++)
                {
                    ldy = l * PixelDist - half_len;
                    ldy *= ldy;
                    //
                    if (only_refBeam)
                    {
                        cpx1[k, l] = field[k, l];
                    }
                    else if (no_refBeam)
                    {
                        cpx1[k, l] = source[k, l] * Utility.Exp2Cpx(1, const1 * (kdx + ldy));
                    }
                    else
                    {
                        cpx1[k, l] = source[k, l] * Utility.CpxConj(field[k,l]) * Utility.Exp2Cpx(1, const1 * (kdx + ldy));                       
                    }
                  
                }
            }
             
            FourierTransform.FFT2(cpx1, FourierTransform.Direction.Forward);
            cpx1 = Utility.Shift2D(cpx1);
           /*

            Complex tcpx1;
            tcpx1.Re = 0;
            tcpx1.Im = 1 / (lambda * zDistance);
            tcpx1 *= Utility.Exp2Cpx(1, -2 * Math.PI * zDistance / lambda);
            const1 = -Math.PI * lambda * zDistance;
            double Ndx = 1 / (size * PixelDist);
            double m2, n2;
            int m, n;
            for (m = 0; m < size; m++)
            {
                m2 = m * Ndx - half_len;
                m2 *= m2;                
                for (n = 0; n < size; n++)
                {
                    n2 = n * Ndx - half_len;
                    n2 *= n2;

                    cpx1[m, n] = cpx1[m, n] * tcpx1 * Utility.Exp2Cpx(1, const1 * (m2 + n2));
                }
            }
            
            //Участок кода из диссертации
            /*
            double f2 = -Math.PI * lambda * zDistance;
            double wnum = Math.PI * 2 / lambda;
            double f3 = -wnum * zDistance;
            double f4 = 1 / (lambda * zDistance);
            double xDist, yDist;
            int x, y;
            Complex var1, var3, var4;
            var3 = Utility.Exp2Cpx(1, f3);
            for (x = 0; x < size; x++)
            {
                xDist = ((x - size + 1) / 2) / (len + PixelDist);
                xDist *= xDist;
                for (y = 0; y < size; y++)
                {
                    yDist = ((y - size + 1) / 2) / (len + PixelDist);
                    yDist *= yDist;

                    var1 = Utility.Exp2Cpx(1, f2 * (xDist + yDist));
                    var4 = var3 * var1;
                    cpx1[x, y] = cpx1[x, y] * var4;
                }
            }
            */
            return new APImage(cpx1);


        }

        public static APImage ReconstructConv(double[,] source, double lambda, double angle, double amplitude, double zDistance, double PixelDist, bool filter)
        {
            if (source == null)
                return null;
            int size = source.GetLength(0);

            if (filter)
            {
                source = Utility.Laplasian(source);
            }


            Complex[,] field = ReferenceBeam.GenerateFlatField(PixelDist, size, amplitude, lambda, angle);
            Complex[,] cpx1 = new Complex[size, size];
            Complex[,] g = new Complex[size, size];

            double const1 = 1 / lambda;
            double const2 = -Math.PI * 2 / lambda;
            double d2 = zDistance * zDistance;
            double dx2 = PixelDist * PixelDist;
            double N2 = size / 2;
            double xPart, yPart;

            int x, y;
            for (x = 0; x < size; x++)
            {
                xPart = d2 + (x - N2) * (x - N2) * dx2;
                for (y = 0; y < size; y++)
                {
                    cpx1[x, y] = source[x, y] * Utility.CpxConj(field[x, y]);

                    yPart = Math.Sqrt( (y - N2) * (y - N2) * dx2 + xPart);
                   

                    g[x, y].Re = 0;
                    g[x, y].Im = const1 / yPart;
                    g[x, y] *= Utility.Exp2Cpx(1, const2 * yPart);
                }
            }

            FourierTransform.FFT2(cpx1, FourierTransform.Direction.Forward);
            FourierTransform.FFT2(g, FourierTransform.Direction.Forward);

            for (x = 0; x < size; x++)
            {
                xPart = d2 + (x - N2) * (x - N2) * dx2;
                for (y = 0; y < size; y++)
                {
                    cpx1[x, y] *= g[x, y];

                   
                }
            }

            FourierTransform.FFT2(cpx1, FourierTransform.Direction.Backward);
            cpx1 = Utility.Shift2D(cpx1);

            return new APImage(cpx1);

        }
            
    }

    public class ReferenceBeam
    {
        public static Complex[,] GenerateFlatField(double pixelDist, int size, double amplitude, double lambda, double alpha, double phase = 0)
        {
            int x, y;
            Complex[,] field = new Complex[size, size];
            double A, tmp1, phi;
            Complex ctmp;
            tmp1 =  pixelDist * Math.Sin(alpha/360*Math.PI) * Math.PI * 2 / lambda;
            for (x = 0; x < size; x++)
            {
                phi = x * tmp1 + phase;
                A = amplitude;
                ctmp = Utility.Exp2Cpx(A, phi);
                
                for (y = 0; y < size; y++)
                {
                    field[x,y] = ctmp;
                }
            }
            return field;
        }

    }


    class ReferenceBeam_old
    {
        protected double l, a;

        public ReferenceBeam_old(double lambda, double a)
        {
            this.l = lambda;
            this.a = a;
        }
        public Complex[,] GenerateField(double pixelDist, int width, int height, double angle)
        {
            Complex[,] field = new Complex[width, height];
            int i, j;
            double c = - 2 * Math.PI / l;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    field[i, j].Re = a * Math.Cos(c*Math.Sin(angle));
                    field[i, j].Im = a * Math.Sin(c * Math.Sin(angle));
                }
            }
            return field;
        }
        public Complex[,] GenerateField(double zDist, double pixelDist, int width, int height, double angle)
        {
            Complex[,] field = new Complex[width, height];
            int i, j;
            double c = -2 * Math.PI / l;
            double dsqrt;
            double d2 = zDist*zDist;
            double d1;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    dsqrt = i * i + j * j + d2;
                    dsqrt = Math.Sqrt(dsqrt);
                    d1 = a / dsqrt;

                    field[i, j].Re = d1 * Math.Cos(c * dsqrt);
                    field[i, j].Im = d1 * Math.Sin(c * dsqrt);
                }
            }
            return field;
        }
        public Complex[,] GenerateSphericField(double d, double lambda, double pixelDist, int width, int height)
        {
            int x, y;
            double tmp1,tx,ty, d2= d*d;
            double const1 =  -2 * Math.PI / lambda;
            Complex[,] field = new Complex[width, height];
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    tx = x * pixelDist;
                    ty = y * pixelDist;
                    tmp1 = tx * tx + ty * ty + d2;
                    tmp1 = Math.Sqrt(tmp1);
                    field[x, y] = Utility.Exp2Cpx(1 / tmp1, const1 * tmp1);
                }
            }
            return field;
        }
        public static Complex[,] GenerateFlatField(int w, int h, double ampl, double lambda, double a)
        {
            Complex[,] field = new Complex[w, h];
            int x, y;
            double tmp = -2*Math.PI/lambda*Math.Sin(a);
            Complex ctmp = Utility.Exp2Cpx(ampl, tmp);
            for (x = 0; x < w; x++)
            {
                for (y = 0; y < h; y++)
                {
                    field[x,y] = ctmp;
                }
            }
            return field;
        }
    }

    class HologrammParametres
    {
        [DisplayName("Длина волны")]
        [Description("Длина волны опорного пучка, нм")]
        [Category("Опорный пучок")]
        public double WaveLength { get; set; }

        [DisplayName("Угол")]
        [Description("Угол наклона опорного пучка")]
        [Category("Опорный пучок")]
        public double Angle { get; set; }

        [DisplayName("Амплитуда")]
        [Description("Амплитуда опорного пучка")]
        [Category("Опорный пучок")]
        public double Amplitude { get; set; }

        [DisplayName("Расстояние")]
        [Description("Расстояние между голограммой и объектной плоскостью, мм")]
        [Category("Голограмма")]
        public double Distance { get; set; }


        [DisplayName("Размер пикселей")]
        [Description("Размер пикселей объектной плоскости, мкм")]
        [Category("Голограмма")]
        public double CCDPixelDistance { get; set; }

        [Browsable(false)]
        public double RefBeamPhase { get; set; }

        public HologrammParametres()
        {
            WaveLength = 628;
            Angle = 1;
            Amplitude = 1;
            Distance = 100;
            CCDPixelDistance = 10;
            RefBeamPhase = 0;
        }


    }

    class Freshnel
    {

        public static APImage Construct(APImage img, int sizeInPixel, double waveLength, double angle, double amplitude, double zDistance, double ccdPixelDistance, double dstDistance)
        {
            Complex[,] src = img.GetComplex();
            
            int x, y;
            int width = src.GetLength(0), height = src.GetLength(1);
            double tx, ty;
            double dtx, dty;
            dtx = -width * dstDistance / 2;
            dty = -height * dstDistance / 2;
            double tmp1;
            //Вначале преобразование фурье...
            Complex[,] cpx1 = new Complex[width, height];
            cpx1 = src;
            tmp1 = -Math.PI/( zDistance * waveLength);
            for (x = 0; x < width; x++)
            {
                tx = dstDistance * x + dtx;
                for (y = 0; y < height; y++)
                {
                    ty = dstDistance * y + dty;
                    cpx1[x, y] = src[x,y] * Utility.Exp2Cpx(1, tmp1 * (tx * tx + ty * ty));
                }
            }
            FourierTransform.FFT2(cpx1, FourierTransform.Direction.Forward);
            cpx1 = Utility.Shift2D(cpx1);

            
            Complex ctmp1;

            //Генерация опорного луча!
            Complex[,] field = ReferenceBeam.GenerateFlatField(ccdPixelDistance, height, amplitude, waveLength, angle);
            return new APImage(field);
            ctmp1.Im = 1 / (zDistance * waveLength);
            ctmp1.Re = 0;

           // ctmp1 = ctmp1 * Utility.Exp2Cpx(1, -2 * Math.PI * zDistance / waveLength);
            double tmp2 = -2 * Math.PI * zDistance / waveLength;
            Complex ctmp2 = Utility.Exp2Cpx(1, tmp2);
            ctmp1 = ctmp1 * ctmp2;

            tmp1 = -Math.PI * waveLength * zDistance;
            double dv = 1 / (ccdPixelDistance * width), dm = 1 / (ccdPixelDistance * height);
            double mdv2, ndm2;
            dtx = -width * ccdPixelDistance / 2;
            dty = -height * ccdPixelDistance / 2;
            int m, n;
            for (m = 0; m < width; m++)
            {
                mdv2 = m * dv +dtx;
                mdv2 = mdv2 * mdv2;
                for (n = 0; n < height; n++)
                {
                    ndm2 = n * dm +dty;
                    ndm2 = ndm2 * ndm2;

                    cpx1[m, n] = cpx1[m, n] * ctmp1 * Utility.Exp2Cpx(1, tmp1 * (mdv2 + ndm2));
                    cpx1[m, n] = cpx1[m, n] + field[m, n];
                }
            }
            
            //Да?
            return new APImage(cpx1);

        }
        
        
        public static APImage Reconstruct(Complex[,] src, int sizeInPixel, double waveLength, double angle, double amplitude, double zDistance, double ccdPixelDistance)
        {
            int x, y;
            int width = src.GetLength(0), height = src.GetLength(1);
            double tx, ty;
            double dth = -sizeInPixel * ccdPixelDistance / 2;
            double tmp1;
            Complex[,] cpx1 = new Complex[width, height];
            Complex[,] result = new Complex[width, height];
            //Генерация опорного луча!
            Complex[,] field = ReferenceBeam.GenerateFlatField(ccdPixelDistance, height, amplitude, waveLength, angle);


            tmp1 = -Math.PI / (waveLength * zDistance);
            double kdx2, ldy2;
            int k, l;
            for (k = 0; k < width; k++)
            {
                kdx2 = k * ccdPixelDistance;
                kdx2 = kdx2 * kdx2;
                for (l = 0; l < height; l++)
                {
                    ldy2 = l * ccdPixelDistance;
                    ldy2 = ldy2 * ldy2;

                    cpx1[k, l] = src[k, l] * Utility.Exp2Cpx(1, tmp1 * (kdx2 + ldy2));
                }
            }

            FourierTransform.FFT2(cpx1, FourierTransform.Direction.Forward);
            Utility.Shift2D(cpx1);

            int m, n;

            Complex ctmp1;
            ctmp1.Im = 1 / (waveLength * zDistance);
            ctmp1.Re = 0;
            ctmp1 = ctmp1 * Utility.Exp2Cpx(1, -2 * Math.PI * zDistance / waveLength);
            tmp1 = -Math.PI * waveLength * zDistance;
            double tmp2 = width * ccdPixelDistance;
            double m2, n2;
            for (m = 0; m < width; m++)
            {
                m2 = m / tmp2;
                m2 = m2 * m2;
                for (n = 0; n < height; n++)
                {
                    n2 = n / tmp2;
                    n2 = n2 * n2;
                    result[m, n] = cpx1[m, n] * ctmp1 * Utility.Exp2Cpx(1, tmp1 * (m2 + n2));
                }
            }

            return new APImage(result);

        }
        public static APImage Reconstruct2(double[,] hologram, int sizeInPixel, double waveLength, double angle, double amp, double zDistance, double ccdPixelDistance, bool filter)
        {
            double ccdLen = (sizeInPixel - 1) * ccdPixelDistance;
            angle = angle / 180 * Math.PI;

            Complex[,] h = new Complex[sizeInPixel, sizeInPixel];

            double[,] hologram2 = new double[sizeInPixel, sizeInPixel];
            double[,] amplitude = new double[sizeInPixel, sizeInPixel];
            double[,] phase = new double[sizeInPixel, sizeInPixel];

            //Фильтрация
            
            if (filter)
                hologram2 = Utility.Laplasian(hologram);
            else
                hologram2 = hologram;

            //Опорный луч
            //ReferenceBeam refBeam = new ReferenceBeam(waveLength, Math.Sqrt(avg2));
            //h = refBeam.GenerateField(ccdPixelDistance, sizeInPixel, sizeInPixel, angle);
            h = ReferenceBeam_old.GenerateFlatField(sizeInPixel, sizeInPixel, amp, waveLength, angle);
            //Выражение 2.29?
            
            double factor1 = -Math.PI / (waveLength * zDistance);   // -pi/(lambda*d)
            double half_ccdlen_inv = -0.5 * ccdLen;
            double ccdPx, Xeuclid, ccdPy, Yeuclid, var1_real, var1_imag, euclid, ref_real, ref_imag;
            int x, y;

            for (x = 0; x < sizeInPixel; x++)
            {
                ccdPx = half_ccdlen_inv + x * ccdPixelDistance;
                Xeuclid = ccdPx * ccdPx;
                for (y = 0; y < sizeInPixel; y++)
                {
                    ccdPy = half_ccdlen_inv + y * ccdPixelDistance;
                    Yeuclid = ccdPy * ccdPy;

                    euclid = Xeuclid + Yeuclid;
                    var1_real = Math.Cos(factor1 * euclid);
                    var1_imag = Math.Sin(factor1 * euclid);
                    ref_real = h[x, y].Re;
                    ref_imag = h[x, y].Im;
                    h[x, y].Re = hologram2[x, y] * (ref_real * var1_real - ref_imag * var1_imag);
                    h[x, y].Im = hologram2[x, y] * (ref_real * var1_imag + ref_imag * var1_real);
                }
            }

            FourierTransform.FFT2(h, FourierTransform.Direction.Forward);
            h = Utility.Shift2D(h);

            double factor2 = -Math.PI * waveLength * zDistance;
            double wavenum = Math.PI * 2 / waveLength;
            double factor3 = -wavenum * zDistance;
            double factor4 = 1 / (waveLength * zDistance);
            double Xdist, Ydist, dist, var4_real, var4_imag, ccd_real, ccd_imag;
            for (x = 0; x < sizeInPixel; x++)
            {
                Xdist = ((x - (sizeInPixel - 1) / 2) / (ccdLen + ccdPixelDistance));
                Xdist *= Xdist;
                for (y = 0; y < sizeInPixel; y++)
                {
                    Ydist = ((y - (sizeInPixel - 1) / 2) / (ccdLen + ccdPixelDistance));
                    Ydist *= Ydist;
                    dist = Xdist + Ydist;
                    var1_real = Math.Cos(factor2 * dist);
                    var1_imag = Math.Sin(factor2 * dist);
                    var4_real = (Math.Sin(factor3) * var1_real - Math.Cos(factor3) * var1_imag) * factor4;
                    var4_imag = (Math.Sin(factor3) * var1_imag + Math.Cos(factor3) * var1_real) * factor4;
                    ccd_real = var4_real * h[x,y].Re - var4_imag * h[x,y].Im;
                    ccd_imag = var4_real * h[x,y].Im + var4_imag * h[x,y].Re;
                    amplitude[x,y] = ccd_real * ccd_real + ccd_imag * ccd_imag;
                    phase[x,y] = Math.Atan2(ccd_imag, ccd_real);
                }
            }

            return new APImage(amplitude, phase);

        }
    }
}
