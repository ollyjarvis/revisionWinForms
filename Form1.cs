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

namespace revision
{
    public partial class Form1 : Form
    {
        string rootPath = "C:/Users/olly/Desktop/revision/";
        string rootPathBS = @"C:\Users\olly\Desktop\revision\";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string session = "0";

            if (!Directory.Exists(rootPath +"log/"))
            {
                Directory.CreateDirectory(rootPath + "log /");
            }

            for (int i = 1; i < 4; i++)
            {
                if (!Directory.Exists(rootPath + i + "/q/"))
                {
                    Directory.CreateDirectory(rootPath + i + "/q/");
                }

                if (!Directory.Exists(rootPath + i + "/a/"))
                {
                    Directory.CreateDirectory(rootPath + i + "/a/");
                }
            }


            if (!File.Exists(rootPath + "/log/log.txt"))
            {
                using (FileStream fs = File.Create(rootPath + "/log/log.txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("0");

                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }

            using (FileStream fs = File.OpenRead(rootPath + "log/log.txt"))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    session = temp.GetString(b);
                }
            }

            double sessionNum = Int32.Parse(session) + 1;
            string sessionNumToStr = sessionNum.ToString();
            string sessionType;

            using (FileStream fs = File.OpenWrite(rootPath + "/log/log.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(sessionNumToStr);

                fs.Write(info, 0, info.Length);
            }
            
            if (sessionNum / 10 == Math.Floor(sessionNum / 10))
            {
                sessionType = "3";
            }
            else if (sessionNum / 4 == Math.Floor(sessionNum / 4))
            {
                sessionType = "2";
            }
            else
            {
                sessionType = "1";
            }

            int directoryCount = Directory.GetDirectories(rootPathBS + sessionType + @"\q").Length;
            int cardNum = newNum(directoryCount);
            DisplayImage(cardNum, sessionType);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void DisplayImage(int number, string session)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            string imageUrl = rootPath + session + "/q/" + number.ToString() + "/page-1.jpg";
            pictureBox1.Image = new Bitmap(imageUrl);
        }

        private int newNum(int dirNum)
        {
            Random random = new Random();
            int num = random.Next(0, dirNum);
            return num;
        }
    }
}
