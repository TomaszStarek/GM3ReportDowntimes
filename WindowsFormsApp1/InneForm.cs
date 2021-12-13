using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Timers;

namespace WindowsFormsApp1
{
    public partial class InneForm : Form
    {
        private static string _opisPomocnicza, _stacjaPomocnicza, _sekcjaPomocnicza;

        public InneForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            TimeCounter.SetTimer(textBox3);
            textBox2.Select();
            stacjaBox.Text = "M0";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            M0MainForm frm = new M0MainForm();
            frm.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            _stacjaPomocnicza = textBox2.Text;
            
        }
        private void stacjaBox_TextChanged(object sender, EventArgs e)
        {
            _sekcjaPomocnicza = stacjaBox.Text;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _opisPomocnicza = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string minutes = textBox3.Text;
            string opis = _opisPomocnicza;
            string stacja = _stacjaPomocnicza;
            string sekcja = _sekcjaPomocnicza;
            string cmdString = "INSERT INTO [dbo].[awarieGM3] ([sekcja],[stacja],[opis],[min],[czas_start],[czas_stop]) VALUES ( @val2, @val3, @val4, @val5, @val6, @val7)";
            string connString = @GlobalDataClass.ConnectionString; 
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    //  comm.Parameters.AddWithValue("@val1", "1");
                    comm.Parameters.AddWithValue("@val2", sekcja);
                    comm.Parameters.AddWithValue("@val3", stacja);
                    comm.Parameters.AddWithValue("@val4", opis);
                    comm.Parameters.AddWithValue("@val5", minutes);
                    comm.Parameters.AddWithValue("@val6", @GlobalDataClass.StartAwarii.ToString("yyyy-MM-ddThh:mm:ss.fff"));
                    comm.Parameters.AddWithValue("@val7", @DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fff"));
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("exception: " + ex);
                    }
                }
            }

            M0MainForm frm = new M0MainForm();
            frm.Show();
            this.Hide();
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (!GlobalDataClass.IsItWindows)
                Onboard.Run();
            TimeCounter.CounterTime.Stop();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (!GlobalDataClass.IsItWindows)
                Onboard.Run();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (!GlobalDataClass.IsItWindows)
                Onboard.Run();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if(textBox3.Text.Length == 0)
                TimeCounter.CounterTime.Start();
        }

    }
}
