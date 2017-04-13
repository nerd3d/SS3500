using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS {
  public partial class DocNameDialog : Form {
    public string filename { get; private set; }

    public DocNameDialog() {
      InitializeComponent();

      OpenInput1.Focus();
    }

    private void Open1_Click(object sender, EventArgs e) {
      filename = OpenInput1.Text + ".sprd";

      SpreadsheetContext.getAppContext().RunForm(new SSForm());

      Close();
    }

    private void Cancel2_Click(object sender, EventArgs e) {
      Close();
    }
  }
}
