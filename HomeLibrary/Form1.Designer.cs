namespace HomeLibrary
{
    partial class HomeForm
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
            dbBranchCB = new ComboBox();
            label1 = new Label();
            dbPathTB = new TextBox();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // dbBranchCB
            // 
            dbBranchCB.DropDownStyle = ComboBoxStyle.DropDownList;
            dbBranchCB.FormattingEnabled = true;
            dbBranchCB.Location = new Point(316, 225);
            dbBranchCB.Name = "dbBranchCB";
            dbBranchCB.Size = new Size(333, 38);
            dbBranchCB.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(74, 225);
            label1.Name = "label1";
            label1.Size = new Size(206, 30);
            label1.TabIndex = 1;
            label1.Text = "Select Library Branch";
            label1.Click += label1_Click;
            // 
            // dbPathTB
            // 
            dbPathTB.Location = new Point(74, 391);
            dbPathTB.Name = "dbPathTB";
            dbPathTB.Size = new Size(682, 35);
            dbPathTB.TabIndex = 2;
            dbPathTB.TextChanged += textBox1_TextChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.InactiveCaption;
            textBox1.ForeColor = SystemColors.WindowText;
            textBox1.Location = new Point(132, 69);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(553, 35);
            textBox1.TabIndex = 3;
            textBox1.Text = "Desktop Catalog";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged_1;
            // 
            // button1
            // 
            button1.Location = new Point(671, 225);
            button1.Name = "button1";
            button1.Size = new Size(76, 38);
            button1.TabIndex = 4;
            button1.Text = "Enter";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(dbPathTB);
            Controls.Add(label1);
            Controls.Add(dbBranchCB);
            ForeColor = Color.ForestGreen;
            Name = "HomeForm";
            Text = "Home Library";
            Load += HomeForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox dbBranchCB;
        private Label label1;
        private TextBox dbPathTB;
        private TextBox textBox1;
        private Button button1;
    }
}
