namespace HoloImmitation
{
    partial class OldForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSource = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chAmplify1 = new System.Windows.Forms.CheckBox();
            this.chAmplify2 = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudZDistance = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudLambda = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.chReconstructFilter = new System.Windows.Forms.CheckBox();
            this.trackMaxAmplitude = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLambda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackMaxAmplitude)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 164);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(530, 164);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(512, 512);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(256, 37);
            this.button2.TabIndex = 4;
            this.button2.Text = "Создать голограмму";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Исходник";
            // 
            // cmbSource
            // 
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Items.AddRange(new object[] {
            "Утка",
            "Логотип Windows",
            "Предыдущий результат"});
            this.cmbSource.Location = new System.Drawing.Point(18, 28);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(256, 21);
            this.cmbSource.TabIndex = 6;
            this.cmbSource.SelectedIndexChanged += new System.EventHandler(this.cmbSource_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(527, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Результат";
            // 
            // chAmplify1
            // 
            this.chAmplify1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chAmplify1.AutoSize = true;
            this.chAmplify1.Checked = true;
            this.chAmplify1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chAmplify1.Location = new System.Drawing.Point(12, 141);
            this.chAmplify1.Name = "chAmplify1";
            this.chAmplify1.Size = new System.Drawing.Size(141, 17);
            this.chAmplify1.TabIndex = 8;
            this.chAmplify1.Text = "Выровнять амплитуды";
            this.chAmplify1.UseVisualStyleBackColor = true;
            this.chAmplify1.CheckedChanged += new System.EventHandler(this.chAmplify1_CheckedChanged);
            // 
            // chAmplify2
            // 
            this.chAmplify2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chAmplify2.AutoSize = true;
            this.chAmplify2.Checked = true;
            this.chAmplify2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chAmplify2.Location = new System.Drawing.Point(530, 141);
            this.chAmplify2.Name = "chAmplify2";
            this.chAmplify2.Size = new System.Drawing.Size(141, 17);
            this.chAmplify2.TabIndex = 9;
            this.chAmplify2.Text = "Выровнять амплитуды";
            this.chAmplify2.UseVisualStyleBackColor = true;
            this.chAmplify2.CheckedChanged += new System.EventHandler(this.chAmplify2_CheckedChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(527, 115);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(30, 13);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "[info]";
            this.lblInfo.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Z Distance, мм";
            // 
            // nudZDistance
            // 
            this.nudZDistance.Location = new System.Drawing.Point(294, 47);
            this.nudZDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudZDistance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZDistance.Name = "nudZDistance";
            this.nudZDistance.Size = new System.Drawing.Size(104, 20);
            this.nudZDistance.TabIndex = 12;
            this.nudZDistance.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(291, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Длина волны, нм";
            // 
            // nudLambda
            // 
            this.nudLambda.Location = new System.Drawing.Point(294, 86);
            this.nudLambda.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.nudLambda.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudLambda.Name = "nudLambda";
            this.nudLambda.Size = new System.Drawing.Size(104, 20);
            this.nudLambda.TabIndex = 14;
            this.nudLambda.Value = new decimal(new int[] {
            628,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(256, 37);
            this.button1.TabIndex = 15;
            this.button1.Text = "Восстановить голограмму";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chReconstructFilter
            // 
            this.chReconstructFilter.AutoSize = true;
            this.chReconstructFilter.Location = new System.Drawing.Point(159, 141);
            this.chReconstructFilter.Name = "chReconstructFilter";
            this.chReconstructFilter.Size = new System.Drawing.Size(173, 17);
            this.chReconstructFilter.TabIndex = 17;
            this.chReconstructFilter.Text = "Фильтр при восстановлении";
            this.chReconstructFilter.UseVisualStyleBackColor = true;
            // 
            // trackMaxAmplitude
            // 
            this.trackMaxAmplitude.Location = new System.Drawing.Point(712, 113);
            this.trackMaxAmplitude.Maximum = 100;
            this.trackMaxAmplitude.Name = "trackMaxAmplitude";
            this.trackMaxAmplitude.Size = new System.Drawing.Size(330, 45);
            this.trackMaxAmplitude.TabIndex = 18;
            this.trackMaxAmplitude.Scroll += new System.EventHandler(this.trackMaxAmplitude_Scroll);
            this.trackMaxAmplitude.ValueChanged += new System.EventHandler(this.trackMaxAmplitude_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 688);
            this.Controls.Add(this.trackMaxAmplitude);
            this.Controls.Add(this.chReconstructFilter);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nudLambda);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudZDistance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.chAmplify2);
            this.Controls.Add(this.chAmplify1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Моделирование создания и расшифровки голограммы";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLambda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackMaxAmplitude)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chAmplify1;
        private System.Windows.Forms.CheckBox chAmplify2;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudZDistance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudLambda;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chReconstructFilter;
        private System.Windows.Forms.TrackBar trackMaxAmplitude;
    }
}

