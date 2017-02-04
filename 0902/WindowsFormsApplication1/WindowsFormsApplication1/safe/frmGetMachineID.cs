using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageInfo.safe
{
    public partial class frmGetMachineID : Form
    {
        WindowsFormsApplication1.safe.getMachineID getID = new WindowsFormsApplication1.safe.getMachineID();
        public frmGetMachineID()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtMachineID.Text = getID.getMNum();
        }

        private void rtMachineID_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(rtMachineID.Text))
                {
                    rtMachineID.SelectAll();
                    Clipboard.SetDataObject(rtMachineID.Text.Trim());
                    MessageBox.Show("机器码已经复制到系统剪切板中！", "提示");
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally{}
        }
    }
}
