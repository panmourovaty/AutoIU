using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace AutoIU_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cmd1 = new ProcessStartInfo();
            string Command1;
            cmd1.UseShellExecute = true;
            Command1 = @"WMIC computersystem where caption='"+Environment.MachineName+@"' rename "+textBox1.Text+ @" && netdom /domain:"+textBox3.Text+@" /user:"+textBox2.Text+@" /password:"+textBox4.Text+@" member "+textBox1.Text+@" /joindomain";
            cmd1.FileName = @"cmd.exe";
            cmd1.Verb = "runas";
            cmd1.Arguments = "/c " + Command1;
            cmd1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(cmd1);
        }
    }
}
