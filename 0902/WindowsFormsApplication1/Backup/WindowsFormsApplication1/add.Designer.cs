namespace WindowsFormsApplication1
{
    partial class add
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblrowid = new System.Windows.Forms.Label();
            this.dtBirthday = new System.Windows.Forms.DateTimePicker();
            this.rtAdd = new System.Windows.Forms.RichTextBox();
            this.cmbSex = new System.Windows.Forms.ComboBox();
            this.txtIdNo = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblAdd = new System.Windows.Forms.Label();
            this.lblBirthday = new System.Windows.Forms.Label();
            this.lblIdNo = new System.Windows.Forms.Label();
            this.lblSex = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCun = new System.Windows.Forms.TextBox();
            this.txtXZ = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picImage);
            this.groupBox2.Location = new System.Drawing.Point(289, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 230);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "图像";
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(6, 16);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(176, 208);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 5;
            this.picImage.TabStop = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(28, 300);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(176, 23);
            this.btnUpload.TabIndex = 15;
            this.btnUpload.Text = "相片上传";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblrowid
            // 
            this.lblrowid.AutoSize = true;
            this.lblrowid.Location = new System.Drawing.Point(39, 307);
            this.lblrowid.Name = "lblrowid";
            this.lblrowid.Size = new System.Drawing.Size(0, 12);
            this.lblrowid.TabIndex = 14;
            // 
            // dtBirthday
            // 
            this.dtBirthday.Location = new System.Drawing.Point(86, 151);
            this.dtBirthday.Name = "dtBirthday";
            this.dtBirthday.Size = new System.Drawing.Size(147, 21);
            this.dtBirthday.TabIndex = 13;
            // 
            // rtAdd
            // 
            this.rtAdd.Location = new System.Drawing.Point(86, 246);
            this.rtAdd.Name = "rtAdd";
            this.rtAdd.Size = new System.Drawing.Size(309, 40);
            this.rtAdd.TabIndex = 12;
            this.rtAdd.Text = "";
            // 
            // cmbSex
            // 
            this.cmbSex.FormattingEnabled = true;
            this.cmbSex.Items.AddRange(new object[] {
            "男",
            "女"});
            this.cmbSex.Location = new System.Drawing.Point(86, 111);
            this.cmbSex.Name = "cmbSex";
            this.cmbSex.Size = new System.Drawing.Size(69, 20);
            this.cmbSex.TabIndex = 11;
            // 
            // txtIdNo
            // 
            this.txtIdNo.Location = new System.Drawing.Point(86, 73);
            this.txtIdNo.Name = "txtIdNo";
            this.txtIdNo.Size = new System.Drawing.Size(197, 21);
            this.txtIdNo.TabIndex = 7;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(87, 34);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(117, 21);
            this.txtName.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCun);
            this.groupBox1.Controls.Add(this.txtXZ);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnUpload);
            this.groupBox1.Controls.Add(this.lblrowid);
            this.groupBox1.Controls.Add(this.dtBirthday);
            this.groupBox1.Controls.Add(this.rtAdd);
            this.groupBox1.Controls.Add(this.cmbSex);
            this.groupBox1.Controls.Add(this.txtIdNo);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.lblAdd);
            this.groupBox1.Controls.Add(this.lblBirthday);
            this.groupBox1.Controls.Add(this.lblIdNo);
            this.groupBox1.Controls.Add(this.lblSex);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Location = new System.Drawing.Point(56, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 329);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本信息";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(289, 296);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(176, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblAdd
            // 
            this.lblAdd.AutoSize = true;
            this.lblAdd.Location = new System.Drawing.Point(27, 260);
            this.lblAdd.Name = "lblAdd";
            this.lblAdd.Size = new System.Drawing.Size(41, 12);
            this.lblAdd.TabIndex = 4;
            this.lblAdd.Text = "地址：";
            // 
            // lblBirthday
            // 
            this.lblBirthday.AutoSize = true;
            this.lblBirthday.Location = new System.Drawing.Point(27, 158);
            this.lblBirthday.Name = "lblBirthday";
            this.lblBirthday.Size = new System.Drawing.Size(65, 12);
            this.lblBirthday.TabIndex = 3;
            this.lblBirthday.Text = "出生日期：";
            // 
            // lblIdNo
            // 
            this.lblIdNo.AutoSize = true;
            this.lblIdNo.Location = new System.Drawing.Point(27, 78);
            this.lblIdNo.Name = "lblIdNo";
            this.lblIdNo.Size = new System.Drawing.Size(53, 12);
            this.lblIdNo.TabIndex = 2;
            this.lblIdNo.Text = "身份证：";
            // 
            // lblSex
            // 
            this.lblSex.AutoSize = true;
            this.lblSex.Location = new System.Drawing.Point(27, 122);
            this.lblSex.Name = "lblSex";
            this.lblSex.Size = new System.Drawing.Size(41, 12);
            this.lblSex.TabIndex = 1;
            this.lblSex.Text = "性别：";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(27, 37);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 12);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "姓名：";
            // 
            // txtCun
            // 
            this.txtCun.Location = new System.Drawing.Point(207, 192);
            this.txtCun.Name = "txtCun";
            this.txtCun.Size = new System.Drawing.Size(75, 21);
            this.txtCun.TabIndex = 26;
            // 
            // txtXZ
            // 
            this.txtXZ.Location = new System.Drawing.Point(86, 192);
            this.txtXZ.Name = "txtXZ";
            this.txtXZ.Size = new System.Drawing.Size(64, 21);
            this.txtXZ.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "行政村：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "乡镇：";
            // 
            // add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 349);
            this.Controls.Add(this.groupBox1);
            this.Name = "add";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "modify";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblrowid;
        private System.Windows.Forms.DateTimePicker dtBirthday;
        private System.Windows.Forms.RichTextBox rtAdd;
        private System.Windows.Forms.ComboBox cmbSex;
        private System.Windows.Forms.TextBox txtIdNo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAdd;
        private System.Windows.Forms.Label lblBirthday;
        private System.Windows.Forms.Label lblIdNo;
        private System.Windows.Forms.Label lblSex;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCun;
        private System.Windows.Forms.TextBox txtXZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}