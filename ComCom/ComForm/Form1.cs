using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComForm
{
    
    public partial class Form1 : Form
    {
        Connection com1 = new Connection();
        //Connection com2 = new Connection();
        
        public Form1()
        {
            InitializeComponent();
            cb_PortNames.Items.AddRange(SerialPort.GetPortNames());
            com1.b_SendFile = b_SendFile;
            com1.MainForm = this;
            com1.Display = richTextBox1;
            com1.TestLabel = label1;
        }

        //private void b_open5_Click(object sender, EventArgs e)
        //{
        //    com1.Log = richTextBox1;
        //    com1.setPortName("COM1"); //давать список для выбора из существующих?
        //    com1.OpenPort();
            
        //    richTextBox1.AppendText(com1.Port.PortName + "  is opened: " + com1.Port.IsOpen + "\n");
        //}

        //private void b_open6_Click(object sender, EventArgs e)
        //{
        //    com2.Log = richTextBox1;
        //    com2.setPortName("COM2");
        //    com2.OpenPort();
            
        //    richTextBox1.AppendText(com2.Port.PortName + "  is opened: " + com2.Port.IsOpen + "\n");
        //}

        /// <summary>
        /// Пишет в лог, есть ли соединение
        /// </summary>
        private void b_con_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(cb_PortNames.SelectedItem.ToString() + " is connected: " + com1.IsConnected() + "\n");
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (com1.IsConnected())
            {
                com1.WriteData("Hello world", Connection.FrameType.MSG);
            }
            else
            {
                richTextBox1.AppendText("error: no connection\n");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (com1.IsConnected())
            {
                com1.WriteData("filebytesbytesbytes", Connection.FrameType.FILE);
            }
            else
            {
                richTextBox1.AppendText("error: no connection\n");
            }
        }

        /// <summary>
        /// Открывает порт com1
        /// </summary>
        private void b_OpenPort_Click(object sender, EventArgs e)
        {
            com1.Log = richTextBox1;
            com1.setPortName(cb_PortNames.SelectedItem.ToString()); //давать список для выбора из существующих?
            com1.OpenPort();

            richTextBox1.AppendText(com1.Port.PortName + "  is opened: " + com1.Port.IsOpen + "\n");
        }

        private void b_ChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                com1.WriteData(openFileDialog.FileName, Connection.FrameType.FILE);
                //if (_com1.loading.i == 2)
                //{
                //    richTextBox1.AppendText("[" + DateTime.Now + "] Вы: файл отправлен" + "\n");
                //}
                richTextBox1.ScrollToCaret();
                //if (_com1.loading.i == 3)
                //{
                //    richTextBox2.AppendText("Передача файла отменена. \n");
                //}
            }
        }

        private void b_SendFile_Click(object sender, EventArgs e)
        {
            b_SendFile.DialogResult = DialogResult.OK;
        }
    }
}
