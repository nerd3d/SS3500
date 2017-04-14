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
    public string servername { get; private set; }

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
      servername = ServerInput1.Text+".eng.utah.edu";

      // Attempt to connect to the server.  We want this blocking and to only exit upon success.

      if (ServerInput1.Text == "true") { /* <<< Replace with connection success bool*/
        // If connection successful, open Document Name Dialog and close this window
        SpreadsheetContext.getAppContext().RunForm(new DocNameDialog());

        Close();
      } else {
        // connection failed should let the user know that something went wrong
        DialogResult badConnect = MessageBox.Show( "Connection to Server failed.",
                                                "Connection Failure Warning", 
                                                MessageBoxButtons.OK, 
                                                MessageBoxIcon.Warning);
      }
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
