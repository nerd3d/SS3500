=====================================================================
Author(s):	Christopher Allan & Lee Neuschwander
Class:		CS 3500
Semester:	Fall 2016
Program:	EAE:CS
School:		University of Utah
UpDate:		10/29/2016

=====================================================================

Library Versions:
	 - DependencyGraph v.20160923
	 - Formula v.20160923
	 - AbstractSpreadsheet v.1.7
	 - Spreadsheet v.20161024
	 - SpreadsheetGUI v.PS6
	
Thoughts about the project:
	 - new Spreadsheet() instantiates a Dictionary of type object; represents all non-empty Cells
	 - new Spreadsheet() instantiates a dependancyGraph; tracks all formulae dependencies
	 - Setting a Cells contents will create a new dictionary item, replace the existing cell, 
			and check for any dependents to re-evaluate
		- Removing a Cell from the Dictionary will remove all dependees for that Cell
		- Setting a cell that contains a formula will add dependees and check for depenency loops
	 - GetCellContents simply returns the dictionary value
	[PS5 update 9/28] 
	 - Setting Cell Contents is now combined into a single method.  It takes in strings for the 
	   name and contents of the cell.  It then determines if the string is a double, a formula or 
	   indeed a string, and calls the appropriate method.
	 - The Constructor is replaced with a 3 parameter Constructor, requiring a normalizer, a 
	   validator and a version. The parameterless Constructor now calls the main Constructor with
	   defaults for those parameters. A third Constructor takes 4 parameters which includes a
	   FilePath to a spreadsheet to load.
	 - Notes about Valid Names: 
			Valid Cell Name: 1 or more alpha, followed by 1 or more integers
			Valid Variable: Delegate Defined; default = 1 alpha or _ followed by 0 or more alpha, 
			  _ or integer (any order)
			Normalizer: Normalizer needs to be applied to CellNames and Variables alike.
			Lookup: Formula evaluation will make use of a sample Lookup. Supplying values for Cell 
			  references and Variables (in unit tester)
	- It was not clear if we needed to provide a functional LookUp function that actually returns Cell
	  Values or not, so I'm including it anyway, to be on the safe side.  
	[PS6 update 10/29]
	- Partnership began with Lee. We've split up the major tasks into 2 categories. We've done so in a 
	  way that give each of us an oportunity to get experience with various tools of the Windows Forms.
	- Tasks for Chris:	Save, Save As, Close, Exit, SetContents (Content Box), addressToGrid
	  - Save:	I plan on setting the Form Version into it's own private variable that will encode into
				the .sprd file upon creation. If attempting to save a document that doesn't already exist,
				saveAs will be called.
	  - SaveAs:	This will open a built in File_Dialog to chose a file/location to save the file. It will 
				default to the extension .sprd, but will allow <anything?>
	  - Close:	This is basically functional from the DemoCode. I will add a "Changed" check to warn of
				data loss; which will give save/ignore options.
	  - Exit:	Call the close method for all open documents.
	  - SetContents:	Text in the contents box, and the address to that cell will be sent to the 
				Spreadsheet to create a new cell object. This will return a list of all cells that need
				to be updated. Loop through that list and update their values in the GUI with getCellValue
				from the Spreadsheet.
	  - addressToGrid:	this is a helper function that will connvert a cell address (label) to a grid
				coordinate for the GUI.

	- Tasks for Lee:	New, Open, Help, SelectChange (Update Banner), gridToAddress
	  - New:	When selected this will open up a new spreadsheet application and create a new blank personal 
				spreadsheetpanel and spreadsheet for the model to use. 
	  -Open:	Open will bring up a new windows application and fill it with information according to the
				information provided when the user selects what file to open from the file selection menue. 
	  -Help:	I plan on making a window apear with information in regards to how the spreadsheet is 
				implamented and how the user should go about operating the spreadsheet. 
	  -SelectionChange(Update Banner):		When the selection of cells changes the content text box
				will be the main focus and the text inside, if any, will then be highlighted and ready to 
				change. If the cell selected contains information the content box will be populated with 
				the provided information to be highlighted within. The address box and value box will also
				be updated with the infomation of the selected cell. 
	  -gridToAdress:	This will take the information from the grid, int col and int row, and change it 
				into a format (adress) that is compatible with the Spreadsheet class naming convention. 
				i.e 1 in row meaning 1 and 1 in collum meaning A into "A1".

	[PS6 update 10/31]
		Finished core functionality for the spreadsheet application. 
		Complete additional features:
			-Save As
			-Exit (closes all open spreadsheets)
			-Highlight selected and focus upon selection change for content banner.
			-Enter to accept content change.
			-Shortcut for menu items. 
			-Arrow Keys for spreadsheet navigation.
			-Form header contains filepath and displays unsaved status
			

Additional Notes:
	I chose NOT to create a Cell class because the dictionary effectively serves the same purpose
	 (as long as I maintain the invarient of NO DUPLICATE KEYS, which I'd have to do anyway). In
	 the even that I absolutely need a Cell class in the future, I can simple add a private toCell
	 method to package the key/value pair into a Cell.
	[PS5 update 9/28] 
	 I immediately regret not making the Cell class. If I don't, I'd need to recalculate
	 the value of each cell every time I want to display the value on the screen. That is not an okay
	 methodology.  I'll make the Cell Class now.
	[PS6 update 10/29] 
	 Coding for View Control and Model has begun. It was decided that work for the project should start
	 with the file and help pull-down menue. Code for New, Open, Close, Exit, Save, Save As, and Help 
	 have been started. In addition the process of how and when to go about commiting changes and merging have
	 been officially decided upon. 
	 [PS6 update 10/30]
	  Possible "Additional Features": 
		Open additional Spreadsheets in tabs
		Pressing Enter will apply contents
		Undo Stack
		Formula Contents Definition locks CellChange/clicks and arrowkeys copy address' into contents
	[PS6 update 11/3]
	 GUI testing issues:
		Recording mouse clicks is next to impossible. The playback seems to click where ever it wants, or
		possibly has some unknown reference frame. To avoid this issue we operated the SSForm using only
		key inputs. Which works for the most part, but there are some keystroke recordings that automatically
		include mouse-clicks (I think it is selecting the input box location.. at least it tries). So to fix
		that issue, I've replaced the default entry type from 'editableItem' to 'sendKeys' in various places.
		This seems to allow the tests to complete properly. However, everytime we add a new recorded UI test,
		the changes made to other tests are reverted and stop working. So we are going to stop adding tests. 
		Code Coverage is currently at 91% for the GUI, and adding more doesn't seem worth the effort.