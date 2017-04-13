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
  public partial class ConnectionDialog : Form {
    public string server { get; private set; }

    /// <summary>
    /// Form Constructor
    /// </summary>
    public ConnectionDialog() {
      InitializeComponent();

      ServerInput1.Focus();
    }

    /// <summary>
    /// Click Accept triggers attempt to connect to server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Accept1_Click(object sender, EventArgs e) {
      server = ServerInput1.Text+".eng.utah.edu";

      SpreadsheetContext.getAppContext().RunForm(new DocNameDialog());

      Close();
    }

    /// <summary>
    /// Cancel should close the applications
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Cancel1_Click(object sender, EventArgs e) {
      Close();
    }
  }
}
