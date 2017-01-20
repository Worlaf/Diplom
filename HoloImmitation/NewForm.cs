using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Math;
using AForge.Imaging;

namespace HoloImmitation
{
    public partial class NewForm : Form
    {
        APImage Source;
        APImage[] Hologramm;
        int holoCount;
        APImage Reconstructed;
        APImage LoadedImage;

        PicBoxContainer picGenSrc;
        PicBoxContainer picGenHolo;
        PicBoxContainer picRecHolo;
        PicBoxContainer picRecReconstructed;
        PicBoxContainer picLoaded;

        LoadImageProperties loadImageProperties;
        HologrammParametres constructParams;
        HologrammParametres reconstructParams;


        public NewForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            picGenSrc = new PicBoxContainer(picHoloGen_Source, lblHoloGen_SrcInfo);
            picGenHolo = new PicBoxContainer(picHoloGen_HoloView, lblHoloGen_HoloView_Info);
            picRecHolo = new PicBoxContainer(picHoloRec_Hologramm, lblHoloRec_Hologramm_Info);
            picRecReconstructed = new PicBoxContainer(picHoloRec_Reconstructed, lblHoloRec_Reconstructed_Info);
            picLoaded = new PicBoxContainer(picImgLoad_Preview, lblImgLoad_PreviewInfo);
            openFileDialog1.InitialDirectory = Application.StartupPath;
            loadImageProperties = new LoadImageProperties();
            pg_LoadImageProperties.SelectedObject = loadImageProperties;
            cbImgLoad_size.SelectedIndex = 1 ;
            LoadedImage = new APImage(1024);
            RefreshImgLoad_PreviewImage();
            constructParams = new HologrammParametres();
            reconstructParams = new HologrammParametres();
            pg_Construct.SelectedObject = constructParams;
            pg_Reconstruct.SelectedObject = reconstructParams;


            Hologramm = new APImage[3];
            Hologramm[0] = new APImage(new Bitmap(Application.StartupPath + "\\images\\i0.png"), true);
            Hologramm[1] = new APImage(new Bitmap(Application.StartupPath + "\\images\\i1.png"), true);
            Hologramm[2] = new APImage(new Bitmap(Application.StartupPath + "\\images\\i2.png"), true);

            nudHoloRec_Hologramm_SeriesSelect.Maximum = 3;
            nudHoloGen_SeriesSelect.Maximum = 3;
            holoCount = 3;

            RefreshHoloGen_Hologramm();
            RefreshHoloRec_Hologramm();
            
        }

        private void btnHoloGen_OpenImage_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (openFileDialog1.FileName == null || openFileDialog1.FileName == "")
            {
                return;
            }

            string file = openFileDialog1.FileName;
            try
            {
                Bitmap bmp = new Bitmap(file);
                if (bmp.Width % 256 > 0 || bmp.Height % 256 > 0)
                {
                    MessageBox.Show("Изображение имеет недопустимые размеры.", "Предупреждение", MessageBoxButtons.OK);
                }
                Source = new APImage(bmp);
                RefreshHoloGen_SourceImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка:\n" + ex.Message, "Ошибка");
            }
        }

        private void RefreshImgLoad_PreviewImage()
        {
            Cursor = Cursors.WaitCursor;
            if (LoadedImage == null)
                return;
            if (rbImgLoad_PreviewAmplitude.Checked)
            {
                picLoaded.SetImage(Utility.MakeBitmap(LoadedImage.GetAmplitudes(1, true)));
            }
            else
            {
                picLoaded.SetImage(Utility.MakeBitmap(LoadedImage.GetPhases(1, true)));
            }
            Cursor = Cursors.Default;
        }

        private void RefreshHoloGen_SourceImage()
        {
            Cursor = Cursors.WaitCursor;
            if (Source == null)
                return;
            if (rbHoloGen_SrcView_Amplitude.Checked)
            {
                picGenSrc.SetImage(Utility.MakeBitmap(Source.GetAmplitudes(1, true)));
            }
            else
            {
                picGenSrc.SetImage(Utility.MakeBitmap(Source.GetPhases(1,true)));
            }
            Cursor = Cursors.Default;
        }

        private void RefreshHoloGen_Hologramm()
        {
            Cursor = Cursors.WaitCursor;
            
            if (Hologramm == null)
                return;
            double amp = tbHoloGen_CutAmplitude.Value / 100.0;
            if (rbHoloGen_HoloView_Amplitude.Checked)
            {
                picGenHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloGen_SeriesSelect.Value - 1].GetAmplitudes(amp, true)));
            }
            else if (rbHoloGen_HoloView_Phase.Checked)
            {
                //Доработать
                picGenHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloGen_SeriesSelect.Value - 1].GetPhases(amp, true)));
            }
            else if (rbHoloGen_HoloView_Imag.Checked)
            {
                picGenHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloGen_SeriesSelect.Value - 1].GetImag(amp, true)));
            }
            else if (rbHoloGen_HoloView_Real.Checked)
            {
                picGenHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloGen_SeriesSelect.Value - 1].GetReal(amp, true)));
            }
            Cursor = Cursors.Default;
        }

        private void RefreshHoloRec_Hologramm()
        {
            Cursor = Cursors.WaitCursor;
            if (Hologramm == null)
                return;
            double amp = tbHoloRec_Hologramm_CutAmpl.Value / 100.0;
            if (rbHoloRec_Hologramm_Amplitude.Checked)
            {

                picRecHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetAmplitudes(amp, true)));
            }
            else if (rbHoloRec_Hologramm_Phase.Checked)
            {

                picRecHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetPhases(amp, true)));
            }
            else if (rbHoloRec_Hologramm_Imag.Checked)
            {
                picRecHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetImag(amp, true)));
            }
            else if (rbHoloRec_Hologramm_Real.Checked)
            {
                picRecHolo.SetImage(Utility.MakeBitmap(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetReal(amp, true)));
            }
            Cursor = Cursors.Default;
        }

        private void RefreshHoloRec_Reconstructed()
        {
            Cursor = Cursors.WaitCursor;
            if (Reconstructed == null)
                return;
            double amp = tbHoloRec_Reconstructed_CutAmpl.Value / 100.0;
            if (rbHoloRec_Reconstructed_Amplitude.Checked)
            {
                picRecReconstructed.SetImage(Utility.MakeBitmap(Reconstructed.GetAmplitudes(amp, true)));
            }
            else if (rbHoloRec_Reconstructed_Phase.Checked)
            {

                picRecReconstructed.SetImage(Utility.MakeBitmap(Reconstructed.GetPhases(amp, true)));
            }
            else if (rbHoloRec_Reconstructed_Imag.Checked)
            {
                picRecReconstructed.SetImage(Utility.MakeBitmap(Reconstructed.GetImag(amp, true)));
            }
            else if (rbHoloRec_Reconstructed_Real.Checked)
            {
                picRecReconstructed.SetImage(Utility.MakeBitmap(Reconstructed.GetReal(amp, true)));
            }
            Cursor = Cursors.Default;
        }

        private void rbHoloGen_SrcView_Amplitude_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_SourceImage();
        }

        private void rbHoloGen_SrcView_Phase_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_SourceImage();
        }

        private void btnHoloGen_SetValue_Click(object sender, EventArgs e)
        {
            if (Source == null)
                return;
            double value = (double)nudHoloGen_SetValue.Value;
            Source.FillAmplitudes(value);
            RefreshHoloGen_SourceImage();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (Source == null)
                return;

            Cursor = Cursors.WaitCursor;
            double dist, angle, lambda, amplitude;

            dist = (double)nudHoloGen_zDist_mm.Value / 1000.0 + (double)nudHoloGen_zDist_mkm.Value / 1000000.0;
            lambda = (double)nudHoloGen_Lambda.Value / 1000000000.0;
            angle = (double)nudHoloGen_Angle.Value / 100;
            amplitude = (double)nudHoloGen_WaveAmplitude.Value;
            if (chHoloGen_MakeMuch.Checked)
            {
                Hologramm = new APImage[4];
                /*Hologramm[0] = HologrammGenerator.Construct(Source, lambda, angle, amplitude, dist, 0.00001, 0.00001, chHoloGen_IntensityOnly.Checked);
                Hologramm[1] = HologrammGenerator.Construct(Source, lambda, angle, amplitude, dist + lambda / 4, 0.00001, 0.00001, chHoloGen_IntensityOnly.Checked);
                Hologramm[2] = HologrammGenerator.Construct(Source, lambda, angle, amplitude, dist + lambda / 2, 0.00001, 0.00001, chHoloGen_IntensityOnly.Checked);*/
                constructParams.RefBeamPhase = 0;
                Hologramm[0] = HologrammGenerator.Construct(Source, constructParams, chHoloGen_IntensityOnly.Checked);
                constructParams.RefBeamPhase = Math.PI / 2;
                Hologramm[1] = HologrammGenerator.Construct(Source, constructParams, chHoloGen_IntensityOnly.Checked);
                constructParams.RefBeamPhase = Math.PI;
                Hologramm[2] = HologrammGenerator.Construct(Source, constructParams, chHoloGen_IntensityOnly.Checked);
                constructParams.RefBeamPhase = 3*Math.PI/2;
                Hologramm[3] = HologrammGenerator.Construct(Source, constructParams, chHoloGen_IntensityOnly.Checked);
                constructParams.RefBeamPhase = 0;

                nudHoloRec_Hologramm_SeriesSelect.Maximum = 4;
                nudHoloGen_SeriesSelect.Maximum = 4;
                holoCount = 4;
            }
            else
            {
                Hologramm = new APImage[1];
                //Hologramm[0] = HologrammGenerator.Construct(Source, lambda, angle, amplitude, dist, 0.00001, 0.00001, chHoloGen_IntensityOnly.Checked);
                Hologramm[0] = HologrammGenerator.Construct(Source, constructParams, chHoloGen_IntensityOnly.Checked);
                holoCount = 1;
                nudHoloRec_Hologramm_SeriesSelect.Maximum = 1;
                nudHoloGen_SeriesSelect.Maximum = 1;

            }
            

           // Hologramm = Freshnel.Construct(Source, Source.Origin.Width, lambda, angle, amplitude, dist, 0.00001, 0.00001);

            Utility.MakeBitmap(Hologramm[0].GetAmplitudes(true)).Save("images\\holo.bmp");
            RefreshHoloGen_Hologramm();
            RefreshHoloRec_Hologramm();
            Cursor = Cursors.Default;
        }

        private void tbHoloGen_CutAmplitude_Scroll(object sender, EventArgs e)
        {
            RefreshHoloGen_Hologramm();
        }

        private void btnHoloGen_SourceSwap_Click(object sender, EventArgs e)
        {
            if (Source == null)
                return;
            Source.SwapAmplitudePhase();
            RefreshHoloGen_SourceImage();
        }

        private void rbHoloGen_HoloView_Phase_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_Hologramm();
        }

        private void rbHoloGen_HoloView_Amplitude_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_Hologramm();
        }

        private void rbHoloGen_HoloView_Real_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_Hologramm();
        }

        private void rbHoloGen_HoloView_Imag_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_Hologramm();
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbHoloRec_Hologramm_Phase_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Hologramm();
        }

        private void rbHoloRec_Hologramm_Amplitude_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Hologramm();
        }

        private void rbHoloRec_Hologramm_Real_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Hologramm();
        }

        private void rbHoloRec_Hologramm_Imag_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Hologramm();
        }

        private void tbHoloRec_Hologramm_CutAmpl_Scroll(object sender, EventArgs e)
        {
            RefreshHoloRec_Hologramm();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnHoloRec_CopyRecOptions_Click(object sender, EventArgs e)
        {
            nudHoloRec_Amplitude.Value = nudHoloGen_WaveAmplitude.Value;
            nudHoloRec_Angle.Value = nudHoloGen_Angle.Value;
            nudHoloRec_Lambda.Value = nudHoloGen_Lambda.Value;
            nudHoloRec_Distance_mm.Value = nudHoloGen_zDist_mm.Value;
            nudHoloRec_Distance_mkm.Value = nudHoloGen_zDist_mkm.Value;
            rbHoloRec_FlatFront.Checked = rbHoloGen_FlatFront.Checked;
            rbHoloRec_SphericFront.Checked = rbHoloGen_SphericFront.Checked;
        }

        private void btnHoloRec_Reconstruct_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            double distance = (double)nudHoloRec_Distance_mm.Value / 1000 + (double)nudHoloRec_Distance_mkm.Value / 1000000;
            double angle = (double)nudHoloRec_Angle.Value / 1000 + (double)nudHoloRec_AngleP.Value / 100000;
            double amplitude = (double)nudHoloRec_Amplitude.Value;
            double lambda = (double)nudHoloRec_Lambda.Value / 1000000000;


            if (rbHoloRec_GetAmplitudeOnly.Checked)
            {
                Complex[,] field;
                if (rbHoloRec_Filter_PhaseShift.Checked && holoCount > 1)
                {
                   
                   
                    //3сдвига
                    if (!chHoloRec_Filter_PhaseShift_Inv.Checked)
                    {
                        field = Utility.PhaseShiftFilter(amplitude, Hologramm[0].GetAmplitudes(true), Hologramm[1].GetAmplitudes(true), Hologramm[2].GetAmplitudes(true));
                    }
                    else
                    {
                        field = Utility.PhaseShiftFilter(amplitude, Hologramm[2].GetAmplitudes(true), Hologramm[1].GetAmplitudes(true), Hologramm[0].GetAmplitudes(true));
                    }
                    Reconstructed = HologrammGenerator.Reconstruct(field, reconstructParams, rbHoloRec_Filter_Laplase.Checked, chHoloRec_NoRefBeam.Checked, chHoloRec_OnlyRefBeam.Checked);
                   
                   
                }
                else
                {
                    field = Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetComplex();
                    //Reconstructed = HologrammGenerator.Reconstruct(amplitudes, lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked, chHoloRec_NoRefBeam.Checked, chHoloRec_OnlyRefBeam.Checked);
                    Reconstructed = HologrammGenerator.Reconstruct(field, reconstructParams, rbHoloRec_Filter_Laplase.Checked, chHoloRec_NoRefBeam.Checked, chHoloRec_OnlyRefBeam.Checked);

                }
                
          
            }
            else
            {


                //Reconstructed = HologrammGenerator.Reconstruct(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetPhases(1, true), lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked, chHoloRec_NoRefBeam.Checked, chHoloRec_OnlyRefBeam.Checked);
               // Reconstructed = HologrammGenerator.Reconstruct(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetPhases(1, true), reconstructParams, rbHoloRec_Filter_Laplase.Checked, chHoloRec_NoRefBeam.Checked, chHoloRec_OnlyRefBeam.Checked);

                //Reconstructed = Freshnel.Reconstruct2(Hologramm.GetPhases(1, true), Hologramm.Origin.Width, lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked);
            }

            RefreshHoloRec_Reconstructed();
            Cursor = Cursors.Default;
        }

        private void rbHoloRec_Reconstructed_Phase_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Reconstructed();
        }

        private void rbHoloRec_Reconstructed_Amplitude_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Reconstructed();
        }

        private void rbHoloRec_Reconstructed_Real_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Reconstructed();
        }

        private void rbHoloRec_Reconstructed_Imag_CheckedChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Reconstructed();
        }

        private void tbHoloRec_Reconstructed_CutAmpl_Scroll(object sender, EventArgs e)
        {
            RefreshHoloRec_Reconstructed();
        }

        private void btnHoloRec_ReconstructConv_Click(object sender, EventArgs e)
        {
            double distance = (double)nudHoloRec_Distance_mm.Value / 1000 + (double)nudHoloRec_Distance_mkm.Value / 1000000;
            double angle = (double)nudHoloRec_Angle.Value / 100;
            double amplitude = (double)nudHoloRec_Amplitude.Value;
            double lambda = (double)nudHoloRec_Lambda.Value / 1000000000;


            if (rbHoloRec_GetAmplitudeOnly.Checked)
            {
                Reconstructed = HologrammGenerator.ReconstructConv(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetAmplitudes(true), lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked);
                //Reconstructed = Freshnel.Reconstruct2(Hologramm.GetAmplitudes(true), Hologramm.Origin.Width, lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked);
            }
            else
            {
                Reconstructed = HologrammGenerator.ReconstructConv(Hologramm[(int)nudHoloRec_Hologramm_SeriesSelect.Value - 1].GetPhases(1, true), lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked);
                //Reconstructed = Freshnel.Reconstruct2(Hologramm.GetPhases(1, true), Hologramm.Origin.Width, lambda, angle, amplitude, distance, 0.00001, rbHoloRec_Filter_Laplase.Checked);
            }

            RefreshHoloRec_Reconstructed();
        }

        private void nudHoloGen_SeriesSelect_ValueChanged(object sender, EventArgs e)
        {
            RefreshHoloGen_Hologramm();
        }

        private void nudHoloRec_Hologramm_SeriesSelect_ValueChanged(object sender, EventArgs e)
        {
            RefreshHoloRec_Hologramm();
        }

        private void btnHoloGen_MakeEmpty_Click(object sender, EventArgs e)
        {
            int size = 512;
            int x, y;
            double[,] values = new double[size, size];
            for (x = 0; x < size; x++)
            {
                for (y = 0; y < size; y++)
                {
                    values[x, y] = 1;
                }
            }
            Source = new APImage(values, values);
            RefreshHoloGen_SourceImage();
        }

        private void btnImgLoad_SetSize_Click(object sender, EventArgs e)
        {
            LoadedImage = new APImage(Convert.ToInt32((string)cbImgLoad_size.SelectedItem));
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_AmplitudeValut_Click(object sender, EventArgs e)
        {
            LoadedImage.FillAmplitudes((double)nudImgLoad_AmplitudeValue.Value);
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_PhaseValue_Click(object sender, EventArgs e)
        {
            LoadedImage.FillPhases((double)nudImgLoad_PhaseValue.Value);
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_AmplitudeRandom_Click(object sender, EventArgs e)
        {
            LoadedImage.RandomizeAmplitudes();
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_PhaseRandom_Click(object sender, EventArgs e)
        {
            LoadedImage.RandomizePhases();
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_AmplitudeOpen_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (openFileDialog1.FileName == null || openFileDialog1.FileName == "")
            {
                return;
            }

            string file = openFileDialog1.FileName;
            try
            {
                Bitmap bmp = new Bitmap(file);
                LoadedImage.LoadAmplitudeImage(bmp, loadImageProperties);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка:\n" + ex.Message, "Ошибка");
            }
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_PhaseOpen_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (openFileDialog1.FileName == null || openFileDialog1.FileName == "")
            {
                return;
            }

            string file = openFileDialog1.FileName;
            try
            {
                Bitmap bmp = new Bitmap(file);
                LoadedImage.LoadPhaseImage(bmp, loadImageProperties);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка:\n" + ex.Message, "Ошибка");
            }
            RefreshImgLoad_PreviewImage();
        }

        private void btnImgLoad_SetAsSource_Click(object sender, EventArgs e)
        {
            Source = LoadedImage;
            RefreshHoloGen_SourceImage();
        }

        private void btnImgLoaded_SetAsHologramm_Click(object sender, EventArgs e)
        {
            Hologramm = new APImage[1];
            Hologramm[0] = LoadedImage;
            RefreshHoloGen_Hologramm();
            RefreshHoloRec_Hologramm();
        }

        private void rbImgLoad_PreviewAmplitude_CheckedChanged(object sender, EventArgs e)
        {
            RefreshImgLoad_PreviewImage();
        }

        private void rbImgLoad_PreviewPhase_CheckedChanged(object sender, EventArgs e)
        {
            RefreshImgLoad_PreviewImage();
        }
    }
}
