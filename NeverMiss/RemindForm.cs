using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;
using neverMiss;
namespace neverMiss
{
    public partial class RemindForm : Form
    {
        private Form1 returnForm = null;//声明一个returnForm变量，用来保存被传递进来的Form1
        public RemindForm(Form1 form1)
        {
            InitializeComponent();
            returnForm = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                this.Close();
            returnForm.Visible = true;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private string path = "";
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // 设置打开文件对话框的初始目录
            openFileDialog1.InitialDirectory = "c:\\";

            // 设置打开文件对话框的标题
            openFileDialog1.Title = "选择文件";

            // 设置打开文件对话框可以选择的文件类型
            openFileDialog1.Filter = "可执行文件(*.exe)|*.exe";

            // 设置打开文件对话框默认选择的文件类型
            openFileDialog1.FilterIndex = 2;

            // 设置打开文件对话框是否记忆上次打开的目录
            openFileDialog1.RestoreDirectory = true;

            // 打开对话框，并判断用户是否选择了文件
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                path = openFileDialog1.FileName;
                // 在这里添加代码，打开文件并进行处理
            }
        }
        
        //private delegate void richTextBox1CallBack();
        //private void setText(string str)
        //{
        //    richTextBox1CallBack callback = delegate ()//使用委托 

        //    {

        //        richTextBox1.Text=str;

        //    };

        //    richTextBox1.Invoke(callback);
        //}
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RemindForm_Load(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {

            int isImportant = 0;
            if (radioButton1.Checked)
            {
                isImportant = 1;
            }
            if (radioButton2.Checked)
            {
                isImportant = 0;
            }
            string minute = "";
            if (int.Parse(numericUpDown2.Value.ToString()) <10)
            {
                minute = "0" + numericUpDown2.Value.ToString();
            }
            else
            {
                minute= numericUpDown2.Value.ToString();
            }
            string hour = "";
            if (int.Parse(numericUpDown1.Value.ToString()) < 10)
            {
                hour = "0" + numericUpDown1.Value.ToString();
            }
            else
            {
                hour= numericUpDown1.Value.ToString();
            }
            if (richTextBox1.Text.Length == 0)
            {
                MessageBox.Show("请输入完整信息");
            }
            else
            {
                string connectionString = "Data Source=demo.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // 执行一个查询
                    int sum = 0;
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
                    string insert = "insert into reminder values (@id,@message,@datetime,@interval,@count,@keeping,@important,@path,@finish)";
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = insert;
                        cmd.Parameters.AddWithValue("@id", sum + 1);
                        cmd.Parameters.AddWithValue("@message", richTextBox1.Text);
                        cmd.Parameters.AddWithValue("@datetime", dateTimePicker1.Value.ToString("yyyy/MM/d")+" "+ hour+":"+ minute);
                        cmd.Parameters.AddWithValue("@interval", int.Parse(numericUpDown4.Text));
                        cmd.Parameters.AddWithValue("@count", int.Parse(numericUpDown5.Text));
                        cmd.Parameters.AddWithValue("@keeping", 5);
                        cmd.Parameters.AddWithValue("@important", isImportant);
                        cmd.Parameters.AddWithValue("@path", path);
                        cmd.Parameters.AddWithValue("@finish", 0);
                        cmd.ExecuteNonQuery();
                    }
                    timer.re[timer.sum].id = sum + 1;
                    timer.re[timer.sum].datetime = dateTimePicker1.Value.ToString("yyyy/MM/d") + " " + hour + ":" + minute;
                    timer.re[timer.sum].count = int.Parse(numericUpDown5.Text);
                }
                
                MessageBox.Show("设置成功");
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
