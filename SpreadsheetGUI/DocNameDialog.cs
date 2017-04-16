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

    /// <summary>
    /// Open Document Dialog Box
    /// </summary>
    public DocNameDialog() {
      InitializeComponent();

      OpenInput1.Focus();
    }

    /// <summary>
    /// Click Open Handeler.  Sends the file name to the server and to the spreadsheet for the header.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Open1_Click(object sender, EventArgs e) {
      filename = OpenInput1.Text + ".sprd"; // may want to do some error checking here (don't want .sprd.sprd)

      // Send the filename to the server


      // open a new Spreadsheet form and apply filename as header.
      SSForm theSS = new SSForm();
      SpreadsheetContext.getAppContext().RunForm(theSS);
      theSS.Text = filename;

      Close();
    }

    private void Cancel2_Click(object sender, EventArgs e) {
      Close();
    }
  }
}
