namespace SS {
  partial class DocNameDialog {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.OpenLabel2 = new System.Windows.Forms.Label();
      this.OpenLabel1 = new System.Windows.Forms.Label();
      this.OpenInput1 = new System.Windows.Forms.TextBox();
      this.Open1 = new System.Windows.Forms.Button();
      this.Cancel2 = new System.Windows.Forms.Button();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // OpenLabel2
      // 
      this.OpenLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.OpenLabel2.AutoSize = true;
      this.OpenLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OpenLabel2.Location = new System.Drawing.Point(250, 45);
      this.OpenLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.OpenLabel2.Name = "OpenLabel2";
      this.OpenLabel2.Size = new System.Drawing.Size(40, 17);
      this.OpenLabel2.TabIndex = 5;
      this.OpenLabel2.Text = ".sprd";
      this.OpenLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // OpenLabel1
      // 
      this.OpenLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.OpenLabel1.AutoSize = true;
      this.OpenLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OpenLabel1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
      this.OpenLabel1.Location = new System.Drawing.Point(79, 45);
      this.OpenLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.OpenLabel1.Name = "OpenLabel1";
      this.OpenLabel1.Size = new System.Drawing.Size(69, 17);
      this.OpenLabel1.TabIndex = 4;
      this.OpenLabel1.Text = "Filename:";
      this.OpenLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // OpenInput1
      // 
      this.OpenInput1.AcceptsReturn = true;
      this.OpenInput1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.OpenInput1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OpenInput1.Location = new System.Drawing.Point(152, 39);
      this.OpenInput1.Margin = new System.Windows.Forms.Padding(2);
      this.OpenInput1.Name = "OpenInput1";
      this.OpenInput1.Size = new System.Drawing.Size(97, 26);
      this.OpenInput1.TabIndex = 3;
      // 
      // Open1
      // 
      this.Open1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.Open1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Open1.Location = new System.Drawing.Point(38, 5);
      this.Open1.Margin = new System.Windows.Forms.Padding(2);
      this.Open1.Name = "Open1";
      this.Open1.Size = new System.Drawing.Size(96, 33);
      this.Open1.TabIndex = 3;
      this.Open1.Text = "Open";
      this.Open1.UseVisualStyleBackColor = true;
      this.Open1.Click += new System.EventHandler(this.Open1_Click);
      // 
      // Cancel2
      // 
      this.Cancel2.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.Cancel2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.Cancel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Cancel2.Location = new System.Drawing.Point(210, 5);
      this.Cancel2.Margin = new System.Windows.Forms.Padding(2);
      this.Cancel2.Name = "Cancel2";
      this.Cancel2.Size = new System.Drawing.Size(96, 33);
      this.Cancel2.TabIndex = 4;
      this.Cancel2.Text = "Cancel";
      this.Cancel2.UseVisualStyleBackColor = true;
      this.Cancel2.Click += new System.EventHandler(this.Cancel2_Click);
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.Open1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.Cancel2, 1, 0);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(29, 82);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(344, 44);
      this.tableLayoutPanel1.TabIndex = 6;
      // 
      // DocNameDialog
      // 
      this.AcceptButton = this.Open1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.Cancel2;
      this.ClientSize = new System.Drawing.Size(399, 146);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.OpenLabel2);
      this.Controls.Add(this.OpenLabel1);
      this.Controls.Add(this.OpenInput1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DocNameDialog";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Open Spreadsheet";
      this.TopMost = true;
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label OpenLabel2;
    private System.Windows.Forms.Label OpenLabel1;
    private System.Windows.Forms.TextBox OpenInput1;
    private System.Windows.Forms.Button Open1;
    private System.Windows.Forms.Button Cancel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
  }
}