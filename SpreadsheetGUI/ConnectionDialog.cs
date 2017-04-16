using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatClient;


namespace SS {
  public partial class ConnectionDialog : Form {
    public string servername { get; private set; }
    private NetworkWarden warden = null;

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
      servername = ServerInput1.Text + ".eng.utah.edu";

      // Attempt to connect to the server.  We want this blocking and to only exit upon success.
      Networking.ConnectToServer(Connected, servername);

      System.Threading.Thread.Sleep(800);

      if (warden != null) { /* <<< Replace with connection success bool*/
        // If connection successful, open Document Name Dialog and close this window
        SpreadsheetContext.getAppContext().RunForm(new DocNameDialog(warden));

        Close();
      } else {
        // connection failed should let the user know that something went wrong
        DialogResult badConnect = MessageBox.Show("Connection to Server failed.",
                                                "Connection Failure Warning",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
      }
    }

    /// <summary>
    /// Callback function upon successful connection attempt
    /// </summary>
    /// <param name="warden"></param>
    private void Connected(NetworkWarden ward) {
      if (ward != null) {
        warden = ward;
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
