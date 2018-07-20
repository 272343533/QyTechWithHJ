namespace HeatingDSC
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.程序参数设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出程序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.信息管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯详情ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.站点树状图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登陆详情ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.所有站点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动服务ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.停止服务ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.指令生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusbarPanel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusbarPanel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusbarPanel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvStations = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtComDetail = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统操作ToolStripMenuItem,
            this.信息管理ToolStripMenuItem,
            this.数据操作ToolStripMenuItem,
            this.视图ToolStripMenuItem,
            this.退出ToolStripMenuItem,
            this.测试ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1002, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统操作ToolStripMenuItem
            // 
            this.系统操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动服务ToolStripMenuItem,
            this.停止服务ToolStripMenuItem,
            this.程序参数设置ToolStripMenuItem,
            this.退出程序ToolStripMenuItem});
            this.系统操作ToolStripMenuItem.Name = "系统操作ToolStripMenuItem";
            this.系统操作ToolStripMenuItem.Size = new System.Drawing.Size(83, 21);
            this.系统操作ToolStripMenuItem.Text = "系统操作(&S)";
            // 
            // 启动服务ToolStripMenuItem
            // 
            this.启动服务ToolStripMenuItem.Name = "启动服务ToolStripMenuItem";
            this.启动服务ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.启动服务ToolStripMenuItem.Text = "启动服务(&B)";
            this.启动服务ToolStripMenuItem.Click += new System.EventHandler(this.启动服务ToolStripMenuItem_Click);
            // 
            // 停止服务ToolStripMenuItem
            // 
            this.停止服务ToolStripMenuItem.Name = "停止服务ToolStripMenuItem";
            this.停止服务ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.停止服务ToolStripMenuItem.Text = "停止服务(&E)";
            this.停止服务ToolStripMenuItem.Click += new System.EventHandler(this.停止服务ToolStripMenuItem_Click);
            // 
            // 程序参数设置ToolStripMenuItem
            // 
            this.程序参数设置ToolStripMenuItem.Name = "程序参数设置ToolStripMenuItem";
            this.程序参数设置ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.程序参数设置ToolStripMenuItem.Text = "程序参数设置(&P)";
            this.程序参数设置ToolStripMenuItem.Click += new System.EventHandler(this.程序参数设置ToolStripMenuItem_Click);
            // 
            // 退出程序ToolStripMenuItem
            // 
            this.退出程序ToolStripMenuItem.Name = "退出程序ToolStripMenuItem";
            this.退出程序ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.退出程序ToolStripMenuItem.Text = "退出程序(&X)";
            // 
            // 信息管理ToolStripMenuItem
            // 
            this.信息管理ToolStripMenuItem.Name = "信息管理ToolStripMenuItem";
            this.信息管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.信息管理ToolStripMenuItem.Text = "信息管理";
            this.信息管理ToolStripMenuItem.Click += new System.EventHandler(this.信息管理ToolStripMenuItem_Click);
            // 
            // 数据操作ToolStripMenuItem
            // 
            this.数据操作ToolStripMenuItem.Name = "数据操作ToolStripMenuItem";
            this.数据操作ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.数据操作ToolStripMenuItem.Text = "数据操作";
            this.数据操作ToolStripMenuItem.Click += new System.EventHandler(this.数据操作ToolStripMenuItem_Click);
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.通讯详情ToolStripMenuItem,
            this.站点树状图ToolStripMenuItem,
            this.登陆详情ToolStripMenuItem});
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.视图ToolStripMenuItem.Text = "视图(&V)";
            // 
            // 通讯详情ToolStripMenuItem
            // 
            this.通讯详情ToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.通讯详情ToolStripMenuItem.Name = "通讯详情ToolStripMenuItem";
            this.通讯详情ToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.通讯详情ToolStripMenuItem.Text = "通讯详情(&C)";
            this.通讯详情ToolStripMenuItem.Click += new System.EventHandler(this.通讯详情ToolStripMenuItem_Click);
            // 
            // 站点树状图ToolStripMenuItem
            // 
            this.站点树状图ToolStripMenuItem.Checked = true;
            this.站点树状图ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.站点树状图ToolStripMenuItem.Name = "站点树状图ToolStripMenuItem";
            this.站点树状图ToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.站点树状图ToolStripMenuItem.Text = "站点树状图(&T)";
            this.站点树状图ToolStripMenuItem.Click += new System.EventHandler(this.站点树状图ToolStripMenuItem_Click);
            // 
            // 登陆详情ToolStripMenuItem
            // 
            this.登陆详情ToolStripMenuItem.Checked = true;
            this.登陆详情ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.登陆详情ToolStripMenuItem.Name = "登陆详情ToolStripMenuItem";
            this.登陆详情ToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.登陆详情ToolStripMenuItem.Text = "登陆详情(&R)";
            this.登陆详情ToolStripMenuItem.Click += new System.EventHandler(this.登陆详情ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // 测试ToolStripMenuItem
            // 
            this.测试ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.所有站点ToolStripMenuItem,
            this.启动服务ToolStripMenuItem1,
            this.停止服务ToolStripMenuItem1,
            this.listViewToolStripMenuItem,
            this.指令生成ToolStripMenuItem});
            this.测试ToolStripMenuItem.Name = "测试ToolStripMenuItem";
            this.测试ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.测试ToolStripMenuItem.Text = "测试";
            // 
            // 所有站点ToolStripMenuItem
            // 
            this.所有站点ToolStripMenuItem.Name = "所有站点ToolStripMenuItem";
            this.所有站点ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.所有站点ToolStripMenuItem.Text = "所有站点";
            this.所有站点ToolStripMenuItem.Click += new System.EventHandler(this.所有站点ToolStripMenuItem_Click);
            // 
            // 启动服务ToolStripMenuItem1
            // 
            this.启动服务ToolStripMenuItem1.Name = "启动服务ToolStripMenuItem1";
            this.启动服务ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.启动服务ToolStripMenuItem1.Text = "启动服务";
            // 
            // 停止服务ToolStripMenuItem1
            // 
            this.停止服务ToolStripMenuItem1.Name = "停止服务ToolStripMenuItem1";
            this.停止服务ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.停止服务ToolStripMenuItem1.Text = "停止服务";
            // 
            // listViewToolStripMenuItem
            // 
            this.listViewToolStripMenuItem.Name = "listViewToolStripMenuItem";
            this.listViewToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.listViewToolStripMenuItem.Text = "ListView";
            this.listViewToolStripMenuItem.Click += new System.EventHandler(this.listViewToolStripMenuItem_Click);
            // 
            // 指令生成ToolStripMenuItem
            // 
            this.指令生成ToolStripMenuItem.Name = "指令生成ToolStripMenuItem";
            this.指令生成ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.指令生成ToolStripMenuItem.Text = "指令生成";
            this.指令生成ToolStripMenuItem.Click += new System.EventHandler(this.指令生成ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusbarPanel1,
            this.statusbarPanel2,
            this.statusbarPanel3,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 602);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1002, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusbarPanel1
            // 
            this.statusbarPanel1.Name = "statusbarPanel1";
            this.statusbarPanel1.Size = new System.Drawing.Size(0, 17);
            // 
            // statusbarPanel2
            // 
            this.statusbarPanel2.Name = "statusbarPanel2";
            this.statusbarPanel2.Size = new System.Drawing.Size(0, 17);
            // 
            // statusbarPanel3
            // 
            this.statusbarPanel3.Name = "statusbarPanel3";
            this.statusbarPanel3.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvStations);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1002, 577);
            this.splitContainer1.SplitterDistance = 140;
            this.splitContainer1.TabIndex = 3;
            // 
            // tvStations
            // 
            this.tvStations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvStations.ImageIndex = 0;
            this.tvStations.ImageList = this.imageList1;
            this.tvStations.Location = new System.Drawing.Point(0, 0);
            this.tvStations.Name = "tvStations";
            this.tvStations.SelectedImageIndex = 0;
            this.tvStations.Size = new System.Drawing.Size(140, 577);
            this.tvStations.TabIndex = 0;
            this.tvStations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvStations_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "268_16_16.BMP");
            this.imageList1.Images.SetKeyName(1, "dtu_16_16.bmp");
            this.imageList1.Images.SetKeyName(2, "dtu_16_16_Light.bmp");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView1);
            this.splitContainer2.Size = new System.Drawing.Size(858, 577);
            this.splitContainer2.SplitterDistance = 646;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.txtComDetail);
            this.splitContainer3.Panel2.ImeMode = System.Windows.Forms.ImeMode.On;
            this.splitContainer3.Size = new System.Drawing.Size(646, 577);
            this.splitContainer3.SplitterDistance = 460;
            this.splitContainer3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(646, 460);
            this.dataGridView1.TabIndex = 0;
            // 
            // txtComDetail
            // 
            this.txtComDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComDetail.Location = new System.Drawing.Point(0, 0);
            this.txtComDetail.Multiline = true;
            this.txtComDetail.Name = "txtComDetail";
            this.txtComDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComDetail.Size = new System.Drawing.Size(646, 113);
            this.txtComDetail.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(208, 577);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 35000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 624);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 程序参数设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出程序ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 信息管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem 通讯详情ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TreeView tvStations;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtComDetail;
        private System.Windows.Forms.ToolStripMenuItem 站点树状图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登陆详情ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 所有站点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动服务ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel statusbarPanel1;
        private System.Windows.Forms.ToolStripStatusLabel statusbarPanel2;
        private System.Windows.Forms.ToolStripStatusLabel statusbarPanel3;
        private System.Windows.Forms.ToolStripMenuItem 停止服务ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem listViewToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem 指令生成ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}

