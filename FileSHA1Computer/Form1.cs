using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace FileSHA1Computer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            ReadFolder(textBox1.Text, 1);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
            ReadFolder(textBox2.Text, 2);
        }

        private void ReadFolder(string path, int which)
        {
            string[] fn = Directory.GetFiles(path);
            foreach (string item in fn)
            {
                if (which == 1)
                {
                    listView1.Items.Clear();
                    listView1.Items.Add(item);
                }
                else
                {
                    listView2.Items.Clear();
                    listView2.Items.Add(item);
                }
            }
        }

        public string ComputeFileSHA1(string FileName)
        {
            try
            {
                byte[] hr;
                using (SHA1Managed Hash = new SHA1Managed()) // 创建Hash算法对象 
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open)) // 创建文件流对象 
                    {
                        hr = Hash.ComputeHash(fs); // 计算
                    }
                }
                return BitConverter.ToString(hr).Replace("-", ""); // 转化为十六进制字符串
            }
            catch (IOException) { return "Error:访问文件时出现异常"; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "正在运行...";
            int countName = 0;
            int countSHA1 = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (textBox1.Text.LastIndexOf('\\') == textBox1.Text.Length - 1)
                {
                    textBox1.Text = textBox1.Text.Replace("\\", "");
                }
                foreach (ListViewItem item2 in listView2.Items)
                {
                    if (textBox2.Text.LastIndexOf('\\') == textBox1.Text.Length - 1)
                    {
                        textBox2.Text = textBox2.Text.Replace("\\", "");
                    }
                    if (item.Text.Replace(textBox1.Text, "") == item2.Text.Replace(textBox2.Text, ""))
                    {
                        countName += 1;
                        if (ComputeFileSHA1(item.Text) == ComputeFileSHA1(item2.Text))
                        {
                            countSHA1 += 1;
                            listView1.Items.Remove(item);
                            listView2.Items.Remove(item2);
                        }
                    }
                }
            }
            label1.Text = string.Format("{0}同名文件，其中{1}个的SHA1相同", countName, countSHA1);
        }
    }
}
