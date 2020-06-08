using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AutoIU
{
    public partial class Form1 : Form
    {
        public bool pass = true;
        public bool debug = true;
        public bool spusteno = false;
        public string pracovnislozka = Directory.GetCurrentDirectory();
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Váš PC nemá IP adresu");
        }
        public Form1()
        {
            InitializeComponent();
            label1.Text = pracovnislozka;
            label12.Text = @"\\" + GetLocalIPAddress() + @"\windows";
            textBox1.Text = GetLocalIPAddress();
            label19.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pass == true)
            {
                textBox3.PasswordChar = '\0';
                pass = false;
            }
            else
            {
                textBox3.PasswordChar = '*';
                pass = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (debug == true)
            {
                label9.Text = GetLocalIPAddress();
                this.Size = new Size(810, 500);
                groupBox1.Visible = true;
                debug = false;
                label16.Text = @"net share windows=" + pracovnislozka + @"\soubory\files\windows /grant:" + textBox2.Text + @",READ /users:" + numericUpDown1.Value;
                label18.Text = @"net use a: \\" + GetLocalIPAddress() + @"\windows /user:" + textBox2.Text + " " + '"' + "<vaseheslo>" + '"';
            }
            else
            {
                this.Size = new Size(810, 350);
                groupBox1.Visible = false;
                debug = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label9.Text = GetLocalIPAddress();
            label16.Text = @"net share windows=" + pracovnislozka + @"\soubory\files\windows /grant:" + textBox2.Text + @",READ /users:" + numericUpDown1.Value;
            label18.Text = @"net use a: \\" + GetLocalIPAddress() + @"\windows /user:" + textBox2.Text + " " + '"' + "<vaseheslo>" + '"';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (spusteno == true)
            {
                //smazani smb sharu
                var cmd = new ProcessStartInfo();
                string Command;
                cmd.UseShellExecute = true;
                Command = @"net share windows /delete";
                cmd.WorkingDirectory = pracovnislozka;
                cmd.FileName = @"cmd.exe";
                cmd.Verb = "runas";
                cmd.Arguments = "/c " + Command;
                cmd.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(cmd);
                //vypnuti tinypxe
                var cmd2 = new ProcessStartInfo();
                string Command2;
                cmd2.UseShellExecute = true;
                Command2 = @"taskkill /IM pxesrv.exe";
                cmd2.WorkingDirectory = pracovnislozka;
                cmd2.FileName = @"cmd.exe";
                cmd2.Verb = "runas";
                cmd2.Arguments = "/c " + Command2;
                cmd2.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(cmd2);
                //zbytek
                spusteno = false;
                button2.Text = "Spustit";
                label19.Visible = false;
                this.ControlBox = true;
            }
            else
            {
                //vytvoreni smb sharu
                var cmd = new ProcessStartInfo();
                string Command;
                cmd.UseShellExecute = true;
                Command = @"net share windows=" + pracovnislozka + @"\soubory\files\windows /grant:" + textBox2.Text + @",READ /users:" + numericUpDown1.Value.ToString();
                cmd.WorkingDirectory = pracovnislozka;
                cmd.FileName = @"cmd.exe";
                cmd.Verb = "runas";
                cmd.Arguments = "/c " + Command;
                cmd.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(cmd);
                //zapis do install.bat
                string[] lines = System.IO.File.ReadAllLines(pracovnislozka + @"\konfigurace\install.bat");
                string allString = "";
                lines[17] = @"net use a: \\" + GetLocalIPAddress() + @"\windows /user:" + textBox2.Text + " " + '"' + textBox3.Text + '"';
                for (int i = 0; i < lines.Length; i++)
                {
                    allString += lines[i] + "\n";
                }
                System.IO.File.WriteAllText(pracovnislozka + @"\konfigurace\install.bat", allString);
                //spusteni tinypxe
                var cmd2 = new ProcessStartInfo();
                cmd2.UseShellExecute = true;
                cmd2.WorkingDirectory = pracovnislozka;
                cmd2.FileName = pracovnislozka + @"\soubory\pxesrv.exe";
                cmd2.Verb = "runas";
                cmd2.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(cmd2);
                //zapis casu spusteni
                string[] lines2 = System.IO.File.ReadAllLines(pracovnislozka + @"\soubory\files\menu.ipxe");
                string allString2 = "";
                lines2[26] = @"choose --default disk --timeout " + numericUpDown2.Value.ToString() + " target && goto ${target}";
                for (int i = 0; i < lines2.Length; i++)
                {
                    allString2 += lines2[i] + "\n";
                }
                System.IO.File.WriteAllText(pracovnislozka + @"\soubory\files\menu.ipxe", allString2);
                //zapis firmwaru
                string firmware = null;
                if (radioButton1.Checked)
                {
                    firmware = "ipxe.pxe";
                }
                if (radioButton2.Checked)
                {
                    firmware = "ipxe-x86_64.efi";
                }
                string[] lines3 = System.IO.File.ReadAllLines(pracovnislozka + @"\soubory\config.ini");
                string allString3 = "";
                lines3[12] = @"filename=" + firmware;
                for (int i = 0; i < lines3.Length; i++)
                {
                    allString3 += lines3[i] + "\n";
                }
                Process.Start(cmd);
                System.IO.File.WriteAllText(pracovnislozka + @"\soubory\config.ini", allString3);
                //kopirovani konfigurace
                var cmd3 = new ProcessStartInfo();
                string Command3;
                cmd3.UseShellExecute = true;
                Command3 = @"xcopy "+ pracovnislozka + @"\konfigurace " + pracovnislozka + @"\soubory\files\sources /y";
                cmd3.WorkingDirectory = pracovnislozka;
                cmd3.FileName = @"cmd.exe";
                cmd3.Verb = "runas";
                cmd3.Arguments = "/c " + Command3;
                cmd3.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(cmd3);
                //zmeny gui
                spusteno = true;
                button2.Text = "Vypnout";
                label19.Visible = true;
                this.ControlBox = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label12.Text = @"\\" + GetLocalIPAddress() + @"\windows";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetLocalIPAddress();
        }

        public void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.windowsafg.com");
        }

        private void label26_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/cs-cz/windows-hardware/get-started/adk-install");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var cmd4 = new ProcessStartInfo();
            string Command4;
            cmd4.UseShellExecute = true;
            Command4 = @"cd " + pracovnislozka + @"\konfigurace &&start .";
            cmd4.WorkingDirectory = pracovnislozka;
            cmd4.FileName = @"cmd.exe";
            cmd4.Verb = "runas";
            cmd4.Arguments = "/c " + Command4;
            cmd4.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd4);
        }
    }

}
