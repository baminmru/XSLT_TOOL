namespace xNS
{
    partial class frmScaner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScaner));
            this.opf = new System.Windows.Forms.OpenFileDialog();
            this.cmdXML = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNS = new System.Windows.Forms.TextBox();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.chkTextOnly = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIgnore = new System.Windows.Forms.TextBox();
            this.txtAddText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkIf = new System.Windows.Forms.CheckBox();
            this.chkTemplate = new System.Windows.Forms.CheckBox();
            this.chkDirect = new System.Windows.Forms.CheckBox();
            this.chkFor = new System.Windows.Forms.CheckBox();
            this.chkXPATH = new System.Windows.Forms.CheckBox();
            this.chkSmartPath = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtState = new System.Windows.Forms.TextBox();
            this.txtTail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkPostProcess = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.chkIfPost = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // opf
            // 
            this.opf.DefaultExt = "xml";
            this.opf.FileName = "text.xml";
            this.opf.Filter = "XML files|*.xml|All files|*.*";
            this.opf.InitialDirectory = ".";
            this.opf.FileOk += new System.ComponentModel.CancelEventHandler(this.opf_FileOk);
            // 
            // cmdXML
            // 
            this.cmdXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmdXML.Location = new System.Drawing.Point(903, 2);
            this.cmdXML.Name = "cmdXML";
            this.cmdXML.Size = new System.Drawing.Size(108, 32);
            this.cmdXML.TabIndex = 0;
            this.cmdXML.Text = "...";
            this.cmdXML.UseVisualStyleBackColor = true;
            this.cmdXML.Click += new System.EventHandler(this.cmdXML_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(18, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "XML файл";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtXML
            // 
            this.txtXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtXML.Location = new System.Drawing.Point(180, 5);
            this.txtXML.Name = "txtXML";
            this.txtXML.ReadOnly = true;
            this.txtXML.Size = new System.Drawing.Size(717, 26);
            this.txtXML.TabIndex = 2;
            this.txtXML.TextChanged += new System.EventHandler(this.txtXML_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Namespace";
            // 
            // txtNS
            // 
            this.txtNS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtNS.Location = new System.Drawing.Point(180, 39);
            this.txtNS.Name = "txtNS";
            this.txtNS.Size = new System.Drawing.Size(115, 26);
            this.txtNS.TabIndex = 4;
            this.txtNS.Text = "*";
            // 
            // txtOut
            // 
            this.txtOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOut.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtOut.Location = new System.Drawing.Point(5, 264);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(1015, 205);
            this.txtOut.TabIndex = 5;
            this.txtOut.WordWrap = false;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.Location = new System.Drawing.Point(12, 71);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(261, 34);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkTextOnly
            // 
            this.chkTextOnly.AutoSize = true;
            this.chkTextOnly.Checked = true;
            this.chkTextOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTextOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkTextOnly.Location = new System.Drawing.Point(331, 41);
            this.chkTextOnly.Name = "chkTextOnly";
            this.chkTextOnly.Size = new System.Drawing.Size(106, 24);
            this.chkTextOnly.TabIndex = 7;
            this.chkTextOnly.Text = "Text or Attr";
            this.chkTextOnly.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(13, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Игнорировать теги";
            // 
            // txtIgnore
            // 
            this.txtIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtIgnore.Location = new System.Drawing.Point(269, 185);
            this.txtIgnore.Name = "txtIgnore";
            this.txtIgnore.Size = new System.Drawing.Size(740, 26);
            this.txtIgnore.TabIndex = 10;
            // 
            // txtAddText
            // 
            this.txtAddText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtAddText.Location = new System.Drawing.Point(546, 39);
            this.txtAddText.Name = "txtAddText";
            this.txtAddText.Size = new System.Drawing.Size(463, 26);
            this.txtAddText.TabIndex = 12;
            this.txtAddText.Text = "<span>, </span>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(472, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Add text";
            // 
            // txtFind
            // 
            this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFind.Location = new System.Drawing.Point(269, 117);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(740, 26);
            this.txtFind.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(13, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Путь";
            // 
            // chkIf
            // 
            this.chkIf.AutoSize = true;
            this.chkIf.Location = new System.Drawing.Point(16, 224);
            this.chkIf.Name = "chkIf";
            this.chkIf.Size = new System.Drawing.Size(35, 17);
            this.chkIf.TabIndex = 15;
            this.chkIf.Text = "IF";
            this.chkIf.UseVisualStyleBackColor = true;
            // 
            // chkTemplate
            // 
            this.chkTemplate.AutoSize = true;
            this.chkTemplate.Location = new System.Drawing.Point(72, 224);
            this.chkTemplate.Name = "chkTemplate";
            this.chkTemplate.Size = new System.Drawing.Size(83, 17);
            this.chkTemplate.TabIndex = 16;
            this.chkTemplate.Text = "TEMPLATE";
            this.chkTemplate.UseVisualStyleBackColor = true;
            // 
            // chkDirect
            // 
            this.chkDirect.AutoSize = true;
            this.chkDirect.Location = new System.Drawing.Point(176, 224);
            this.chkDirect.Name = "chkDirect";
            this.chkDirect.Size = new System.Drawing.Size(66, 17);
            this.chkDirect.TabIndex = 17;
            this.chkDirect.Text = "DIRECT";
            this.chkDirect.UseVisualStyleBackColor = true;
            // 
            // chkFor
            // 
            this.chkFor.AutoSize = true;
            this.chkFor.Location = new System.Drawing.Point(263, 224);
            this.chkFor.Name = "chkFor";
            this.chkFor.Size = new System.Drawing.Size(48, 17);
            this.chkFor.TabIndex = 18;
            this.chkFor.Text = "FOR";
            this.chkFor.UseVisualStyleBackColor = true;
            // 
            // chkXPATH
            // 
            this.chkXPATH.AutoSize = true;
            this.chkXPATH.Checked = true;
            this.chkXPATH.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXPATH.Location = new System.Drawing.Point(332, 224);
            this.chkXPATH.Name = "chkXPATH";
            this.chkXPATH.Size = new System.Drawing.Size(62, 17);
            this.chkXPATH.TabIndex = 19;
            this.chkXPATH.Text = "XPATH";
            this.chkXPATH.UseVisualStyleBackColor = true;
            // 
            // chkSmartPath
            // 
            this.chkSmartPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSmartPath.AutoSize = true;
            this.chkSmartPath.Checked = true;
            this.chkSmartPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSmartPath.Location = new System.Drawing.Point(903, 224);
            this.chkSmartPath.Name = "chkSmartPath";
            this.chkSmartPath.Size = new System.Drawing.Size(106, 17);
            this.chkSmartPath.TabIndex = 20;
            this.chkSmartPath.Text = "Smart path finder";
            this.chkSmartPath.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClear.Location = new System.Drawing.Point(121, 111);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(142, 34);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "clear+copy+start";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtState
            // 
            this.txtState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtState.Location = new System.Drawing.Point(373, 80);
            this.txtState.Name = "txtState";
            this.txtState.ReadOnly = true;
            this.txtState.Size = new System.Drawing.Size(635, 26);
            this.txtState.TabIndex = 22;
            // 
            // txtTail
            // 
            this.txtTail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtTail.Location = new System.Drawing.Point(269, 153);
            this.txtTail.Name = "txtTail";
            this.txtTail.Size = new System.Drawing.Size(739, 26);
            this.txtTail.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(13, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 20);
            this.label6.TabIndex = 23;
            this.label6.Text = "Окончание пути";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(292, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 20);
            this.label7.TabIndex = 25;
            this.label7.Text = "Статус";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(180, 155);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 24);
            this.button1.TabIndex = 26;
            this.button1.Text = ">";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkPostProcess
            // 
            this.chkPostProcess.AutoSize = true;
            this.chkPostProcess.Checked = true;
            this.chkPostProcess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPostProcess.Location = new System.Drawing.Point(415, 224);
            this.chkPostProcess.Name = "chkPostProcess";
            this.chkPostProcess.Size = new System.Drawing.Size(85, 17);
            this.chkPostProcess.TabIndex = 27;
            this.chkPostProcess.Text = "PostProcess";
            this.chkPostProcess.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(230, 155);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 24);
            this.button2.TabIndex = 28;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(230, 187);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 24);
            this.button3.TabIndex = 30;
            this.button3.Text = "X";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(180, 187);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(33, 24);
            this.button4.TabIndex = 29;
            this.button4.Text = ">";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // chkIfPost
            // 
            this.chkIfPost.AutoSize = true;
            this.chkIfPost.Location = new System.Drawing.Point(526, 224);
            this.chkIfPost.Name = "chkIfPost";
            this.chkIfPost.Size = new System.Drawing.Size(56, 17);
            this.chkIfPost.TabIndex = 31;
            this.chkIfPost.Text = "If-Post";
            this.chkIfPost.UseVisualStyleBackColor = true;
            // 
            // frmScaner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 470);
            this.Controls.Add(this.chkIfPost);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chkPostProcess);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTail);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtState);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkSmartPath);
            this.Controls.Add(this.chkXPATH);
            this.Controls.Add(this.chkFor);
            this.Controls.Add(this.chkDirect);
            this.Controls.Add(this.chkTemplate);
            this.Controls.Add(this.chkIf);
            this.Controls.Add(this.txtFind);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAddText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIgnore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkTextOnly);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.txtNS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdXML);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(20, 120);
            this.Name = "frmScaner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "XML to XPath";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog opf;
        private System.Windows.Forms.Button cmdXML;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNS;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox chkTextOnly;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIgnore;
        private System.Windows.Forms.TextBox txtAddText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkIf;
        private System.Windows.Forms.CheckBox chkTemplate;
        private System.Windows.Forms.CheckBox chkDirect;
        private System.Windows.Forms.CheckBox chkFor;
        private System.Windows.Forms.CheckBox chkXPATH;
        private System.Windows.Forms.CheckBox chkSmartPath;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.TextBox txtTail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkPostProcess;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox chkIfPost;
    }
}

