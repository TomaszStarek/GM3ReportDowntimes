using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class StartMenu : Form
    {
        public StartMenu()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            GlobalDataClass.StartAwarii = DateTime.Now;

            if (GlobalDataClass.SectionName.Contains("M0"))
            {
                M0MainForm m0Form = new M0MainForm();
                m0Form.Show();
            }
            else if (GlobalDataClass.SectionName.Contains("M1"))
            {
                M1MainForm m1Form = new M1MainForm();
                m1Form.Show();
            }
            else if (GlobalDataClass.SectionName.Contains("M2"))
            {
                M2MainForm m2Form = new M2MainForm();
                m2Form.Show();
            }
            else if (GlobalDataClass.SectionName.Contains("M3A"))
            {
                M3aMainForm m3aForm = new M3aMainForm();
                m3aForm.Show();
            }
            else if (GlobalDataClass.SectionName.Contains("M3B"))
            {
                M3bMainForm m3bForm = new M3bMainForm();
                m3bForm.Show();
            }
            else if (GlobalDataClass.SectionName.Contains("M4"))
            {
                M4MainForm m4Form = new M4MainForm();
                m4Form.Show();
            }
            else if (GlobalDataClass.SectionName.Contains("M5"))
            {
                M5MainForm m5Form = new M5MainForm();
                m5Form.Show();
            }

            Thread.Sleep(100);
            this.Hide();

        }

        private void button28_Click(object sender, EventArgs e)
        {
            GlobalDataClass.StartAwarii = DateTime.Now;

            Button clickedButton = (Button)sender;
            GlobalDataClass.StationName = @clickedButton.Text;

            G020Form frm = new G020Form();
            frm.Show();
            Thread.Sleep(100);
            this.Hide();
        }

        private void button_quit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
