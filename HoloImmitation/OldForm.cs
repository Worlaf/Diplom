using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HoloImmitation
{
    public partial class OldForm : Form
    {
        APImage source;
        APImage result;



        public OldForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // pictureBox1.Image = APImage.MakeGrayscale3(Properties.Resources.IMG_Win_Logo);
            source = new APImage(Properties.Resources.IMG_DUCK);
            cmbSource.SelectedIndex = 0;
            pictureBox1.Image = source.GetAmplitudeImage(chAmplify1.Checked);
            pictureBox1.Refresh();
            
        }

       

       

        private void button2_Click(object sender, EventArgs e)
        {           
            Cursor = Cursors.WaitCursor;

            double distance = (double)nudZDistance.Value / 1000.0;
            double lambda = (double)nudLambda.Value / 1000000000.0;

            result = Freshnel.Construct(source, source.Origin.Width, lambda, 3, 1, distance, 0.00001, 0.00001);           
            
            

            Cursor = Cursors.Default;
            pictureBox2.Image = result.GetAmplitudeImage(chAmplify2.Checked);          

            
            pictureBox2.Refresh();

            double[] minmax = result.GetAmpliduteBounds();
            lblInfo.Text = minmax[0].ToString() + " : " + minmax[1].ToString();
        }

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSource.SelectedIndex == 2 && result == null)
            {
                return;
            }

            if (cmbSource.SelectedIndex == 0)
            {
                source = new APImage(Properties.Resources.IMG_DUCK);
            }
            if (cmbSource.SelectedIndex == 1)
            {
                source = new APImage(Properties.Resources.IMG_Win_Logo);
            }
            if (cmbSource.SelectedIndex == 2)
            {
                source = result;
            }
            pictureBox1.Image = source.GetAmplitudeImage(chAmplify1.Checked);
            pictureBox1.Refresh();
        }

        private void chAmplify1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = source.GetAmplitudeImage(chAmplify1.Checked);
            pictureBox1.Refresh();
        }

        private void chAmplify2_CheckedChanged(object sender, EventArgs e)
        {
            if (result != null)
            {
                pictureBox2.Image = result.GetAmplitudeImage(chAmplify2.Checked);
                pictureBox2.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            double distance = (double)nudZDistance.Value / 1000.0;
            double lambda = (double)nudLambda.Value / 1000000000.0;

            //result = Freshnel.Reconstruct(source.GetComplex(), source.Origin.Width, lambda, 3, distance, 0.00001);
            result = Freshnel.Reconstruct2(source.GetAmplitudes(), source.Origin.Width, lambda, 3, 1, distance, 0.00001, chReconstructFilter.Checked);


            Cursor = Cursors.Default;
            pictureBox2.Image = result.GetAmplitudeImage(chAmplify2.Checked);


            pictureBox2.Refresh();

            double[] minmax = result.GetAmpliduteBounds();
            lblInfo.Text = minmax[0].ToString() + " : " + minmax[1].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            double distance = (double)nudZDistance.Value / 1000.0;
            double lambda = (double)nudLambda.Value / 1000000000.0;

            result = Freshnel.Reconstruct(source.GetComplex(), source.Origin.Width, lambda, 3, 1, distance, 0.00001);            


            Cursor = Cursors.Default;
            pictureBox2.Image = result.GetAmplitudeImage(chAmplify2.Checked);


            pictureBox2.Refresh();

            double[] minmax = result.GetAmpliduteBounds();
            lblInfo.Text = minmax[0].ToString() + " : " + minmax[1].ToString();

        }

        private void trackMaxAmplitude_Scroll(object sender, EventArgs e)
        {

        }

        private void trackMaxAmplitude_ValueChanged(object sender, EventArgs e)
        {
            if (result != null)
            {
                double pcnt = (double)trackMaxAmplitude.Value / 100.0;
                pictureBox2.Image = result.GetAmplitudeImage(pcnt, chAmplify2.Checked);
                pictureBox2.Refresh();
            }
        }
    }
}
