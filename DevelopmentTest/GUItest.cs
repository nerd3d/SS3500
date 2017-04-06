using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using System.Xml;

namespace DevelopmentTest
{
    /// <summary>
    /// Summary description for GUItest
    /// </summary>
    [CodedUITest]
    public class GUItest
    {
        public GUItest()
        {
            string exePath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SpreadsheetGUI.exe";
            if (System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SpreadsheetGUI.exe"))
            {
                System.IO.File.Delete(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SpreadsheetGUI.exe");
            }

            System.IO.File.Copy( System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+ 
                "\\Source\\Repos\\cs3500\\PS4\\SpreadsheetGUI\\bin\\Debug\\SpreadsheetGUI.exe", exePath);
        }

        /// <summary>
        /// Saves a blank new spreadsheet.  (removes it ahead of time if file already exists)
        /// </summary>
        [TestMethod]
        public void TestSave()
        {
            if(System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\testFile.sprd"))
            {
                System.IO.File.Delete(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\testFile.sprd");
            }
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.testSave1();

        }

        /// <summary>
        /// Creates a Large file then opens it.  Changes refernce cell that updates all other cells.
        /// </summary>
        [TestMethod]
        public void TestOpen()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            };

            using (XmlWriter file = XmlWriter.Create(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\testFile.sprd", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet");
                file.WriteAttributeString("version", "PS6"); // add Version details

                for (int col = 0; col < 26; col++)
                {
                    for (int row = 0; row < 99; row++)
                    {
                        file.WriteStartElement("cell");
                        file.WriteStartElement("name");
                        string cellName = char.ConvertFromUtf32(col + 65) + (row + 1).ToString();
                        file.WriteString(cellName);
                        file.WriteEndElement();
                        file.WriteStartElement("contents");
                        if (col == 0 && row == 0)
                            file.WriteString("10");
                        else
                            file.WriteString("=A1");
                        file.WriteEndElement();
                        file.WriteEndElement();
                    }
                }

                file.WriteEndElement();
            }

            this.UIMap.testLoad1();
            
        }

        /// <summary>
        /// Creates a new Spreadsheet, tests the maximize shortcut, and closes a spreadsheet.
        /// </summary>
        [TestMethod]
        public void TestNew()
        {

            this.UIMap.testNew1();

        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
