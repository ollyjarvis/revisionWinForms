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
        int cardNum = 0;
        string sessionType;
        string qOrA = "q";

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

            for (int i = 1; i < Directory.GetDirectories(rootPathBS + @"q").Length + 1; i++)
            {
                if (!File.Exists(rootPath + "/q/" + i + "/session.txt"))
                {
                    using (FileStream fs = File.Create(rootPath + "/q/" + i + "/session.txt"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("1");
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
                if (!File.Exists(rootPath + "/q/" + i + "/done.txt"))
                {
                    using (FileStream fs = File.Create(rootPath + "/q/" + i + "/done.txt"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("0");
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
            }


            if (!File.Exists(rootPath + "/log/log.txt"))
            {
                using (FileStream fs = File.Create(rootPath + "/log/log.txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("0");

                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    fs.Close();
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
                fs.Close();
            }

            double sessionNum = Int32.Parse(session) + 1;
            string sessionNumToStr = sessionNum.ToString();

            using (FileStream fs = File.OpenWrite(rootPath + "/log/log.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(sessionNumToStr);

                fs.Write(info, 0, info.Length);
                fs.Close();
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

            int directoryCount = Directory.GetDirectories(rootPathBS + @"\q").Length;
            cardNum = newNum(directoryCount);
            DisplayImage(cardNum, "q");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int newSession = Int32.Parse(getBox(cardNum)) + 1;
            if (newSession > 3)
            {
                newSession = 3;
            }

            cardDone(newSession);

            int directoryCount = Directory.GetDirectories(rootPathBS + @"\q").Length;
            cardNum = newNum(directoryCount);
            DisplayImage(cardNum, "q");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int newSession = Int32.Parse(getBox(cardNum)) - 1;
            if (newSession < 1)
            {
                newSession = 1;
            }

            cardDone(newSession);

            int directoryCount = Directory.GetDirectories(rootPathBS + @"\q").Length;
            cardNum = newNum(directoryCount);
            DisplayImage(cardNum, "q");

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (qOrA == "q")
            {
                qOrA = "a";
            }
            else if (qOrA == "a")
            {
                qOrA = "q";
            }
            else
            {
                qOrA = "q";
            }
            DisplayImage(cardNum, qOrA);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 1; i < Directory.GetDirectories(rootPathBS + @"q").Length + 1; i++)
            {
                using (FileStream fs = File.OpenWrite(rootPath + "q/" + i + "/done.txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("0");
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }
        }

        private void DisplayImage(int number, string qOrA)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            string imageUrl = rootPath + qOrA + "/" + number.ToString() + "/page-1.jpg";
            pictureBox1.Image = new Bitmap(imageUrl);
        }

        private int newNum(int dirNum)
        {
            Random random = new Random();
            int num = random.Next(1, dirNum);
            if (getBox(num) == sessionType)
            {
                string fileContent = "";
                using (FileStream fs = File.OpenRead(rootPath + "q/" + num + "/done.txt"))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        fileContent = temp.GetString(b);
                    }
                    fs.Close();
                }
                string fileContStr = fileContent.Replace("\0", string.Empty);
                if (fileContStr == "0")
                {
                    return num;
                }
            }

            for (int i = 1; i < dirNum; i++)
            {
                string fileContent = "";
                using (FileStream fs = File.OpenRead(rootPath + "q/" + i + "/session.txt"))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        fileContent = temp.GetString(b);
                    }
                    fs.Close();
                }
                string fileContStr = fileContent.Replace("\0", string.Empty);
                if (fileContStr == sessionType)
                {
                    string fileContent2 = "";
                    using (FileStream fs = File.OpenRead(rootPath + "q/" + i + "/done.txt"))
                    {
                        byte[] b = new byte[1024];
                        UTF8Encoding temp = new UTF8Encoding(true);

                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            fileContent2 = temp.GetString(b);
                        }
                        fs.Close();
                    }
                    string fileContStr2 = fileContent2.Replace("\0", string.Empty);
                    if (fileContStr2 == "0")
                    {
                        return i;
                    }
                }
            }
            return num;
        }

        private void cardDone(int newSession)
        {
            using (FileStream fs = File.OpenWrite(rootPath + "q/" + cardNum + "/done.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("1");
                fs.Write(info, 0, info.Length);
                fs.Close();
            }

            using (FileStream fs = File.OpenWrite(rootPath + "q/" + cardNum + "/session.txt"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(newSession.ToString());
                fs.Write(info, 0, info.Length);
                fs.Close();
            }

        }

        private string getBox(int num)
        {
            string box = "";
            using (FileStream fs = File.OpenRead(rootPath + "q/" + num.ToString() + "/session.txt"))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    box = temp.GetString(b);
                }
                fs.Close();
            }
            string output = box.Replace("\0", string.Empty);
            return output;
        }


    }
}
