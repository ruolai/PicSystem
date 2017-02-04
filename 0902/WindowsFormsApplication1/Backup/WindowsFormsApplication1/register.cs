using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }
        Form1 fm1 = new Form1();
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strUsername = txtUsername.Text.Trim();
            string strPassword = txtPassword.Text.Trim();
            string strPasswordRetry = txtPasswordRetry.Text.Trim();
            if (checkIsNull() == -1) { return; }
            if (checkIsSame(strPassword, strPasswordRetry) != 1)
            {
                MessageBox.Show("重复输入密码不一致！请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                clear();
                return;
            }
            fm1.connect();
            string strInsertSql = @"insert into login(username,password)values('{0}','{1}')";
            strInsertSql = string.Format(strInsertSql, strUsername, strPassword);
            try
            {
                using (SQLiteCommand cmd = fm1.conn.CreateCommand())
                {
                    cmd.CommandText = strInsertSql;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功，请返回登陆界面登陆", "提示");
                    Close();
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                fm1.close1();
            }
        }
        private int checkIsSame(string a, string b)
        {
            if (a != b)
            { return -1; }
            return 1;
        }
        private void clear()
        {
            txtPassword.Text = "";
            txtPasswordRetry.Text = "";
        }
        private int checkIsNull()
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text.Trim()) || string.IsNullOrEmpty(txtPasswordRetry.Text.Trim()))
            {
                MessageBox.Show("所有的必填项必须完善！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
            return 1;
        }

        private void txtPasswordRetry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
        }

    }
}
