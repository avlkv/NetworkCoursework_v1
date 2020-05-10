using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace ComForm
{
    class Connection
    {
        SerialPort _Port = new SerialPort();
        public SerialPort Port 
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
                if (_Port.IsOpen)
                {
                    _Port.DiscardInBuffer();
                    _Port.DiscardOutBuffer();
                }
            }
        }

        public bool setPortName(string name)
        {
            string[] PortList = SerialPort.GetPortNames();

            if (Port.IsOpen)
            {
                Log.AppendText("port " + name + ": you can't change port name while it is opened\n");
                return false;
            }
            
            if (PortList.Contains(name))
            {
                Port.PortName = name;
                return true;
            }
            Log.AppendText("port " + name + " not found\n");  //нет такого порта
            return false;
        }

        public bool OpenPort()
        {
            try
            {
                Port.Open();
                Port.DtrEnable = true;
                InitializeHandlers();

                return true;
            }

            catch (System.IO.IOException) 
            {
                Log.AppendText("port " + Port.PortName + " not found\n");
                return false;
            }

            catch (System.InvalidOperationException) //открыт в этом приложении
            {
                Log.AppendText("port " + Port.PortName + "  is already opened\n");
                return false;
            }

            catch (System.UnauthorizedAccessException) //уже открыт в другом приложении/другим окном
            {
                Log.AppendText("port " + Port.PortName + "  is already used\n");
                return false;
            }
        }

        public bool ClosePort()
        {
            if (!Port.IsOpen)
            {
                Log.AppendText("fail: port is already closed\n");
                return false;
            }
            Port.Close();
            return true;
        }

        public bool IsConnected() //оба порта открыты и готовы слать данные
        {
            return Port.IsOpen && Port.DsrHolding;
        }



        //==================================================*/*/*/*

        public const byte STARTBYTE = 0xFF;

        public enum FrameType : byte
        {
            ACK,
            MSG,
            RET_MSG,
            RET_FILE,
            FILE,
        }



        public void WriteData(string input, FrameType type)
            //пока считаем, что input строка символов
        {
            byte[] Header = { STARTBYTE, (byte)type };
            byte[] BufferToSend;
            byte[] Telegram;

            //Я добавил:
            //byte[] Header = { STARTBYTE, receiver, (byte)type, sender };
            //byte[] BufferToSend;
            byte[] spd;
            byte[] size;
            byte[] ByteToEncode;
            byte[] ByteEncoded;
            byte[] NumOfFrames;
            byte[] fileId = { 0 };
            int i;

            switch (type)
            {
                case FrameType.MSG:
                    #region MSG
                    if (IsConnected())
                    {
                        // Telegram[] = Coding(input); 
                        Telegram = Encoding.Default.GetBytes(input); //потом это кыш

                        BufferToSend = new byte[Header.Length + Telegram.Length]; //буфер для отправки = заголовок+сообщение
                        Header.CopyTo(BufferToSend, 0);
                        Telegram.CopyTo(BufferToSend, Header.Length);

                        Port.Write(BufferToSend, 0, BufferToSend.Length);
                        Log.AppendText("(" + Port.PortName + ") WriteData: sent message >  " + Encoding.Default.GetString(Telegram) + "\n");
                    }
                    break;
                #endregion


                case FrameType.FILE:
                    #region FILE
                    if (IsConnected())
                    {

                        //byte[] size = new byte[10];
                        //size = Encoding.Default.GetBytes((input.Length).ToString()); //размер файла
                        //Log.AppendText("Длина Размер файла: " + size.Length + "\n");

                        ////Telegram = Coding(input); 
                        //Telegram = Encoding.Default.GetBytes(input); //кыш
                        //Log.AppendText("Длина Telegram: " + Telegram.Length + "\n");

                        //BufferToSend = new byte[Header.Length + 10 + Telegram.Length];

                        //Header.CopyTo(BufferToSend, 0);
                        //size.CopyTo(BufferToSend, Header.Length);
                        //Telegram.CopyTo(BufferToSend, Header.Length + 10);//!
                        //Log.AppendText("Длина BufferToSend: " + BufferToSend.Length + "\n");

                        //Port.Write(BufferToSend, 0, BufferToSend.Length);
                        //Log.AppendText("(" + Port.PortName + ") WriteData: sent file >  " + input.Length + " bytes\n");

                        //чёрная магия на случай, когда файл больше 2048 и не влезает в буфер
                        //нужна ли она

                        //Я добавил:
                        ByteToEncode = File.ReadAllBytes(@input);
                        size = new byte[10];
                        //MessageBox.Show(((double)ByteToEncode.Length / 1048576).ToString());
                        size = Encoding.Unicode.GetBytes(((double)ByteToEncode.Length / 1024).ToString()); //1048576 байт = 1 Мбайт
                        //size = Encoding.Default.GetBytes((input.Length).ToString());
                        ByteEncoded = new byte[ByteToEncode.Length * 2];
                        i = 0;

                        string typeFile = @input.Split('.')[1];
                        fileId[0] = TypeFile_to_IdFile(typeFile);

                        Display.AppendText("Длина ByteToEncode:" + ByteToEncode.Length + "\n");
                        foreach (byte item in ByteToEncode)
                        {
                            Hamming.HammingEncode74(ByteToEncode[i]).CopyTo(ByteEncoded, i * 2);
                            i++;
                            //Display.AppendText(i / ByteToEncode.Length * 100 + ", ");
                           

                            //loading.progressBar1.Increment(i / ByteToEncode.Length * 100);
                        }
                        double parts;

                        parts = (int)Math.Ceiling((double)ByteEncoded.Length / (double)(Port.WriteBufferSize - Header.Length - fileId.Length - 10));
                        //NumOfFrames = Encoding.Unicode.GetBytes((parts).ToString());
                        if (parts == 1/*Header.Length + fileId.Length + ByteEncoded.Length + 8 < Port.WriteBufferSize*/)
                        {
                            BufferToSend = new byte[Header.Length + fileId.Length + 10 + ByteEncoded.Length];//!
                            Header.CopyTo(BufferToSend, 0);
                            fileId.CopyTo(BufferToSend, Header.Length);
                            size.CopyTo(BufferToSend, Header.Length + fileId.Length);
                            //NumOfFrames.CopyTo(BufferToSend, Header.Length + fileId.Length + size.Length);
                            ByteEncoded.CopyTo(BufferToSend, Header.Length + fileId.Length + 10 /*+ NumOfFrames.Length*/);//!
                            bool flag = false;
                            while (!flag)
                            {


                                //Application.DoEvents();
                                if (MessageBox.Show("Send?", "Test", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {

                                    flag = true;
                                    Port.Write(BufferToSend, 0, BufferToSend.Length);
                                    //loading.Hide();
                                    MessageBox.Show("Готово!");
                                    //loading.progressBar1.Value = 0;
                                    //loading.i = 1;

                                }
                                else if (i == 3)
                                {
                                    flag = true;
                                    //loading.Hide();
                                    //loading.progressBar1.Value = 0;
                                    MessageBox.Show("Вы отменили передачу файла.");
                                    // loading.i = 1;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Объем файла больше объема буфера!", "Костыль", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                            //MessageBox.Show(Port.WriteBufferSize.ToString() + " " + parts.ToString() + ((int)Math.Ceiling(parts)).ToString());
                            for (i = 0; i <= parts/*(int)Math.Ceiling(parts)*/; i++)
                            {
                                BufferToSend = new byte[Port.WriteBufferSize];//!
                                Header.CopyTo(BufferToSend, 0);
                                fileId.CopyTo(BufferToSend, Header.Length);
                                byte[] Part_ByteEncoded;
                                Part_ByteEncoded = new byte[Port.WriteBufferSize - Header.Length - fileId.Length];
                                Array.ConstrainedCopy(ByteEncoded, i * (Port.WriteBufferSize - Header.Length - fileId.Length), Part_ByteEncoded, 0, (Port.WriteBufferSize - Header.Length - fileId.Length));
                                Part_ByteEncoded.CopyTo(BufferToSend, Header.Length + fileId.Length);//!
                                Port.Write(BufferToSend, 0, BufferToSend.Length);
                            }
                        }


                        
                        //fileId[0] = TypeFile_to_IdFile(typeFile);
                    }
                    break;
                #endregion

                default:
                    if (IsConnected())
                        Port.Write(Header, 0, Header.Length);
                    break;
            }

                                                                                                                                                  //Зачем такая конструкция?
            Log.Invoke(new EventHandler(delegate
            {
                Log.AppendText("sent frame " + type + "\n"); //всё записываем, мы же снобы
            }));
        }


        public void InitializeHandlers()
        {
            Port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
        }


        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Port.ReadByte() == STARTBYTE)
            {
                GetData(Port.ReadByte());
            }
        }

        public void GetData(int typeId)
        {
            FrameType type = (FrameType)typeId;
            //FrameType frametype = (FrameType)Enum.ToObject(typeof(FrameType), frametypeid);
            int bytesToRead;
            byte[] byteBuffer;
            int i;
            byte[] ToDecode;
            byte[] Decoded;
            int parts;
            string parts_s;
            //WriteLog(DirectionType.INCOMING, frametype);
            Log.Invoke(new EventHandler(delegate
            {
                Log.AppendText("get frame " + type +"\n");
            }));


            switch (type)
            {
                case FrameType.MSG:
                    #region MSG
                    if (IsConnected())
                    {
                        int n = Port.BytesToRead;
                        byte[] msgByteBuffer = new byte[n];
                        
                        Port.Read(msgByteBuffer, 0, n); //считываем сообщение
                        string Message = Encoding.Default.GetString(msgByteBuffer);
                        Log.Invoke(new EventHandler(delegate
                        {
                            Log.AppendText("(" + Port.PortName + ") GetData: new message > " + Message + "\n");
                        }));

                        WriteData(null, FrameType.ACK);
                    }
                    else
                    {
                        WriteData(null, FrameType.RET_MSG);
                    }
                    break;
                #endregion

                
                case FrameType.FILE:

                    #region FILE
                    if (IsConnected())
                    {
                        //byte[] byteBuffer;

                        //byte[] size = new byte[10];
                        //Port.Read(size, 0, 10);
                        //string size_s = Encoding.Default.GetString(size);

                        //double ssize = Double.Parse(size_s); //размер файла //нужен ли он вообще /наверное, нужен

                        //int n = Port.BytesToRead;
                        //byteBuffer = new byte[n];
                        //Port.Read(byteBuffer, 0, n);

                        //Log.Invoke(new EventHandler(delegate
                        //{
                        //    Log.AppendText("(" + Port.PortName + ") : GetData: new file > " + byteBuffer.Length + " bytes\n");
                        //}));

                        //WriteData(null, FrameType.ACK);

                        //Port.ReadByte();
                        byte fileId = (byte)Port.ReadByte();
                        string typeFile = TypeFileAnalysis(fileId);
                        byte[] size;
                        //byte[] NumOfFrames;
                        string size_s;
                        size = new byte[10];
                        for ( i = 0; i<size.Length; i++)
                        {
                            size[i] = 0;
                        }
                        Port.Read(size, 0, 10);
                        size_s = Encoding.Unicode.GetString(size);
                        //NumOfFrames = new byte[10];
                        //for (i = 0; i < NumOfFrames.Length; i++)
                        //{
                        //    NumOfFrames[i] = 0;
                        //}
                        //Port.Read(NumOfFrames, 0, 10);
                        //parts_s = Encoding.Unicode.GetString(NumOfFrames);
                        //parts = int.Parse(parts_s);
                        //MessageBox.Show(spd_s)
                        //double ssize;
                        //ssize = double.Parse(size_s);
                        
                        DialogResult result;
                        result = MessageBox.Show("Файл. Размер: "+size_s+" Кбайт.\nПринять?", "Прием файла", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            //Decoded = new byte[(int)Math.Ceiling((double)Port.BytesToRead * parts / 2)];
                           
                            //for (int j = 0; j <= parts; j++)
                            //{
                            bytesToRead = Port.BytesToRead;
                                byteBuffer = new byte[bytesToRead];
                                Port.Read(byteBuffer, 0, bytesToRead);
                                Decoded = new byte[bytesToRead / 2];
                                ToDecode = new byte[2];
                                ;
                                for (i = 0; i < bytesToRead / 2; i++)
                                {
                                    ToDecode[0] = byteBuffer[i * 2];
                                    ToDecode[1] = byteBuffer[(i * 2) + 1];
                                    Decoded[i] = Hamming.Decode(ToDecode);
                                }
                            //}
                            SaveFileDialog saveFileDialog = new SaveFileDialog();

                            MainForm.Invoke(new EventHandler(delegate
                            {
                                saveFileDialog.FileName = "";
                                saveFileDialog.Filter = "TypeFile (*." + typeFile + ")|*." + typeFile + "|All files (*.*)|*.*";
                                if (DialogResult.OK == saveFileDialog.ShowDialog())
                                {
                                    File.WriteAllBytes(saveFileDialog.FileName, Decoded);//!
                                    WriteData(null, FrameType.ACK);
                                    Display.Invoke(new EventHandler(delegate
                                    {
                                        Display.AppendText(
                                        "[" + DateTime.Now + "] : "
                                        + "Файл успешно получен"
                                        + "\r\n");
                                        Display.ScrollToCaret();
                                    }));
                                }
                                else
                                {
                                    // MessageBox.Show("Отмена ");
                                    Display.Invoke(new EventHandler(delegate
                                    {
                                        Display.AppendText(
                                        "[" + DateTime.Now + "] : " + ": "
                                        + "Вы не сохранили файл"
                                        + "\r\n");
                                        Display.ScrollToCaret();
                                    }));
                                }
                            }));
                        }
                        else
                        {
                            // MessageBox.Show("Отмена ");
                            Display.Invoke(new EventHandler(delegate
                            {
                                Display.AppendText(
                                "[" + DateTime.Now + "] : " + ": "
                                + "Вы не сохранили файл"
                                + "\r\n");
                                Display.ScrollToCaret();
                            }));
                        }


                    }
                    else
                    {
                        WriteData(null, FrameType.RET_FILE);
                    }

                    break;
                #endregion
                //======================================================



                case FrameType.ACK:
                    #region ACK
                    break;
                #endregion

                case FrameType.RET_MSG:
                    #region RET_MSG
                    Log.AppendText("Message error! No connection\n");
                    break;
                #endregion

                case FrameType.RET_FILE:
                    #region RET_FILE
                    Log.AppendText("File error! No connection\n");
                    break;
                    #endregion
            }
        }

        private Button _b_SendFile;
        public Button b_SendFile
        {
            get
            {
                return _b_SendFile;
            }
            set
            {
                _b_SendFile = value;
            }
        }

        private RichTextBox _Log; //штука, чтобы видеть, что творится
        public RichTextBox Log
        {
            get
            {
                return _Log;
            }
            set
            {
                _Log = value;
            }
        }

        private Label _TestLabel;
        public Label TestLabel
        {
            get
            {
                return _TestLabel;
            }
            set
            {
                _TestLabel = value;
            }
        }

        private Form _mainForm;
        public Form MainForm
        {
            get
            {
                return _mainForm;
            }
            set
            {
                _mainForm = value;
            }
        }

        private RichTextBox _Display;
        /// <summary>
        /// Окно вывода сообщений
        /// </summary>
        public RichTextBox Display
        {
            get
            {
                return _Display;
            }
            set
            {
                _Display = value;
            }
        }
        private string TypeFileAnalysis(byte fileId)
        {
            switch (fileId)
            {
                case 1:
                    return "txt";
                case 2:
                    return "png";
                case 3:
                    return "pdf";
                case 4:
                    return "docx";
                case 5:
                    return "jpeg";
                case 6:
                    return "avi";
                case 7:
                    return "mp3";
                case 8:
                    return "rar";
                default:
                    return "typenotfound";
            }
        }

        private byte TypeFile_to_IdFile(string str)
        {
            switch (str)
            {
                case "txt":
                    return 1;
                case "png":
                    return 2;
                case "pdf":
                    return 3;
                case "docx":
                    return 4;
                case "jpeg":
                    return 5;
                case "avi":
                    return 6;
                case "mp3":
                    return 7;
                case "rar":
                    return 8;
                default:
                    return 9;
            }
        }

    }
}
