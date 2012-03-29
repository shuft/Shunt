using System;
using System.Collections.Generic;
using System.IO.Ports;
using Shunt.Common;
using Shunt.Common.XpressNet;

namespace Shunt.Controller.Elite
{
    public sealed class EliteController
    {
        private SerialPort port;

        private bool allStopped = false;

        public event EventHandler<EventArgs<XpressNetPacket>> DataReceived;
        public event EventHandler<EventArgs<LocoStatusPacket>> LocoStatusReceived;

        public EliteController(string portName)
        {
            this.port = new SerialPort(portName);
            this.port.BaudRate = 19200;
            this.port.DataBits = 8;
            this.port.StopBits = StopBits.One;
            this.port.Parity = Parity.None;
        }

        public bool AllStopped
        {
            set { allStopped = value; }
        }

        public void Connect()
        {
            this.port.Open();

            if (this.port.IsOpen)
            {
                this.port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);

                this.GetFirmwareVersion();
            }
        }

        public void Disconnect()
        {
            if (this.port.IsOpen)
            {
                this.port.DataReceived -= this.Port_DataReceived;

                this.port.Close();
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.port.IsOpen;
            }
        }

        public void SetSpeedAndDirection(int address, LocoDirection direction, int speed)
        {
            if (!this.IsConnected)
            {
                return;
            }

            byte speedByte = 0x12;

            if (direction == LocoDirection.Reverse)
            {
                speed += 0x80;
            }

            int xor = 0xe4 ^ speedByte;
            xor = xor;
            xor ^= 5;
            xor ^= speed;

            byte[] command = new byte[6];
            command[0] = 0xe4;
            command[1] = speedByte;
            command[3] = (byte)address;
            command[4] = (byte)speed;
            command[5] = (byte)xor;

            this.port.Write(command, 0, command.Length);
        }

        public void GetStatus(int address)
        {
            if (!this.IsConnected)
            {
                return;
            }

            byte[] command = new byte[5];

            int xor = 0xe3;

            xor = xor;
            xor ^= address;

            command[0] = 0xe3;
            command[3] = (byte)address;
            command[4] = (byte)xor;

            this.port.Write(command, 0, command.Length);
        }

        public void GetFirmwareVersion()
        {
            if (!this.IsConnected)
            {
                return;
            }

            byte[] command = new byte[3];

            command[0] = 0x21;
            command[1] = 0x21;
            command[2] = 0x00;

            this.port.Write(command, 0, command.Length);
        }

        public void ToggleAllStop()
        {
            if (!this.IsConnected || this.allStopped)
            {
                return;
            }

            byte[] command = new byte[2];

            command[0] = 0x80;
            command[1] = 0x80;

            this.port.Write(command, 0, command.Length);

            this.allStopped = !this.allStopped;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialData.Chars:
                    this.ReadBytes();
                    break;
                case SerialData.Eof:
                    break;
                default:
                    break;
            }
        }

        private void ReadBytes()
        {
            int bytesToRead = this.port.BytesToRead;

            byte[] buffer = new byte[bytesToRead];

            if (bytesToRead > 0)
            {
                this.port.Read(buffer, 0, bytesToRead);
            }

            Console.WriteLine(BitConverter.ToString(buffer));
            Console.WriteLine("--------------------------------------");

            List<byte> dataReceivedBuffer = new List<byte>(buffer);

            this.ProcessDataReceived(dataReceivedBuffer);
        }

        private void ProcessDataReceived(List<byte> dataReceivedBuffer)
        {
            if (dataReceivedBuffer[0] == 0x01)
            {
                //Message from the Elite
                if (dataReceivedBuffer[1] == 0x01)
                {
                    //Timeout talking to the Elite over the serial port?
                }
                else if (dataReceivedBuffer[1] == 0x02)
                {
                    //Timeout talking to command station - can this happen with the Elite?
                }
                else if (dataReceivedBuffer[1] == 0x03)
                {
                    //Unknown comms error
                }
                else if (dataReceivedBuffer[1] == 0x04)
                {
                    //Instruction was sent from the Elite successfully
                }
                else if (dataReceivedBuffer[1] == 0x05)
                {
                    //Command station no longer providing the Elite with a comms timeslot?
                }
                else if (dataReceivedBuffer[1] == 0x06)
                {
                    //Buffer overflow in the Elite?
                }
            }
            else if (dataReceivedBuffer[0] == 0xe4 || dataReceivedBuffer[0] == 0xe5)
            {
                //A broadcast packet from the Elite
            }
            else if (dataReceivedBuffer[0] == 0xe3 && dataReceivedBuffer[1] == 0x40)
            {
                //Loco is being operated by another XpressNet device
                this.OnDataReceived(dataReceivedBuffer);
            }
            else if (dataReceivedBuffer[0] == 0xe4)
            {
                //Loco status information
                this.OnLocoStatusReceived(dataReceivedBuffer);
            }
        }

        private void OnDataReceived(List<byte> data)
        {
            if (this.DataReceived != null)
            {
                this.DataReceived(this, new EventArgs<XpressNetPacket>(new XpressNetPacket(data)));
            }
        }

        private void OnLocoStatusReceived(List<byte> data)
        {
            if (this.LocoStatusReceived != null)
            {
                this.LocoStatusReceived(this, new EventArgs<LocoStatusPacket>(new LocoStatusPacket(data)));
            }
        }
    }
}