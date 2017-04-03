namespace repack {
    partial class FormMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvChannels = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboxProjectName = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.editConsole = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.editApkFilename = new System.Windows.Forms.TextBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.labelApkName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lvChannels);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 442);
            this.panel1.TabIndex = 1;
            // 
            // lvChannels
            // 
            this.lvChannels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvChannels.FullRowSelect = true;
            this.lvChannels.GridLines = true;
            this.lvChannels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvChannels.Location = new System.Drawing.Point(0, 36);
            this.lvChannels.MultiSelect = false;
            this.lvChannels.Name = "lvChannels";
            this.lvChannels.Size = new System.Drawing.Size(265, 406);
            this.lvChannels.TabIndex = 1;
            this.lvChannels.UseCompatibleStateImageBehavior = false;
            this.lvChannels.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 48;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "渠道";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "状态";
            this.columnHeader3.Width = 48;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboxProjectName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(265, 36);
            this.panel2.TabIndex = 0;
            // 
            // cboxProjectName
            // 
            this.cboxProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxProjectName.FormattingEnabled = true;
            this.cboxProjectName.Location = new System.Drawing.Point(25, 8);
            this.cboxProjectName.Name = "cboxProjectName";
            this.cboxProjectName.Size = new System.Drawing.Size(215, 25);
            this.cboxProjectName.TabIndex = 0;
            this.cboxProjectName.SelectedIndexChanged += new System.EventHandler(this.cboxProjectName_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.editConsole);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(265, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(359, 442);
            this.panel3.TabIndex = 2;
            // 
            // editConsole
            // 
            this.editConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editConsole.Location = new System.Drawing.Point(0, 163);
            this.editConsole.Multiline = true;
            this.editConsole.Name = "editConsole";
            this.editConsole.ReadOnly = true;
            this.editConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editConsole.Size = new System.Drawing.Size(359, 279);
            this.editConsole.TabIndex = 1;
            this.editConsole.WordWrap = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnAbort);
            this.panel4.Controls.Add(this.btnExecute);
            this.panel4.Controls.Add(this.labelApkName);
            this.panel4.Controls.Add(this.editApkFilename);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(8);
            this.panel4.Size = new System.Drawing.Size(359, 163);
            this.panel4.TabIndex = 0;
            // 
            // editApkFilename
            // 
            this.editApkFilename.Dock = System.Windows.Forms.DockStyle.Top;
            this.editApkFilename.Location = new System.Drawing.Point(8, 8);
            this.editApkFilename.Name = "editApkFilename";
            this.editApkFilename.ReadOnly = true;
            this.editApkFilename.Size = new System.Drawing.Size(343, 23);
            this.editApkFilename.TabIndex = 0;
            // 
            // btnAbort
            // 
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(198, 132);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(80, 23);
            this.btnAbort.TabIndex = 10;
            this.btnAbort.Text = "中断";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Enabled = false;
            this.btnExecute.Location = new System.Drawing.Point(80, 131);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(80, 23);
            this.btnExecute.TabIndex = 9;
            this.btnExecute.Text = "一键打包";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // labelApkName
            // 
            this.labelApkName.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelApkName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelApkName.Location = new System.Drawing.Point(6, 36);
            this.labelApkName.Name = "labelApkName";
            this.labelApkName.Size = new System.Drawing.Size(345, 89);
            this.labelApkName.TabIndex = 8;
            this.labelApkName.Text = "拖放APK文件至此……";
            this.labelApkName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "渠道包制作";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lvChannels;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboxProjectName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox editConsole;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox editApkFilename;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label labelApkName;

    }
}

