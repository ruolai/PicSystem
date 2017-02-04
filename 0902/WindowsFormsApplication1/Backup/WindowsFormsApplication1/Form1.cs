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
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
       public SQLiteConnection conn = null;
        FileStream m_fileStream = null;
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
        private string root;
        public string _root
        {
            get { return root; }
            set { root = value; }
        }

        #region 第三方类
        //验证身份证
        private CheckId checkID = new CheckId();
        //导入excel
        private exportExcel excelNew = new exportExcel();
        //private SQLiteHelper helper = new SQLiteHelper(); 
        //机器码
        safe.getMachineID get = new WindowsFormsApplication1.safe.getMachineID();
        //WindowsFormsApplication1.login lg = new WindowsFormsApplication1.login();
        #endregion
        string username, password;
        int iOnLoad = 0;
        bool isModfiy = false;
        private string xmlFile = Environment.CurrentDirectory + "/xml/printSetting.xml";

        public Form1(string a,string b)
        {
            InitializeComponent();
            username = a;
            password = b;
        }
        public Form1()
        {
            InitializeComponent();
            //this.lblrowid.Text = "1";
            connect();
            cmbSelect.SelectedIndex = 1;

            //默认显示第一条数据
            string sqlQueryByDefault = @"select *,rowid from info where rowid='{0}'";
            sqlQueryByDefault = string.Format(sqlQueryByDefault, '1');
            addData(sqlQueryByDefault);
            close1();
        }
        public int addData(string sql)
        {
            SQLiteCommand cmdQuery = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmdQuery.ExecuteReader();
            if (reader.Read())
            {
                clear();
                this.txtIdNo.Text = reader["idno"].ToString();
                this.txtName.Text = reader["name"].ToString();
                this.cmbSex.Text = reader["sex"].ToString();
                this.dtBirthday.Value = DateTime.ParseExact(reader["birthday"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                //this.dtBirthday.Value = DateTime.Parse(reader["birthday"].ToString());
                this.rtAdd.Text = reader["addr"].ToString();
                this.lblrowid.Text = reader["rowid"].ToString();
                //this.cmbXZ.Text = reader["town"].ToString();
                this.txtXZ.Text = reader["town"].ToString();
                this.txtCun.Text = reader["village"].ToString();
                //this.cmbXZ.Text = reader["village"].ToString();
                if (reader["image"] != System.DBNull.Value)
                {
                    byte[] barrImage = (byte[])reader["image"];
                    try
                    {
                        MemoryStream ms = new MemoryStream(barrImage);
                        ms.Write(barrImage, 0, barrImage.Length);
                        Bitmap image = new Bitmap(ms);
                        picImage.Image = image;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {

                    }
                }
                else
                {
                    picImage.Image = null;
                }
                reader.Close();
                return 1;
            }
            else
            {
                reader.Close();
                return -1;
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            connect();
            string rowid = this.lblrowid.Text;
            string maxRowid = "";
            string sqlQueryMaxRowid = @"select max(rowid) from info";
            SQLiteCommand cmdQueryMaxRowid = new SQLiteCommand(sqlQueryMaxRowid, conn);
            SQLiteDataReader maxReader = cmdQueryMaxRowid.ExecuteReader();
            if (maxReader.Read())
            {
                maxRowid = maxReader[0].ToString();
            }
            maxReader.Close();
            if (maxRowid == this.lblrowid.Text)
            {
                MessageBox.Show("已经到达最后一条数据", "警告");
                return;
            }

            string sqlQueryNext = @"select *,rowid from info where rowid='{0}'";
            sqlQueryNext = string.Format(sqlQueryNext, int.Parse(rowid) + 1);
            int i = addData(sqlQueryNext);
            if (i == -1 && int.Parse(rowid) < int.Parse(maxRowid))
            {
                for (int a = int.Parse(rowid) + 2; a <= int.Parse(maxRowid); a++)
                {
                    string sqlQueryNext1 = @"select *,rowid from info where rowid='{0}'";
                    sqlQueryNext1 = string.Format(sqlQueryNext1, a);
                    if (addData(sqlQueryNext1) == 1)
                    {
                        break;
                    }
                }
            }
            close1();
            scrollToRow(lblrowid.Text);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            connect();
            int min = getMinRowid();
            string rowid = this.lblrowid.Text;
            string sqlQueryNext = @"select *,rowid from info where rowid='{0}'";
            sqlQueryNext = string.Format(sqlQueryNext, int.Parse(rowid) - 1);
            if (int.Parse(rowid) <= min)
            {
                MessageBox.Show("已经到达第一条数据", "警告");
                return;
            }
            int i = addData(sqlQueryNext);

            if (i == -1 && int.Parse(rowid) > min)
            {
                for (int a = int.Parse(rowid) - 1; a >= min; a--)
                {
                    string sqlQueryLast1 = @"select *,rowid from info where rowid='{0}'";
                    sqlQueryLast1 = string.Format(sqlQueryLast1, a);
                    if (addData(sqlQueryLast1) == 1)
                    {
                        break;
                    }
                }
            }
            scrollToRow(lblrowid.Text);
            close1();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            string strIdno = this.txtQuery.Text.Trim();
            if (!checkID.CheckIDCard(strIdno))
            {
                MessageBox.Show("请输入正确的身份证号码！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            connect();
            string sqlQueryData= returnSqlByCombox(cmbSelect.SelectedIndex);
            //string sqlQueryData = @"select *,rowid from info where idno='{0}'";
            sqlQueryData = string.Format(sqlQueryData, strIdno);
            //int i = addData(sqlQueryData);
            //if (i == -1) MessageBox.Show("没有检索到数据", "提示");
            
            serach.frmSerach serach1 = new WindowsFormsApplication1.serach.frmSerach();
            serach1.ds = ExecuteDataSet(sqlQueryData);
            serach1.ShowDialog();
            if (serach1.returnSearch() == "-1") { return; }
            searchByRowid(serach1.returnSearch());
            close1();
            scrollToRow(this.lblrowid.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = @"delete from info where rowid='{0}'";
                    sqlDelete = string.Format(sqlDelete, this.lblrowid.Text);
                    connect();
                    SQLiteCommand cmdDelete = new SQLiteCommand(sqlDelete, conn);
                    cmdDelete.ExecuteNonQuery();

                    MessageBox.Show("删除成功", "提示");
                    clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据删除失败，原因：" + ex.Message, "提示");
                }
                finally
                {
                    close1();
                    loadData();
                }
            }
        }

        private void btnNull_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIdNo.Text) && !string.IsNullOrEmpty(txtName.Text))
            {
                if (MessageBox.Show(txtName.Text.Trim() + "基本信息已经存在是否覆盖?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }

                connect();
                //创建事物 
                SQLiteTransaction trans = conn.BeginTransaction();
                string strTempRowid = "";
                string str_time = DateTime.Now.ToString();
                try
                {

                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"update info set name='{0}',sex='{1}',birthday='{2}',addr='{3}',idno='{4}',town='{6}',village='{7}',update_time='{8}',image=@data
              where rowid={5}";
                        if (m_fileStream != null)
                        {
                            SQLiteParameter para = new SQLiteParameter("@data", DbType.Binary);
                            byte[] buffer = StreamUtil.ReadFully(m_fileStream);
                            m_fileStream.Close();
                            para.Value = buffer;
                            cmd.Parameters.Add(para);
                            cmd.CommandText = string.Format(cmd.CommandText, txtName.Text.Trim(), cmbSex.Text, dtBirthday.Value.ToString("yyyyMMdd"), rtAdd.Text.Trim(), txtIdNo.Text.Trim(), lblrowid.Text, txtXZ.Text.Trim(), txtCun.Text.Trim(), str_time);
                        }
                        else
                        {
                            cmd.CommandText = @"update info set name='{0}',sex='{1}',birthday='{2}',addr='{3}',idno='{4}',town='{6}',village='{7}'
          where rowid='{5}'";
                            cmd.CommandText = string.Format(cmd.CommandText, txtName.Text.Trim(), cmbSex.Text, (dtBirthday.Value.ToString("yyyyMMdd")), rtAdd.Text.Trim(), txtIdNo.Text.Trim(), lblrowid.Text, txtXZ.Text.Trim(), txtCun.Text.Trim());
                        }
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                        MessageBox.Show("修改成功", "提示");
                        strTempRowid = lblrowid.Text;
                        m_fileStream = null;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("出错了，错误信息：" + ex.Message, "提示");
                    trans.Rollback();
                }
                finally
                {
                    close1();
                    trans.Dispose();
                    loadData();
                    scrollToRow(strTempRowid);

                }
            }
            else
            {
                MessageBox.Show("没有信息需要保存！");
                return;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            add m = new add();
            //m.showUpdate += new displayUpdate(loadData);
            m.ShowDialog();
            loadData();

        }


        //加载图像
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                if (MessageBox.Show(txtName.Text.Trim() + "相片已经存在，是否更改？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    OpenFileDialog dlg = new OpenFileDialog { Filter = "常见图片|*.jpg;*.gif;*.png;*.bmp|全部文件|*.*" };
                    dlg.Multiselect = true;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        picImage.Image = Image.FromFile(dlg.FileName);
                        m_fileStream = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read); //读取图片
                    }
                }
            }
            else
            {
                OpenFileDialog dlg = new OpenFileDialog { Filter = "常见图片|*.jpg;*.gif;*.png;*.bmp|全部文件|*.*" };
                dlg.Multiselect = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    picImage.Image = Image.FromFile(dlg.FileName);
                    m_fileStream = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read); //读取图片
                }
            }
            
        }
        //清屏
        private void clear()
        {
            this.txtIdNo.Text = "";
            this.txtName.Text = "";
            this.cmbSex.Text = "";
            this.dtBirthday.Text = "";
            this.rtAdd.Text = "";
            this.picImage.Image = null;
            //this.cmbXZ.Text = "";
            //this.cmbCun.Text = "";
            this.txtXZ.Text = "";
            this.txtCun.Text = "";
        }
        //获取最小rowid
        private int getMinRowid()
        {
            string sqlGetMinRowId = @"select min(rowid) from info";
            SQLiteCommand cmdGetMin = new SQLiteCommand(sqlGetMinRowId, conn);
            SQLiteDataReader reader1 = cmdGetMin.ExecuteReader();
            try
            {
                if (reader1.Read())
                {
                    return int.Parse(reader1[0].ToString());
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("报错啦：" + ex.Message);
                return 1;
            }
            finally
            {
                reader1.Close();
            }

        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuery_Click(sender, e);
            }
        }
        private int isNull()
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is Label || item is Button || item is DateTimePicker || item is GroupBox)
                {
                    continue;
                }
                else
                {
                    if (item.Text != null || item.Text != "")
                    {
                        return 1;
                    }
                    else { return 0; }
                }
            }
            return 2;
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    this.txtIdNo.Focus();
                }
                else
                {
                    MessageBox.Show("请输入姓名！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void txtIdNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (checkID.CheckIDCard(this.txtIdNo.Text.Trim()))
                {
                    this.cmbSex.Focus();
                }
                else
                {
                    MessageBox.Show("请输入正确的身份证号码！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void cmbSex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(this.cmbSex.Text.Trim()))
                {
                    this.dtBirthday.Focus();
                }
                else
                {
                    MessageBox.Show("请选择性别！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dtBirthday_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                rtAdd.Focus();
            }
        }

        private void rtAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (MessageBox.Show("是否插入新的数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.btnAdd_Click(sender, e);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iOnLoad = 1;
            loadData();
            loadJBXX();
            //写入机器码进数据库
            insertMachineID();
            //检查授权时间
            checkMachineID();
            //备份数据库
            backupDatabase();
            //关闭登陆界面
            //login lg = new login();
            //lg.Hide();
            //login lg = new login();
          
            if (string.IsNullOrEmpty(root))
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
                btnSave.Visible = false;
                btnNull.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                btnUpload.Visible = false;
                btnInitialize.Visible = false;
            }
            
            //this.BackgroundImage=Image.FromFile(Application.StartupPath +"\\pic\\backPic.jpg");
            clear();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (iOnLoad == 0)
            {
                if (dataGridView1.Rows.Count > 1)
                {
                    if (this.dataGridView1.SelectionMode != DataGridViewSelectionMode.FullColumnSelect)
                    {
                        int index = dataGridView1.CurrentRow.Index;
                        if (index >= 0 && index <= dataGridView1.Rows.Count - 2)
                        {

                            //先清空，防止数据不同步
                            clear();
                            txtName.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                            txtIdNo.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                            cmbSex.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                            dtBirthday.Text = DateTime.ParseExact(dataGridView1.Rows[index].Cells[3].Value.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString();
                            rtAdd.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
                            if (!string.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                            {
                                byte[] by = (byte[])(dataGridView1.Rows[index].Cells[5].Value);
                                picImage.Image = convertImg(by);
                            }
                            //cmbXZ.Text = dataGridView1.Rows[index].Cells[6].Value.ToString();
                            //cmbCun.Text = dataGridView1.Rows[index].Cells[7].Value.ToString();
                            txtXZ.Text = dataGridView1.Rows[index].Cells[6].Value.ToString();
                            txtCun.Text = dataGridView1.Rows[index].Cells[7].Value.ToString();
                            lblrowid.Text = dataGridView1.Rows[index].Cells[9].Value.ToString();
                        }

                    }
                }
            }
            iOnLoad = 0;
        }
        private Image convertImg(byte[] datas)
        {
            if (datas == null)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream(datas))
            {
                Image img = Image.FromStream(ms);
                ms.Flush();
                return img;
            }
        }

        ///<summary> 
        /// 序列化 
        /// </summary> 
        /// <param name="data">要序列化的对象</param> 
        /// <returns>返回存放序列化后的数据缓冲区</returns> 
        public static byte[] Serialize(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, data);
            return rems.GetBuffer();
        }
        /// <summary>
        /// 封装加载datagridview方法
        /// </summary>
        public void loadData()
        {
            connect();
            string sqlLoad = @"select idno,name,sex,birthday,addr,image,town,village,update_time,rowid from info";
            SQLiteDataAdapter ad = new SQLiteDataAdapter(sqlLoad, conn);
            DataSet ds = new DataSet();
            ad.Fill(ds, "info");
            dataGridView1.DataSource = ds.Tables["info"];
            //自定义列头
            dataGridView1.Columns[0].HeaderText = "身份证";
            dataGridView1.Columns[1].HeaderText = "姓名";
            dataGridView1.Columns[2].HeaderText = "性别";
            dataGridView1.Columns[3].HeaderText = "出生日期";
            dataGridView1.Columns[4].HeaderText = "地址";
            dataGridView1.Columns[5].HeaderText = "相片";
            dataGridView1.Columns[6].HeaderText = "乡镇";
            dataGridView1.Columns[7].HeaderText = "行政村";
            dataGridView1.Columns[8].HeaderText = "上传时间";

            dataGridView1.Columns[0].Width = 127;
            dataGridView1.Columns[2].Width = 55;
            dataGridView1.Columns[6].Width = 70;
            dataGridView1.Columns[7].Width = 70;
            dataGridView1.Columns[8].Width = 127;

            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[4].Visible = false;

            close1();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show("1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImportExcel im = new ImportExcel();
            im.ShowDialog();
            loadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog { Filter = "常见图片|*.jpg;*.gif;*.png;*.bmp|全部文件|*.*" };
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int i = 0;
                //m_fileStream = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read); //读取图片
                connect();
                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (string filename in dlg.FileNames)
                    {
                 
                        FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"update info set image=@data,update_time='{1}' where idno={0}";
                            //截取文件名，即身份证号
                            int index = dlg.SafeFileNames[i].ToString().IndexOf(".");
                            string idNew = dlg.SafeFileNames[i].ToString().Substring(0, index);
                            //判断身份证是否重复
                            if (checkIdno(idNew) == -1)
                            {
                                string str = "检测到身份证重复,身份证号码为:{0}请更正后重新上传，现所有数据已经回滚，请核实！";
                                str = string.Format(str, idNew);
                                MessageBox.Show(str, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                trans.Rollback();
                                return;
                            }
                            if (checkIdno(idNew) == -2)
                            {
                                string str = "系统内无与该相片匹配的个人信息(身份证号码为：'{0}')，请核对！，所有数据已经回滚，请核实！";
                                str = string.Format(str, idNew);
                                MessageBox.Show(str, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                trans.Rollback();
                                return;
                            }
                            if (!IsNumAndEnCh(idNew))
                            {
                                cmd.CommandText = string.Format(cmd.CommandText, idNew, DateTime.Now.ToString());
                            }
                            else
                            {
                                string strRowid=returnRowIDByIdno(idNew);
                                cmd.CommandText = @"update info set image=@data,update_time='{1}' where rowid={0}";
                                cmd.CommandText = string.Format(cmd.CommandText, strRowid, DateTime.Now.ToString());
                            }
                            SQLiteParameter para = new SQLiteParameter("@data", DbType.Binary);
                            byte[] buffer = StreamUtil.ReadFully(stream);
                            para.Value = buffer;
                            cmd.Parameters.Add(para);
                            cmd.ExecuteNonQuery();
                            i++;
                        }
                    }

                    MessageBox.Show("相片批量更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    trans.Commit();
                    loadData();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    close1();
                    throw (ex);

                }
                finally
                {
                    //close(); 因为已经在loadData中关闭了连接，所以这里就不需要了
                    trans.Dispose();
                }
            }
        }
        //判断身份证是否重复(在上传了相片的前提下)
        public int checkIdno(string idno)
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select count(*) from info where idno='{0}'";
                cmd.CommandText = string.Format(cmd.CommandText, idno);

                SQLiteDataReader reader1 = cmd.ExecuteReader();
                if (reader1.Read())
                {
                    if (reader1[0].ToString() == "0")
                    { return -2; }
                }
            }
            using (SQLiteCommand cmd1 = conn.CreateCommand())
            {
                cmd1.CommandText = @"select count(*) from info where idno='{0}' and (info.image is not null or info.image<>'')";
                cmd1.CommandText = string.Format(cmd1.CommandText, idno);
                
               SQLiteDataReader reader = cmd1.ExecuteReader();
                if (reader.Read())
                {
                    if (reader[0].ToString()!= "0")
                    { return -1; }
                }
            }

            return 1;
        }

        /// <summary>
        /// 根据选择的检索方式返回相对应的sql
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string returnSqlByCombox(int selectIndex)
        {
            string strSql = "";
            switch (selectIndex)
            {
                //姓名
                case 0:
                    strSql = @"select *,rowid from info where name like '%{0}%'";
                    break;
                //身份证
                case 1:
                    strSql = @"select *,rowid from info where idno='{0}'";
                    break;
                //乡镇
                case 2:
                    strSql = @"select *,rowid from info where town='{0}'";
                    break;
                //行政村
                case 3:
                    strSql = @"select *,rowid from info where village='{0}'";
                    break;
            }
            return strSql;
        }
        //为datagridview增加序号栏
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            //隔行换色
            this.dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(230,180,80);
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(199,237,233);
            this.dataGridView1.GridColor = Color.FromArgb(208, 255, 255);
        }
        /// <summary>
        /// 为combobox加载信息
        /// </summary>
        public void loadJBXX()
        {
            //connect();
            //SQLiteDataAdapter sqlAd = new SQLiteDataAdapter("select * from com_dictionary a where a.type='dist' and a.parent_code='1000'", conn);
            //DataTable dt = new DataTable();
            //sqlAd.Fill(dt); 
            //close();

            ////cmbXZ.DisplayMember = "name";
            ////cmdXZ.ValueMember = "code";  改写入明文
            //cmbXZ.ValueMember = "name";
            //cmbXZ.DataSource = dt;
            //cmbXZ.SelectedIndex = 0;
        }

        private void cmdXZ_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cmbXZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbCun.Text = "";
            //connect();
            //string strTownName = cmbXZ.Text;
            //string sql = @"select * from com_dictionary a where a.type='dist' and a.parent_name='{0}'";
            //sql = string.Format(sql, strTownName);
            //SQLiteDataAdapter sqlAd = new SQLiteDataAdapter(sql, conn);
            //DataTable dt = new DataTable();
            //sqlAd.Fill(dt);

            //cmbCun.DisplayMember = "name";
            //cmbCun.ValueMember = "name";
            //cmbCun.DataSource = dt;
            //close();
        }
        /// <summary>
        /// 查询返回ds
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {

            SQLiteDataAdapter da = new SQLiteDataAdapter(sql,conn);
            DataSet ds = new DataSet();
            da.Fill(ds,"info");
            
            da.Dispose();
            return ds;
            
        }
        //dr滚动
        public void scrollToRow(string rowid)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int a = 0; a < dataGridView1.Rows[i].Cells.Count; a++)
                    {
                        if (dataGridView1.Rows[i].Cells[a].Value.ToString() == rowid)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = i;
                            
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            finally { }
        }
        public void searchByRowid(string rowid)
        {
            
            string sql = @"select * ,rowid from info where rowid='{0}'";
            sql = string.Format(sql, rowid);
            addData(sql);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text = "CPU:"+get.getCpu();
            //richTextBox1.Text +=";"+"注册码："+ get.getRNum();
            richTextBox1.Text = "";
            richTextBox1.Text = get.getMNum();
        }
        private void insertMachineID()
        {
            connect();
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                string machineID = get.getMNum();
                string selectMachineID = @"select count(*) from safe where machineID='{0}'";
                selectMachineID = string.Format(selectMachineID,machineID);
                cmd.CommandText = selectMachineID;
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader[0].ToString() != "0")
                    { return; }
                    else
                    {
                        reader.Dispose();
                        using (SQLiteCommand cmd1 = new SQLiteCommand())
                        {
                            string insertID = @"insert into safe(machineID)values('{0}')";
                            insertID = string.Format(insertID, machineID);
                            cmd.CommandText = insertID;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            close1();
        }
        private void checkMachineID()
        {
            connect();
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                string machineID = get.getMNum();
                string selectMachineID = @"select * from safe where machineID='{0}'";
                selectMachineID = string.Format(selectMachineID, machineID);
                cmd.CommandText = selectMachineID;
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["machineID"].ToString() == machineID)
                    {
                        if (reader["time"].ToString() == "")
                        {
                            reader.Dispose();
                            using (SQLiteCommand cmd1 = conn.CreateCommand())
                            {
                                string updateDefaultDate = @"update safe set time='{0}' where machineID='{1}'";
                                updateDefaultDate = string.Format(updateDefaultDate,DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"), machineID);
                                cmd1.CommandText = updateDefaultDate;
                                cmd1.ExecuteNonQuery();
                            }
                            MessageBox.Show("软件未经授权，初次使用期限为3天！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            close1();
                        }
                        else
                        {
                            DateTime oldTime = Convert.ToDateTime(reader["time"]);
                            DateTime newTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                            if (oldTime <= newTime)
                            {
                                MessageBox.Show("软件已超授权期限,请联系管理员!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                Close();
                                return;
                            }
                        }
                    }
                }
            }
            //Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string strRegister = rtRegister.Text.Trim();
            if (string.IsNullOrEmpty(strRegister)) { MessageBox.Show("请输入注册码！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string jmRegister = jiemi.Base64Decrypt(strRegister);

            int index = jmRegister.IndexOf("@");
            string strTime = jmRegister.Substring(index+1);
            string strMachineID=jmRegister.Substring(0,index);

            connect();
            string strUpdateTime = @"update safe set time='{0}' where machineID='{1}'";
            strUpdateTime = string.Format(strUpdateTime, strTime,strMachineID);
            try
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
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
                close1();
            }
        }

        private void backupDatabase()
        {
            string toPath = Environment.CurrentDirectory + "\\backup" + "/userInfo.db";
            string fromPath = Environment.CurrentDirectory + "/userInfo.db";
            try
            {
                File.Copy(fromPath, toPath, true);
            }
            catch (Exception ex)
            { throw ex; }
            finally { }

        }

        private void btnSQ_Click(object sender, EventArgs e)
        {
            connect();
            string machineID = get.getMNum();
            string time="";
            string queryTime = @"Select time from safe where machineID='{0}'";
            queryTime = string.Format(queryTime, machineID);
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = queryTime;
               SQLiteDataReader reader= cmd.ExecuteReader();
               if (reader.Read())
               {
                    time = reader["time"].ToString();
                    reader.Dispose();
               }
            }
            MessageBox.Show("当前授权截至日期为：" +time , "提示");
            close1();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("请确认是否有需要保存的内容，是否退出？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.ExitThread();
                    this.Dispose();
            }
            else
            { e.Cancel = true; }
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("即将清空数据库中所有已录入人员的全部信息，是否继续？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            //    if (MessageBox.Show("请再次确认是否清空所有信息？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //    {
            //        try
            //        {
            //            close1();
            //            string sqlDelete = @"delete from info where rowid='{0}'";
            //            sqlDelete = string.Format(sqlDelete, this.lblrowid.Text);
            //            connect();
            //            SQLiteCommand cmdDelete = new SQLiteCommand(sqlDelete, conn);
            //            cmdDelete.ExecuteNonQuery();

            //            MessageBox.Show("删除成功", "提示");
            //            clear();

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("数据删除失败，原因：" + ex.Message, "提示");
            //        }
            //        finally
            //        {
            //            close1();
            //            loadData();
            //        }

            //    }
            //}
            if (MessageBox.Show("确认删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = @"delete from info where rowid='{0}'";
                    sqlDelete = string.Format(sqlDelete, this.lblrowid.Text);
                    connect();
                    SQLiteCommand cmdDelete = new SQLiteCommand(sqlDelete, conn);
                    cmdDelete.ExecuteNonQuery();

                    MessageBox.Show("删除成功", "提示");
                    clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据删除失败，原因：" + ex.Message, "提示");
                }
                finally
                {
                    close1();
                    loadData();
                }
            }
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = @"delete from info where rowid='{0}'";
                    sqlDelete = string.Format(sqlDelete, this.lblrowid.Text);
                    connect();
                    SQLiteCommand cmdDelete = new SQLiteCommand(sqlDelete, conn);
                    cmdDelete.ExecuteNonQuery();

                    MessageBox.Show("删除成功", "提示");
                    clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据删除失败，原因：" + ex.Message, "提示");
                }
                finally
                {
                    close1();
                    loadData();
                }
            }
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            //ControlPaint.DrawBorder(e.Graphics,
            //                        this.groupBox1.ClientRectangle,
            //                        Color.Black,
            //                        1,
            //                        ButtonBorderStyle.Solid,
            //                        Color.Black,
            //                        1,
            //                        ButtonBorderStyle.Solid,
            //                        Color.Black,
            //                        1,
            //                        ButtonBorderStyle.Solid,
            //                        Color.Black,
            //                        1,
            //                        ButtonBorderStyle.Solid);
        }
        /// <summary>  
        /// 判断输入的字符串是否只包含数字和英文字母  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        private string returnRowIDByIdno(string idno)
        {
            string strRowid = "";
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                string strSql = @"select rowid from info where idno='{0}'";
                strSql = string.Format(strSql,idno);
                cmd.CommandText = strSql;
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    strRowid = reader[0].ToString();
                }
            }
            return strRowid;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (picImage.Image != null)
            {
                string strFileDir = "";
                SaveFileDialog sDig = new SaveFileDialog();
                sDig.FileName = txtIdNo.Text.Trim();
                sDig.Filter = "相片文件（*.jpg）|*.jpg|相片文件（*.png）|*.png";
                if (picImage.Image != null)
                {
                    if (sDig.ShowDialog() == DialogResult.OK)
                    {
                        strFileDir = sDig.FileName;
                        Bitmap bmp = new Bitmap(picImage.Image);
                        bmp.Save(strFileDir);
                        //SaveFileDialog save = new SaveFileDialog();
                        
                        MessageBox.Show("导出成功");
                    }
                }
            }
            else
            {
                MessageBox.Show("没有需要导出的相片！");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.printDialog1.ShowDialog();
        }

        private void btnPrintPriview_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtIdNo.Text) && !string.IsNullOrEmpty(cmbSex.Text))
            {
                this.printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先选择要打印的内容！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtIdNo.Text) && !string.IsNullOrEmpty(cmbSex.Text))
            {
                if (this.printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                } 
            }
            else
            {
                MessageBox.Show("请先选择要打印的内容！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {                          
            try
            {
                //XmlDocument xmlSetting = new XmlDocument();
                //xmlSetting.Load(xmlFile);//加载xml文件
                //XmlElement rootItem = xmlSetting.DocumentElement; //获取根节点
                //XmlNodeList nameNodes = rootItem.GetElementsByTagName("name"); //获取name子节点集合

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNode xn = xmlDoc.SelectSingleNode("printSet");
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xnf in xnl)
                {
                    XmlElement xe = (XmlElement)xnf;
                    XmlNodeList xnf1 = xe.ChildNodes;

                    foreach (XmlNode xn2 in xnf1)
                    {
                        MessageBox.Show(xn2.InnerText);
                    }
                }


                    Font font = new Font("宋体", 15);
                    Brush bru = Brushes.Blue;
                    
                    e.Graphics.DrawString("姓名：" + txtName.Text, font, bru,20, 20);
                    e.Graphics.DrawString("出生日期：" + dtBirthday.Value.ToString("yyyyMMdd"), font, bru, 20, 50);
                    e.Graphics.DrawString("性别：" + cmbSex.Text.ToString(), font, bru, 230, 50);
                    e.Graphics.DrawString("身份证：" + txtIdNo.Text.Trim(), font, bru, 20, 80);
                    e.Graphics.DrawString("地址：" + txtXZ.Text.Trim() + txtCun.Text.Trim(),font,bru, 20, 110);
               
            }
            catch (Exception ex)
            { throw ex; }
            finally { }
        }
        #region 菜单
        private void 个人信息新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnAdd_Click(sender, e);
        }

        private void 个人信息保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnSave_Click(sender, e);
        }

        private void 个人信息删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnDelete_Click(sender, e);
        }

        private void 打印ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.btnPrint_Click_1(sender, e);
        }
        #endregion

        private void 打印预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnPrintPriview_Click(sender, e);
        }

        private void 查询授权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnSQ_Click(sender, e);
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox2 about = new AboutBox2();
            about.ShowDialog();
        }

        private void 获取机器码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageInfo.safe.frmGetMachineID getID = new ImageInfo.safe.frmGetMachineID();
            getID.ShowDialog();
        }

        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageInfo.safe.frmRegister frm1 = new ImageInfo.safe.frmRegister();
            frm1.ShowDialog();
        }

        private void btnPlPrint_Click(object sender, EventArgs e)
        {
            ImageInfo.frmPlPrint frm1 = new ImageInfo.frmPlPrint();
            frm1.ShowDialog();
        }
    }
}
