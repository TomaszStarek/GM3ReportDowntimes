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
using System.Data.SqlClient;
using System.Timers;
using System.Runtime.InteropServices;


namespace WindowsFormsApp1
{

    public partial class G020Form : Form
    {

        private static string _komentarzToDb;
        private static string _minutes;
        private static string _opisToDb;

        private ChangeControls _changeControls;

        public G020Form()
        {
            InitializeComponent();

            _changeControls = new ChangeControls();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.Text = GlobalDataClass.StationName;
            _changeControls.UpdateControl(button7, SystemColors.ActiveCaption, GlobalDataClass.StationName, true);
            ChangeButtonTextToDefault();

            WindowState = FormWindowState.Maximized;
            //  SetTimer();
            TimeCounter.SetTimer(label1);
            _komentarzToDb = "brak";

            comboBox1.KeyDown += comboBox1_KeyDown;
            textBox1.KeyDown += textBox1_KeyDown;

            comboBox1.ItemHeight = 120;
            if (button4.Text.Length < 2)
                button4.Visible = false;
            /////////////////////////////////////// aktualizowanie tekstu przycisków według wystąpięń w bazie danych
            //try
            //{
            //    pobieranie();
            //}
            //catch (SqlException ex)
            //{
            //    MessageBox.Show("Exception: " + ex);
            //}
            ///////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void ChangeButtonTextToDefault()
        {
            if (GlobalDataClass.SectionName.Contains("M0"))
                switch (GlobalDataClass.StationName)
            {
                case @"Manipulator G030":
                    _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ MOLDA", true);
                    _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZAWIESZONY HOMOWANIE", true);
                    _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                    _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                    break;
                case @"Robot R010":
                    _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ MOLDA", true);
                    _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "UPUŚCIŁ MOLDA", true);
                    _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UDERZYŁ W KARTON", true);
                    _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ŹLE ODŁOŻYŁ MOLDA", true);
                    //if (!comboBox1.Items.Contains("HOMOWANIE"))
                    //    comboBox1.Items.Add("HOMOWANIE");
                    break;
                case @"Conveyor OP10":
                    _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANE MOLDY", true);
                    _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                    _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY PAS TRANSMISYJNY", true);
                    _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                    break;

                case @"LPA":
                    _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                    _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                    _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                    _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                    _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                    _minutes = "0";
                    _opisToDb = "LPA";
                    ConfirmMinutes();
                    //QuestionAboutAddComment();
                    break;

                case @"INNE":
                    _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                    _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                    _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                    _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                    if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                        comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                    if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                        comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                    if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                        comboBox1.Items.Add("Brak pradu".ToUpper());
                    break;

                default:
                    _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                    _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                    _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                    _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                    break;

            }
            else if (GlobalDataClass.SectionName.Contains("M1"))
                switch (GlobalDataClass.StationName)
                {
                    case @"Flag Shaft G020":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONT.KOMP", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "CHWYTAK NIE POBRAŁ KOMP.", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CHWYTAK", true);
                        break;
                    case @"Oring G030":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIE POBRANY KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tulejka G031":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIE POBRANY KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Prawa Flaga G040":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIE POBRANY KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Lewa Flaga G041":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIE POBRANY KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Montaż Flag G044":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIE POBRANY KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Podajnik Lever G050":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOK. KOMP W PODAJNIKU WIBR.", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIE POBRANY KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Montaż Lever G053":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "CHWYTAK NIE POBRAŁ KOMP.", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CHWYTAK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ŹLE ZAMONTOWANY KOMP.", true);
                        if (!comboBox1.Items.Contains("ZAWIESZONY SYSTEM WIZYJNY"))
                            comboBox1.Items.Add("ZAWIESZONY SYSTEM WIZYJNY");
                        break;
                    case @"R010":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMP.", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"R030":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONT. KOMP.", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"R040":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMP.", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"R050":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMP.", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"Conveyor Wjazd OP00":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CONVEYOR", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ŻLE WŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZGUBIONY PUNKT ZEROWY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Conveyor Wyjazd OP10":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD Z M0", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIE PUSZCZA MOLDÓW", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Reject OP80.1":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PEŁNY REJECT", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOK. KOMPONENT W REJECT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"LPA":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                        _minutes = "0";
                        _opisToDb = "LPA";
                        ConfirmMinutes();
                        //     pytanie_komentarz();
                        break;

                    case @"INNE":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                        if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                        if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                        if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                            comboBox1.Items.Add("Brak pradu".ToUpper());
                        break;
                    default:
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                }
            else if (GlobalDataClass.SectionName.Contains("M2"))
                switch (GlobalDataClass.StationName)
                {
                    case @"Przenośnik Palet G000":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CONVEYOR", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ŹLE WŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ZGUBIONY PUNKT ZEROWY", true);
                        break;
                    case @"Conv_wjazd OP00":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIE PUSZCZA MOLDÓW", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM ZE SKANEREM", true);
                        break;
                    case @"Conv_wjazd T001":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD Z M0", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"R010.2":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT NA PRZENOŚNIK PALET", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT NA TESTER", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO POBRAŁ KOMPONENT", true);
                        if (!comboBox1.Items.Contains("UPUŚCIŁ KOMPONENT"))
                            comboBox1.Items.Add("UPUŚCIŁ KOMPONENT");
                        if (!comboBox1.Items.Contains("PROBLEM Z CHWYTAKIEM"))
                            comboBox1.Items.Add("PROBLEM Z CHWYTAKIEM");
                        if (!comboBox1.Items.Contains("PROBLEM Z CZUJNIKIEM"))
                            comboBox1.Items.Add("PROBLEM Z CZUJNIKIEM");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"Stół Gniazd Memb G020":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY STÓŁ", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "PROBLEM Z GNIAZDEM MEMBRAN", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Manipulator Memb G021":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Stół Zgrzew Memb G022":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY STÓŁ", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONE GNIAZDO", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA ŚRUBY/PODPORY POD WELDING", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Plate G023":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Zgrzew Memb G025":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW ZGRZEWANIA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA SONOTRODA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "BŁĘDY NA GENERATORZE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Counter Plate G024":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Manip Memb Mold G026":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CHWYTAK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        if (!comboBox1.Items.Contains("PROBLEM Z KOMPONENTEM"))
                            comboBox1.Items.Add("PROBLEM Z KOMPONENTEM");
                        break;
                    case @"Wibracyjny Coun Plate G027":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Wibracyjny Plate G028":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Wizja Memb G030":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONA KAMERA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "CZYSZCZENIE KAMERY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW KAMERY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USTAWIANIE KAMERY", true);
                        break;
                    case @"Stół Bridel G040":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY STÓŁ", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONE GNIAZDO RAMEK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Manip Bridel G042":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CHWYTAK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("USZKODZONY CHWYTAK");
                        if (!comboBox1.Items.Contains("USZKODZONY SIŁOWNIK"))
                            comboBox1.Items.Add("USZKODZONY SIŁOWNIK");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        break;
                    case @"Zgrzew Ramek1 G050":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW ZGRZEWANIA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA SONOTRODA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY POPYCHACZ", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY DOCISK", true);
                        if (!comboBox1.Items.Contains("USTAWIENIE POPYCHACZA/DOCISKU"))
                            comboBox1.Items.Add("USTAWIENIE POPYCHACZA/DOCISKU");
                        if (!comboBox1.Items.Contains("BŁĘDY NA GENERATORZE"))
                            comboBox1.Items.Add("BŁĘDY NA GENERATORZE");
                        break;
                    case @"Zgrzew Ramek2 G052":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW ZGRZEWANIA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA SONOTRODA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY POPYCHACZ", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY DOCISK", true);
                        if (!comboBox1.Items.Contains("USTAWIENIE POPYCHACZA/DOCISKU"))
                            comboBox1.Items.Add("USTAWIENIE POPYCHACZA/DOCISKU");
                        if (!comboBox1.Items.Contains("BŁĘDY NA GENERATORZE"))
                            comboBox1.Items.Add("BŁĘDY NA GENERATORZE");
                        break;
                    case @"Test Wys Memb G060":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW/REGULACJA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tester G070.2":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZAWIESZONY TESTER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "INTERWENCJA DZIAŁU TE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tester G071.2":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZAWIESZONY TESTER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "INTERWENCJA DZIAŁU TE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Reject G080.2":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PEŁNY REJECT", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W REJECT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Obracanie Mold G090":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CHWYTAK", true);
                        break;
                    case @"LPA":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                        _minutes = "0";
                        _opisToDb = "LPA";
                        ConfirmMinutes();
                        //     pytanie_komentarz();

                        break;

                    case @"INNE":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                        if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                        if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                        if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                            comboBox1.Items.Add("Brak pradu".ToUpper());
                        break;
                    default:
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                }
            else if (GlobalDataClass.SectionName.Contains("M3A"))
                switch (GlobalDataClass.StationName)
                {
                    case @"Conv_wjazd TR01A":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIE PUSZCZA MOLDÓW", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM ZE SKANEREM", true);
                        break;
                    case @"Conv_wyjazd TR02A":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ŹLE ODŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Reject OP10A":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PEŁNY REJECT", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W REJECT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"R010A":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ MOLDA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT NA TESTER", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO POBRAŁ SHELL", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ SHELL DO BRANSONA", true);
                        if (!comboBox1.Items.Contains("UPUŚCIŁ KOMPONENT"))
                            comboBox1.Items.Add("UPUŚCIŁ KOMPONENT");
                        if (!comboBox1.Items.Contains("PROBLEM Z CHWYTAKIEM"))
                            comboBox1.Items.Add("PROBLEM Z CHWYTAKIEM");
                        if (!comboBox1.Items.Contains("PROBLEM Z CZUJNIKIEM"))
                            comboBox1.Items.Add("PROBLEM Z CZUJNIKIEM");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"Podajnik pasowyT020":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY SHELL", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SILNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY PAS", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Podajnik Zasypowy L20":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY PAS", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SILNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZABLOKOWANE KOMPONENTY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK KLAPY", true);
                        if (!comboBox1.Items.Contains("USZKODZONA KLAPA"))
                            comboBox1.Items.Add("USZKODZONA KLAPA");
                        break;
                    case @"Podajnik Wibr Shell L20":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY SHELL", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE MECHANICZNE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        break;
                    case @"Sortownik G020":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK OBROTOWY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY NAPĘD LINIOWY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONE GNIAZDO", true);
                        break;
                    case @"Tester G030.3A":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZAWIESZONY TESTER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "INTERWENCJA DZIAŁU TE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Branson Shell G040A":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONE DRZWI", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY NAPĘD DRZWI", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONE NARZĘDZIE ZGRZEWAJĄCE", true);
                        if (!comboBox1.Items.Contains("PROBLEM Z HYDRAULIKĄ"))
                            comboBox1.Items.Add("PROBLEM Z HYDRAULIKĄ");
                        if (!comboBox1.Items.Contains("REGULACJE MECHANICZNE"))
                            comboBox1.Items.Add("REGULACJE MECHANICZNE");
                        if (!comboBox1.Items.Contains("REGULACJA PODCIŚNIENIA GNIAZDA"))
                            comboBox1.Items.Add("REGULACJA PODCIŚNIENIA GNIAZDA");
                        if (!comboBox1.Items.Contains("ZAKŁÓCONY CYKL"))
                            comboBox1.Items.Add("ZAKŁÓCONY CYKL");
                        break;
                    case @"Próbki G050":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIEZAMKNIĘTA SZUFLADA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"LPA":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                        _minutes = "0";
                        _opisToDb = "LPA";
                        ConfirmMinutes();
                        //     pytanie_komentarz();

                        break;


                    case @"INNE":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                        if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                        if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                        if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                            comboBox1.Items.Add("Brak pradu".ToUpper());
                        break;
                    default:
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                }
            else if (GlobalDataClass.SectionName.Contains("M3B"))
                switch (GlobalDataClass.StationName)
                {
                    case @"Conv_wjazd TR01B":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIE PUSZCZA MOLDÓW", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM ZE SKANEREM", true);
                        break;
                    case @"Conv_wyjazd TR02B":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ŹLE ODŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Reject OP10B":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PEŁNY REJECT", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W REJECT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"R010B":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ MOLDA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT NA TESTER", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO POBRAŁ SHELL", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ SHELL DO BRANSONA", true);
                        if (!comboBox1.Items.Contains("UPUŚCIŁ KOMPONENT"))
                            comboBox1.Items.Add("UPUŚCIŁ KOMPONENT");
                        if (!comboBox1.Items.Contains("PROBLEM Z CHWYTAKIEM"))
                            comboBox1.Items.Add("PROBLEM Z CHWYTAKIEM");
                        if (!comboBox1.Items.Contains("PROBLEM Z CZUJNIKIEM"))
                            comboBox1.Items.Add("PROBLEM Z CZUJNIKIEM");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"Podajnik pasowyT021":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY SHELL", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SILNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY PAS", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Podajnik Zasypowy L21":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY PAS", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SILNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZABLOKOWANE KOMPONENTY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK KLAPY", true);
                        if (!comboBox1.Items.Contains("USZKODZONA KLAPA"))
                            comboBox1.Items.Add("USZKODZONA KLAPA");
                        break;
                    case @"Podajnik Wibr Shell L21":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY SHELL", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE MECHANICZNE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        break;
                    case @"Sortownik G021":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK OBROTOWY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY NAPĘD LINIOWY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONE GNIAZDO", true);
                        break;
                    case @"Tester G031.3B":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZAWIESZONY TESTER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "INTERWENCJA DZIAŁU TE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Branson Shell G040B":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONE DRZWI", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY NAPĘD DRZWI", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONE NARZĘDZIE ZGRZEWAJĄCE", true);
                        if (!comboBox1.Items.Contains("PROBLEM Z HYDRAULIKĄ"))
                            comboBox1.Items.Add("PROBLEM Z HYDRAULIKĄ");
                        if (!comboBox1.Items.Contains("REGULACJE MECHANICZNE"))
                            comboBox1.Items.Add("REGULACJE MECHANICZNE");
                        if (!comboBox1.Items.Contains("REGULACJA PODCIŚNIENIA GNIAZDA"))
                            comboBox1.Items.Add("REGULACJA PODCIŚNIENIA GNIAZDA");
                        if (!comboBox1.Items.Contains("ZAKŁÓCONY CYKL"))
                            comboBox1.Items.Add("ZAKŁÓCONY CYKL");
                        break;
                    case @"Próbki G051":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIEZAMKNIĘTA SZUFLADA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"LPA":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                        _minutes = "0";
                        _opisToDb = "LPA";
                        ConfirmMinutes();
                        //     pytanie_komentarz();

                        break;


                    case @"INNE":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                        if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                        if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                        if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                            comboBox1.Items.Add("Brak pradu".ToUpper());
                        break;
                    default:
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                }
            else if (GlobalDataClass.SectionName.Contains("M4"))
                switch (GlobalDataClass.StationName)
                {
                    case @"conv_wjazd":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ŻLE ODŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Conveyor Valve Cover T060":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KARTON", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "PROBLEM ZE STOPEREM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA OBECNOŚCI KARTONU", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA SZEROKOŚCI TRANSPORTU", true);
                        if (!comboBox1.Items.Contains("USZKODZONY SILNIK"))
                            comboBox1.Items.Add("USZKODZONY SILNIK");
                        break;
                    case @"Conveyor Valve Seat T061":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KARTON", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "PROBLEM ZE STOPEREM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA OBECNOŚCI KARTONU", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA SZEROKOŚCI TRANSPORTU", true);
                        if (!comboBox1.Items.Contains("USZKODZONY SILNIK"))
                            comboBox1.Items.Add("USZKODZONY SILNIK");
                        break;
                    case @"ConveyorWy Pusty Kart T063":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KARTON", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "PROBLEM ZE STOPEREM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA OBECNOŚCI KARTONU", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA SZEROKOŚCI TRANSPORTU", true);
                        if (!comboBox1.Items.Contains("USZKODZONY SILNIK"))
                            comboBox1.Items.Add("USZKODZONY SILNIK");
                        break;
                    case @"cnv_wyjazd":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ŻLE ODŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Przenośnik Palet OP00":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY CONVEYOR", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ŹLE WŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ZGUBIONY PUNKT ZEROWY", true);
                        break;

                    case @"Załadunek OP10":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIEPOBRANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY/USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM ZE SKANEREM", true);
                        if (!comboBox1.Items.Contains("ŹLE POBRANY MOLD"))
                            comboBox1.Items.Add("ŹLE POBRANY MOLD");
                        if (!comboBox1.Items.Contains("ŹLE ODŁOŻONY MOLD"))
                            comboBox1.Items.Add("ŹLE ODŁOŻONY MOLD");
                        break;
                    case @"Montaż Shafta OP50":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZAKŁÓCONY CYKL", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM Z PODAJNIKIEM KOMPONENTU", true);
                        break;

                    case @"R060":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PROBLEM Z POBIERANIEM VALVE SEAT", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "PROBLEM Z POBIERANIEM VALVE COVER", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "PROBLEM Z ODKŁADANIEM VALVE SEAT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLREM Z ODKŁADANIEM VALVE COVER", true);
                        if (!comboBox1.Items.Contains("USZKODZONY GRIPPER"))
                            comboBox1.Items.Add("USZKODZONY GRIPPER");
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("KOLIZJA ROBOTA"))
                            comboBox1.Items.Add("KOLIZJA ROBOTA");
                        if (!comboBox1.Items.Contains("ZMIANA POZYCJI ROBOTA"))
                            comboBox1.Items.Add("ZMIANA POZYCJI ROBOTA");
                        if (!comboBox1.Items.Contains("ZAWIESZONY KOMPUTER SKANERA 3D"))
                            comboBox1.Items.Add("ZAWIESZONY KOMPUTER SKANERA 3D");
                        if (!comboBox1.Items.Contains("ZAWIESZONY SKANER 3D - CAŁY CZAS SKANUJE"))
                            comboBox1.Items.Add("ZAWIESZONY SKANER 3D - CAŁY CZAS SKANUJE");
                        if (!comboBox1.Items.Contains("USZKODZONY SKANER 3D"))
                            comboBox1.Items.Add("USZKODZONY SKANER 3D");
                        break;



                    case @"Klejarka G020":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USTAWIENIE/REGULACJA IGŁY", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "CZYSZCZENIE/WYMIANA IGŁY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZMIANA TRAJEKTORI KLEJU", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ZMIANA WAGI KLEJU", true);
                        if (!comboBox1.Items.Contains("BŁĘDNY POMIAR WAGI"))
                            comboBox1.Items.Add("BŁĘDNY POMIAR WAGI");
                        if (!comboBox1.Items.Contains("RESTART APLIKACJI"))
                            comboBox1.Items.Add("RESTART APLIKACJI");
                        if (!comboBox1.Items.Contains("PROBLEM Z ZAWOREM KLEJU"))
                            comboBox1.Items.Add("PROBLEM Z ZAWOREM KLEJU");
                        if (!comboBox1.Items.Contains("USZKODZONY WĄŻ"))
                            comboBox1.Items.Add("USZKODZONY WĄŻ");
                        break;
                    case @"Klejarka G030":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USTAWIENIE/REGULACJA IGŁY", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "CZYSZCZENIE/WYMIANA IGŁY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZMIANA TRAJEKTORI KLEJU", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ZMIANA WAGI KLEJU", true);
                        if (!comboBox1.Items.Contains("BŁĘDNY POMIAR WAGI"))
                            comboBox1.Items.Add("BŁĘDNY POMIAR WAGI");
                        if (!comboBox1.Items.Contains("RESTART APLIKACJI"))
                            comboBox1.Items.Add("RESTART APLIKACJI");
                        if (!comboBox1.Items.Contains("PROBLEM Z ZAWOREM KLEJU"))
                            comboBox1.Items.Add("PROBLEM Z ZAWOREM KLEJU");
                        if (!comboBox1.Items.Contains("USZKODZONY WĄŻ"))
                            comboBox1.Items.Add("USZKODZONY WĄŻ");
                        break;
                    case @"Profilometr G040":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY PROFILOMETR", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "RESTART APLIKACJI", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "ZMIANA PARAMETRÓW RECEPTURY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Manipulator Przekładki G061":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBIERA PRZEKŁADKI", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA SSAWKA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "PROBLEM/REGULACJE NAPED LINIOWY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        if (!comboBox1.Items.Contains("KOLIZJA Z ROBOTEM"))
                            comboBox1.Items.Add("KOLIZJA Z ROBOTEM");
                        if (!comboBox1.Items.Contains("NIE POBIERA KARTONU"))
                            comboBox1.Items.Add("NIE POBIERA KARTONU");
                        if (!comboBox1.Items.Contains("USZKODZONE SZPILKI DO POBIERANIA KARTONU"))
                            comboBox1.Items.Add("USZKODZONE SZPILKI DO POBIERANIA KARTONU");
                        if (!comboBox1.Items.Contains("USZKODZONY EJECTOR"))
                            comboBox1.Items.Add("USZKODZONY EJECTOR");
                        if (!comboBox1.Items.Contains("USZKODZONY/REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("USZKODZONY/REGULACJA CZUJNIKA");
                        break;
                    case @"Docisk G062":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJA DOCISKU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Bufor G063":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "REGULACJA GNIAZDA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Docisk G064":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJA SIŁY DOCISKU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"Manipulator Roz G070":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIEPOBRANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY/USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ŻLE POBRANY MOLD", true);
                        if (!comboBox1.Items.Contains("ŹLE ODŁOŻONY MOLD"))
                            comboBox1.Items.Add("ŹLE ODŁOŻONY MOLD");
                        if (!comboBox1.Items.Contains("USZKODZONY GRIPPER"))
                            comboBox1.Items.Add("USZKODZONY GRIPPER");
                        if (!comboBox1.Items.Contains("USZKODZONY/REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("USZKODZONY/REGULACJA CZUJNIKA");
                        break;

                    case @"Skaner G071":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJA SKANERA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "WYMIANA SKANERA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"Reject G080":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "REGULACJA/WYMIANA CZUJNIKA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"LPA":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                        _minutes = "0";
                        _opisToDb = "LPA";
                        ConfirmMinutes();
                        //     pytanie_komentarz();

                        break;

                    case @"INNE":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                        if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                        if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                        if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                            comboBox1.Items.Add("Brak pradu".ToUpper());
                        break;
                    default:
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                }
            else if (GlobalDataClass.SectionName.Contains("M5"))
                switch (GlobalDataClass.StationName)
                {
                    case @"Conveyor wjazd T000":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PROWADNICY", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJA/USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("REGULACJA/USZKODZONY CZUJNIK");
                        break;
                    case @"Przenosnik palet OP00":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ŹLE WŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "ZGUBIONY PUNKT ZEROWY", true);
                        break;
                    case @"Skaner OP10":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJA SKANERA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "WYMIANA SKANERA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Przenośnik Wyjściowy T001":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ŹLE ODŁOŻONY MOLD", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY MOLD", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONA PROWADNICA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"R010.5":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLREM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        break;
                    case @"R050.5":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLEM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        break;
                    case @"R080.5":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLREM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("KOMUNIKAT ES STOP, RESTART KONTROLERA"))
                            comboBox1.Items.Add("KOMUNIKAT ES STOP, RESTART KONTROLERA");
                        break;
                    case @"R090.5":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ODŁOŻYŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "PROBLREM Z CHWYTAKIEM", true);
                        if (!comboBox1.Items.Contains("HOMOWANIE"))
                            comboBox1.Items.Add("HOMOWANIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        break;

                    case @"Docieranie G020":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY VALVE COVER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SILNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA/USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJE STANOWISKA", true);
                        if (!comboBox1.Items.Contains("USZKODZONY SIŁOWNIK"))
                            comboBox1.Items.Add("USZKODZONY SIŁOWNIK");
                        break;
                    case @"Podajnik GearsSupport G030":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        if (!comboBox1.Items.Contains("NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT"))
                            comboBox1.Items.Add("NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT");
                        if (!comboBox1.Items.Contains("UPUŚCIŁ KOMPONENT"))
                            comboBox1.Items.Add("UPUŚCIŁ KOMPONENT");
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Manipulator Support G031":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Wkrętak G032.1":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANA ŚRUBA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY/REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        if (!comboBox1.Items.Contains("USTAWIENIA/REGULACJE"))
                            comboBox1.Items.Add("USTAWIENIA/REGULACJE");
                        if (!comboBox1.Items.Contains("BŁĄD NA OŚ Z NIEPRAWIDŁOWO TRAFIŁ ŚRUBKĄ"))
                            comboBox1.Items.Add("BŁĄD NA OŚ Z NIEPRAWIDŁOWO TRAFIŁ ŚRUBKĄ");
                        break;
                    case @"Podajnik śrubek G032.2":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANA ŚRUBA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIE PODAŁ ŚRUBY", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        break;
                    case @"Pozycjoner Mold G033":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY GRIPPER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Test. Wkręcenia śrub G034":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Podajnik MiddelGear G040":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        if (!comboBox1.Items.Contains("NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT"))
                            comboBox1.Items.Add("NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT");
                        if (!comboBox1.Items.Contains("UPUŚCIŁ KOMPONENT"))
                            comboBox1.Items.Add("UPUŚCIŁ KOMPONENT");
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Podajnik DrivingGear G050":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        if (!comboBox1.Items.Contains("NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT"))
                            comboBox1.Items.Add("NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT");
                        if (!comboBox1.Items.Contains("UPUŚCIŁ KOMPONENT"))
                            comboBox1.Items.Add("UPUŚCIŁ KOMPONENT");
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;

                    case @"Pozycjoner Mold G051":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY GRIPPER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"Holder Supportu G052":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJA/USTAWIENIE", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY HOLDER", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"Podajnik ReverseLock G060":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        break;

                    case @"Manipulator Reverse G061":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Holder Supportu G062":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "REGULACJA/USTAWIENIA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY HOLDER", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Pozycjoner Mold G063":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY GRIPPER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY HOLDER", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Podajnik Crank G070":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        break;
                    case @"Manipulator Crank G071":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "NIE POBRAŁ KOMPONENTU", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "NIEPRAWIDŁOWO ZAMONTOWANY KOMPONENT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "UPUŚCIŁ KOMPONENT", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Pozycjoner Mold G072":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY GRIPPER", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tightness Tester G081":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "INTERWENCJA TE", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tightness Tester G082":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "INTERWENCJA TE", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tightness Tester G083":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "INTERWENCJA TE", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Tightness Tester G084":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "INTERWENCJA TE", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Podajnik Link G090":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU LINIOWYM", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZABLOKOWANY KOMPONENT W PODAJNIKU BĘBNOWYM", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA PODAJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        if (!comboBox1.Items.Contains("NIE POBRAŁ KOMPONENTU"))
                            comboBox1.Items.Add("NIE POBRAŁ KOMPONENTU");
                        if (!comboBox1.Items.Contains("USZKODZONY CZUJNIK"))
                            comboBox1.Items.Add("USZKODZONY CZUJNIK");
                        if (!comboBox1.Items.Contains("REGULACJE/USTAWIENIE"))
                            comboBox1.Items.Add("REGULACJE/USTAWIENIE");
                        if (!comboBox1.Items.Contains("REGULACJA CZUJNIKA"))
                            comboBox1.Items.Add("REGULACJA CZUJNIKA");
                        if (!comboBox1.Items.Contains("PROBLEM Z KOMPONENTEM"))
                            comboBox1.Items.Add("PROBLEM Z KOMPONENTEM");
                        if (!comboBox1.Items.Contains("CZYSZCZENIE/USZKODZONY CHWYTAK"))
                            comboBox1.Items.Add("CZYSZCZENIE/USZKODZONY CHWYTAK");
                        break;
                    case @"Pozycjoner Holder G091":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "USZKODZONY TOPOR", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "USZKODZONY SIŁOWNIK", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJE/USTAWIENIE", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        break;
                    case @"PressureLoss Tester G100":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PROBLEM Z POMPĄ PODCIŚNIENIA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "INTERWENCJA TE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"PressureLoss Tester G101":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PROBLEM Z POMPĄ PODCIŚNIENIA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "INTERWENCJA TE", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "REGULACJA CZUJNIKA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;
                    case @"Reject G110":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "PEŁNY REJECT", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "ZBLOKOWANY KOMPONENT W REJECT", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "USZKODZONY CZUJNIK", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                    case @"INNE":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "Brak komponentu - operator".ToUpper(), true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "Brak operatorow".ToUpper(), true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "Problem z jakoscia komponentu".ToUpper(), true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "Problem z MES".ToUpper(), true);

                        if (!comboBox1.Items.Contains("Brak komponentu - dystrybucja".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - dystrybucja".ToUpper());

                        if (!comboBox1.Items.Contains("Brak komponentu - zakupy".ToUpper()))
                            comboBox1.Items.Add("Brak komponentu - zakupy".ToUpper());

                        if (!comboBox1.Items.Contains("Brak pradu".ToUpper()))
                            comboBox1.Items.Add("Brak pradu".ToUpper());
                        break;

                    case @"LPA":
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "LPA", true);
                        _changeControls.UpdateControl(button6, System.Drawing.Color.Tomato, "ANULUJ - EKRAN STARTOWY ", true);
                        _minutes = "0";
                        _opisToDb = "LPA";
                        ConfirmMinutes();
                        //     pytanie_komentarz();

                        break;

                    default:
                        _changeControls.UpdateControl(button1, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button2, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button3, System.Drawing.Color.PaleGreen, "", true);
                        _changeControls.UpdateControl(button4, System.Drawing.Color.PaleGreen, "", true);
                        break;

                }
        }
        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(textBox1.Text.Length >= 1)
                {
                    _minutes = textBox1.Text;
                    textBox1.Clear();
                    _changeControls.UpdateControl(label6, SystemColors.Control, "", false);
                    _changeControls.UpdateControl(label5, SystemColors.Control, "", false);
                    textBox1.Visible = false;
                    button8.Visible = false;
                    Sql.SendDataToDb("G020", _opisToDb, _komentarzToDb, _minutes);
                }
            }
        }

        private void comboBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                _opisToDb = comboBox1.Text;
                comboBox1.Visible = false;
                button9.Visible = false;
                QuestionAboutAddComment();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _opisToDb = button1.Text;

            QuestionAboutAddComment();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _opisToDb = button2.Text;

            QuestionAboutAddComment();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _opisToDb = button3.Text;

            QuestionAboutAddComment();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _opisToDb = button4.Text;

            QuestionAboutAddComment();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button2.Visible = false;
            button7.Visible = false;
          //  FillCombobox();
            Sql.FillCombobox(comboBox1);
            comboBox1.Visible = true;
            button9.Visible = true;
            comboBox1.Select();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (GlobalDataClass.StationName.Contains("LPA"))
            {
                StartMenu startMenu = new StartMenu();
                startMenu.Show();
             //   _changeControls.HideForms(startMenu);
                return;
            }              
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
            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form != m0Form)
            //        form.Hide();
            //}

            this.Hide();

            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                _minutes = textBox1.Text;
                textBox1.Clear();
                _changeControls.UpdateControl(label6, SystemColors.Control, "", false);
                _changeControls.UpdateControl(label5, SystemColors.Control, "", false);
                textBox1.Visible = false;
                button8.Visible = false;
                //   SendDataToDb("G020", opisToDb);
                Sql.SendDataToDb("G020", _opisToDb, _komentarzToDb, _minutes);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _opisToDb = comboBox1.Text;
            comboBox1.Visible = false;
            button9.Visible = false;
            if (_opisToDb.ToUpper().Equals("INNE"))
            {
                ShowKomentarzWindow();
            }
            else
            {
                DialogResult r = MessageBox.Show("Czy chcesz dodać szczegółowy opis // komentarz ", "Potwierdzenie", MessageBoxButtons.YesNo);

                if (r == DialogResult.Yes)
                {
                    ShowKomentarzWindow();
                }
                else if (r == DialogResult.No)
                {
                    ConfirmMinutes();
                }
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if(!GlobalDataClass.IsItWindows)
                Onboard.Run();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (!GlobalDataClass.IsItWindows)
                Onboard.Run();
        }
      
        private void textBox1_TextChanged(object sender, EventArgs e)                       
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Bardzo proszę wpisywać jednak cyfry...");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
        }

        private void ConfirmMinutes()
        {
            if (GlobalDataClass.StationName.Equals("LPA"))
            {
                textBox1.Select();
                _changeControls.UpdateControl(label6, SystemColors.Control, "", true);
                _changeControls.UpdateControl(label5, SystemColors.Control, "Podaj czas awarii w minutach:", true);
                label5.BringToFront();
                textBox1.Visible = true;
                textBox1.BringToFront();
                button8.Visible = true;
                button8.BringToFront();
                return;
            }

            DialogResult r5 = MessageBox.Show($"Czy potwierdzasz czas awarii: {TimeCounter.RoundedMinutesToTimer.ToString()}", "Potwierdz czas awarii", MessageBoxButtons.YesNo);

            if (r5 == DialogResult.Yes)
            {
                _minutes = TimeCounter.RoundedMinutesToTimer.ToString();
                Sql.SendDataToDb("G020", _opisToDb, _komentarzToDb, _minutes);
            }
            else if (r5 == DialogResult.No)
            {
                textBox1.Select();
                _changeControls.UpdateControl(label6, SystemColors.Control, "", true);
                _changeControls.UpdateControl(label5, SystemColors.Control, "Podaj czas awarii w minutach:", true);
                label5.BringToFront();
                textBox1.Visible = true;
                textBox1.BringToFront();
                button8.Visible = true;
                button8.BringToFront();
                return;
            }          
        }

        private void QuestionAboutAddComment()
        {
            DialogResult r = MessageBox.Show("Czy chcesz dodać szczegółowy opis // komentarz ", "Potwierdzenie", MessageBoxButtons.YesNo);

            if (r == DialogResult.Yes)
            {
                ShowKomentarzWindow();
            }
            else if (r == DialogResult.No)
            {
                ConfirmMinutes();
            }
        }

        private void ShowKomentarzWindow()
        {
            Komentarz frm = new Komentarz();
            frm.Show();
            frm.FormClosed += frm_FormClosed;
        }
        private void frm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            _komentarzToDb = GlobalDataClass.KomentarzToDb;
            ConfirmMinutes();
        }
    }
}
