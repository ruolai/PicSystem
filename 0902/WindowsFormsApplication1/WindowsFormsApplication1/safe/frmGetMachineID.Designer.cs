namespace ImageInfo.safe
{
    partial class frmGetMachineID
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
            this.rtMachineID = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtMachineID
            // 
            this.rtMachineID.Location = new System.Drawing.Point(-1, 2);
            this.rtMachineID.Name = "rtMachineID";
            this.rtMachineID.Size = new System.Drawing.Size(285, 166);
            this.rtMachineID.TabIndex = 0;
            this.rtMachineID.Text = "";
            this.rtMachineID.Click += new System.EventHandler(this.rtMachineID_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(103, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "获取机器码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(27, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请复制机器码并发送给管理员进行注册！";
            // 
            // frmGetMachineID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 221);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rtMachineID);
            this.Name = "frmGetMachineID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "获取本机机器码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtMachineID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}