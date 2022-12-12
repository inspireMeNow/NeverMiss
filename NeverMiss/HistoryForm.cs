using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace neverMiss
{
    public partial class HistoryForm : Form
    {
        private Form1 returnForm = null;//声明一个returnForm变量，用来保存被传递进来的Form1

        private void UpdateDataTable()//更新数据表
        {

            string connectionString = "Data Source=demo.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "select * from reminder";
                SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("id", typeof(String));
                dt1.Columns.Add("datetime", typeof(String));
                dt1.Columns.Add("message", typeof(String));
                dt1.Columns.Add("important", typeof(String));
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("id", typeof(String));
                dt2.Columns.Add("datetime", typeof(String));
                dt2.Columns.Add("message", typeof(String));
                dt2.Columns.Add("important", typeof(String));
                dt2.Columns.Add("finish", typeof(String));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //进行时间的比对
                    DateTime nowtime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/d H:mm"));
                    DateTime datetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["datetime"]);
                    if (DateTime.Compare(datetime, nowtime) < 0&& ds.Tables[0].Rows[i]["finish"].ToString()=="0")
                    {

                        string overtime = "update reminder set finish = -1 where id = @id";
                        using (SQLiteCommand cmd = new SQLiteCommand(connection))
                        {
                            cmd.CommandText = overtime;
                            cmd.Parameters.AddWithValue("@id", ds.Tables[0].Rows[i]["id"].ToString());
                            cmd.ExecuteNonQuery();
                        }
                        ds.Tables[0].Rows[i]["finish"] = -1;

                    }
                    if (ds.Tables[0].Rows[i]["finish"].ToString() == "0")
                    {
                        string im = "否";
                        if (ds.Tables[0].Rows[i]["important"].ToString() == "1")
                        {
                            im = "是";
                        }
                        dt1.Rows.Add(ds.Tables[0].Rows[i]["id"], ds.Tables[0].Rows[i]["datetime"], ds.Tables[0].Rows[i]["message"], im);
                    }
                    else
                    {
                        string fi = "完成";
                        string im = "否";
                        if (ds.Tables[0].Rows[i]["finish"].ToString() == "-1")
                        {
                            fi = "过期";
                        }
                        if (ds.Tables[0].Rows[i]["important"].ToString() == "1")
                        {
                            im = "是";
                        }
                        dt2.Rows.Add(ds.Tables[0].Rows[i]["id"], ds.Tables[0].Rows[i]["datetime"], ds.Tables[0].Rows[i]["message"], im, fi);
                    }

                }
                dataGridView1.DataSource = dt1;
                dataGridView2.DataSource = dt2;

                //不允许添加行
                dataGridView1.AllowUserToAddRows = false;
                dataGridView2.AllowUserToAddRows = false;
                connection.Close();
            }
        }
        public HistoryForm(Form1 f)
        {
            InitializeComponent();
            this.returnForm = f;
            this.UpdateDataTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            returnForm.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex > -1)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                Console.WriteLine(id);
                string connectionString = "Data Source=demo.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "update reminder set finish = 1 where id = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                this.UpdateDataTable();
            }
        }
    }
}
