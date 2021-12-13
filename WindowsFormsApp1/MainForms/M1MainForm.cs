using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class M1MainForm : Form
    {
        public M1MainForm()
        {
            InitializeComponent();
            TimeCounter.SetTimer(label99);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void button_Click(object sender, EventArgs e)
        {
         //   Program.global_czasstart = DateTime.Now;

            Button clickedButton = (Button)sender;
            GlobalDataClass.StationName = @clickedButton.Text;
            G020Form frm = new G020Form();
            frm.Show();
            Thread.Sleep(100);
            this.Hide();

        }
        private void button27_Click(object sender, EventArgs e)
        {

            StartMenu frm = new StartMenu();

            frm.Show();
            Thread.Sleep(100);
            foreach (Form form in Application.OpenForms)
            {
                if (form != frm)
                    form.Hide();
            }

            this.Hide();
        }
    }
}
