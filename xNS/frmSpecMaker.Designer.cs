namespace xNS
{
    partial class FrmSpecMaker
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
            this.txtXML = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdXML = new System.Windows.Forms.Button();
            this.txtDocx = new System.Windows.Forms.TextBox();
            this.cmdDocX = new System.Windows.Forms.Button();
            this.opf = new System.Windows.Forms.OpenFileDialog();
            this.opf2 = new System.Windows.Forms.OpenFileDialog();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.tv = new System.Windows.Forms.TreeView();
            this.cmdRead = new System.Windows.Forms.Button();
            this.cmdDt = new System.Windows.Forms.Button();
            this.cmdCm = new System.Windows.Forms.Button();
            this.cmdCap = new System.Windows.Forms.Button();
            this.cmdProcess = new System.Windows.Forms.Button();
            this.textSaveMap = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textLoadMap = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.svf = new System.Windows.Forms.SaveFileDialog();
            this.cmdLf = new System.Windows.Forms.Button();
            this.cmdAutoDot = new System.Windows.Forms.Button();
            this.cmdShiftUp = new System.Windows.Forms.Button();
            this.cmdDel = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPDF = new System.Windows.Forms.RadioButton();
            this.chkScreen = new System.Windows.Forms.RadioButton();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkShiftDot = new System.Windows.Forms.CheckBox();
            this.chkReInit = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtXML
            // 
            this.txtXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtXML.Location = new System.Drawing.Point(125, 12);
            this.txtXML.Name = "txtXML";
            this.txtXML.ReadOnly = true;
            this.txtXML.Size = new System.Drawing.Size(937, 26);
            this.txtXML.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "XML файл";
            // 
            // cmdXML
            // 
            this.cmdXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdXML.Location = new System.Drawing.Point(1068, 9);
            this.cmdXML.Name = "cmdXML";
            this.cmdXML.Size = new System.Drawing.Size(39, 32);
            this.cmdXML.TabIndex = 3;
            this.cmdXML.Text = "...";
            this.cmdXML.UseVisualStyleBackColor = true;
            this.cmdXML.Click += new System.EventHandler(this.cmdXML_Click);
            // 
            // txtDocx
            // 
            this.txtDocx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDocx.Location = new System.Drawing.Point(125, 44);
            this.txtDocx.Name = "txtDocx";
            this.txtDocx.ReadOnly = true;
            this.txtDocx.Size = new System.Drawing.Size(937, 26);
            this.txtDocx.TabIndex = 8;
            // 
            // cmdDocX
            // 
            this.cmdDocX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDocX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDocX.Location = new System.Drawing.Point(1068, 41);
            this.cmdDocX.Name = "cmdDocX";
            this.cmdDocX.Size = new System.Drawing.Size(39, 32);
            this.cmdDocX.TabIndex = 6;
            this.cmdDocX.Text = "...";
            this.cmdDocX.UseVisualStyleBackColor = true;
            this.cmdDocX.Click += new System.EventHandler(this.cmdDocX_Click);
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
            // txtOut
            // 
            this.txtOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOut.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtOut.Location = new System.Drawing.Point(642, 191);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(465, 353);
            this.txtOut.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txtOut, "Сгенерированный код");
            this.txtOut.WordWrap = false;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Location = new System.Drawing.Point(736, 162);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(153, 17);
            this.chkDebug.TabIndex = 11;
            this.chkDebug.Text = "Отладочная информация";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // tv
            // 
            this.tv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tv.Location = new System.Drawing.Point(4, 191);
            this.tv.Name = "tv";
            this.tv.Size = new System.Drawing.Size(622, 247);
            this.tv.TabIndex = 12;
            this.toolTip1.SetToolTip(this.tv, "Структура документа");
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            this.tv.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_NodeMouseDoubleClick);
            // 
            // cmdRead
            // 
            this.cmdRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdRead.Location = new System.Drawing.Point(7, 41);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.Size = new System.Drawing.Size(112, 28);
            this.cmdRead.TabIndex = 13;
            this.cmdRead.Text = "Read SPEC";
            this.cmdRead.UseVisualStyleBackColor = true;
            this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
            // 
            // cmdDt
            // 
            this.cmdDt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDt.ForeColor = System.Drawing.Color.Blue;
            this.cmdDt.Location = new System.Drawing.Point(408, 151);
            this.cmdDt.Name = "cmdDt";
            this.cmdDt.Size = new System.Drawing.Size(39, 34);
            this.cmdDt.TabIndex = 15;
            this.cmdDt.Text = "Dt";
            this.cmdDt.UseVisualStyleBackColor = true;
            this.cmdDt.Click += new System.EventHandler(this.cmdDt_Click);
            // 
            // cmdCm
            // 
            this.cmdCm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdCm.ForeColor = System.Drawing.Color.Blue;
            this.cmdCm.Location = new System.Drawing.Point(317, 151);
            this.cmdCm.Name = "cmdCm";
            this.cmdCm.Size = new System.Drawing.Size(46, 34);
            this.cmdCm.TabIndex = 16;
            this.cmdCm.Text = "Cm";
            this.cmdCm.UseVisualStyleBackColor = true;
            this.cmdCm.Click += new System.EventHandler(this.cmdCm_Click);
            // 
            // cmdCap
            // 
            this.cmdCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdCap.ForeColor = System.Drawing.Color.Blue;
            this.cmdCap.Location = new System.Drawing.Point(369, 151);
            this.cmdCap.Name = "cmdCap";
            this.cmdCap.Size = new System.Drawing.Size(33, 34);
            this.cmdCap.TabIndex = 17;
            this.cmdCap.Text = "^";
            this.cmdCap.UseVisualStyleBackColor = true;
            this.cmdCap.Click += new System.EventHandler(this.cmdCap_Click);
            // 
            // cmdProcess
            // 
            this.cmdProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdProcess.Location = new System.Drawing.Point(642, 150);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(88, 34);
            this.cmdProcess.TabIndex = 18;
            this.cmdProcess.Text = "Process";
            this.cmdProcess.UseVisualStyleBackColor = true;
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // textSaveMap
            // 
            this.textSaveMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSaveMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textSaveMap.Location = new System.Drawing.Point(125, 76);
            this.textSaveMap.Name = "textSaveMap";
            this.textSaveMap.ReadOnly = true;
            this.textSaveMap.Size = new System.Drawing.Size(937, 26);
            this.textSaveMap.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(1068, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 32);
            this.button1.TabIndex = 19;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textLoadMap
            // 
            this.textLoadMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLoadMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textLoadMap.Location = new System.Drawing.Point(125, 108);
            this.textLoadMap.Name = "textLoadMap";
            this.textLoadMap.ReadOnly = true;
            this.textLoadMap.Size = new System.Drawing.Size(937, 26);
            this.textLoadMap.TabIndex = 24;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(1068, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 32);
            this.button2.TabIndex = 22;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(7, 75);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 29);
            this.button3.TabIndex = 25;
            this.button3.Text = "Save MAP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(7, 108);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 29);
            this.button4.TabIndex = 26;
            this.button4.Text = "Load MAP";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // svf
            // 
            this.svf.DefaultExt = "xml";
            this.svf.Filter = "XML files|*.xml|All files|*.*";
            this.svf.Title = "Save MAP to";
            // 
            // cmdLf
            // 
            this.cmdLf.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdLf.ForeColor = System.Drawing.Color.Blue;
            this.cmdLf.Location = new System.Drawing.Point(277, 151);
            this.cmdLf.Name = "cmdLf";
            this.cmdLf.Size = new System.Drawing.Size(34, 34);
            this.cmdLf.TabIndex = 27;
            this.cmdLf.Text = "Lf";
            this.cmdLf.UseVisualStyleBackColor = true;
            this.cmdLf.Click += new System.EventHandler(this.cmdLf_Click);
            // 
            // cmdAutoDot
            // 
            this.cmdAutoDot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdAutoDot.ForeColor = System.Drawing.Color.Blue;
            this.cmdAutoDot.Location = new System.Drawing.Point(180, 151);
            this.cmdAutoDot.Name = "cmdAutoDot";
            this.cmdAutoDot.Size = new System.Drawing.Size(91, 34);
            this.cmdAutoDot.TabIndex = 28;
            this.cmdAutoDot.Text = "Auto Dot";
            this.cmdAutoDot.UseVisualStyleBackColor = true;
            this.cmdAutoDot.Click += new System.EventHandler(this.cmdAutoDot_Click);
            // 
            // cmdShiftUp
            // 
            this.cmdShiftUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdShiftUp.ForeColor = System.Drawing.Color.Red;
            this.cmdShiftUp.Location = new System.Drawing.Point(4, 151);
            this.cmdShiftUp.Name = "cmdShiftUp";
            this.cmdShiftUp.Size = new System.Drawing.Size(41, 34);
            this.cmdShiftUp.TabIndex = 29;
            this.cmdShiftUp.Text = "UP";
            this.cmdShiftUp.UseVisualStyleBackColor = true;
            this.cmdShiftUp.Click += new System.EventHandler(this.cmdShiftUp_Click);
            // 
            // cmdDel
            // 
            this.cmdDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdDel.ForeColor = System.Drawing.Color.Red;
            this.cmdDel.Location = new System.Drawing.Point(125, 151);
            this.cmdDel.Name = "cmdDel";
            this.cmdDel.Size = new System.Drawing.Size(42, 34);
            this.cmdDel.TabIndex = 30;
            this.cmdDel.Text = "Del";
            this.cmdDel.UseVisualStyleBackColor = true;
            this.cmdDel.Click += new System.EventHandler(this.cmdDel_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button5.ForeColor = System.Drawing.Color.Red;
            this.button5.Location = new System.Drawing.Point(51, 151);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(68, 34);
            this.button5.TabIndex = 31;
            this.button5.Text = "LEFT";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkPDF);
            this.groupBox1.Controls.Add(this.chkScreen);
            this.groupBox1.Location = new System.Drawing.Point(453, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 39);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Form version";
            // 
            // chkPDF
            // 
            this.chkPDF.AutoSize = true;
            this.chkPDF.Checked = true;
            this.chkPDF.Location = new System.Drawing.Point(89, 16);
            this.chkPDF.Name = "chkPDF";
            this.chkPDF.Size = new System.Drawing.Size(46, 17);
            this.chkPDF.TabIndex = 1;
            this.chkPDF.TabStop = true;
            this.chkPDF.Text = "PDF";
            this.chkPDF.UseVisualStyleBackColor = true;
            // 
            // chkScreen
            // 
            this.chkScreen.AutoSize = true;
            this.chkScreen.Location = new System.Drawing.Point(6, 16);
            this.chkScreen.Name = "chkScreen";
            this.chkScreen.Size = new System.Drawing.Size(59, 17);
            this.chkScreen.TabIndex = 0;
            this.chkScreen.Text = "Screen";
            this.chkScreen.UseVisualStyleBackColor = true;
            // 
            // txtErrors
            // 
            this.txtErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtErrors.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtErrors.Location = new System.Drawing.Point(4, 444);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrors.Size = new System.Drawing.Size(622, 100);
            this.txtErrors.TabIndex = 34;
            this.toolTip1.SetToolTip(this.txtErrors, "Ошибки");
            this.txtErrors.WordWrap = false;
            // 
            // chkShiftDot
            // 
            this.chkShiftDot.AutoSize = true;
            this.chkShiftDot.Checked = true;
            this.chkShiftDot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShiftDot.Location = new System.Drawing.Point(895, 162);
            this.chkShiftDot.Name = "chkShiftDot";
            this.chkShiftDot.Size = new System.Drawing.Size(120, 17);
            this.chkShiftDot.TabIndex = 35;
            this.chkShiftDot.Text = "Shift Dot to Children";
            this.chkShiftDot.UseVisualStyleBackColor = true;
            // 
            // chkReInit
            // 
            this.chkReInit.AutoSize = true;
            this.chkReInit.Location = new System.Drawing.Point(1036, 161);
            this.chkReInit.Name = "chkReInit";
            this.chkReInit.Size = new System.Drawing.Size(57, 17);
            this.chkReInit.TabIndex = 36;
            this.chkReInit.Text = "Re Init";
            this.chkReInit.UseVisualStyleBackColor = true;
            // 
            // frmSpecMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 556);
            this.Controls.Add(this.chkReInit);
            this.Controls.Add(this.chkShiftDot);
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.cmdDel);
            this.Controls.Add(this.cmdShiftUp);
            this.Controls.Add(this.cmdAutoDot);
            this.Controls.Add(this.cmdLf);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textLoadMap);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textSaveMap);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdProcess);
            this.Controls.Add(this.cmdCap);
            this.Controls.Add(this.cmdCm);
            this.Controls.Add(this.cmdDt);
            this.Controls.Add(this.cmdRead);
            this.Controls.Add(this.tv);
            this.Controls.Add(this.chkDebug);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.txtDocx);
            this.Controls.Add(this.cmdDocX);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdXML);
            this.Name = "FrmSpecMaker";
            this.Text = "Process specification";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdXML;
        private System.Windows.Forms.TextBox txtDocx;
        private System.Windows.Forms.Button cmdDocX;
        private System.Windows.Forms.OpenFileDialog opf;
        private System.Windows.Forms.OpenFileDialog opf2;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.Button cmdRead;
        private System.Windows.Forms.Button cmdDt;
        private System.Windows.Forms.Button cmdCm;
        private System.Windows.Forms.Button cmdCap;
        private System.Windows.Forms.Button cmdProcess;
        private System.Windows.Forms.TextBox textSaveMap;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textLoadMap;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.SaveFileDialog svf;
        private System.Windows.Forms.Button cmdLf;
        private System.Windows.Forms.Button cmdAutoDot;
        private System.Windows.Forms.Button cmdShiftUp;
        private System.Windows.Forms.Button cmdDel;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton chkPDF;
        private System.Windows.Forms.RadioButton chkScreen;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkShiftDot;
        private System.Windows.Forms.CheckBox chkReInit;
    }
}