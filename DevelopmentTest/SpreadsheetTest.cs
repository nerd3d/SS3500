/*******************************************************************
 * Author:		Christopher Allan
 * Class:		CS 3500
 * Semester:	Fall 2016
 * Program:	    EAE_CS
 * School:		University of Utah
 * Date:		09/23/2016
 ******************************************************************/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SS;
using SpreadsheetUtilities;
using System.Xml;

namespace DevelopmentTest
{
    [TestClass]
    public class SpreadsheetTest
    {
        /// <summary>
        /// Set up test files for read/write tests
        /// </summary>
        public SpreadsheetTest()
        {
            createTestFiles();
        }

        //====================================================================
        //      Empty Sheet Tests
        //====================================================================

        /// <summary>
        /// Create a new Spreadsheet without getting an error
        /// </summary>
        [TestMethod]
        public void EmptySuccessTest()
        {
            Spreadsheet t = new Spreadsheet();
            Assert.IsNotNull(t);    // spreadsheet object exists
        }

        /// <summary>
        /// GetCellContents(string name):
        ///     If name is null, throws an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptyContentNullTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.GetCellContents(null);    // null name
        }

        /// <summary>
        /// "Empty" cells should return content "" and value 0.0
        /// </summary>
        [TestMethod]
        public void EmptyContentValidTest()
        {
            Spreadsheet t = new Spreadsheet();
            Assert.AreEqual("", t.GetCellContents("A1"));
            Assert.AreEqual("", t.GetCellValue("A1"));
        }

        /// <summary>
        /// GetCellContents(string name):
        ///     If name is invalid, throws an InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptyContentInvalidTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.GetCellContents("5D"); // invalid Cell Name
        }

        /// <summary>
        /// GetNamesOfAllNonemptyCells():
        ///     Empty Spreadsheet should return an empty IEnumerable
        /// </summary>
        [TestMethod]
        public void EmptyGetAllTest()
        {
            Spreadsheet t = new Spreadsheet();
            IEnumerator<string> s =  t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(s.MoveNext());   // contains no elements
        }


        /// <summary>
        /// SetCellContents(string name, Formula formula):
        ///     Valid name and formula should succeed
        /// </summary>
        [TestMethod]
        public void EmptySetFormulaTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "=2+2");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(s.MoveNext());    // contains 1 element
            Assert.IsFalse(s.MoveNext());   // doesn't contain more
        }

        /// <summary>
        /// Add a Formula that contains an invalid variable.  Creation Needs to succeed,
        ///     but the value needs to contain a FormulaError
        /// </summary>
        [TestMethod]
        public void EmptySetFormulaError()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "=A_5*8"); // invalid Cell Name, Valid Variable Name
            t.SetContentsOfCell("A2", "=A1+3");  // Valid reference to a cell with FormulaError
            
            Assert.IsTrue(t.GetCellValue("A1") is FormulaError);
            Assert.IsTrue(t.GetCellValue("A2") is FormulaError);
        }

        /// <summary>
        /// SetCellContents(string name, Formula formula):
        ///     If name is null, throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySetNullNameTest1()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell(null, "=2+2");    // null name
        }

        /// <summary>
        /// SetCellContents(string name, Formula formula):
        ///     If name is invalid, throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySetInvalidNameTest1()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("1A", "=2+2");    // invalid name
        }

        /// <summary>
        /// SetCellContents(string name, string text):
        ///     Valid name and text should succeed
        /// </summary>
        [TestMethod]
        public void EmptySetTextTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("B1", "Hello");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(s.MoveNext());    // contains 1 element
            Assert.IsFalse(s.MoveNext());   // doesn't contain more
        }

        /// <summary>
        /// SetCellContents(string name, string text):
        ///     If text is null, throws an ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptySetNullTextTest()
        {
            string s = null;
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("B1", s); // null text
            
        }

        /// <summary>
        /// SetCellContents(string name, string text):
        ///     If name is null, throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySetNullNameTest2()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell(null, "Hello"); // null name
        }

        /// <summary>
        /// SetCellContents(string name, string text):
        ///     If name is invalid, throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySetInvalidNameTest2()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("1B", "Hello");   //invalid name
        }

        /// <summary>
        /// SetCellContents(string name, double number):
        ///     Valid name should succeed
        /// </summary>
        [TestMethod]
        public void EmptySetDoubleTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("C1", "8.03");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(s.MoveNext());    // contains 1 element
            Assert.IsFalse(s.MoveNext());   // doesn't contain more than 1 element
            Assert.AreEqual((object)8.03, t.GetCellContents("C1"));
            Assert.AreEqual(8.03, t.GetCellValue("C1"));
        }

        /// <summary>
        /// SetCellContents(string name, double number):
        ///     If name is null, throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySetNullNameTest3()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell(null, "8.03"); // name is null
        }

        /// <summary>
        /// SetCellContents(string name, double number):
        ///     If name is invalid, throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySetInvalidNameTest3()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("1C", "8.03"); // invalid name
        }


        //====================================================================
        //      Cell Replacement Tests
        //====================================================================

        /// <summary>
        /// Uses SetCellContents(string, double):
        ///     Varifies replacment works properly
        /// </summary>
        [TestMethod]
        public void ReplaceCellDouble()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "4");
            t.SetContentsOfCell("A1", "8.05");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(s.MoveNext());    // contains 1 element
            Assert.IsFalse(s.MoveNext());   // doesn't contain more than 1 element
        }


        /// <summary>
        /// Uses SetCellContents(string, double):
        ///     Varifies replacment works properly
        /// </summary>
        [TestMethod]
        public void ReplaceCellFormula()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "=2+2");
            t.SetContentsOfCell("A1", "=8+8");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(s.MoveNext());    // contains 1 element
            Assert.IsFalse(s.MoveNext());   // doesn't contain more than 1 element
        }

        /// <summary>
        /// Uses SetCellContents(string, double):
        ///     Varifies replacment works properly
        /// </summary>
        [TestMethod]
        public void ReplaceCellString()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "Hello");
            t.SetContentsOfCell("A1", "Bye");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(s.MoveNext());    // contains 1 element
            Assert.IsFalse(s.MoveNext());   // doesn't contain more than 1 element
        }

        /// <summary>
        /// Uses SetCellContents(string, string):
        ///     Cell removal works properly
        /// </summary>
        [TestMethod]
        public void ReplaceCellStringEmpty()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "Hello");
            t.SetContentsOfCell("A1", "");
            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(s.MoveNext());   // doesn't contain any elements
        }

        //====================================================================
        //      (single) Double/Text Cell Content Tests
        //====================================================================

        /// <summary>
        /// SetCellContents(string name, double number):
        ///     Valid name should succeed
        /// GetNamesOfAllNonemptyCells():
        ///     Should return {name}
        /// </summary>
        [TestMethod]
        public void GetAllDoubleTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "7");
            IEnumerator<string> nameList = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(nameList.MoveNext());         // contains >=0 elements
            Assert.IsTrue(nameList.Current == "A1");    // Name == "A1"
            Assert.IsFalse(nameList.MoveNext());        // contains no more elements

        }

        /// <summary>
        /// SetCellContents(string name, double number):
        ///     Valid name should succeed
        /// GetCellContents(string name):
        ///     Should return number
        /// </summary>
        [TestMethod]
        public void GetContentDoubleTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "7");
            
            Assert.AreEqual((object)7.0, t.GetCellContents("A1")); // verify contents
        }
        
        /// <summary>
        /// SetCellContents(string name, string text):
        ///     Valid name should succeed
        /// GetNamesOfAllNonemptyCells():
        ///     Should return {name}
        /// </summary>
        [TestMethod]
        public void GetAllTextTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "Hello");
            IEnumerator<string> nameList = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(nameList.MoveNext());         // contains >=0 elements
            Assert.IsTrue(nameList.Current == "A1");    // Name == "A1"
            Assert.IsFalse(nameList.MoveNext());        // contains no more elements
        }

        /// <summary>
        /// SetCellContents(string name, string text):
        ///     Valid name should succeed
        /// GetCellContents(string name):
        ///     Should return text
        /// </summary>
        [TestMethod]
        public void GetContentTextTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "Hello");

            Assert.AreEqual((object)"Hello", t.GetCellContents("A1")); // verify contents
        }
        

        //====================================================================
        //      (single) Formula Cell Content Tests
        //====================================================================

        /// <summary>
        /// SetCellContents(string name, Formula formula):
        ///     Valid name should succeed
        /// GetNamesOfAllNonemptyCells():
        ///     Should return {name}
        /// </summary>
        [TestMethod]
        public void GetAllFormulaTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "=2+2");
            IEnumerator<string> nameList = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(nameList.MoveNext());         // contains >=0 elements
            Assert.IsTrue(nameList.Current == "A1");    // Name == "A1"
            Assert.IsFalse(nameList.MoveNext());        // contains no more elements

        }

        /// <summary>
        /// SetCellContents(string name, Formula formula):
        ///     Valid name should succeed
        /// GetCellContents(string name):
        ///     Should return formula
        /// </summary>
        [TestMethod]
        public void GetContentFormulaTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "=2+2");

            Assert.AreEqual(new Formula("2+2"), t.GetCellContents("A1")); // verify contents
        }
        

        //====================================================================
        //      (multiple) Cell Content Tests 
        //====================================================================

        /// <summary>
        /// Using the following Cell Contents:
        ///     A1 = 36
        ///     A2 = A1/2
        ///     A3 = A2+7
        ///     A4 = A1-A3
        ///     B1 = "Older"
        ///     B3 = "Younger"
        ///     B4 = "Difference"
        /// Validate dependencies are functional and accurate
        /// </summary>
        [TestMethod]
        public void MultiCellTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "36");
            t.SetContentsOfCell("A2", "=A1/2");
            t.SetContentsOfCell("A3", "=A2+7");
            t.SetContentsOfCell("A4", "=A1-A3");
            t.SetContentsOfCell("B1", "Older");
            t.SetContentsOfCell("B3", "Younger");
            t.SetContentsOfCell("B4", "Difference");
            // Cells should be set up properly

            HashSet<string> names = (HashSet<string>)t.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(names.Count, 7);
            Assert.AreEqual((object)36.0, t.GetCellContents("A1")); // checking first added content
            Assert.AreEqual(new Formula("A2+7"), t.GetCellContents("A3")); // checking random added content
            Assert.AreEqual((object)"Difference", t.GetCellContents("B4")); // checking last added content

            Assert.AreEqual(18.0, t.GetCellValue("A2"));  // checking placeholder evaluate (= 36.0 /2)
            Assert.AreEqual(25.0, t.GetCellValue("A3"));  // checking placeholder evaluate (= 18.0 + 7)
            Assert.AreEqual(11.0, t.GetCellValue("A4"));  // checking placeholder evaluate (= 36.0 - 25.0)

            Assert.AreEqual("Difference", t.GetCellValue("B4"));
        }

        /// <summary>
        /// Using the following Cell Contents:
        ///     A1 = 36
        ///     A2 = A1/2
        ///     A3 = A2+7
        ///     A4 = A1-A3
        ///     B1 = "Older"
        ///     B3 = "Younger"
        ///     B4 = "Difference"
        /// Validate dependencies are functional and accurate
        /// ADDITONALLY, test that replacing cells maintains integrety
        /// </summary>
        [TestMethod]
        public void MultiCellReplaceTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "DeleteMe");
            t.SetContentsOfCell("A1", "36");
            t.SetContentsOfCell("A2", "=A1/2");
            t.SetContentsOfCell("A3", "=A2+7");
            t.SetContentsOfCell("A4", "=A1-A3");
            t.SetContentsOfCell("A5", "DeleteMe2");
            t.SetContentsOfCell("B1", "Older");
            t.SetContentsOfCell("B3", "Younger");
            t.SetContentsOfCell("B4", "Difference");
            t.SetContentsOfCell("A5", "");
            // Cells should be set up properly

            HashSet<string> names = (HashSet<string>)t.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(names.Count, 7);
            Assert.AreEqual((object)36.0, t.GetCellContents("A1")); // checking first added content
            Assert.AreEqual(new Formula("A2+7"), t.GetCellContents("A3")); // checking random added content
            Assert.AreEqual((object)"Difference", t.GetCellContents("B4")); // checking last added content
        }

        /// <summary>
        /// Modify Cells that affect other cells; varify resulting state
        /// </summary>
        [TestMethod]
        public void MultiCellReEvaluate()
        {

            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "18");
            t.SetContentsOfCell("A2", "=A1/2");
            t.SetContentsOfCell("A3", "=A2+7");
            t.SetContentsOfCell("A4", "=A1-A3");
            t.SetContentsOfCell("B1", "Older");
            t.SetContentsOfCell("B3", "Younger");
            t.SetContentsOfCell("B4", "Difference");
            // Cells should be set up properly
            
            Assert.AreEqual(9.0, t.GetCellValue("A2"));  // checking placeholder evaluate (= 18.0 /2)
            Assert.AreEqual(16.0, t.GetCellValue("A3"));  // checking placeholder evaluate (= 9.0 + 7)
            Assert.AreEqual(2.0, t.GetCellValue("A4"));  // checking placeholder evaluate (= 18.0 - 16.0)

            // Make a Change to A1
            t.SetContentsOfCell("A1", "36");

            Assert.AreEqual(18.0, t.GetCellValue("A2"));  // checking placeholder evaluate (= 36.0 /2)
            Assert.AreEqual(25.0, t.GetCellValue("A3"));  // checking placeholder evaluate (= 18.0 + 7)
            Assert.AreEqual(11.0, t.GetCellValue("A4"));  // checking placeholder evaluate (= 36.0 - 25.0)
        }

        /// <summary>
        /// Using the following Cell Contents:
        ///     A1 = A4
        ///     A2 = A1/2
        ///     A3 = A2+7
        ///     A4 = A1-A3
        /// Should create a circular error exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void MultiCellCircleTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "=A4");
            t.SetContentsOfCell("A2", "=A1/2");
            t.SetContentsOfCell("A3", "=A2+7");
            t.SetContentsOfCell("A4", "=A1-A3");
            // Cells should not be able to complete: Circular Reference Present
            
        }


        //====================================================================
        //      Read/Write Tests 
        //====================================================================

        /// <summary>
        /// a Bad file name should throw an Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetVersionBadFilename()
        {
            Spreadsheet t = new Spreadsheet();

            t.GetSavedVersion("bogusFilename.XML");
        }

        /// <summary>
        /// GetVersion on file with no "Version" Atribute should Throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetVersionBadAttribute()
        {
            Spreadsheet t = new Spreadsheet();

            t.GetSavedVersion("corruptSheetNoAttribute.XML");
        }

        /// <summary>
        /// GetVersion on file with no Elements should Throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetVersionNoElements()
        {
            Spreadsheet t = new Spreadsheet();

            t.GetSavedVersion("corruptSheetNoElements.XML");
        }

        /// <summary>
        /// GetVersion on file with invalid starting element should Throw
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetVersionWrongElements()
        {
            Spreadsheet t = new Spreadsheet();

            t.GetSavedVersion("corruptSheetWrongElement.XML");
        }

        /// <summary>
        /// Saving an empty spreadsheet should creat a file
        /// </summary>
        [TestMethod]
        public void EmptyWriteTest()
        {
            Spreadsheet t = new Spreadsheet();

            t.Save("testSaveSheet.XML"); // save to file
            Assert.IsTrue(System.IO.File.Exists("testSaveSheet.XML"));
        }

        /// <summary>
        /// A saved, empty spreadsheet still has a version
        /// </summary>
        [TestMethod]
        public void EmptyGetVersionTest()
        {
            Spreadsheet t = new Spreadsheet();

            t.Save("testSaveSheet.XML"); // save to file
            Assert.AreEqual("default", t.GetSavedVersion("testSaveSheet.XML")); // get saved version
        }

        /// <summary>
        /// Test saving a spreadsheet with a single cell
        /// </summary>
        [TestMethod]
        public void SingleWriteTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1","42");

            t.Save("testSaveSheet.XML"); // save to file
            Assert.IsTrue(System.IO.File.Exists("testSaveSheet.XML"));
        }

        /// <summary>
        /// Test saving a spreadsheet with multiple cells
        /// </summary>
        [TestMethod]
        public void ManyWriteTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "36");
            t.SetContentsOfCell("A2", "=A1/2");
            t.SetContentsOfCell("A3", "=A2+7");
            t.SetContentsOfCell("A4", "=A1-A3");
            t.SetContentsOfCell("B1", "Older");
            t.SetContentsOfCell("B3", "Younger");
            t.SetContentsOfCell("B4", "Difference");
            // Cells should be set up properly

            t.Save("testSaveSheet.XML"); // save to file
            Assert.IsTrue(System.IO.File.Exists("testSaveSheet.XML"));
        }

        /// <summary>
        /// Test GetVersion with a saved spreadsheet that has many cells
        /// </summary>
        [TestMethod]
        public void ManyGetVersionTest()
        {
            Spreadsheet t = new Spreadsheet();
            t.SetContentsOfCell("A1", "36");
            t.SetContentsOfCell("A2", "=A1/2");
            t.SetContentsOfCell("A3", "=A2+7");
            t.SetContentsOfCell("A4", "=A1-A3");
            t.SetContentsOfCell("B1", "Older");
            t.SetContentsOfCell("B3", "Younger");
            t.SetContentsOfCell("B4", "Difference");
            // Cells should be set up properly

            t.Save("testSaveSheet.XML"); // save to file
            Assert.AreEqual("default", t.GetSavedVersion("testSaveSheet.XML")); // get saved version
        }

        /// <summary>
        /// Validates a unique version properly saves and is retrieved from a file
        /// </summary>
        [TestMethod]
        public void TestVersionGetVersionTest()
        {
            Spreadsheet t = new Spreadsheet(s=>true, s=>s, "versionTest");

            t.Save("testSaveSheet.XML"); // save to file
            Assert.AreEqual("versionTest", t.GetSavedVersion("testSaveSheet.XML")); // get saved version
        }

        /// <summary>
        /// Load a spreadsheet from an empty legal file
        /// </summary>
        [TestMethod]
        public void TestLoadEmptyFile()
        {
            Spreadsheet t = new Spreadsheet("loadEmptySheet.XML", x => true, x => x, "PS6");

            IEnumerator<string> s = t.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsFalse(s.MoveNext());   // contains no elements
        }

        /// <summary>
        /// Load a spreadsheet from file of different version
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestLoadVersionMismatch()
        {
            Spreadsheet t = new Spreadsheet("loadEmptySheet.XML", x => true, x => x, "badVersion");
            // Should fail to load due to  version mismatch
        }

        /// <summary>
        /// Load a non-empty Spreadsheet and verify contents
        /// </summary>
        [TestMethod]
        public void TestLoadManyFile()
        {
            Spreadsheet t = new Spreadsheet("loadTestSheet.XML", x => true, x => x, "PS6");

            HashSet<string> compareList = new HashSet<string>();
            compareList.Add("A1");
            compareList.Add("A2");
            compareList.Add("A3");
            compareList.Add("A4");
            compareList.Add("B1");
            compareList.Add("B3");
            compareList.Add("B4");

            HashSet<string> cellList = (HashSet<string>)t.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(7, cellList.Count);
            Assert.IsTrue(cellList.SetEquals(compareList));
        }

        /// <summary>
        /// Loads a huge file and determins the size is correct.
        /// </summary>
        [TestMethod]
        public void TestLoadHugeFile()
        {
            Spreadsheet t = new Spreadsheet("loadHugeSheet.XML", x => true, x => x, "PS6"); // added for PS6 

            Assert.AreEqual(99*26, ((HashSet<string>)t.GetNamesOfAllNonemptyCells()).Count);
        }

        /// <summary>
        /// Attempts to load a spreadsheet with a corrupt cell
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadBadCellsTest()
        {
            Spreadsheet t = new Spreadsheet("corruptSheetBadCells.XML", x => true, x => x, "PS6");
            // Should fail due to Bad Cell Name/Content
        }

        /// <summary>
        /// Attempts to load a spreadsheet with a corrupt cell
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void LoadBadCellNameTest()
        {
            Spreadsheet t = new Spreadsheet("corruptSheetBadCellName.XML", x => true, x => x.ToUpper(), "PS6");
            // Should fail due to Bad Cell Name (not normalized)
        }

        private void createTestFiles()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            };

            using (XmlWriter file = XmlWriter.Create("loadEmptySheet.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet"); 
                file.WriteAttributeString("version", "PS6"); // add Version details
                file.WriteEndElement();
            }

            using (XmlWriter file = XmlWriter.Create("corruptSheetWrongElement.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("SSheet"); // invalid element name
                file.WriteAttributeString("version", "PS6"); // add Version details
                file.WriteEndElement();
            }

                using (XmlWriter file = XmlWriter.Create("corruptSheetNoElements.XML", settings))
            { } // creates an empty file

                using (XmlWriter file = XmlWriter.Create("corruptSheetNoAttribute.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet");
                // Version details is missing

                // Cell Block
                file.WriteStartElement("cell");

                // Cell Name Block -> invalid
                file.WriteStartElement("name");
                file.WriteString("A1");
                file.WriteEndElement();

                // Cell Content Block
                file.WriteStartElement("contents");
                file.WriteString("36");
                file.WriteEndElement();
                file.WriteEndElement();
                file.WriteEndElement();
            }

            using (XmlWriter file = XmlWriter.Create("corruptSheetBadCells.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet");
                file.WriteAttributeString("version", "PS6"); // add Version details

                // Cell Block
                file.WriteStartElement("cell");

                // Cell Name is missing

                // Cell Content Block
                file.WriteStartElement("contents");
                file.WriteString("36");
                file.WriteEndElement();
                file.WriteEndElement();
                file.WriteEndElement();
            }

            using (XmlWriter file = XmlWriter.Create("corruptSheetBadCellName.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet");
                file.WriteAttributeString("version", "PS6"); // add Version details
                
                // Cell Block
                file.WriteStartElement("cell");

                // Cell Name Block -> invalid
                file.WriteStartElement("name");
                file.WriteString("a_1");
                file.WriteEndElement();

                // Cell Content Block
                file.WriteStartElement("contents");
                file.WriteString("36");
                file.WriteEndElement();
                file.WriteEndElement();
                file.WriteEndElement();
            }

            using (XmlWriter file = XmlWriter.Create("loadTestSheet.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet");
                file.WriteAttributeString("version", "PS6"); // add Version details

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("A1");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("36");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("A2");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("=A1/2");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("A3");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("=A2+7");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("A4");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("=A1-A3");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("B1");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("Older");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("B3");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("Younger");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteStartElement("cell");
                file.WriteStartElement("name");
                file.WriteString("B4");
                file.WriteEndElement();
                file.WriteStartElement("contents");
                file.WriteString("Difference");
                file.WriteEndElement();
                file.WriteEndElement();

                file.WriteEndElement();
            }
            
            using (XmlWriter file = XmlWriter.Create("loadHugeSheet.XML", settings))
            {
                // Begin Spreadsheet Block
                file.WriteStartElement("spreadsheet");
                file.WriteAttributeString("version", "PS6"); // add Version details

                for (int col = 0; col < 26; col++)
                {
                    for(int row = 0; row < 99; row++)
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
        }
    }
}
