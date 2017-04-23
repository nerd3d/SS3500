using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using ChatClient;

namespace SS
{
    public partial class SSForm : Form
    {
        // Spreadsheet Data associated with current Form
        private Spreadsheet personalSpreadsheet;
        private NetworkWarden warden;
        private NetworkWarden listenWarden;
        private bool istyping;
        private string oldAddress;
        private bool started = false;
        /// <summary>
        /// Constructor used for Multi-user Spreadsheet
        /// </summary>
        public SSForm(NetworkWarden ward, string filename)
        {
            InitializeComponent();

            istyping = false;
            oldAddress = "A1";
            personalSpreadsheet = new Spreadsheet(validAddress, s =>
s.ToUpper(), "PS6");
            this.Text = filename;
            addressBox.Text = "A1";
            warden = ward;
            warden.callNext = Sent_Message;

            // Set warden callback function for server messages
            listenWarden = new NetworkWarden(warden.socket, warden.ID);
            listenWarden.callNext = Receive_Message;

            // Send the filename to the server
            Networking.getData(listenWarden);
            System.Threading.Thread.Sleep(800);
            Networking.Send(warden, "Connect\t" + filename + "\t\n");

        }


        /********************************************************************************************
         * Network Comunication Section - Client -> Server
         ********************************************************************************************/

        /// <summary>
        /// Sends the "Edit" command to the server
        /// </summary>
        private void ContentButton_Click(object sender, EventArgs e)
        {
            int col, row;
            spreadsheetPanel1.GetSelection(out col, out row);
            string address = gridToAddress(col, row);
            Send_DoneTyping(address);
            string content = contentBox.Text;

            try
            {
                personalSpreadsheet.CheckContentsAreValid(content);
                Networking.Send(warden, "Edit\t" + address + "\t" +
content + "\t\n");
            }
            catch (Exception err) //something went wrong while settingthe contents
            {
                MessageBox.Show(err.Message, "Error Detected!",
MessageBoxButtons.OK, MessageBoxIcon.Stop);
                contentBox.SelectAll();
                contentBox.Focus();
            }
            }


        /// <summary>
        /// Sends the "Undo" command to the server
        /// </summary>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Networking.Send(warden, "Undo\t\n");
        }

        /// <summary>
        /// Send the "IsTyping" message to the server
        /// </summary>
        private void Send_IsTyping(string address)
        {
            if (!(istyping))
            {
                istyping = true;
                Networking.Send(warden, "IsTyping\t" + warden.ID +
"\t" + address.ToString() + "\t\n");
            }
        }
        /// <summary>
        /// Send the "DoneTyping" message to the server
        /// </summary>
        private void Send_DoneTyping(string address)
        {
            if (istyping)
            {
                istyping = false;
                Networking.Send(warden, "DoneTyping\t" + warden.ID +
"\t" + address.ToString() + "\t\n");
            }

        }

        /// <summary>
        /// Gracfully close the socket upon closing a spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SSForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /// <summary>
        /// Use to send messages to the server
        /// </summary>
        /// <param name="ward"></param>
        private void Sent_Message(NetworkWarden ward)
        {
            if (ward.buffString == null)
            {
                return;
            }

            string msg = ward.buffString;
            ward.buffString = null;
            string[] parsedMsg = msg.Split('\t');
            started = true;
            this.Invoke(new MethodInvoker(() => Receive_Startup(parsedMsg)));

        }

        /********************************************************************************************
         * Network Comunication Section - Server -> Client
         ********************************************************************************************/

        /// <summary>
        /// Receive hub - decodes message and activate proper command
        /// </summary>
        public void Receive_Message(NetworkWarden ward)
        {
            if (ward.buffString == null)
            {
                return;
            }
            string msg = ward.buffString;
            Array.Clear(ward.buffer, 0, ward.buffer.Length);
            ward.buffString = "";
            ward.message.Clear();
            string[] parsedMsg = msg.Split('\t');

            switch (parsedMsg[0])
            {
                case "Change":
                    this.Invoke(new MethodInvoker(() =>
Receive_Change(parsedMsg)));
                    break;
                case "IsTyping":
                    this.Invoke(new MethodInvoker(() => Receive_IsTyping(parsedMsg)));
                    break;
                case "DoneTyping":
                    this.Invoke(new MethodInvoker(() => Receive_DoneTyping(parsedMsg)));
                    break;
                default:
                    // error in Received message: gracefully close everything
                    System.Diagnostics.Debug.Print("Bad Message Recieved");
                    break;
            }
            Networking.getData(ward);
        }

        /// <summary>
        /// Pupulates an empty spreadsheet with given data
        /// </summary>
        private void Receive_Startup(string[] message)
        {

            if (!Int32.TryParse(message[1], out warden.ID))
            {
                //throw new Exception("Invalid user ID");
            }
            else
            {
                listenWarden.ID = warden.ID;
                for (int i = 2; i < message.Length - 1; i += 2)
                {
                    string[] convertToChange = { "Change", message[i],
message[i + 1] };
                    Receive_Change(convertToChange);
                }
            }
        }

        /// <summary>
        /// Receives a Change from the server and applys it to the spreadsheet
        /// </summary>
        private void Receive_Change(string[] message)
        {
            int col, row;

            string address = message[1];
            string content;
            if (message.Length > 2)
                content = message[2];
            else
                content = "";

            HashSet<string> cellsToUpdate = null;

            try
            {
                // set the contents and determine cells to recalculate
                Console.WriteLine("actual content being set: " + content);
                cellsToUpdate =
(HashSet<string>)personalSpreadsheet.SetContentsOfCell(address,
content);
                foreach (string cell in cellsToUpdate)
                {
                    //get col, row
                    addressToGrid(cell, out col, out row);

                    object value = personalSpreadsheet.GetCellValue(cell);
                    if (value is FormulaError)
                    {
                        value = ((FormulaError)value).Reason; //display the reason for error
                    }
                    //set value to display at cell
                    spreadsheetPanel1.SetValue(col, row, value.ToString());

                }
                addressBox.Text = address;
                string cellVal;
                addressToGrid(address, out col, out row);
                spreadsheetPanel1.GetValue(col, row, out cellVal);
                valueBox.Text = cellVal;

                //if (personalSpreadsheet.Changed && (!Regex.IsMatch(this.Text, @"(\(unsaved\))$")))
                    //this.Text = this.Text + " (unsaved)";
            }
            catch (Exception err) //something went wrong while settingthe contents
            {
                MessageBox.Show(err.Message, "Error Detected!",
MessageBoxButtons.OK, MessageBoxIcon.Stop);
                contentBox.SelectAll();
                contentBox.Focus();
            }

            }

        /// <summary>
        /// Sets a cell to "being edited" status
        /// </summary>
        private void Receive_IsTyping(string[] message)
        {
            int col, row;
            int idCheck;
            string id = message[1];
            string address = message[2];
            int.TryParse(id, out idCheck);
            //if (idCheck != warden.ID)
            //{               
                byte[] ascii = Encoding.ASCII.GetBytes(address);
                col = ascii[0] - 65;
                int.TryParse(address.Substring(1), out row);
                row -= 1;
                spreadsheetPanel1.SetValue(col, row, "...");
            //}
        }

        /// <summary>
        /// Removes "being edited" status from a cell
        /// </summary>
        private void Receive_DoneTyping(string[] message)
        {
            int col, row;
            int idCheck;
            string id = message[1];
            string address = message[2];
            int.TryParse(id, out idCheck);
            //if (idCheck != warden.ID)
            //{
                byte[] ascii = Encoding.ASCII.GetBytes(address);
                col = ascii[0] - 65;
                int.TryParse(address.Substring(1), out row);
                row -= 1;
                spreadsheetPanel1.SetValue(col, row, personalSpreadsheet.GetCellValue(address).ToString());
            //}
            /*
            Console.WriteLine("done typing recieved");
            int col, row;
            string id = message[1];
            string address = message[2];
            System.Diagnostics.Debug.Print("ID " + id + " is done editing cell: " + address);
            byte[] ascii = Encoding.ASCII.GetBytes(address);
            col = ascii[0]-65;
            int.TryParse(address.Substring(1), out row);
            row -= 1;
            System.Diagnostics.Debug.Print("cell returning to origin value: " + col + " " + row);
            System.Diagnostics.Debug.Print("value of that cell: " + personalSpreadsheet.GetCellValue(address).ToString()); 
            spreadsheetPanel1.SetValue(col, row, personalSpreadsheet.GetCellValue(address).ToString());
            */
        }

        /********************************************************************************************
         * Local Actions
         ********************************************************************************************/

        /// <summary>
        /// Given the address to a cell; it is converted and returns
        ///     the row and column to that cell in zero-index version.
        ///     (ie: "B71" -> (1,70))
        /// </summary>
        /// <param name="address">Cell Address</param>
        /// <param name="row">Row of Cell (zero indexed)</param>
        /// <param name="col">column of Cell (zero indexed)</param>
        private void addressToGrid(string address, out int col, out int row)
        {
            char colChar = address[0]; // get first character of string
            col = (int)colChar - 65;
            string rowStr = address.TrimStart(colChar); // get integerportion of string
                        row = int.Parse(rowStr) - 1;
        }

        /// <summary>
        /// Given a row and column; convert to address form (ie:(5,11) -> "F12")
        /// </summary>
        /// <param name="row">Row containing Cell</param>
        /// <param name="col">Column containing Cell</param>
        /// <returns>Address relating to Cell</returns>
        private string gridToAddress(int col, int row)
        {
            string address = char.ConvertFromUtf32(col + 65); // convert Column
            address += (row + 1).ToString();    // Convert Row

            return address;

        }

        /// <summary>
        /// Determines if the address exists on the spreadsheet
        ///     -> intended for IsValid delegate
        /// </summary>
        /// <param name="address">Address Attempt</param>
        /// <returns>Leagal Address Boolean</returns>
        private bool validAddress(string address)
        {
            if (Regex.IsMatch(address, @"(^[a-zA-Z][1-9][0-9]?$)"))
                return true;
            else
                throw new InvalidNameException();
        }


        /// <summary>
        /// When you select New it will create an empty spreadsheetwindows application.
        /// Using the first SSForm constructor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetContext.getAppContext().RunForm(new ConnectionDialog());

        }

        /// <summary>
        /// When selected it will Close down the current SpreadsheetWindows Application Form.
/// If the current form is the last one open it will shut downthe application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Closes all Open Spreadsheets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Will populate the banner with content if it is present andhighlight the text
/// so that it can be edited. If there is no content to befound then the focus is
        /// redirected to the content box without displaying anything.
        /// </summary>
        /// <param name="sender"></param>
        private void spreadsheetPanel1_SelectionChanged(SpreadsheetPanel sender)
        {
            contentBox.ResetText();

            int col, row;
            string address;
            string value;
            string content = contentBox.Text;

            Send_DoneTyping(oldAddress);
            //Gets the currently selected cell's location and Value.
            spreadsheetPanel1.GetSelection(out col, out row);

            if (spreadsheetPanel1.GetValue(col, row, out value))
            {
                //Assuming the cell now has a value it displays the address and corresponding value.
                addressBox.Text = address = gridToAddress(col, row);
                oldAddress = address;
                valueBox.Text = value;

                if (personalSpreadsheet.GetCellContents(address) is
Formula)//Checks if the content is of type formula.
                    contentBox.Text = "=" +
personalSpreadsheet.GetCellContents(address).ToString();
                else
                    contentBox.Text =
personalSpreadsheet.GetCellContents(address).ToString();

                contentBox.SelectAll();//Highlights the text inside the banner.
            }
            //set oldCellAddress to this new cell address

            contentBox.Focus();
        }

        /// <summary>
        /// When the Help menu is pulled down and About is selected itwill display information.
/// This information will instruct the user how to work theSpreadsheet Windows Application Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Below are instructions and features forthe Spreadsheet Application: \n\n" +
                "-Use arrow keys: Right,Left,Up,Down to traverse thespreadsheet. \n" +
                "-Selecting a cell with your mouse will highlight thecell to be changed. \n" +
                "-Additional features include: New Connection, Exit,Menu Item Hotkeys, and an application form name. \n\n " +
                "-New Connection will allow user to connect to aserver and open a spreadsheet.\n" +
                "-Exit will close out of all open windows and shut offthe applicaiton.\n" +
                "-Menu Item Hotkeys can be found to the right of theaction they corrispond too. \n" +
                "-To maximize and or revert the size of the screen usehotkey F11.",
                "Spreadsheet Information and Controls",
                MessageBoxButtons.OK, MessageBoxIcon.None);//Can choose what kind of symbol is displayed.
        }

        /// <summary>
        /// Looks for key presses while in the content dialog box
        ///     Enter: calls the content button method
        ///     Others can be added for more features.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentBox_KeyDown(object sender, KeyEventArgs e)
        {

            int col, row;
            switch (e.KeyCode)
            {
                case Keys.Enter: // accept contents change
                    ContentButton_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Up://Change grid to location one row aboveselected, unless it's at the top(0) row.
                    spreadsheetPanel1.GetSelection(out col, out row);
                    if (row <= 0)
                        row = 0;
                    else
                        row--;
                    spreadsheetPanel1.SetSelection(col, row);
                    spreadsheetPanel1_SelectionChanged(spreadsheetPanel1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Down://Change grid to location one row belowselected, unless it's at the botton(99) row.
                    spreadsheetPanel1.GetSelection(out col, out row);
                    if (row >= 99)
                        row = 99;
                    else
                        row++;
                    spreadsheetPanel1.SetSelection(col, row);
                    spreadsheetPanel1_SelectionChanged(spreadsheetPanel1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Left://Change grid to location one col leftof selected, unless it's at the left most collum(A/0).
                    spreadsheetPanel1.GetSelection(out col, out row);
                    if (col <= 0)
                        col = 0;
                    else
                        col--;
                    spreadsheetPanel1.SetSelection(col, row);
                    spreadsheetPanel1_SelectionChanged(spreadsheetPanel1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Right://Change grid to location one colright of selected, unless it's at the right most collum(Z/26).
                    spreadsheetPanel1.GetSelection(out col, out row);
                    if (col >= 26)
                        col = 26;
                    else
                        col++;
                    spreadsheetPanel1.SetSelection(col, row);
                    spreadsheetPanel1_SelectionChanged(spreadsheetPanel1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.F11:
                    if (!(this.WindowState == FormWindowState.Maximized))
                        this.WindowState = FormWindowState.Maximized;
                    else
                        this.WindowState = FormWindowState.Normal;
                    break;

                default:
                    spreadsheetPanel1.GetSelection(out col, out row);
                    string address = gridToAddress(col, row);
                    Send_IsTyping(address);
                    break;
            }
        }

        /// <summary>
        /// If/When spreadsheetPanel ever gets focus, immediately givefocs to the contentBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_Enter(object sender, EventArgs e)
        {
            contentBox.SelectAll();
            contentBox.Focus();
        }

    }
}