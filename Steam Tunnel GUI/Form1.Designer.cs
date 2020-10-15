namespace SteamTunnel.GUI
{
    partial class Form1
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
            this.moveToDestButton = new System.Windows.Forms.Button();
            this.moveBackButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.refresh2 = new System.Windows.Forms.Button();
            this.refresh1 = new System.Windows.Forms.Button();
            this.listView1 = new SteamTunnel.GUI.GameListView();
            this.listView2 = new SteamTunnel.GUI.GameListView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.openSettingsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // moveToDestButton
            // 
            this.moveToDestButton.Enabled = false;
            this.moveToDestButton.Location = new System.Drawing.Point(242, 110);
            this.moveToDestButton.Name = "moveToDestButton";
            this.moveToDestButton.Size = new System.Drawing.Size(34, 23);
            this.moveToDestButton.TabIndex = 2;
            this.moveToDestButton.Text = ">>";
            this.moveToDestButton.UseVisualStyleBackColor = true;
            this.moveToDestButton.Click += new System.EventHandler(this.moveToDestButton_Click);
            // 
            // moveBackButton
            // 
            this.moveBackButton.Enabled = false;
            this.moveBackButton.Location = new System.Drawing.Point(242, 139);
            this.moveBackButton.Name = "moveBackButton";
            this.moveBackButton.Size = new System.Drawing.Size(34, 23);
            this.moveBackButton.TabIndex = 4;
            this.moveBackButton.Text = "<<";
            this.moveBackButton.UseVisualStyleBackColor = true;
            this.moveBackButton.Click += new System.EventHandler(this.moveBackButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(59, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(147, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Source:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(212, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 20);
            this.button1.TabIndex = 7;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Destination:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(482, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 20);
            this.button2.TabIndex = 10;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(348, 19);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(128, 20);
            this.textBox2.TabIndex = 9;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // refresh2
            // 
            this.refresh2.Location = new System.Drawing.Point(282, 46);
            this.refresh2.Name = "refresh2";
            this.refresh2.Size = new System.Drawing.Size(224, 23);
            this.refresh2.TabIndex = 11;
            this.refresh2.Text = "Refresh";
            this.refresh2.UseVisualStyleBackColor = true;
            this.refresh2.Click += new System.EventHandler(this.refresh2_Click);
            // 
            // refresh1
            // 
            this.refresh1.Location = new System.Drawing.Point(12, 46);
            this.refresh1.Name = "refresh1";
            this.refresh1.Size = new System.Drawing.Size(224, 23);
            this.refresh1.TabIndex = 12;
            this.refresh1.Text = "Refresh";
            this.refresh1.UseVisualStyleBackColor = true;
            this.refresh1.Click += new System.EventHandler(this.refresh1_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 75);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(224, 238);
            this.listView1.TabIndex = 13;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(282, 75);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(224, 238);
            this.listView2.TabIndex = 14;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 319);
            this.progressBar1.Maximum = 4;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 34);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 357);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Done";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // openSettingsButton
            // 
            this.openSettingsButton.Location = new System.Drawing.Point(415, 359);
            this.openSettingsButton.Name = "openSettingsButton";
            this.openSettingsButton.Size = new System.Drawing.Size(91, 23);
            this.openSettingsButton.TabIndex = 17;
            this.openSettingsButton.Text = "Settings";
            this.openSettingsButton.UseVisualStyleBackColor = true;
            this.openSettingsButton.Click += new System.EventHandler(this.openSettingsButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 387);
            this.Controls.Add(this.openSettingsButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.refresh1);
            this.Controls.Add(this.refresh2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.moveBackButton);
            this.Controls.Add(this.moveToDestButton);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Steam Tunnel";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button moveToDestButton;
        private System.Windows.Forms.Button moveBackButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button refresh2;
        private System.Windows.Forms.Button refresh1;
        private GameListView listView1;
        private GameListView listView2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button openSettingsButton;
    }
}

