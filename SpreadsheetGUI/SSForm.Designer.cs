namespace SS
{
    partial class SSForm
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
      this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addressBox = new System.Windows.Forms.TextBox();
      this.valueBox = new System.Windows.Forms.TextBox();
      this.contentBox = new System.Windows.Forms.TextBox();
      this.ContentButton = new System.Windows.Forms.Button();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // spreadsheetPanel1
      // 
      this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 80);
      this.spreadsheetPanel1.Name = "spreadsheetPanel1";
      this.spreadsheetPanel1.Size = new System.Drawing.Size(998, 555);
      this.spreadsheetPanel1.TabIndex = 0;
      this.spreadsheetPanel1.SelectionChanged += new SS.SelectionChangedHandler(this.spreadsheetPanel1_SelectionChanged);
      this.spreadsheetPanel1.Enter += new System.EventHandler(this.spreadsheetPanel1_Enter);
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(999, 33);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(288, 6);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
      this.closeToolStripMenuItem.Text = "Close";
      this.closeToolStripMenuItem.ToolTipText = "Close Current Spreadsheet";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.ToolTipText = "Close All Open Spreadsheets";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(178, 30);
      this.aboutToolStripMenuItem.Text = "About";
      this.aboutToolStripMenuItem.ToolTipText = "About This Application";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // addressBox
      // 
      this.addressBox.Location = new System.Drawing.Point(14, 37);
      this.addressBox.Name = "addressBox";
      this.addressBox.ReadOnly = true;
      this.addressBox.Size = new System.Drawing.Size(100, 26);
      this.addressBox.TabIndex = 2;
      this.addressBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // valueBox
      // 
      this.valueBox.Location = new System.Drawing.Point(120, 37);
      this.valueBox.Name = "valueBox";
      this.valueBox.ReadOnly = true;
      this.valueBox.Size = new System.Drawing.Size(200, 26);
      this.valueBox.TabIndex = 3;
      this.valueBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // contentBox
      // 
      this.contentBox.AcceptsReturn = true;
      this.contentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.contentBox.Location = new System.Drawing.Point(328, 37);
      this.contentBox.Name = "contentBox";
      this.contentBox.Size = new System.Drawing.Size(626, 26);
      this.contentBox.TabIndex = 4;
      this.contentBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contentBox_KeyDown);
      // 
      // ContentButton
      // 
      this.ContentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ContentButton.Location = new System.Drawing.Point(960, 37);
      this.ContentButton.Margin = new System.Windows.Forms.Padding(0);
      this.ContentButton.Name = "ContentButton";
      this.ContentButton.Size = new System.Drawing.Size(38, 32);
      this.ContentButton.TabIndex = 5;
      this.ContentButton.Text = "←";
      this.ContentButton.UseVisualStyleBackColor = true;
      this.ContentButton.Click += new System.EventHandler(this.ContentButton_Click);
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
      this.newToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
      this.newToolStripMenuItem.Text = "&New Connection";
      this.newToolStripMenuItem.ToolTipText = "Create New Spreadsheet";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
      // 
      // SSForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(999, 634);
      this.Controls.Add(this.ContentButton);
      this.Controls.Add(this.contentBox);
      this.Controls.Add(this.valueBox);
      this.Controls.Add(this.addressBox);
      this.Controls.Add(this.spreadsheetPanel1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "SSForm";
      this.Text = "Spreadshet";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SSForm_FormClosing);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox addressBox;
        private System.Windows.Forms.TextBox valueBox;
        private System.Windows.Forms.TextBox contentBox;
        private System.Windows.Forms.Button ContentButton;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
  }
}

