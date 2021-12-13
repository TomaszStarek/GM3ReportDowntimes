using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class Sql
    {
        private static DataTable dt;
        private static SqlDataAdapter da;
        private static DataSet ds;

        public static void FillCombobox(ComboBox comboBox1)
        {
            string cmdString = "SELECT ([opis]) FROM [dbo].[awarieGM3] WHERE [stacja] = '" + GlobalDataClass.StationName + "'" + " AND [sekcja] = 'M0' GROUP BY ([opis]) ORDER BY COUNT(*) DESC OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;";    //OFFSET 3 ROWS FETCH NEXT 3 ROWS ONLY

            using (SqlConnection conn = new SqlConnection(GlobalDataClass.ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {

                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    try
                    {
                        da = new SqlDataAdapter(comm.CommandText, comm.Connection);
                        ds = new DataSet();
                        da.Fill(ds, "asd");

                        dt = ds.Tables["asd"];


                        int i;
                        for (i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            string pom = dt.Rows[i].ItemArray[0].ToString();
                            if (!comboBox1.Items.Contains(pom))
                                comboBox1.Items.Add(pom);

                        }
                        if (!comboBox1.Items.Contains("INNE"))
                            comboBox1.Items.Add("INNE");

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex);
                    }
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex);
                    }
                }
            }
        }

        public static void AskDbAndFillButtonsWithMostFrequentText(Button button1, Button button2, Button button3, Button button4)
        {

            string cmdString = "SELECT ([opis]) FROM [dbo].[awarieGM3] WHERE [stacja] = '" + GlobalDataClass.StationName + "'" + " AND [sekcja] = 'M0' GROUP BY ([opis]) ORDER BY COUNT(*) DESC OFFSET 0 ROWS FETCH NEXT 3 ROWS ONLY;";    //OFFSET 3 ROWS FETCH NEXT 3 ROWS ONLY

            using (SqlConnection conn = new SqlConnection(GlobalDataClass.ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {

                    comm.Connection = conn;
                    comm.CommandText = cmdString;

                    try
                    {
                        da = new SqlDataAdapter(comm.CommandText, comm.Connection);
                        ds = new DataSet();
                        da.Fill(ds, "asd");

                        dt = ds.Tables["asd"];

                        if (dt.Rows.Count > 0)
                            button1.Text = dt.Rows[0].ItemArray[0].ToString();
                        if (dt.Rows.Count > 1)
                            button2.Text = dt.Rows[1].ItemArray[0].ToString();
                        if (dt.Rows.Count > 2)
                            button3.Text = dt.Rows[2].ItemArray[0].ToString();
                        if (dt.Rows.Count > 3)
                        {
                            button4.Text = dt.Rows[3].ItemArray[0].ToString();
                            button4.Visible = true;
                        }

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex);
                    }
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex);
                    }
                }
            }
        }

        public static void SendDataToDb(string stacja, string opis, string komentarzToDb, string minutes)
        {

            string cmdString = "INSERT INTO [dbo].[awarieGM3] ([sekcja],[stacja],[opis],[komentarz],[min],[czas_start],[czas_stop]) VALUES ( @val2, @val3, @val4, @val5, @val6, @val7, @val8)";

            using (SqlConnection conn = new SqlConnection(GlobalDataClass.ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {

                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    //  comm.Parameters.AddWithValue("@val1", "1");             //ID unnessesary
                    comm.Parameters.AddWithValue("@val2", "M0");
                    comm.Parameters.AddWithValue("@val3", GlobalDataClass.StationName);
                    comm.Parameters.AddWithValue("@val4", opis);

                    comm.Parameters.AddWithValue("@val5", komentarzToDb);
                    comm.Parameters.AddWithValue("@val6", minutes);
                    //comm.Parameters.AddWithValue("@val6", @Program.global_czasstart.ToString("yyyy/MM/dd HH:mm:ss"));
                    //comm.Parameters.AddWithValue("@val7", @DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    comm.Parameters.AddWithValue("@val7", @GlobalDataClass.StartAwarii.ToString("yyyy-MM-ddThh:mm:ss.fff"));
                    comm.Parameters.AddWithValue("@val8", @DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fff"));
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

            StartMenu frm = new StartMenu();

            foreach (Form form in Application.OpenForms)
            {
                form.Hide();
            }

        //    this.Hide();
            frm.Show();
        }

    }
}
