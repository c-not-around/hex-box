namespace HexBoxControl
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            HexBoxControl.AnsiCharConvertor ansiCharConvertor1 = new HexBoxControl.AnsiCharConvertor();
            this.Mode = new System.Windows.Forms.ComboBox();
            this.AutoSize = new System.Windows.Forms.CheckBox();
            this.Enable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Columns = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.EncodingSelect = new System.Windows.Forms.ComboBox();
            this.DumpBox = new HexBoxControl.HexBox();
            this.OpenFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Columns)).BeginInit();
            this.SuspendLayout();
            // 
            // Mode
            // 
            this.Mode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Mode.FormattingEnabled = true;
            this.Mode.Location = new System.Drawing.Point(393, 28);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(112, 21);
            this.Mode.TabIndex = 1;
            this.Mode.SelectedIndexChanged += new System.EventHandler(this.ModeSelectedIndexChanged);
            // 
            // AutoSize
            // 
            this.AutoSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoSize.AutoSize = true;
            this.AutoSize.Location = new System.Drawing.Point(393, 55);
            this.AutoSize.Name = "AutoSize";
            this.AutoSize.Size = new System.Drawing.Size(68, 17);
            this.AutoSize.TabIndex = 2;
            this.AutoSize.Text = "AutoSize";
            this.AutoSize.UseVisualStyleBackColor = true;
            this.AutoSize.CheckedChanged += new System.EventHandler(this.AutoSizeCheckedChanged);
            // 
            // Enable
            // 
            this.Enable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Enable.AutoSize = true;
            this.Enable.Location = new System.Drawing.Point(393, 78);
            this.Enable.Name = "Enable";
            this.Enable.Size = new System.Drawing.Size(59, 17);
            this.Enable.TabIndex = 3;
            this.Enable.Text = "Enable";
            this.Enable.UseVisualStyleBackColor = true;
            this.Enable.CheckedChanged += new System.EventHandler(this.EnableCheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(393, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Columns:";
            // 
            // Columns
            // 
            this.Columns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Columns.Location = new System.Drawing.Point(442, 96);
            this.Columns.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.Columns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Columns.Name = "Columns";
            this.Columns.Size = new System.Drawing.Size(63, 20);
            this.Columns.TabIndex = 5;
            this.Columns.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Columns.ValueChanged += new System.EventHandler(this.ColumnsValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(393, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Encoding:";
            // 
            // EncodingSelect
            // 
            this.EncodingSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EncodingSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EncodingSelect.FormattingEnabled = true;
            this.EncodingSelect.Location = new System.Drawing.Point(393, 136);
            this.EncodingSelect.Name = "EncodingSelect";
            this.EncodingSelect.Size = new System.Drawing.Size(112, 21);
            this.EncodingSelect.TabIndex = 7;
            this.EncodingSelect.SelectedIndexChanged += new System.EventHandler(this.EncodingSelectSelectedIndexChanged);
            // 
            // DumpBox
            // 
            this.DumpBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DumpBox.BackColor = System.Drawing.Color.White;
            this.DumpBox.CharConverter = ansiCharConvertor1;
            this.DumpBox.Columns = 8;
            this.DumpBox.ColumnsAuto = false;
            this.DumpBox.Dump = null;
            this.DumpBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DumpBox.Location = new System.Drawing.Point(12, 12);
            this.DumpBox.Name = "DumpBox";
            this.DumpBox.Size = new System.Drawing.Size(375, 238);
            this.DumpBox.TabIndex = 0;
            this.DumpBox.Text = "hexBox1";
            this.DumpBox.ViewMode = HexBoxControl.HexBoxViewMode.BytesAscii;
            // 
            // OpenFile
            // 
            this.OpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenFile.Location = new System.Drawing.Point(393, 163);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(112, 23);
            this.OpenFile.TabIndex = 8;
            this.OpenFile.Text = "OpenFile";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFileClick);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(393, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "ViewMode:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 262);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OpenFile);
            this.Controls.Add(this.EncodingSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Columns);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Enable);
            this.Controls.Add(this.AutoSize);
            this.Controls.Add(this.Mode);
            this.Controls.Add(this.DumpBox);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "MainForm";
            this.Text = "HexBoxSample";
            ((System.ComponentModel.ISupportInitialize)(this.Columns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HexBox DumpBox;
        private System.Windows.Forms.ComboBox Mode;
        private System.Windows.Forms.CheckBox AutoSize;
        private System.Windows.Forms.CheckBox Enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Columns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox EncodingSelect;
        private System.Windows.Forms.Button OpenFile;
        private System.Windows.Forms.Label label3;
    }
}

