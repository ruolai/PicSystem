using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;



namespace WindowsFormsApplication1
{
    public partial class add : Form
    {
        Form1 father = new Form1();
        SQLiteConnection conn = null;
        FileStream m_fileStream = null;
        //声明一个委托
        public delegate void displayUpdate();
        //声明事件
        public event displayUpdate showUpdate;
        public add()
        {
            InitializeComponent();
        }
        public void connect()
        {

            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/userInfo.db";
            conn = new SQLiteConnection(dbPath);
            conn.Open();
        }
        public void closeConn()
        {
            conn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                connect();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"insert into info (idno,name,sex,birthday,addr,town,village,image) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',@data) ";
                    if (m_fileStream != null)
                    {
                        SQLiteParameter para = new SQLiteParameter("@data", DbType.Binary);
                        byte[] buffer = StreamUtil.ReadFully(m_fileStream);
                        m_fileStream.Close();
                        para.Value = buffer;
                        cmd.Parameters.Add(para);
                    }
                    else
                    {
                        cmd.CommandText = @"insert into info (idno,name,sex,birthday,addr,town,village) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ";
                    }
                    cmd.CommandText = string.Format(cmd.CommandText, txtIdNo.Text.Trim(), txtName.Text.Trim(), cmbSex.Text, dtBirthday.Value.ToString("yyyyMMdd"), rtAdd.Text.Trim(),txtXZ.Text.Trim(),txtCun.Text.Trim());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show("数据插入成功", "提示");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("插入数据失败，报错信息：" + ex.Message, "提示");
            }
            finally
            {
                closeConn();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "常见图片|*.jpg;*.gif;*.png;*.bmp|全部文件|*.*" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                picImage.Image = Image.FromFile(dlg.FileName);
                
                m_fileStream = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read); //读取图片
            }
        }
    }
}
