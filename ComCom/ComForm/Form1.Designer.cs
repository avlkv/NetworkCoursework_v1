namespace ComForm
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.b_con = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cb_PortNames = new System.Windows.Forms.ComboBox();
            this.b_OpenPort = new System.Windows.Forms.Button();
            this.b_ChooseFile = new System.Windows.Forms.Button();
            this.b_SendFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(21, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(512, 380);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(557, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Send \"Hello\" as msg";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // b_con
            // 
            this.b_con.Location = new System.Drawing.Point(557, 248);
            this.b_con.Name = "b_con";
            this.b_con.Size = new System.Drawing.Size(141, 23);
            this.b_con.TabIndex = 4;
            this.b_con.Text = "Проверить соединение";
            this.b_con.UseVisualStyleBackColor = true;
            this.b_con.Click += new System.EventHandler(this.b_con_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(557, 151);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Send \"Hello\" as file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cb_PortNames
            // 
            this.cb_PortNames.FormattingEnabled = true;
            this.cb_PortNames.Location = new System.Drawing.Point(557, 191);
            this.cb_PortNames.Name = "cb_PortNames";
            this.cb_PortNames.Size = new System.Drawing.Size(141, 21);
            this.cb_PortNames.TabIndex = 6;
            // 
            // b_OpenPort
            // 
            this.b_OpenPort.Location = new System.Drawing.Point(557, 219);
            this.b_OpenPort.Name = "b_OpenPort";
            this.b_OpenPort.Size = new System.Drawing.Size(141, 23);
            this.b_OpenPort.TabIndex = 7;
            this.b_OpenPort.Text = "Открыть порт";
            this.b_OpenPort.UseVisualStyleBackColor = true;
            this.b_OpenPort.Click += new System.EventHandler(this.b_OpenPort_Click);
            // 
            // b_ChooseFile
            // 
            this.b_ChooseFile.Location = new System.Drawing.Point(557, 296);
            this.b_ChooseFile.Name = "b_ChooseFile";
            this.b_ChooseFile.Size = new System.Drawing.Size(141, 23);
            this.b_ChooseFile.TabIndex = 8;
            this.b_ChooseFile.Text = "Выбрать файл...";
            this.b_ChooseFile.UseVisualStyleBackColor = true;
            this.b_ChooseFile.Click += new System.EventHandler(this.b_ChooseFile_Click);
            // 
            // b_SendFile
            // 
            this.b_SendFile.Location = new System.Drawing.Point(557, 326);
            this.b_SendFile.Name = "b_SendFile";
            this.b_SendFile.Size = new System.Drawing.Size(141, 23);
            this.b_SendFile.TabIndex = 9;
            this.b_SendFile.Text = "Отправить файл";
            this.b_SendFile.UseVisualStyleBackColor = true;
            this.b_SendFile.Click += new System.EventHandler(this.b_SendFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(557, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 413);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_SendFile);
            this.Controls.Add(this.b_ChooseFile);
            this.Controls.Add(this.b_OpenPort);
            this.Controls.Add(this.cb_PortNames);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.b_con);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "COMpass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button b_con;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cb_PortNames;
        private System.Windows.Forms.Button b_OpenPort;
        private System.Windows.Forms.Button b_ChooseFile;
        public System.Windows.Forms.Button b_SendFile;
        private System.Windows.Forms.Label label1;
    }
}

