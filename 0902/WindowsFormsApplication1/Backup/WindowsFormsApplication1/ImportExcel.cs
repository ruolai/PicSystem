using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public partial class ImportExcel : Form
    {
        public ImportExcel()
        {
            InitializeComponent();
        }
        exportExcel excelNew = new exportExcel();
        DataTable dt = new DataTable();
        SQLiteConnection conn = null;

        private void button1_Click(object sender, EventArgs e)
        {
            
            string filePath = Environment.CurrentDirectory + "/jbxx.xlsx";
            dt = excelNew.ExcelSheetImportToDataTable(filePath, "Sheet1");
            dataGridView1.DataSource = dt;
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("是否确认导入数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
            {
                connect();
                //开启事物
                SQLiteTransaction trans = conn.BeginTransaction();
                //临时变量，为了在报错中提示第几条数据有误
                int tempI = 0;
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            string sql = @"insert into info (idno,name,sex,birthday,addr,town,village) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ";
                            sql = string.Format(sql, dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3], dt.Rows[i][4], dt.Rows[i][5],dt.Rows[i][6],dt.Rows[i][7]);
                            cmd.CommandText = sql;
                            tempI = i+1;
                            cmd.ExecuteNonQuery();
                            
                        }
                    }
                    trans.Commit();
                    MessageBox.Show("批量数据导入成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    string strErr = "第'{0}'行数据插入失败,数据已回滚，错误详细信息：";
                    strErr = string.Format(strErr, tempI);
                    strErr += ex.Message;
                    trans.Rollback();
                    MessageBox.Show(strErr, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                finally
                {
                    close1();
                    trans.Dispose();
                }
            }
        }
        public void connect()
        {

            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/userInfo.db";
            conn = new SQLiteConnection(dbPath);
            conn.Open();


        }
        public void close1()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
