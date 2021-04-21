namespace DegreePrjWinForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonSelectShedule = new System.Windows.Forms.Button();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxResFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxParkingsCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSelectShedule
            // 
            this.buttonSelectShedule.Location = new System.Drawing.Point(31, 59);
            this.buttonSelectShedule.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSelectShedule.Name = "buttonSelectShedule";
            this.buttonSelectShedule.Size = new System.Drawing.Size(245, 28);
            this.buttonSelectShedule.TabIndex = 0;
            this.buttonSelectShedule.Text = "Выбрать файл с расписанием";
            this.buttonSelectShedule.UseVisualStyleBackColor = true;
            this.buttonSelectShedule.Click += new System.EventHandler(this.buttonSelectShedule_Click);
            // 
            // textBoxWorkPath
            // 
            this.textBoxWorkPath.Location = new System.Drawing.Point(31, 27);
            this.textBoxWorkPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxWorkPath.Name = "textBoxWorkPath";
            this.textBoxWorkPath.Size = new System.Drawing.Size(585, 22);
            this.textBoxWorkPath.TabIndex = 1;
            this.textBoxWorkPath.Text = "C:\\Users\\chetv_va\\Desktop\\Education\\Diploma\\Git\\Degree-project\\DegreePrjWinForm\\D" +
    "egreePrjWinForm\\Source\\work.xlsx";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(31, 236);
            this.checkButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(100, 28);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "Рассчитать";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.ComputeButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(543, 234);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Выход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // textBoxResFilePath
            // 
            this.textBoxResFilePath.Location = new System.Drawing.Point(31, 191);
            this.textBoxResFilePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxResFilePath.Name = "textBoxResFilePath";
            this.textBoxResFilePath.Size = new System.Drawing.Size(585, 22);
            this.textBoxResFilePath.TabIndex = 4;
            this.textBoxResFilePath.Text = "D:\\chetv_va\\Диплом 2021\\Данные для работы\\Results.txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 111);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Число стоянок в блоке";
            // 
            // textBoxParkingsCount
            // 
            this.textBoxParkingsCount.Location = new System.Drawing.Point(223, 107);
            this.textBoxParkingsCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxParkingsCount.Name = "textBoxParkingsCount";
            this.textBoxParkingsCount.Size = new System.Drawing.Size(52, 22);
            this.textBoxParkingsCount.TabIndex = 6;
            this.textBoxParkingsCount.Text = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 154);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Диапазон ";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(147, 148);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(185, 22);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.Value = new System.DateTime(2020, 5, 1, 9, 24, 0, 0);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(440, 148);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(185, 22);
            this.dateTimePicker2.TabIndex = 9;
            this.dateTimePicker2.Value = new System.DateTime(2020, 5, 3, 9, 46, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 154);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "с";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 154);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "по";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 276);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxParkingsCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxResFilePath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.textBoxWorkPath);
            this.Controls.Add(this.buttonSelectShedule);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Модуль для расчёта оптимального количества СНО";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectShedule;
        private System.Windows.Forms.TextBox textBoxWorkPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxResFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxParkingsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

