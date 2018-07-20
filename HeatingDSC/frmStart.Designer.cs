namespace HeatingDSC
{
    partial class frmStart
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStart));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.启动服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trDtuTimeOut = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Column70 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.heaSeaSetIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hSFromTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hSToTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minHeatTempDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxHeatTempDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dgvOffsetTemp = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dgvYear = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSimNo = new System.Windows.Forms.TextBox();
            this.txtSimNoUn = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkIgnoreJxLen = new System.Windows.Forms.CheckBox();
            this.rbNoReDisp = new System.Windows.Forms.RadioButton();
            this.rbOnlyData = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.chkSend = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txtSendData = new System.Windows.Forms.TextBox();
            this.txtReceiveData = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.txtCommNo = new System.Windows.Forms.TextBox();
            this.txtSendCmd = new System.Windows.Forms.TextBox();
            this.chkCRC = new System.Windows.Forms.CheckBox();
            this.btnSendCmd = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.txtWcf = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.test2 = new System.Windows.Forms.TextBox();
            this.test1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.btnResetDtu = new System.Windows.Forms.Button();
            this.txtCmd2Send = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucTvDevice1 = new HeatingDSC.Controls.ucTvDevice();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tmrJiXunDtuData = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOffsetTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYear)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "数据采集DSC";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动服务ToolStripMenuItem,
            this.服务配置ToolStripMenuItem,
            this.显示数据ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 92);
            // 
            // 启动服务ToolStripMenuItem
            // 
            this.启动服务ToolStripMenuItem.CheckOnClick = true;
            this.启动服务ToolStripMenuItem.Name = "启动服务ToolStripMenuItem";
            this.启动服务ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.启动服务ToolStripMenuItem.Text = "启动服务";
            this.启动服务ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.启动服务ToolStripMenuItem_CheckedChanged);
            // 
            // 服务配置ToolStripMenuItem
            // 
            this.服务配置ToolStripMenuItem.CheckOnClick = true;
            this.服务配置ToolStripMenuItem.Name = "服务配置ToolStripMenuItem";
            this.服务配置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.服务配置ToolStripMenuItem.Text = "服务配置";
            this.服务配置ToolStripMenuItem.Click += new System.EventHandler(this.服务配置ToolStripMenuItem_Click);
            // 
            // 显示数据ToolStripMenuItem
            // 
            this.显示数据ToolStripMenuItem.Checked = true;
            this.显示数据ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.显示数据ToolStripMenuItem.Name = "显示数据ToolStripMenuItem";
            this.显示数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.显示数据ToolStripMenuItem.Text = "显示数据";
            this.显示数据ToolStripMenuItem.Click += new System.EventHandler(this.显示数据ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // trDtuTimeOut
            // 
            this.trDtuTimeOut.Enabled = true;
            this.trDtuTimeOut.Interval = 60000;
            this.trDtuTimeOut.Tick += new System.EventHandler(this.trDtuTimeOut_Tick);
            // 
            // Column70
            // 
            this.Column70.Frozen = true;
            this.Column70.HeaderText = "序号";
            this.Column70.Name = "Column70";
            this.Column70.Width = 60;
            // 
            // heaSeaSetIDDataGridViewTextBoxColumn
            // 
            this.heaSeaSetIDDataGridViewTextBoxColumn.DataPropertyName = "HeaSeaSetID";
            this.heaSeaSetIDDataGridViewTextBoxColumn.HeaderText = "HeaSeaSetID";
            this.heaSeaSetIDDataGridViewTextBoxColumn.Name = "heaSeaSetIDDataGridViewTextBoxColumn";
            this.heaSeaSetIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.heaSeaSetIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // hSFromTimeDataGridViewTextBoxColumn
            // 
            this.hSFromTimeDataGridViewTextBoxColumn.DataPropertyName = "HSFromTime";
            this.hSFromTimeDataGridViewTextBoxColumn.HeaderText = "开始日期";
            this.hSFromTimeDataGridViewTextBoxColumn.Name = "hSFromTimeDataGridViewTextBoxColumn";
            // 
            // hSToTimeDataGridViewTextBoxColumn
            // 
            this.hSToTimeDataGridViewTextBoxColumn.DataPropertyName = "HSToTime";
            this.hSToTimeDataGridViewTextBoxColumn.HeaderText = "结束日期";
            this.hSToTimeDataGridViewTextBoxColumn.Name = "hSToTimeDataGridViewTextBoxColumn";
            // 
            // minHeatTempDataGridViewTextBoxColumn
            // 
            this.minHeatTempDataGridViewTextBoxColumn.DataPropertyName = "MinHeatTemp";
            this.minHeatTempDataGridViewTextBoxColumn.HeaderText = "最低供热温度";
            this.minHeatTempDataGridViewTextBoxColumn.Name = "minHeatTempDataGridViewTextBoxColumn";
            // 
            // maxHeatTempDataGridViewTextBoxColumn
            // 
            this.maxHeatTempDataGridViewTextBoxColumn.DataPropertyName = "MaxHeatTemp";
            this.maxHeatTempDataGridViewTextBoxColumn.HeaderText = "最高供热温度";
            this.maxHeatTempDataGridViewTextBoxColumn.Name = "maxHeatTempDataGridViewTextBoxColumn";
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(12, 20);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.Size = new System.Drawing.Size(466, 112);
            this.dataGridView3.TabIndex = 130;
            // 
            // dgvOffsetTemp
            // 
            this.dgvOffsetTemp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOffsetTemp.Location = new System.Drawing.Point(12, 14);
            this.dgvOffsetTemp.Name = "dgvOffsetTemp";
            this.dgvOffsetTemp.RowTemplate.Height = 23;
            this.dgvOffsetTemp.Size = new System.Drawing.Size(250, 91);
            this.dgvOffsetTemp.TabIndex = 124;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 21);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(250, 91);
            this.dataGridView2.TabIndex = 130;
            // 
            // dgvYear
            // 
            this.dgvYear.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvYear.Location = new System.Drawing.Point(12, 21);
            this.dgvYear.Name = "dgvYear";
            this.dgvYear.RowTemplate.Height = 23;
            this.dgvYear.Size = new System.Drawing.Size(250, 91);
            this.dgvYear.TabIndex = 130;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.splitter1);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1026, 570);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "控制";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(431, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(592, 564);
            this.panel2.TabIndex = 31;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(592, 564);
            this.splitContainer1.SplitterDistance = 523;
            this.splitContainer1.TabIndex = 33;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(592, 523);
            this.tabControl2.TabIndex = 36;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtSimNo);
            this.tabPage1.Controls.Add(this.txtSimNoUn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(584, 497);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "显示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 33;
            this.label1.Text = "在线dtu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "在线有问题dtu";
            // 
            // txtSimNo
            // 
            this.txtSimNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSimNo.Location = new System.Drawing.Point(3, 29);
            this.txtSimNo.Multiline = true;
            this.txtSimNo.Name = "txtSimNo";
            this.txtSimNo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSimNo.Size = new System.Drawing.Size(565, 242);
            this.txtSimNo.TabIndex = 32;
            // 
            // txtSimNoUn
            // 
            this.txtSimNoUn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSimNoUn.Location = new System.Drawing.Point(6, 303);
            this.txtSimNoUn.Multiline = true;
            this.txtSimNoUn.Name = "txtSimNoUn";
            this.txtSimNoUn.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSimNoUn.Size = new System.Drawing.Size(562, 497);
            this.txtSimNoUn.TabIndex = 34;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkIgnoreJxLen);
            this.tabPage2.Controls.Add(this.rbNoReDisp);
            this.tabPage2.Controls.Add(this.rbOnlyData);
            this.tabPage2.Controls.Add(this.rbAll);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.chkSend);
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Controls.Add(this.btnClear);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtCmd);
            this.tabPage2.Controls.Add(this.txtCommNo);
            this.tabPage2.Controls.Add(this.txtSendCmd);
            this.tabPage2.Controls.Add(this.chkCRC);
            this.tabPage2.Controls.Add(this.btnSendCmd);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(584, 497);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "调试";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreJxLen
            // 
            this.chkIgnoreJxLen.AutoSize = true;
            this.chkIgnoreJxLen.Checked = true;
            this.chkIgnoreJxLen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreJxLen.Location = new System.Drawing.Point(471, 84);
            this.chkIgnoreJxLen.Name = "chkIgnoreJxLen";
            this.chkIgnoreJxLen.Size = new System.Drawing.Size(96, 16);
            this.chkIgnoreJxLen.TabIndex = 130;
            this.chkIgnoreJxLen.Text = "计讯忽略长度";
            this.chkIgnoreJxLen.UseVisualStyleBackColor = true;
            // 
            // rbNoReDisp
            // 
            this.rbNoReDisp.AutoSize = true;
            this.rbNoReDisp.Location = new System.Drawing.Point(350, 127);
            this.rbNoReDisp.Name = "rbNoReDisp";
            this.rbNoReDisp.Size = new System.Drawing.Size(59, 16);
            this.rbNoReDisp.TabIndex = 54;
            this.rbNoReDisp.TabStop = true;
            this.rbNoReDisp.Text = "不显示";
            this.rbNoReDisp.UseVisualStyleBackColor = true;
            // 
            // rbOnlyData
            // 
            this.rbOnlyData.AutoSize = true;
            this.rbOnlyData.Location = new System.Drawing.Point(350, 105);
            this.rbOnlyData.Name = "rbOnlyData";
            this.rbOnlyData.Size = new System.Drawing.Size(59, 16);
            this.rbOnlyData.TabIndex = 53;
            this.rbOnlyData.TabStop = true;
            this.rbOnlyData.Text = "数据包";
            this.rbOnlyData.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(350, 83);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(59, 16);
            this.rbAll.TabIndex = 52;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "所有包";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 50;
            this.button1.Text = "清空发送区";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "发送内容：";
            // 
            // chkSend
            // 
            this.chkSend.AutoSize = true;
            this.chkSend.Location = new System.Drawing.Point(90, 122);
            this.chkSend.Name = "chkSend";
            this.chkSend.Size = new System.Drawing.Size(72, 16);
            this.chkSend.TabIndex = 48;
            this.chkSend.Text = "显示发包";
            this.chkSend.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkSend.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(12, 148);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtSendData);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtReceiveData);
            this.splitContainer2.Size = new System.Drawing.Size(566, 497);
            this.splitContainer2.SplitterDistance = 272;
            this.splitContainer2.TabIndex = 47;
            // 
            // txtSendData
            // 
            this.txtSendData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendData.Location = new System.Drawing.Point(0, 0);
            this.txtSendData.Multiline = true;
            this.txtSendData.Name = "txtSendData";
            this.txtSendData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSendData.Size = new System.Drawing.Size(272, 497);
            this.txtSendData.TabIndex = 43;
            // 
            // txtReceiveData
            // 
            this.txtReceiveData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceiveData.Location = new System.Drawing.Point(0, 0);
            this.txtReceiveData.Multiline = true;
            this.txtReceiveData.Name = "txtReceiveData";
            this.txtReceiveData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReceiveData.Size = new System.Drawing.Size(290, 497);
            this.txtReceiveData.TabIndex = 44;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(428, 119);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 46;
            this.btnClear.Text = "清空接收区";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "命令：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(284, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 45;
            this.label6.Text = "接收内容：";
            // 
            // txtCmd
            // 
            this.txtCmd.Location = new System.Drawing.Point(74, 33);
            this.txtCmd.Name = "txtCmd";
            this.txtCmd.Size = new System.Drawing.Size(464, 21);
            this.txtCmd.TabIndex = 35;
            // 
            // txtCommNo
            // 
            this.txtCommNo.Location = new System.Drawing.Point(74, 12);
            this.txtCommNo.Name = "txtCommNo";
            this.txtCommNo.Size = new System.Drawing.Size(464, 21);
            this.txtCommNo.TabIndex = 42;
            // 
            // txtSendCmd
            // 
            this.txtSendCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendCmd.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtSendCmd.Location = new System.Drawing.Point(74, 53);
            this.txtSendCmd.Name = "txtSendCmd";
            this.txtSendCmd.Size = new System.Drawing.Size(493, 21);
            this.txtSendCmd.TabIndex = 39;
            // 
            // chkCRC
            // 
            this.chkCRC.AutoSize = true;
            this.chkCRC.Location = new System.Drawing.Point(175, 84);
            this.chkCRC.Name = "chkCRC";
            this.chkCRC.Size = new System.Drawing.Size(72, 16);
            this.chkCRC.TabIndex = 37;
            this.chkCRC.Text = "含校验位";
            this.chkCRC.UseVisualStyleBackColor = true;
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.Location = new System.Drawing.Point(78, 80);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(75, 23);
            this.btnSendCmd.TabIndex = 38;
            this.btnSendCmd.Text = "发送";
            this.btnSendCmd.UseVisualStyleBackColor = true;
            this.btnSendCmd.Click += new System.EventHandler(this.btnSendCmd_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "通讯号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "实际发送：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button6);
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.txtWcf);
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Controls.Add(this.test2);
            this.tabPage4.Controls.Add(this.test1);
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(584, 497);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "测试";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(38, 376);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "测试协议";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(110, 328);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // txtWcf
            // 
            this.txtWcf.Location = new System.Drawing.Point(209, 162);
            this.txtWcf.Multiline = true;
            this.txtWcf.Name = "txtWcf";
            this.txtWcf.Size = new System.Drawing.Size(278, 224);
            this.txtWcf.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(80, 201);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "pre";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // test2
            // 
            this.test2.Location = new System.Drawing.Point(97, 120);
            this.test2.Name = "test2";
            this.test2.Size = new System.Drawing.Size(246, 21);
            this.test2.TabIndex = 2;
            this.test2.Text = "01 03 00 00 00 65 85 E1";
            // 
            // test1
            // 
            this.test1.Location = new System.Drawing.Point(97, 79);
            this.test1.Name = "test1";
            this.test1.Size = new System.Drawing.Size(246, 21);
            this.test1.TabIndex = 1;
            this.test1.Text = "386338823";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(97, 161);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button14);
            this.panel3.Controls.Add(this.btnResetDtu);
            this.panel3.Controls.Add(this.txtCmd2Send);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(344, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(87, 564);
            this.panel3.TabIndex = 30;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 408);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 129;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(0, 335);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(78, 23);
            this.button14.TabIndex = 128;
            this.button14.Text = "重启所有dtu";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // btnResetDtu
            // 
            this.btnResetDtu.Location = new System.Drawing.Point(9, 34);
            this.btnResetDtu.Name = "btnResetDtu";
            this.btnResetDtu.Size = new System.Drawing.Size(75, 23);
            this.btnResetDtu.TabIndex = 127;
            this.btnResetDtu.Text = "重启dtu";
            this.btnResetDtu.UseVisualStyleBackColor = true;
            this.btnResetDtu.Click += new System.EventHandler(this.btnResetDtu_Click);
            // 
            // txtCmd2Send
            // 
            this.txtCmd2Send.Location = new System.Drawing.Point(931, 15);
            this.txtCmd2Send.Name = "txtCmd2Send";
            this.txtCmd2Send.Size = new System.Drawing.Size(74, 21);
            this.txtCmd2Send.TabIndex = 37;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(341, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 564);
            this.splitter1.TabIndex = 27;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ucTvDevice1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 564);
            this.panel1.TabIndex = 26;
            // 
            // ucTvDevice1
            // 
            this.ucTvDevice1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTvDevice1.Location = new System.Drawing.Point(0, 0);
            this.ucTvDevice1.Name = "ucTvDevice1";
            this.ucTvDevice1.Size = new System.Drawing.Size(338, 564);
            this.ucTvDevice1.TabIndex = 0;
            this.ucTvDevice1.delNodeMouseClickHandler += new HeatingDSC.Controls.delNodeMouseClick(this.ucTvDevice1_delNodeMouseClickHandler);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1034, 596);
            this.tabControl1.TabIndex = 30;
            // 
            // tmrJiXunDtuData
            // 
            this.tmrJiXunDtuData.Interval = 1000;
            this.tmrJiXunDtuData.Tick += new System.EventHandler(this.tmrJiXunDtuData_Tick);
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 596);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "frmStart";
            this.Text = "数据采集程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStart_FormClosing);
            this.Load += new System.EventHandler(this.frmStart_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOffsetTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYear)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 启动服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 服务配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示数据ToolStripMenuItem;
        private System.Windows.Forms.Timer trDtuTimeOut;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dgvOffsetTemp;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dgvYear;

        private System.Windows.Forms.DataGridViewTextBoxColumn Column70;
        private System.Windows.Forms.DataGridViewTextBoxColumn heaSeaSetIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hSFromTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hSToTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minHeatTempDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxHeatTempDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn heaSeaSetIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeTemplateIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zTDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSimNo;
        private System.Windows.Forms.TextBox txtSimNoUn;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.TextBox txtSendData;
        private System.Windows.Forms.TextBox txtCommNo;
        private System.Windows.Forms.TextBox txtSendCmd;
        private System.Windows.Forms.CheckBox chkCRC;
        private System.Windows.Forms.Button btnSendCmd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button btnResetDtu;
        private System.Windows.Forms.TextBox txtCmd2Send;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private Controls.ucTvDevice ucTvDevice1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.RadioButton rbOnlyData;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkSend;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtReceiveData;
        private System.Windows.Forms.RadioButton rbNoReDisp;
        private System.Windows.Forms.Timer tmrJiXunDtuData;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox test2;
        private System.Windows.Forms.TextBox test1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txtWcf;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.CheckBox chkIgnoreJxLen;
    }
}