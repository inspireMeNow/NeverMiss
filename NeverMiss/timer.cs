using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace neverMiss
{
    public class timer
    {
        private string path = "";
        public class reminder
        {
            public int id { get; set; }
            public string message { get; set; }
            public string datetime { get; set; }
            public int interval { get; set; }
            public int count { get; set; }
            public int keeping { get; set; }
            public int important { get; set; }
            public string path { get; set; }
            public int finish { get; set; }
        }
        //private int[] id;
        //private string[] message;
        //private string[] datetime;
        //private int[] interval;
        //private int[] count;
        //private int[] keeping;
        //private int[] important;
        //private int[]
        public static reminder[] re;
        public static int sum=0;
        public timer()
        {
            //int sum = getSum();
            re = new reminder[100];
            for (int i = 0; i < 100; i++)
            {
                re[i] = new reminder();
            }
            string connectionString = "Data Source=demo.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                int i = 0;
                string count = "select * from reminder";
                using (SQLiteCommand command = new SQLiteCommand(count, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Console.WriteLine(reader.GetValue(2).ToString());
                            re[i].datetime = reader.GetValue(2).ToString();
                            re[i].id = int.Parse(reader.GetValue(0).ToString());
                            re[i].interval = int.Parse(reader.GetValue(3).ToString());
                            re[i].count = int.Parse(reader.GetValue(4).ToString());
                            
                            i++;
                        }
                        sum = i;
                    }
                }
                connection.Close();
            }
        }
        private void DisplayToast(string id,string msg,string important,string keeping,string path)
        {
            string is_important = "";
            if (important == "1")
            {
                is_important = "重要";
            }
            if (important == "0")
            {
                is_important = "";
            }
            ToastNotificationManagerCompat.History.Clear();
            // Construct the content and show the toast!
            new ToastContentBuilder()
            // Profile (app logo override) image
            .AddAppLogoOverride(new Uri(Directory.GetCurrentDirectory() + "\\neverMiss.png"), ToastGenericAppLogoCrop.Circle)
            .AddText(is_important)
            .AddText(msg)
            // Buttons
            .AddButton(new ToastButton()
                .SetContent("我知道了")
                .AddArgument("now")
                .SetBackgroundActivation())
            .AddButton(new ToastButton()
                .SetContent("稍等一下")
                .AddArgument("later")
                .SetBackgroundActivation())
            .AddButton(new ToastButton()
                .SetContent("我已完成")
                .AddArgument("finish")
                .SetBackgroundActivation())
            .Show(toast =>
            {
                //toast.ExpirationTime = DateTime.Now;
                toast.ExpirationTime = DateTime.Now.AddSeconds(10);
            });
            int count = 0;
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                if (count == 0)
                {
                    CheckInput(id, toastArgs.Argument.ToString(), path);
                //ToastNotificationManagerCompat.Uninstall();
                }
                count++;
            };
        }
        private void CheckInput(string id,string toastArgs,string path)
        {
            string connectionString = "Data Source=demo.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            if (toastArgs == "now")
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = path;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    //process.WaitForExit();
                }
                catch (InvalidOperationException)
                {
                    
                }
                re[int.Parse(id) - 1].count = 0;
                // 使用 SQLiteCommand 执行 SQL 语句
                string sql = "UPDATE reminder SET count = 0 WHERE id = " + id + ";";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                // 关闭数据库连接
                connection.Close();
            }
            else if (toastArgs == "later")
            {
                
                re[int.Parse(id) - 1].count = re[int.Parse(id) - 1].count - 1;
                int interval=re[int.Parse(id) - 1].interval;
                //String sql1 = "SELECT interval FROM reminder where id=" + id + ";";
                //// 使用 SQLiteCommand 执行 SQL 语句
                //SQLiteCommand command1 = new SQLiteCommand(sql1, connection);
                //SQLiteDataReader reader = command1.ExecuteReader();
                //String s=reader.GetValue(3).ToString();
                //int interval = 1;
                //connection.Close();

                String time = DateTime.Now.AddMinutes(interval).ToString("yyyy/MM/d HH:mm");

                string sql = "UPDATE reminder SET count = count-1,datetime='"+time+"' WHERE id = " + id + ";";

                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                // 关闭数据库连接
                connection.Close();
            }
            else if(toastArgs=="finish")
            {
                // 使用 SQLiteCommand 执行 SQL 语句
                string sql = "UPDATE reminder SET finish = 1 WHERE id = " + id + ";";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                // 关闭数据库连接
                connection.Close();
            }
            else
            {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = path;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();
                }
                catch (InvalidOperationException)
                {

                }
                re[int.Parse(id) - 1].count = 0;
            }
        }
        public int getSum()
        {
            int sum = 0;
            string connectionString = "Data Source=demo.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                sum = 0;
                string count = "select count(*) from reminder";
                using (SQLiteCommand command = new SQLiteCommand(count, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sum = int.Parse(reader.GetValue(0).ToString());
                        }
                    }
                }
                connection.Close();
            }
            return sum;
        }
        public void QueryTimer(string time)
        {
            int id = 0;
            string connectionString = "Data Source=demo.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM reminder";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = int.Parse(reader.GetValue(0).ToString());
                            //getData();
                            //Console.WriteLine(re[id - 1].datetime);
                            if (re[id-1].datetime ==time)
                            {
                                if (int.Parse(reader.GetValue(8).ToString()) == 0|| int.Parse(reader.GetValue(8).ToString()) == -1)
                                {
                                    if (re[id - 1].count > 0)
                                    {
                                        //re[id-1].datetime = reader.GetValue(2).ToString();
                                        DisplayToast(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(6).ToString(), reader.GetValue(5).ToString(), reader.GetValue(7).ToString());
                                        DateTime dateTime = Convert.ToDateTime(re[id - 1].datetime);
                                        TimeSpan timeSpan = new TimeSpan(0, int.Parse(reader.GetValue(3).ToString()), 0);
                                        dateTime = dateTime.Add(timeSpan);
                                        re[id - 1].datetime = dateTime.ToString("yyyy/MM/d HH:mm");
                                    }
                                }
                            }
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
