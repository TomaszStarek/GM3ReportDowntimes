using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class GlobalDataClass
    {
        public static DateTime StartAwarii { get; set; }
        public static string SectionName { get; set; }
        public static string StationName { get; set; }
        public static string KomentarzToDb { get; set; }
        public static bool IsItWindows { get => System.Environment.OSVersion.Platform.ToString().ToUpper().Contains("WIN"); }
        public  static string ConnectionString
        { 
            get
            {
                if(IsItWindows)
                    return ConfigurationManager.ConnectionStrings["windows"].ConnectionString;
                else
                    return ConfigurationManager.ConnectionStrings["raspberry"].ConnectionString;
            }
        }

    }
}
