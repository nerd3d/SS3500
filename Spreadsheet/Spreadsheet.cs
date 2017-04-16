/*******************************************************************
 * Author:		Christopher Allan
 * Class:		CS 3500
 * Semester:	Fall 2016
 * Program:	    EAE_CS
 * School:		University of Utah
 * Date:		09/23/2016
 ******************************************************************/

/*
 * Version Update!
 *     Branched PS4 into PS5
 *     Replaced AbstractSpreadsheet with PS5 version
 */

/*
 * Version Update!
 *    Branched PS4 into PS6
 *    Invited Lee Neuschwander to edit
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Xml;

namespace SS {
  /// <summary>
  /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
  /// spreadsheet consists of an infinite number of named cells.
  /// 
  /// A string is a cell name if and only if it consists of one or more letters,
  /// followed by one or more digits AND it satisfies the predicate IsValid.
  /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
  /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
  /// regardless of IsValid.
  /// 
  /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
  /// must be normalized with the Normalize method before it is used by or saved in 
  /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
  /// the Formula "x3+a5" should be converted to "X3+A5" before use.
  /// 
  /// A spreadsheet contains a cell corresponding to every possible cell name.  
  /// In addition to a name, each cell has a contents and a value.  The distinction is
  /// important.
  /// 
  /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
  /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
  /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
  /// 
  /// In a new spreadsheet, the contents of every cell is the empty string.
  ///  
  /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
  /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
  /// in the grid.)
  /// 
  /// If a cell's contents is a string, its value is that string.
  /// 
  /// If a cell's contents is a double, its value is that double.
  /// 
  /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
  /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
  /// of course, can depend on the values of variables.  The value of a variable is the 
  /// value of the spreadsheet cell it names (if that cell's value is a double) or 
  /// is undefined (otherwise).
  /// 
  /// Spreadsheets are never allowed to contain a combination of Formulas that establish
  /// a circular dependency.  A circular dependency exists when a cell depends on itself.
  /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
  /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
  /// dependency.
  /// </summary>
  public class Spreadsheet : AbstractSpreadsheet {
    private Dictionary<string, Cell> AllCells;// Key= NAME of cell; Value= Cell Object
    private DependencyGraph Dependencies;       // Manages Dependencies
    public override bool Changed { get; protected set; }

    /// <summary>
    /// Easy constructor. Creates a simple new spreadsheed with no extra variable 
    ///   restrictions and a "default" version
    /// </summary>
    public Spreadsheet() : this(s => true, s => s, "default") { }

    /// <summary>
    /// Load from file Constructor. Loads a spreadsheet from a file, validates the version, 
    ///   variable names, cell names and circular dependencies.
    /// </summary>
    /// <param name="filePath">File path and name to a current Spreadsheet file</param>
    /// <param name="isValid"> Method used to determine whether a string that consists of one or more letters
    /// followed by one or more digits is a valid variable name.</param>
    /// <param name="normalize">Method used to convert a cell name to its standard form.  For example,
    /// Normalize might convert names to upper case.</param>
    /// <param name="version">Version information</param>
    public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) :
        base(isValid, normalize, version) {
      AllCells = new Dictionary<string, Cell>();
      Dependencies = new DependencyGraph();


      // Varify Version Match
      if (!(GetSavedVersion(filePath) == version))
        throw new SpreadsheetReadWriteException("File Version Mismatch");

      // Open File - Store contents into Spreadsheet
      using (XmlReader file = XmlReader.Create(filePath)) {
        file.Read(); // move past spreadsheet Node
        string currentElement = null;
        string cellName = null;
        while (file.Read()) {
          switch (file.NodeType) {
            // Set Element Name
            case XmlNodeType.Element:
              currentElement = file.Name;
              break;

            // If Element Name is name, save name for next iteration
            // If Element Name is content, add the Cell to the Spreadsheet
            case XmlNodeType.Text:
              if (currentElement == "name") {
                if (file.Value != Normalize(file.Value))
                  throw new SpreadsheetReadWriteException("Invalid Cell Name Found");
                cellName = file.Value;

              } else if (currentElement == "contents")
                if (cellName != null) {
                  SetContentsOfCell(cellName, file.Value);    // add the cell
                  cellName = null;                            // reset cellName
                  currentElement = null;                      // reset currentElement
                } else
                  throw new SpreadsheetReadWriteException("Invalid Cell Node");
              break;

            // Skip over anything else
            default:
              break;
          }
        }
      }
      Changed = false;

    }

    /// <summary>
    /// Standard Constructor. Creates a new empty spreadsheet with the given variable restrictions and version.
    /// </summary>
    /// <param name="isValid"> Method used to determine whether a string that consists of one or more letters
    /// followed by one or more digits is a valid variable name.</param>
    /// <param name="normalize">Method used to convert a cell name to its standard form.  For example,
    /// Normalize might convert names to upper case.</param>
    /// <param name="version">Version information</param>
    public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) :
        base(isValid, normalize, version) {
      AllCells = new Dictionary<string, Cell>();
      Dependencies = new DependencyGraph();
      Changed = false;
    }


    /// <summary>
    /// Cell Name Validator; Takes the given name and determines if it is a leagal
    ///     Variable/CELL name. Returns true if so, throws InvalidNameExecption if not.
    ///     VALID CELL NAMES: [1 or more alpha][1 or more digits]
    /// </summary>
    /// <param name="name">Name to validate</param>
    /// <returns>True if valid</returns>
    private bool validName(string name) {
      if (ReferenceEquals(name, null)) //validates that name isn't null
        throw new InvalidNameException();

      // checks name against valid regex and IsValid
      if (Regex.IsMatch(name, @"^[a-zA-Z]+[\d]+") && IsValid(name))
        return true;
      else
        throw new InvalidNameException();
    }

    /// <summary>
    /// If name is null or invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
    /// value should be either a string, a double, or a Formula.
    public override object GetCellContents(string name) {
      name = Normalize(name);                 // Normalize name before checking for Contents
      if (validName(name) && AllCells.ContainsKey(name)) // validates name, and presence
        return AllCells[name].CellContent; //return the cell contents
      else
        return "";                          // empty cells contain the empty string
    }

    /// <summary>
    /// If name is null or invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
    /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
    /// </summary>
    /// <param name="name">Cell Name</param>
    /// <returns>Value of Cell Contents</returns>
    public override object GetCellValue(string name) {
      name = Normalize(name);                 // Normalize name before checking for Value
      if (validName(name) && AllCells.ContainsKey(name)) // validates name, and presence
      {
        return AllCells[name].CellValue;    //return the cell value
      } else
        return "";                         // empty cells have a value of 0
    }

    /// <summary>
    /// Enumerates the names of all the non-empty cells in the spreadsheet.
    /// </summary>
    public override IEnumerable<string> GetNamesOfAllNonemptyCells() {
      HashSet<string> nameList = new HashSet<string>();
      foreach (string key in AllCells.Keys) // fill nameList with Keys
        nameList.Add(key);

      return nameList;
    }

    /// <summary>
    /// If content is null, throws an ArgumentNullException.
    /// 
    /// Otherwise, if name is null or invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, if content parses as a double, the contents of the named
    /// cell becomes that double.
    /// 
    /// Otherwise, if content begins with the character '=', an attempt is made
    /// to parse the remainder of content into a Formula f using the Formula
    /// constructor.  There are then three possibilities:
    /// 
    ///   (1) If the remainder of content cannot be parsed into a Formula, a 
    ///       SpreadsheetUtilities.FormulaFormatException is thrown.
    ///       
    ///   (2) Otherwise, if changing the contents of the named cell to be f
    ///       would cause a circular dependency, a CircularException is thrown.
    ///       
    ///   (3) Otherwise, the contents of the named cell becomes f.
    /// 
    /// Otherwise, the contents of the named cell becomes content.
    /// 
    /// If an exception is not thrown, the method returns a set consisting of
    /// name plus the names of all other cells whose value depends, directly
    /// or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// set {A1, B1, C1} is returned.
    /// </summary>
    /// <param name="name">Cell Name</param>
    /// <param name="content">Contents of cell</param>
    /// <returns>Set containing the cell and All Extended Dependents</returns>
    public override ISet<string> SetContentsOfCell(string name, string content) {
      name = Normalize(name);             // Normalize name before validating and adding

      if (ReferenceEquals(content, null)) // check content for null
        throw new ArgumentNullException();

      validName(name); // Checks name for validity
      HashSet<string> recalculateSet = new HashSet<string>();

      if (content != "") {
        double value;
        if (double.TryParse(content, out value))    // if double, call method(double)
          recalculateSet = (HashSet<string>)SetCellContents(name, value);
        else if (content[0] == '=')                 // if Formula, call method(Formula)
          recalculateSet = (HashSet<string>)SetCellContents(name,
              new Formula(content.TrimStart('='), Normalize, IsValid));
        else                                        // must be a string, call method(string)
          recalculateSet = (HashSet<string>)SetCellContents(name, content);

        Changed = true;
      } else if (AllCells.ContainsKey(name)) // Remove any current cell
        {
        Changed = true;
        recalculateSet = (HashSet<string>)SetCellContents(name, content);
        AllCells.Remove(name);
      }

      return recalculateSet;
    }

    /// <summary>
    /// If text is null, throws an ArgumentNullException. 
    /// 
    /// Otherwise, if name is null or invalid, throws an InvalidNameException. 
    /// (these are checked in SetContentsOfCell)
    /// 
    /// Otherwise, the contents of the named cell becomes text.  The method returns a
    /// set consisting of name plus the names of all other cells whose value depends, 
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// set {A1, B1, C1} is returned.
    /// </summary>
    protected override ISet<string> SetCellContents(string name, string text) {
      if (AllCells.ContainsKey(name)) // Key already exists
      {
        AllCells.Remove(name);      // remove before replacing
        Dependencies.ReplaceDependees(name, new HashSet<string>()); // remove dependees
      }
      AllCells.Add(name, new Cell(text));           // add Cell Contents; strings have no dependees

      return GetRecalculateSet(name);
    }

    /// <summary>
    /// If name is null or invalid, throws an InvalidNameException.
    /// (this is checked in SetContentsOfCell)
    /// 
    /// Otherwise, the contents of the named cell becomes number.  The method returns a
    /// set consisting of name plus the names of all other cells whose value depends, 
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// set {A1, B1, C1} is returned.
    /// </summary>
    protected override ISet<string> SetCellContents(string name, double number) {
      if (AllCells.ContainsKey(name)) // Key already exists
      {
        AllCells.Remove(name);      // remove before replacing
        Dependencies.ReplaceDependees(name, new HashSet<string>()); // remove dependees
      }
      AllCells.Add(name, new Cell((double)number)); // add Cell Contents; doubles have no dependees

      return GetRecalculateSet(name);

    }

    /// <summary>
    /// If the formula parameter is null, throws an ArgumentNullException.
    /// 
    /// Otherwise, if name is null or invalid, throws an InvalidNameException.
    /// (these are checked in SetContentsOfCell)
    /// 
    /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
    /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
    /// 
    /// Otherwise, the contents of the named cell becomes formula.  The method returns a
    /// Set consisting of name plus the names of all other cells whose value depends,
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// set {A1, B1, C1} is returned.
    /// </summary>
    protected override ISet<string> SetCellContents(string name, Formula formula) {
      List<string> backupDees = (List<string>)Dependencies.GetDependees(name);        //backup dependees
      Dependencies.ReplaceDependees(name, (HashSet<string>)formula.GetVariables());   //replace dependees

      try { GetRecalculateSet(name); } // try get Recalculate Set
      catch (CircularException e) {
        Dependencies.ReplaceDependees(name, backupDees); //Restore dependees

        if(AllCells.ContainsKey(name))
          AllCells.Remove(name);
        AllCells.Add(name, new Cell("="+formula.ToString()));           // add Cell Contents; strings have no dependees
        AllCells[name].SetCircularReference();

        return GetRecalculateSet(name);

        //throw e; // re-throw error
      }

      if (AllCells.ContainsKey(name))     // Key already exists
        AllCells.Remove(name);          // remove before replacing

      AllCells.Add(name, new Cell(formula, ssLookUp));        // Add the formula w/ placeholder "LookUp"

      return (HashSet<string>)GetRecalculateSet(name);

    }

    /// <summary>
    /// Uses GetCellToRecalculate to get a list of names of cells that are depeneded
    ///     on the given cell name. Then converts that list into a set to return.
    /// </summary>
    /// <param name="name">Name of cell</param>
    /// <returns>Set of all cell names that are dirrectly or indirectly dependedant</returns>
    private ISet<string> GetRecalculateSet(string name) {
      HashSet<string> recalcSet = new HashSet<string>();

      foreach (string key in GetCellsToRecalculate(name)) // add each cell from list to set
      {
        recalcSet.Add(key);             // Add to the Return Set
        if (recalcSet.Count > 1)        // Skip the first element (avoid inf loop)
          AllCells[key].ReEvaluate(ssLookUp); // ReEvaluate the cells in order
      }

      return recalcSet; // return the set
    }

    /// <summary>
    /// If name is null, throws an ArgumentNullException.
    /// 
    /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
    /// 
    /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
    /// values depend directly on the value of the named cell.  In other words, returns
    /// an enumeration, without duplicates, of the names of all cells that contain
    /// formulas containing name.
    /// 
    /// For example, suppose that
    /// A1 contains 3
    /// B1 contains the formula A1 * A1
    /// C1 contains the formula B1 + A1
    /// D1 contains the formula B1 - C1
    /// The direct dependents of A1 are B1 and C1
    /// </summary>
    protected override IEnumerable<string> GetDirectDependents(string name) {
      // checks name for null reference.  Should Be Unreachable Code
      System.Diagnostics.Debug.Assert(!ReferenceEquals(name, null));

      validName(name); // checks name for valid format

      return Dependencies.GetDependents(name); // return dependencies

    }

    /// <summary>
    /// Returns the version information of the spreadsheet saved in the named file.
    /// If there are any problems opening, reading, or closing the file, the method
    /// should throw a SpreadsheetReadWriteException with an explanatory message.
    /// </summary>
    /// <param name="filename">File location and File name</param>
    /// <returns>Version of saved File</returns>
    public override string GetSavedVersion(string filename) {
      if (!System.IO.File.Exists(filename))
        throw new SpreadsheetReadWriteException("File does not exist");
      else
        using (XmlReader file = XmlReader.Create(filename)) {
          // try to move to start element, throw if aything goes wrong
          try { file.IsStartElement(); }
          catch { throw new SpreadsheetReadWriteException("Start Element not found"); }

          // if starting element isn't "spreadsheet"
          if (file.Name != "spreadsheet")
            throw new SpreadsheetReadWriteException("Spreadsheet node not found.");

          // if starting element doesn't have any attributes
          else if (!file.MoveToAttribute("version"))
            throw new SpreadsheetReadWriteException("Version Attribute not found");
          else
            return file.GetAttribute("version");

        }
    }


    /// The XML elements should be structured as follows:
    /// 
    /// <spreadsheet version="version information goes here">
    /// 
    /// <cell>
    /// <name>
    /// cell name goes here
    /// </name>
    /// <contents>
    /// cell contents goes here
    /// </contents>    
    /// </cell>
    /// 
    /// </spreadsheet>
    /// 
    /// There should be one cell element for each non-empty cell in the spreadsheet.  
    /// If the cell contains a string, it should be written as the contents.  
    /// If the cell contains a double d, d.ToString() should be written as the contents.  
    /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
    /// 
    /// If there are any problems opening, writing, or closing the file, the method should throw a
    /// SpreadsheetReadWriteException with an explanatory message.
    public override void Save(string filename) {
      XmlWriterSettings settings = new XmlWriterSettings {
        Indent = true,
        IndentChars = "  "
      };
      try {
        using (XmlWriter file = XmlWriter.Create(filename, settings)) {
          // Begin Spreadsheet Block
          file.WriteStartElement("spreadsheet");
          file.WriteAttributeString("version", Version); // add Version details

          foreach (string cell in GetNamesOfAllNonemptyCells()) {
            // Cell Block
            file.WriteStartElement("cell");

            // Cell Name Block
            file.WriteStartElement("name");
            file.WriteString(cell);
            file.WriteEndElement();

            // Cell Content Block
            file.WriteStartElement("contents");
            object cont = GetCellContents(cell);
            if (cont is Formula)
              file.WriteString("=" + cont.ToString());
            else
              file.WriteString(cont.ToString());
            file.WriteEndElement();

            file.WriteEndElement();
          }
          file.WriteEndElement();
        }
      }
      catch {
        throw new SpreadsheetReadWriteException("Path not found.");
      }
      Changed = false;
    }

    /// <summary>
    /// Spreadsheet LookUp Method.  Looks in the Dictionary for the variable and returns the value. 
    ///     If the value is not a double, throws an argument exception
    /// </summary>
    /// <param name="name">name of Cell to get value</param>
    /// <returns>returns the value as a double</returns>
    private double ssLookUp(string name) {
      if (!validName(name))
        throw new ArgumentException("Invalid Cell Name");
      object cellVal = GetCellValue(name);
      double dummy;
      if (!(double.TryParse(cellVal.ToString(), out dummy)))
        throw new ArgumentException("Empty Content");
      return (double)GetCellValue(name);
    }


    /// <summary>
    /// Cell class, contains a Cell's Contents and Value
    ///     The name will be used for the dictionary key
    /// </summary>
    private class Cell {
      public object CellContent { get; protected set; }  // Content of Cell
      public object CellValue { get; protected set; }    // Value of Cell

      /// <summary>
      /// Creates a cell and sets CellName and CellContent
      /// </summary>
      /// <param name="content">CellContent</param>
      public Cell(string content) {
        CellContent = content;
        CellValue = content;
      }

      /// <summary>
      /// Creates a cell and sets CellName and CellContent
      ///     Double Varient: Cell Value will equal Content
      /// </summary>
      /// <param name="content">CellContent</param>
      public Cell(double content) {
        CellContent = content;
        CellValue = content;
      }

      /// <summary>
      /// Creates a cell and sets CellName and CellContent
      ///     Formula Varient: Cell Value will need to be calculated
      /// </summary>
      /// <param name="content">CellContent</param>
      /// <param name="LookUp">Variable Lookup Delegate</param>
      public Cell(Formula content, Func<string, double> LookUp) {
        CellContent = content;
        CellValue = content.Evaluate(LookUp);
      }

      /// <summary>
      /// Re-Evaluates the Cells.Value with the given Lookup Function
      /// </summary>
      /// <param name="LookUp">LookUp Method for evaluating variables</param>
      /// <returns></returns>
      public bool ReEvaluate(Func<string, double> LookUp) {
        if (!(CellContent is Formula))
          return false;
        Formula temp = (Formula)CellContent;
        CellValue = temp.Evaluate(LookUp);
        return true;
      }

      public void SetCircularReference() {
        this.CellValue = "Circular Reference Error";
      }

    }
  }
}
