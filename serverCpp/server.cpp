/********************************************************************
Chris Allan
Adam Bennion
Aaron Benson
Shafigh Bohamin

U of U
CS3500
Spring 2017

Spread Sheet Server

The pupose of this file is act as a network controller using TCP as
well as a server to send and recieves messages across the controller 
and store the spread sheet data.

********************************************************************/

#include <mutex>
#include <cstdlib>
#include <iostream>
#include <memory>
#include <utility>
#include <boost/asio.hpp>
#include <vector>
#include <string.h>
#include <fstream>
#include <map>
#include <stack>
#include <boost/lexical_cast.hpp>

using boost::asio::ip::tcp;
using namespace std;

static int clientID = 0; // next available user ID

// contains all info related to a single user connection (socket)
typedef struct{
  int ID; // client ID (unique resuired)
  tcp::socket* socket; // socket connection for user
  char* spreadsheet; // spreadsheet user is editing (required)
  char* inCell; // cell user is currently editing (null == none)
}Warden;

// contains all info to impliment an Undo
typedef struct{
  char* cellName;
  char* oldContent;
}undo_pak;

// contains all data related to a single spreadsheet
typedef struct{
  map<int, Warden*> *Users; // dictionary of users connected to this spreadsheet
  map<string, string> *CellContents; // key = CellName; value = CellContent
  stack<undo_pak> *Undos; // stack of undo-pak's
}Sheet_pak;

// contains all currently open spreadsheets.
map<string, Sheet_pak*> Spreadsheets;

// contains a means for a socket to find it's warden
map<string, Warden*> wardenLookup;

void AddUser(Warden* user);
int save(string spreadsheetName,  map<string, string>* CellContents);

//lock for the race conditions
mutex mtx;

class session
  : public std::enable_shared_from_this<session>
{
public:
  session(tcp::socket socket)
    : socket_(std::move(socket))
  {
  }

  void start()
  {
    string port = boost::lexical_cast<string>(socket_.remote_endpoint());
    cout << port << " " << &socket_ << endl;
    
    // create a new warden; remember to clean this up
    Warden* newClient = new Warden;
    newClient->ID = clientID++; // need to lock this
    newClient->socket = &socket_;
    newClient->spreadsheet = NULL;
    newClient->inCell = NULL;

    wardenLookup[port] = newClient; 
    mtx.lock();
    do_read();
    mtx.unlock();
  }

private: 
  void do_read()
  {
    auto self(shared_from_this());
    socket_.async_read_some(boost::asio::buffer(data_, max_length),
	[this, self](boost::system::error_code ec, std::size_t length)
	{
	  if (!ec)
	    {
	      cout << "read: " << data_ << endl; // debug message (recieved input)
	      // parse the string
	      vector<string> words;
	      string word;

	      //lock for data_
	      for(int i = 0; i < max_length; i++)
		{
		  int j = data_[i];
		  if(j == 9) // tab detected
		    {
		      words.push_back(word);
		      word = "";
		    }
		  else if(j == 10) // newline detected
		    {
		      words.push_back(word);
		      break;
		    }
		  else
		    word+=data_[i];
		
		}
	      // port is a unique string assigned to each client
	      string port = boost::lexical_cast<string>(socket_.remote_endpoint());



	      // user is the warden that holds the current client connection
	      Warden* user = wardenLookup[port];

	      /*********************************************
	       * Connect <spreadsheetName> message recieved
	       *********************************************/
	      if(!words[0].compare("Connect"))
		{
		  // create a malloc'd string of spreadsheet name
		  char* sheetName = new char[words[1].length()+1];
		  strcpy(sheetName, words[1].c_str());
		      
		  // set user's spreadsheet field to new string		
		  user->spreadsheet = sheetName;
		      
		  // add user to the spreadsheet's users list
		  // calls open if required 
		  AddUser(user);
		      
		  // Test out the dictionary for proper contents
		  cout << "Stored Sheet Name: ";
		  cout << (*(Spreadsheets[sheetName]->Users))[user->ID]->spreadsheet << endl;
		      
		  // build string to send to the client
		  string toSend = "Startup\t";
		  toSend.append(to_string(user->ID));
		  toSend.append("\t");

		  // get a pointer to the spreadsheet data
		  map<string, string> temp;
		  temp = *(Spreadsheets[sheetName]->CellContents);

		  /* itterate thought the spreadsheet the client wants to use and append
		      it to the string the sever sends them */
		  for(auto it = temp.cbegin(); it != temp.cend(); ++it)
		    {
		      toSend.append(it->first);
		      toSend.append("\t");
		      toSend.append(it->second);
		      toSend.append("\t");
		    }
		      
		  // add a next line on 
		  toSend.append("\n");
		  
 /** WARNING! This is capable of overflow; need to fix **/
		  strcpy(data_, toSend.c_str());
		  do_write(user, data_, 0, toSend.size() + 1);
		}
	      /**************************************************
	       * Edit <cellName> <cellContents> message recieved
	       **************************************************/
	      else if(!words[0].compare("Edit"))
		{
		  // get the spreadsheet linked to the user
		  char* sheetName = user->spreadsheet;
		  
		  // create a backup for current content in cell
		  undo_pak* newUndo = new undo_pak;
		  char* cellN = new char[words[1].length()+1];
		  strcpy(cellN, words[1].c_str());
		  newUndo->cellName = cellN; // add cell name to the Undo Pak
		  
		  std::map<string,string>::iterator it; // spreadsheet contents iterator
		  
		  // point the iterator at the proper cell (if exists)
		  it = (*(Spreadsheets[sheetName]->CellContents)).find(cellN);
		  if (it == (*(Spreadsheets[sheetName]->CellContents)).end())
		    { // key not found in sheet -> set undo.oldContent to an empty string
		      newUndo->oldContent = new char[1];
		      *(newUndo->oldContent) = 0;
		    }
		  else
		    { // key found -> set undo.oldContent to current content of cell
		      char* backUp = new char[(*(Spreadsheets[sheetName]->CellContents))[cellN].length()+1];
		      strcpy(backUp, (*(Spreadsheets[sheetName]->CellContents))[cellN].c_str());
		      newUndo->oldContent = backUp;
		    }
		      
		  // push undo pak into undo stack 
		  (*(Spreadsheets[sheetName]->Undos)).push(*newUndo);

		  // create a new string for the new contents to put in cell
		  char* newContent = new char[words[2].length()+1];
		  strcpy(newContent, words[2].c_str());
		  (*(Spreadsheets[sheetName]->CellContents))[cellN] = newContent;

		  // save the updated spreadsheet
		  int saveAttempt;
		  saveAttempt = save(sheetName, Spreadsheets[sheetName]->CellContents);

		  // cerate string to send to client(s)
		  string message = string("Change\t") + words[1] + "\t" + words[2] + "\t\n";
		  strcpy(data_, message.c_str());

		  // sent the message to the user(s)
		  do_write(user, data_, 1, message.size() + 1);
		  
		}
	      /************************
	       * Undo message received
	       ************************/
	      else if(!words[0].compare("Undo"))
		{
		  Sheet_pak* sp = Spreadsheets[user->spreadsheet];
	      
		  if((*(sp->Undos)).size()>0)
		    {
		      undo_pak* up = &((*(sp->Undos)).top());
		  
		      (*(sp->CellContents))[up->cellName] = up->oldContent;

		      string message = string("Change\t") + (*(sp->Undos)).top().cellName + "\t" +
			(*(sp->Undos)).top().oldContent + "\t\n";
		      (*(sp->Undos)).pop();

		      strcpy(data_, message.c_str());
		      do_write(user, data_, 1, message.size() + 1);
		    }
		}
	      /**************************************************
	       * IsTyping <clientID> <cellName> message received
	       **************************************************/
	      else if(!words[0].compare("IsTyping"))
		{
		  // build response string
		  string message = string("IsTyping\t") + words[1] + "\t" +
		    words[2] + "\t\n";
		  strcpy(data_, message.c_str());

		  cout << "IsTyping message Received" << endl;
		  
		  // send response string
		  do_write(user, data_, 1, message.size() + 1);

		}
	      /************************************************
	       * DoneTyping <clientID> <cellName> Message received
	       ************************************************/
	      else if(!words[0].compare("DoneTyping"))
		{
		  // build response string
		  string message = string("DoneTyping\t") + words[1] + "\t" +
		    words[2] + "\t\n";
		  strcpy(data_, message.c_str());
		  
		  cout << "IsTyping message Received" << endl;

		  // send response string
		  do_write(user, data_, 1, message.size() + 1);

		}
	   
	    }
	});
  }

  void do_write(Warden *user, char* msg, bool multi, std::size_t length)
  {

    auto self(shared_from_this());

    // copy the message into the buffer to send to the specified socket
    strcpy(data_, msg);



    if(multi)
      {
	
	for(auto it = (*(Spreadsheets[user->spreadsheet]->Users)).cbegin(); it != (*(Spreadsheets[user->spreadsheet]->Users)).cend(); ++it)
	  {
	    // cout<< "id : " << it->first << "\t\t " << "value: " << it->second << endl;

    

	    boost::asio::async_write(*((it->second)->socket), boost::asio::buffer(data_, length),
		  [this, self](boost::system::error_code ec, std::size_t /*length*/)
		  {
		    if (!ec)
		      {
			cout << "write: " << data_ << endl;
			do_read();
		      }
		  });
	  }

      }
    else
      {

	boost::asio::async_write(*(user->socket), boost::asio::buffer(data_, length),
	 [this, self](boost::system::error_code ec, std::size_t /*length*/)
	 {
	   if (!ec)
	     {
	       cout << "write: " << data_ << endl;
	       do_read();
	     }
	 });
      }

     

  }



  tcp::socket socket_;
  enum { max_length = 1024 };
  char data_[max_length];
};

class server
{
public:
  server(boost::asio::io_service& io_service, short port)
    : acceptor_(io_service, tcp::endpoint(tcp::v4(), port)),
      socket_(io_service)
  {
    do_accept();
  }
private:
  void do_accept()
  {
    acceptor_.async_accept(socket_,
      [this](boost::system::error_code ec)
      {
	if (!ec)
	  {
	    string port = boost::lexical_cast<string>(socket_.remote_endpoint());
	    cout << port << " " << &socket_ << endl;
	    
	    std::make_shared<session>(std::move(socket_))->start();
	  }

	do_accept();
      });
  }

  tcp::acceptor acceptor_;
  tcp::socket socket_;
};

int main(int argc, char* argv[])
{
  try
    {
      if (argc != 2)
	{
	  std::cerr << "Usage: async_tcp_echo_server <port>\n";
	  return 1;
	}

      boost::asio::io_service io_service;

      server s(io_service, std::atoi(argv[1]));

      io_service.run();
    }
  catch (std::exception& e)
    {
      std::cerr << "Exception: " << e.what() << "\n";
    }

  return 0;
}

// saves changes to the spreadsheet returns success or failure.
int save(string spreadsheetName,  map<string, string>* CellContents)
{
  map<string,string>::iterator it;// = new map<string,string

  // open a file to write to
  ofstream save(spreadsheetName);


  // iterate through the map and write them to the .txt file
  for(it = CellContents->begin(); it != CellContents->end(); it++)
    {
      save << it->first << "\t" << it->second << "\t\n";
    }
}

int updateClients()
{

}

void AddUser(Warden* user){
  string sheetName = user->spreadsheet;

  // check if spreadsheet already exists in dictionary
  map< string, Sheet_pak*>::iterator sheetIT;
  sheetIT = Spreadsheets.find(sheetName);

  if (sheetIT != Spreadsheets.end()){
    // if so -> add user to the users list
    (*(Spreadsheets[sheetName]->Users))[user->ID] = user;
  } else{
    // if not -> create a new (empty) sheet pak and contents
    Sheet_pak* newSheet = new Sheet_pak;
    map<int, Warden*> *newUsers = new map<int, Warden*>; // needs to be in heap
    map<string, string> *newCells = new map<string, string>; // needs to be in the heap
    stack<undo_pak> *newUndos = new stack<undo_pak>; // needs to be in the heap

    // set contents to new allocated structures
    newSheet->Users = newUsers;
    newSheet->CellContents = newCells;
    newSheet->Undos = newUndos;    

    // add the user to the Users dictionary
    (*newUsers)[user->ID] = user;

    // add the Sheet Pak to the Spreadsheet Dictionary
    Spreadsheets[sheetName] = newSheet;

    if(ifstream(sheetName))
      {
	string line;
	ifstream inputFile;
	string buff;
	string s[2];
	int index = 0;
	inputFile.open(sheetName);

	while(getline(inputFile, line))
	  {

	    istringstream iss(line);
	    string token;
	    cout << line << endl;

	    while(getline(iss,token, '\t'))
	      {
		s[index++] = token;
	      }
      
	    (*newCells)[s[0]] = s[1];
	    //cout << "s0 " << s[0] << " s1 " << s[1] << endl;
	   
	    index = 0;
	  }


	for(auto it = (*newCells).cbegin(); it != (*newCells).cend(); ++it)
	  {
	    cout<< "key: " << it->first << "\t\t " << "value: " << it->second << endl;

	  }
      }
  }
}
