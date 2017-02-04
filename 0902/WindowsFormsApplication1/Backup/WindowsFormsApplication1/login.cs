using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        private string name;
        public string _name
        {
            get { return name; }
            set { name = value; }
        }
        private string pwd;
        public string _pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        string strUsername = "";
        string strPassword = "";
        string strRoot = "";
        WindowsFormsApplication1.Form1 fm1 = new WindowsFormsApplication1.Form1();
        register re1 = new register();
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (checkIsNull() == -1) { return; }
            strUsername = txtUsername.Text.Trim();
            strPassword = txtPassword.Text.Trim();
            fm1.connect();
            try
            {
            using(SQLiteCommand cmd =fm1.conn.CreateCommand())
            {
                string strSqlQuery=@"select count(*),root from login where username='{0}' and password='{1}'";
                strSqlQuery=string.Format(strSqlQuery,strUsername,strPassword);
                cmd.CommandText=strSqlQuery;
                SQLiteDataReader reader=cmd.ExecuteReader();
                if(reader.Read())
                {
                    if(reader[0].ToString()=="0")
                    {
                        MessageBox.Show("用户名或密码错误，请重新输入！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        clear();
                        txtUsername.Focus();
                        return;
                    }
                    else
                    {
                        strRoot=reader[1].ToString();
                    }
                    reader.Dispose();
                }

            }
                }
            catch(Exception ex)
            {throw ex;}
            finally{
            fm1.close1();}
            this.Hide();
            
            fm1._name=strUsername;
            fm1._pwd = strPassword;
            fm1._root = strRoot;
            fm1.Show();
            
            
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            re1.ShowDialog();
        }
        private int checkIsNull()
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show("请输入用户名或密码！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
            return 1;
        }
        private void clear()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
        public string getRoot()
        {
            strUsername = txtUsername.Text.Trim();
            strPassword = txtPassword.Text.Trim();
            try
            {
                fm1.connect();
                using (SQLiteCommand cmd = fm1.conn.CreateCommand())
                {
                    string strSqlQuery = @"select root from login where username='{0}' and password='{1}'";
                    strSqlQuery = string.Format(strSqlQuery, strUsername, strPassword);
                    cmd.CommandText = strSqlQuery;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (string.IsNullOrEmpty(reader["root"].ToString()))
                        {
                            return "0";
                        }
                        else
                        { return "1"; }
                        
                    }
                    else { return "-1"; }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                fm1.close1();
            }
        }

        private void login_Load(object sender, EventArgs e)
        {

           
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {

        }
    }
}
