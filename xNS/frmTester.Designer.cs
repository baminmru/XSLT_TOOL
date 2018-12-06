namespace xNS
{
    partial class frmTester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.cmdSelectFile = new System.Windows.Forms.Button();
            this.txtXSD = new System.Windows.Forms.TextBox();
            this.txtXSLT = new System.Windows.Forms.TextBox();
            this.opf = new System.Windows.Forms.OpenFileDialog();
            this.opf2 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdXML = new System.Windows.Forms.Button();
            this.cmdRun = new System.Windows.Forms.Button();
            this.prcNum = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.prcNum)).BeginInit();
            this.SuspendLayout();
            // 
            // wb
            // 
            this.wb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wb.Location = new System.Drawing.Point(14, 246);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(971, 313);
            this.wb.TabIndex = 0;
            // 
            // cmdSelectFile
            // 
            this.cmdSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdSelectFile.Location = new System.Drawing.Point(946, 49);
            this.cmdSelectFile.Name = "cmdSelectFile";
            this.cmdSelectFile.Size = new System.Drawing.Size(39, 26);
            this.cmdSelectFile.TabIndex = 46;
            this.cmdSelectFile.Text = "...";
            this.cmdSelectFile.UseVisualStyleBackColor = true;
            this.cmdSelectFile.Click += new System.EventHandler(this.cmdSelectFile_Click);
            // 
            // txtXSD
            // 
            this.txtXSD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXSD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtXSD.Location = new System.Drawing.Point(123, 49);
            this.txtXSD.Name = "txtXSD";
            this.txtXSD.ReadOnly = true;
            this.txtXSD.Size = new System.Drawing.Size(817, 26);
            this.txtXSD.TabIndex = 45;
            // 
            // txtXSLT
            // 
            this.txtXSLT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXSLT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtXSLT.Location = new System.Drawing.Point(123, 12);
            this.txtXSLT.Name = "txtXSLT";
            this.txtXSLT.ReadOnly = true;
            this.txtXSLT.Size = new System.Drawing.Size(817, 26);
            this.txtXSLT.TabIndex = 43;
            // 
            // opf
            // 
            this.opf.DefaultExt = "xml";
            this.opf.FileName = "text.xml";
            this.opf.Filter = "XML files|*.xml|All files|*.*";
            this.opf.InitialDirectory = ".";
            // 
            // opf2
            // 
            this.opf2.DefaultExt = "xml";
            this.opf2.FileName = "text.xml";
            this.opf2.Filter = "DOCX files|*.docx|All files|*.*";
            this.opf2.InitialDirectory = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 44;
            this.label2.Text = "XSD файл";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 20);
            this.label1.TabIndex = 42;
            this.label1.Text = "XSLT файл";
            // 
            // cmdXML
            // 
            this.cmdXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdXML.Location = new System.Drawing.Point(946, 12);
            this.cmdXML.Name = "cmdXML";
            this.cmdXML.Size = new System.Drawing.Size(39, 26);
            this.cmdXML.TabIndex = 41;
            this.cmdXML.Text = "...";
            this.cmdXML.UseVisualStyleBackColor = true;
            this.cmdXML.Click += new System.EventHandler(this.cmdXML_Click);
            // 
            // cmdRun
            // 
            this.cmdRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdRun.Location = new System.Drawing.Point(18, 90);
            this.cmdRun.Name = "cmdRun";
            this.cmdRun.Size = new System.Drawing.Size(337, 45);
            this.cmdRun.TabIndex = 47;
            this.cmdRun.Text = "Тестирование";
            this.cmdRun.UseVisualStyleBackColor = true;
            this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
            // 
            // prcNum
            // 
            this.prcNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.prcNum.Location = new System.Drawing.Point(612, 97);
            this.prcNum.Name = "prcNum";
            this.prcNum.Size = new System.Drawing.Size(123, 29);
            this.prcNum.TabIndex = 48;
            this.prcNum.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(431, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 20);
            this.label3.TabIndex = 49;
            this.label3.Text = "Процент пропуска";
            // 
            // txtError
            // 
            this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtError.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtError.Location = new System.Drawing.Point(12, 155);
            this.txtError.Multiline = true;
            this.txtError.Name = "txtError";
            this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtError.Size = new System.Drawing.Size(972, 85);
            this.txtError.TabIndex = 50;
            // 
            // frmTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 576);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.prcNum);
            this.Controls.Add(this.cmdRun);
            this.Controls.Add(this.cmdSelectFile);
            this.Controls.Add(this.txtXSD);
            this.Controls.Add(this.txtXSLT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdXML);
            this.Controls.Add(this.wb);
            this.Name = "frmTester";
            this.Text = "Тестирование преобразования на основе XSD";
            ((System.ComponentModel.ISupportInitialize)(this.prcNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser wb;
        private System.Windows.Forms.Button cmdSelectFile;
        private System.Windows.Forms.TextBox txtXSD;
        private System.Windows.Forms.TextBox txtXSLT;
        private System.Windows.Forms.OpenFileDialog opf;
        private System.Windows.Forms.OpenFileDialog opf2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdXML;
        private System.Windows.Forms.Button cmdRun;
        private System.Windows.Forms.NumericUpDown prcNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtError;
    }
}