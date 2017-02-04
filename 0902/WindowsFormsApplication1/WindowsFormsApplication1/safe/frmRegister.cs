using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace ImageInfo.safe
{
    public partial class frmRegister : Form
    {
        private WindowsFormsApplication1.Form1 fr1 = new WindowsFormsApplication1.Form1();
        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string strRegister = rtRegister.Text.Trim();
            if (string.IsNullOrEmpty(strRegister)) { MessageBox.Show("请输入注册码！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string jmRegister = WindowsFormsApplication1.jiemi.Base64Decrypt(strRegister);

            int index = jmRegister.IndexOf("@");
            string strTime = jmRegister.Substring(index + 1);
            string strMachineID = jmRegister.Substring(0, index);

            fr1.connect();
            string strUpdateTime = @"update safe set time='{0}' where machineID='{1}'";
            strUpdateTime = string.Format(strUpdateTime, strTime, strMachineID);
            try
            {
                using (SQLiteCommand cmd = fr1.conn.CreateCommand())
                {
                    cmd.CommandText = strUpdateTime;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功，有效期延长至:" + strTime, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    rtRegister.Text = "";
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                fr1.close1();
            }
        }
    }
}
