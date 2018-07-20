namespace TestInHand
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtAddDtuWM = new System.Windows.Forms.TextBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.txtSendUser = new System.Windows.Forms.TextBox();
            this.txtUsers = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(43, 58);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(445, 265);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(493, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(104, 14);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(182, 21);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "5002";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(637, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(515, 87);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtAddDtuWM
            // 
            this.txtAddDtuWM.Location = new System.Drawing.Point(317, 13);
            this.txtAddDtuWM.Name = "txtAddDtuWM";
            this.txtAddDtuWM.Size = new System.Drawing.Size(64, 21);
            this.txtAddDtuWM.TabIndex = 5;
            this.txtAddDtuWM.Text = "100";
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(242, 345);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(337, 21);
            this.txtSend.TabIndex = 6;
            this.txtSend.Text = "0103000000104406";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(585, 343);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "send";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtSendUser
            // 
            this.txtSendUser.Location = new System.Drawing.Point(44, 345);
            this.txtSendUser.Name = "txtSendUser";
            this.txtSendUser.Size = new System.Drawing.Size(180, 21);
            this.txtSendUser.TabIndex = 8;
            this.txtSendUser.Text = "13800020002";
            // 
            // txtUsers
            // 
            this.txtUsers.Location = new System.Drawing.Point(494, 116);
            this.txtUsers.Multiline = true;
            this.txtUsers.Name = "txtUsers";
            this.txtUsers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUsers.Size = new System.Drawing.Size(236, 184);
            this.txtUsers.TabIndex = 9;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(80, 372);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "获取用户信息";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 462);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.txtUsers);
            this.Controls.Add(this.txtSendUser);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.txtAddDtuWM);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtAddDtuWM;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtSendUser;
        private System.Windows.Forms.TextBox txtUsers;
        private System.Windows.Forms.Button button5;
    }
}

