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
            this.buttonSelectShedule = new System.Windows.Forms.Button();
            this.textBoxWorkPath = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxResFilePath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonSelectShedule
            // 
            this.buttonSelectShedule.Location = new System.Drawing.Point(23, 48);
            this.buttonSelectShedule.Name = "buttonSelectShedule";
            this.buttonSelectShedule.Size = new System.Drawing.Size(184, 23);
            this.buttonSelectShedule.TabIndex = 0;
            this.buttonSelectShedule.Text = "Выбрать файл с расписанием";
            this.buttonSelectShedule.UseVisualStyleBackColor = true;
            this.buttonSelectShedule.Click += new System.EventHandler(this.buttonSelectShedule_Click);
            // 
            // textBoxWorkPath
            // 
            this.textBoxWorkPath.Location = new System.Drawing.Point(23, 22);
            this.textBoxWorkPath.Name = "textBoxWorkPath";
            this.textBoxWorkPath.Size = new System.Drawing.Size(440, 20);
            this.textBoxWorkPath.TabIndex = 1;
            this.textBoxWorkPath.Text = "D:\\chetv_va\\Диплом 2021\\Данные для работы\\work.xlsx";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(23, 129);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "check";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.ComputeButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(240, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 50);
            this.button1.TabIndex = 3;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxResFilePath
            // 
            this.textBoxResFilePath.Location = new System.Drawing.Point(23, 90);
            this.textBoxResFilePath.Name = "textBoxResFilePath";
            this.textBoxResFilePath.Size = new System.Drawing.Size(440, 20);
            this.textBoxResFilePath.TabIndex = 4;
            this.textBoxResFilePath.Text = "D:\\chetv_va\\Диплом 2021\\Данные для работы\\Results.txt";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 450);
            this.Controls.Add(this.textBoxResFilePath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.textBoxWorkPath);
            this.Controls.Add(this.buttonSelectShedule);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Программа для расчёта оптимального количества СНО";
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
    }
}

