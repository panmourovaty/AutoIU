using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace AutoIU
{
    public class Ulti
    {
        private string textBox2 = "";
        private string textBox3 = "";
        private string numericUpDown1 = "";
        private string numericUpDown2 = "";
        private bool radioButton1 = false;
        private bool radioButton2 = false;
        private ProcessStartInfo cmd = null;
        public Form1 form;

        public Ulti(Form1 form)
        {
            this.form = form;
        }

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

        public void Spusteno()
        {
            smazanismbsharu();
            vypnutitinypxe();

            //zbytek
            form.spusteno = false;
        }

        public void Nespusteno(string TextBox2, string TextBox3, decimal NumericUpDown1, decimal NumericUpDown2, bool RadioButton1, bool RadioButton2)
        {
            //Incializace hodnot
            textBox2 = TextBox2;
            textBox3 = TextBox3;
            numericUpDown1 = NumericUpDown1.ToString();
            numericUpDown2 = NumericUpDown2.ToString();
            radioButton1 = RadioButton1;
            radioButton2 = RadioButton2;

            vytvorenismbsharu();

            zapisdoinstall();

            spustenitinypxe();

            zapiscasuspusteni();

            zapisfirmwaru();

            kopirovanikonfigurace();

            //zmeny gui
            form.spusteno = true;
        }

        public void button7()
        {
            var cmd4 = new ProcessStartInfo();
            string Command4;
            cmd4.UseShellExecute = true;
            Command4 = @"cd " + form.pracovnislozka + @"\konfigurace &&start .";
            cmd4.WorkingDirectory = form.pracovnislozka;
            cmd4.FileName = @"cmd.exe";
            cmd4.Verb = "runas";
            cmd4.Arguments = "/c " + Command4;
            cmd4.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd4);
        }

        private void smazanismbsharu()
        {
            //smazani smb sharu
            var cmd = new ProcessStartInfo();
            string Command;
            cmd.UseShellExecute = true;
            Command = @"net share windows /delete";
            cmd.WorkingDirectory = form.pracovnislozka;
            cmd.FileName = @"cmd.exe";
            cmd.Verb = "runas";
            cmd.Arguments = "/c " + Command;
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd);
        }

        private void vypnutitinypxe()
        {
            //vypnuti tinypxe
            var cmd2 = new ProcessStartInfo();
            string Command2;
            cmd2.UseShellExecute = true;
            Command2 = @"taskkill /IM pxesrv.exe";
            cmd2.WorkingDirectory = form.pracovnislozka;
            cmd2.FileName = @"cmd.exe";
            cmd2.Verb = "runas";
            cmd2.Arguments = "/c " + Command2;
            cmd2.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd2);
        }

        private void vytvorenismbsharu()
        {
            //vytvoreni smb sharu
            cmd = new ProcessStartInfo();
            string Command;

            cmd.UseShellExecute = true;
            Command = $@"net share windows={form.pracovnislozka}\soubory\files\windows /grant:{textBox2},READ /users:{numericUpDown1}";
            cmd.WorkingDirectory = form.pracovnislozka;
            cmd.FileName = @"cmd.exe";
            cmd.Verb = "runas";
            cmd.Arguments = "/c " + Command;
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd);
        }

        private void zapisdoinstall()
        {
            //zapis do install.bat
            string[] lines = System.IO.File.ReadAllLines(form.pracovnislozka + @"\konfigurace\install.bat");
            string allString = "";
            string v = @"net use a: \\" + Ulti.GetLocalIPAddress() + @"\windows /user:" + textBox2 + " " + '"' + textBox3 + '"';
            lines[17] = v;
            for (int i = 0; i < lines.Length; i++)
            {
                allString += lines[i] + "\n";
            }
            System.IO.File.WriteAllText(form.pracovnislozka + @"\konfigurace\install.bat", allString);
        }

        private void spustenitinypxe()
        {
            //spusteni tinypxe
            var cmd2 = new ProcessStartInfo();
            cmd2.UseShellExecute = true;
            cmd2.WorkingDirectory = form.pracovnislozka;
            cmd2.FileName = form.pracovnislozka + @"\soubory\pxesrv.exe";
            cmd2.Verb = "runas";
            cmd2.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(cmd2);
        }

        private void zapiscasuspusteni()
        {
            //zapis casu spusteni
            string[] lines2 = System.IO.File.ReadAllLines(form.pracovnislozka + @"\soubory\files\menu.ipxe");
            string allString2 = "";
            lines2[26] = @"choose --default disk --timeout " + numericUpDown2 + " target && goto ${target}";
            for (int i = 0; i < lines2.Length; i++)
            {
                allString2 += lines2[i] + "\n";
            }
            System.IO.File.WriteAllText(form.pracovnislozka + @"\soubory\files\menu.ipxe", allString2);
        }

        private void zapisfirmwaru()
        {
            //zapis firmwaru
            string firmware = null;
            if (radioButton1)
            {
                firmware = "ipxe.pxe";
            }
            if (radioButton2)
            {
                firmware = "ipxe-x86_64.efi";
            }
            string[] lines3 = System.IO.File.ReadAllLines(form.pracovnislozka + @"\soubory\config.ini");
            string allString3 = "";
            lines3[12] = @"filename=" + firmware;
            for (int i = 0; i < lines3.Length; i++)
            {
                allString3 += lines3[i] + "\n";
            }
            Process.Start(cmd);
            System.IO.File.WriteAllText(form.pracovnislozka + @"\soubory\config.ini", allString3);
        }

        private void kopirovanikonfigurace()
        {
            //kopirovani konfigurace
            var cmd3 = new ProcessStartInfo();
            string Command3;
            cmd3.UseShellExecute = true;
            Command3 = @"xcopy " + form.pracovnislozka + @"\konfigurace " + form.pracovnislozka + @"\soubory\files\sources /y";
            cmd3.WorkingDirectory = form.pracovnislozka;
            cmd3.FileName = @"cmd.exe";
            cmd3.Verb = "runas";
            cmd3.Arguments = "/c " + Command3;
            cmd3.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd3);
        }
    }
}