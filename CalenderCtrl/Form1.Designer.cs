namespace CalenderCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ljyDateTime1 = new LJYControls.LJYDateTime();
            this.SuspendLayout();
            // 
            // ljyDateTime1
            // 
            this.ljyDateTime1.BackColor = System.Drawing.Color.Transparent;
            this.ljyDateTime1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ljyDateTime1.BackgroundImage")));
            this.ljyDateTime1.ColorName = System.Drawing.Color.Empty;
            this.ljyDateTime1.Day = 0;
            this.ljyDateTime1.Location = new System.Drawing.Point(82, 31);
            this.ljyDateTime1.MonthTxt = "04";
            this.ljyDateTime1.Name = "ljyDateTime1";
            this.ljyDateTime1.Size = new System.Drawing.Size(350, 318);
            this.ljyDateTime1.TabIndex = 0;
            this.ljyDateTime1.YearTxt = "2021";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 404);
            this.Controls.Add(this.ljyDateTime1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private LJYControls.LJYDateTime ljyDateTime1;
    }
}

