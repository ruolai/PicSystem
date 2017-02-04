using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace WindowsFormsApplication1.serach
{
    public partial class frmSerach : Form
    {
        public DataSet ds = new DataSet();
        //Form1 frm1 = new Form1();
        WindowsFormsApplication1.Form1 frm1 = new WindowsFormsApplication1.Form1();
        ArrayList al = new ArrayList();
        public frmSerach()
        {
            InitializeComponent();
            
        }

        private void frmSerach_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ds.Tables["info"];
            dataGridView1.Columns[0].HeaderText = "身份证";
            dataGridView1.Columns[1].HeaderText = "姓名";
            dataGridView1.Columns[2].HeaderText = "性别";
            dataGridView1.Columns[3].HeaderText = "出生日期";
            dataGridView1.Columns[4].HeaderText = "地址";
            dataGridView1.Columns[5].HeaderText = "相片";
            dataGridView1.Columns[6].HeaderText = "乡镇";
            dataGridView1.Columns[7].HeaderText = "行政村";
            dataGridView1.Columns[8].HeaderText = "上传时间";

            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[9].Visible = false;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            al.Clear();
            al.Add(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
            Close();
        }
        public string returnSearch()
        {
            if (al.Count == 0) { return "-1"; }
            if (al[0] == null) { return "-1"; }
            return al[0].ToString(); ;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            //隔行换色
            this.dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(230, 180, 80);
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(199, 237, 233);
            this.dataGridView1.GridColor = Color.FromArgb(208, 255, 255);
        }
    }
}
