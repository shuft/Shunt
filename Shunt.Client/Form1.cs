using System;
using System.Windows.Forms;
using Shunt.Common;
using Shunt.Common.XpressNet;
using Shunt.Controller.Elite;

namespace Shunt.Client
{
    public partial class Form1 : Form
    {
        private EliteController elite = new EliteController("COM4");

        int address = 5;

        public Form1()
        {
            InitializeComponent();

            this.elite.LocoStatusReceived += new EventHandler<EventArgs<LocoStatusPacket>>(Elite_LocoStatusReceived);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.elite.Connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.elite.Disconnect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.elite.IsConnected)
            {
                if (this.radioButton1.Checked)
                {
                    this.elite.SetSpeedAndDirection(address, LocoDirection.Forward, this.trackBar1.Value);
                }
                else
                {
                    this.elite.SetSpeedAndDirection(address, LocoDirection.Reverse, this.trackBar1.Value);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.elite.GetStatus(address);
        }

        private void Elite_LocoStatusReceived(object sender, EventArgs<LocoStatusPacket> e)
        {
            if (this.InvokeRequired)
            {
                Action<object, EventArgs<LocoStatusPacket>> action = new Action<object, EventArgs<LocoStatusPacket>>(this.Elite_LocoStatusReceived);

                this.EndInvoke(this.BeginInvoke(action, new object[] { this, e }));
            }
            else
            {
                this.trackBar1.Maximum = (int)e.Data.SpeedStep;
                this.trackBar1.Value = e.Data.Speed;

                switch (e.Data.Direction)
                {
                    case LocoDirection.Forward:
                        this.radioButton2.Select();
                        break;
                    case LocoDirection.Reverse:
                        this.radioButton1.Select();
                        break;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.elite.ToggleAllStop();
        }
    }
}