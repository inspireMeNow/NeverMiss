using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.Win32;
//using System.Diagnostics;
namespace neverMiss
{
    public partial class SettingForm : Form
    {
        private Form1 returnForm = null;//声明一个returnForm变量，用来保存被传递进来的Form1
        private PictureBox pictureBox1;
        public SettingForm(Form1 form1)
        {
            InitializeComponent();
            this.returnForm = form1;
        }
        public SettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            returnForm.Visible = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            // 检查名为 "MyValue" 的键值是否存在
            if (key.GetValue("neverMiss") != null)
            {
                DialogResult dr = MessageBox.Show("您确定取消开机自启吗？", "提示", MessageBoxButtons.OKCancel,
           MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.OK)
                {
                    key.DeleteValue("neverMiss");
                }
            }
            else
            {
                RegistryKey RKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                RKey.SetValue("neverMiss", @Directory.GetCurrentDirectory() + "\\neverMiss.exe");
                RKey.Close();
                MessageBox.Show("设置成功");
            }
            key.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String url = Application.StartupPath.ToString() + "help.chm";
            Help.ShowHelp(null, url);
        }
    }
    }
