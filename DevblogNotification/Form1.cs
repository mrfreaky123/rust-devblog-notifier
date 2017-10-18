using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevblogNotification
{
    public partial class Form1 : Form
    {
        public bool running = false;
        public int devblog;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) != true && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(maskedTextBox1.Text.Length == 0)
            {
                MessageBox.Show("Please input a number in the textbox");
                return;
            }
            running = !running;

            if(running)
            {
                Thread th = new Thread(Watcher);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            if(!running)
            {
                label2.Text = "Status: stopped";
            }
        }

        string webstring;
        private void Watcher()
        {
            while(running)
            {
                label2.Text = "Status: running";
                using (WebClient web = new WebClient())
                {
                    webstring = web.DownloadString("https://rust.facepunch.com/blog/");
                }
                if (webstring.Contains($"Devblog {devblog}"))
                {
                    Thread th = new Thread(Notification);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                    Application.ExitThread();
                }
                Thread.Sleep(180000);
            }
        }

        private void Notification()
        {
            while(running)
            {
                Console.Beep();
                Console.Beep(500, 300);
                Console.Beep();
                Thread.Sleep(15000);
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            int i;
            bool pass;
            pass = int.TryParse(maskedTextBox1.Text, out i);
            devblog = i;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
