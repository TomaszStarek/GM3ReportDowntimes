using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Diagnostics;


namespace WindowsFormsApp1
{
    public partial class M0MainForm : Form
    {
        public M0MainForm()
        {
            InitializeComponent();
            TimeCounter.SetTimer(label1);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void button_Click(object sender, EventArgs e)
        {
            //Program.global_czasstart = DateTime.Now;

            Button clickedButton = (Button)sender;
            GlobalDataClass.StationName = @clickedButton.Text;
            G020Form frm = new G020Form();
            frm.Show();
            Thread.Sleep(100);
            this.Hide();

        }
        private void button6_Click(object sender, EventArgs e)
        {           
            StartMenu frm = new StartMenu();

            frm.Show();
            Thread.Sleep(100);
            foreach (Form form in Application.OpenForms)
            {
                if (form != frm)
                    form.Hide();
            }
         //   frm.Show();
            this.Hide();
            
        }

    }
}
