namespace SpreadsheetGUI {
  partial class ConnectionDialog {
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
      this.ServerInput1 = new System.Windows.Forms.TextBox();
      this.InputLabel1 = new System.Windows.Forms.Label();
      this.InputLabel2 = new System.Windows.Forms.Label();
      this.Accept1 = new System.Windows.Forms.Button();
      this.Cancel1 = new System.Windows.Forms.Button();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // ServerInput1
      // 
      this.ServerInput1.AcceptsReturn = true;
      this.ServerInput1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.ServerInput1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ServerInput1.Location = new System.Drawing.Point(211, 46);
      this.ServerInput1.Name = "ServerInput1";
      this.ServerInput1.Size = new System.Drawing.Size(143, 35);
      this.ServerInput1.TabIndex = 0;
      // 
      // InputLabel1
      // 
      this.InputLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.InputLabel1.AutoSize = true;
      this.InputLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InputLabel1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
      this.InputLabel1.Location = new System.Drawing.Point(124, 56);
      this.InputLabel1.Name = "InputLabel1";
      this.InputLabel1.Size = new System.Drawing.Size(76, 25);
      this.InputLabel1.TabIndex = 1;
      this.InputLabel1.Text = "Server:";
      this.InputLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.InputLabel1.Click += new System.EventHandler(this.label1_Click);
      // 
      // InputLabel2
      // 
      this.InputLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.InputLabel2.AutoSize = true;
      this.InputLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InputLabel2.Location = new System.Drawing.Point(359, 56);
      this.InputLabel2.Name = "InputLabel2";
      this.InputLabel2.Size = new System.Drawing.Size(131, 25);
      this.InputLabel2.TabIndex = 2;
      this.InputLabel2.Text = ".eng.utah.edu";
      this.InputLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // Accept1
      // 
      this.Accept1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.Accept1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Accept1.Location = new System.Drawing.Point(67, 16);
      this.Accept1.Name = "Accept1";
      this.Accept1.Size = new System.Drawing.Size(124, 35);
      this.Accept1.TabIndex = 3;
      this.Accept1.Text = "Connect";
      this.Accept1.UseVisualStyleBackColor = true;
      // 
      // Cancel1
      // 
      this.Cancel1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.Cancel1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.Cancel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Cancel1.Location = new System.Drawing.Point(325, 16);
      this.Cancel1.Name = "Cancel1";
      this.Cancel1.Size = new System.Drawing.Size(124, 35);
      this.Cancel1.TabIndex = 4;
      this.Cancel1.Text = "Cancel";
      this.Cancel1.UseVisualStyleBackColor = true;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.Accept1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.Cancel1, 1, 0);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(41, 130);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(516, 68);
      this.tableLayoutPanel1.TabIndex = 5;
      // 
      // ConnectionDialog
      // 
      this.AcceptButton = this.Accept1;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.Cancel1;
      this.ClientSize = new System.Drawing.Size(599, 224);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.InputLabel2);
      this.Controls.Add(this.InputLabel1);
      this.Controls.Add(this.ServerInput1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ConnectionDialog";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Connect to Server";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox ServerInput1;
    private System.Windows.Forms.Label InputLabel1;
    private System.Windows.Forms.Label InputLabel2;
    private System.Windows.Forms.Button Accept1;
    private System.Windows.Forms.Button Cancel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
  }
}