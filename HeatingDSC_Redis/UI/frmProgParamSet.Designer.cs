namespace HeatingDSC.UI
{
    partial class frmProgParamSet
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
            this.updqueryTimer = new System.Windows.Forms.NumericUpDown();
            this.updgprsComPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudCmdInterval = new System.Windows.Forms.NumericUpDown();
            this.nudDtuTimeOut = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.updqueryTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updgprsComPort)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCmdInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtuTimeOut)).BeginInit();
            this.SuspendLayout();
            // 
            // updqueryTimer
            // 
            this.updqueryTimer.Location = new System.Drawing.Point(141, 22);
            this.updqueryTimer.Maximum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.updqueryTimer.Name = "updqueryTimer";
            this.updqueryTimer.Size = new System.Drawing.Size(78, 21);
            this.updqueryTimer.TabIndex = 0;
            this.updqueryTimer.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // updgprsComPort
            // 
            this.updgprsComPort.Location = new System.Drawing.Point(141, 145);
            this.updgprsComPort.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.updgprsComPort.Name = "updgprsComPort";
            this.updgprsComPort.Size = new System.Drawing.Size(78, 21);
            this.updgprsComPort.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "采集数据间隔(s)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "DTU通讯端口";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(98, 201);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "保存(&C)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudCmdInterval);
            this.groupBox1.Controls.Add(this.nudDtuTimeOut);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.updqueryTimer);
            this.groupBox1.Controls.Add(this.updgprsComPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(30, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 183);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "传输参数设置";
            // 
            // nudCmdInterval
            // 
            this.nudCmdInterval.Location = new System.Drawing.Point(141, 104);
            this.nudCmdInterval.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudCmdInterval.Name = "nudCmdInterval";
            this.nudCmdInterval.Size = new System.Drawing.Size(78, 21);
            this.nudCmdInterval.TabIndex = 12;
            this.nudCmdInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudDtuTimeOut
            // 
            this.nudDtuTimeOut.Location = new System.Drawing.Point(141, 63);
            this.nudDtuTimeOut.Maximum = new decimal(new int[] {
            700,
            0,
            0,
            0});
            this.nudDtuTimeOut.Name = "nudDtuTimeOut";
            this.nudDtuTimeOut.Size = new System.Drawing.Size(78, 21);
            this.nudDtuTimeOut.TabIndex = 11;
            this.nudDtuTimeOut.Value = new decimal(new int[] {
            135,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "DTU超时阈值(s)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "命令发送间隔(s)";
            // 
            // frmProgParamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 245);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgParamSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数设置";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.frmProgParamSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updqueryTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updgprsComPort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCmdInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtuTimeOut)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown updqueryTimer;
        private System.Windows.Forms.NumericUpDown updgprsComPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudCmdInterval;
        private System.Windows.Forms.NumericUpDown nudDtuTimeOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}