namespace HeatingDSC.Controls
{
    partial class RangeInput
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDown = new System.Windows.Forms.TextBox();
            this.txtUp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtDown
            // 
            this.txtDown.Location = new System.Drawing.Point(2, 0);
            this.txtDown.Name = "txtDown";
            this.txtDown.Size = new System.Drawing.Size(64, 21);
            this.txtDown.TabIndex = 0;
            // 
            // txtUp
            // 
            this.txtUp.Location = new System.Drawing.Point(90, 0);
            this.txtUp.Name = "txtUp";
            this.txtUp.Size = new System.Drawing.Size(66, 21);
            this.txtUp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "—";
            // 
            // RangeInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUp);
            this.Controls.Add(this.txtDown);
            this.Name = "RangeInput";
            this.Size = new System.Drawing.Size(157, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDown;
        private System.Windows.Forms.TextBox txtUp;
        private System.Windows.Forms.Label label1;

    }
}
