using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutoIU
{
    public partial class Form1 : Form
    {
        private bool pass = true;
        private bool debug = true;
        public bool spusteno = false;
        public string pracovnislozka = Directory.GetCurrentDirectory();
        private Ulti ulti;
        private string path1;
        private string path2;

        public Form1()
        {
            InitializeComponent();
            label1.Text = pracovnislozka;
            label12.Text = $@"\\{Ulti.GetLocalIPAddress()}\windows";
            textBox1.Text = Ulti.GetLocalIPAddress();
            label19.Visible = false;
            ulti = new Ulti(this);

            path1 = @"./programy";//adressa pro .bat kopirovani start
            path2 = @"./programy2";//adressa pro kopirovani konec-cil
            string[] dirs = Directory.GetFiles(@"./programy", "*.bat");
            checkedListBox1.Items.Clear();
            foreach (string item in dirs)
            {
                string va = Path.GetFileName(item);
                checkedListBox1.Items.Add(va);
            }
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
                label9.Text = Ulti.GetLocalIPAddress();
                this.Size = new Size(810, 500);//Proc
                groupBox1.Visible = true;
                debug = false;
                NastaveniBuhviceho();
            }
            else
            {
                this.Size = new Size(810, 350);//???????
                groupBox1.Visible = false;
                debug = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label9.Text = Ulti.GetLocalIPAddress();
            NastaveniBuhviceho();
        }

        private void NastaveniBuhviceho()//prejmenovat a zkontrolovat mezery
        {
            label16.Text = $@"net share windows={pracovnislozka}\soubory\files\windows /grant:{textBox2.Text},READ /users:{numericUpDown1.Value}";
            label18.Text = $@"net use a: \\{Ulti.GetLocalIPAddress()}\windows /user:{textBox2.Text}" + '"' + "<vaseheslo>" + '"';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string item in checkedListBox1.CheckedItems)
            {
                File.Copy(Path.Combine(path1, item), Path.Combine(path2, item), true);
            }

            if (spusteno == true)
            {
                ulti.Spusteno();
                this.button2.Text = "Spustit";
                this.label19.Visible = false;
                this.ControlBox = true;
            }
            else
            {
                ulti.Nespusteno(textBox2.Text, textBox3.Text, numericUpDown1.Value, numericUpDown2.Value, radioButton1.Checked, radioButton2.Checked);
                button2.Text = "Vypnout";
                label19.Visible = true;
                this.ControlBox = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label12.Text = @"\\" + Ulti.GetLocalIPAddress() + @"\windows";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = Ulti.GetLocalIPAddress();
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
            ulti.button7();
        }
    }
}