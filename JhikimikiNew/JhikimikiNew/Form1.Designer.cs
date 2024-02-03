namespace JhikimikiNew
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            butSave = new Button();
            ScanCOM = new Button();
            butCreate = new Button();
            butDisconnect = new Button();
            butLoad = new Button();
            comboBox1 = new ComboBox();
            butPopulate = new Button();
            butConnect = new Button();
            textBox1 = new TextBox();
            lblCOMStatus = new Label();
            Input = new Label();
            dataGridView1 = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(butSave);
            groupBox1.Controls.Add(ScanCOM);
            groupBox1.Controls.Add(butCreate);
            groupBox1.Controls.Add(butDisconnect);
            groupBox1.Controls.Add(butLoad);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(butPopulate);
            groupBox1.Controls.Add(butConnect);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(lblCOMStatus);
            groupBox1.Controls.Add(Input);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(600, 159);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // butSave
            // 
            butSave.Location = new Point(493, 110);
            butSave.Name = "butSave";
            butSave.Size = new Size(94, 29);
            butSave.TabIndex = 7;
            butSave.Text = "Save";
            butSave.UseVisualStyleBackColor = true;
            butSave.Click += butSave_click;
            // 
            // ScanCOM
            // 
            ScanCOM.Location = new Point(178, 38);
            ScanCOM.Name = "ScanCOM";
            ScanCOM.Size = new Size(94, 29);
            ScanCOM.TabIndex = 1;
            ScanCOM.Text = "ScanCOM";
            ScanCOM.UseVisualStyleBackColor = true;
            ScanCOM.Click += butScanCOM_click;
            // 
            // butCreate
            // 
            butCreate.Location = new Point(393, 110);
            butCreate.Name = "butCreate";
            butCreate.Size = new Size(94, 29);
            butCreate.TabIndex = 5;
            butCreate.Text = "Create";
            butCreate.UseVisualStyleBackColor = true;
            butCreate.Click += butCreate_click;
            // 
            // butDisconnect
            // 
            butDisconnect.Location = new Point(493, 38);
            butDisconnect.Name = "butDisconnect";
            butDisconnect.Size = new Size(94, 29);
            butDisconnect.TabIndex = 4;
            butDisconnect.Text = "Disconnect";
            butDisconnect.UseVisualStyleBackColor = true;
            butDisconnect.Click += butDisconnect_click;
            // 
            // butLoad
            // 
            butLoad.Location = new Point(293, 111);
            butLoad.Name = "butLoad";
            butLoad.Size = new Size(94, 29);
            butLoad.TabIndex = 4;
            butLoad.Text = "Load";
            butLoad.UseVisualStyleBackColor = true;
            butLoad.Click += butLoad_click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(21, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 3;
            // 
            // butPopulate
            // 
            butPopulate.Location = new Point(152, 111);
            butPopulate.Name = "butPopulate";
            butPopulate.Size = new Size(94, 29);
            butPopulate.TabIndex = 3;
            butPopulate.Text = "Populate";
            butPopulate.UseVisualStyleBackColor = true;
            butPopulate.Click += butPopulate_click;
            // 
            // butConnect
            // 
            butConnect.Location = new Point(393, 38);
            butConnect.Name = "butConnect";
            butConnect.Size = new Size(94, 29);
            butConnect.TabIndex = 2;
            butConnect.Text = "Connect";
            butConnect.UseVisualStyleBackColor = true;
            butConnect.Click += butConnect_click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(21, 112);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 2;
            // 
            // lblCOMStatus
            // 
            lblCOMStatus.AutoSize = true;
            lblCOMStatus.Location = new Point(278, 42);
            lblCOMStatus.Name = "lblCOMStatus";
            lblCOMStatus.Size = new Size(109, 20);
            lblCOMStatus.TabIndex = 0;
            lblCOMStatus.Text = "Not Connected";
            // 
            // Input
            // 
            Input.AutoSize = true;
            Input.Location = new Point(21, 89);
            Input.Name = "Input";
            Input.Size = new Size(56, 20);
            Input.TabIndex = 1;
            Input.Text = "R input";
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 190);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(776, 248);
            dataGridView1.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button ScanCOM;
        private Button butDisconnect;
        private ComboBox comboBox1;
        private Button butConnect;
        private Label Input;
        private Label lblCOMStatus;
        private DataGridView dataGridView1;
        private TextBox textBox1;
        private Button butPopulate;
        private Button butLoad;
        private Button butCreate;
        private Button butSave;
    }
}
