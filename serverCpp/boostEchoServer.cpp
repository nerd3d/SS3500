//
// async_tcp_echo_server.cpp
// ~~~~~~~~~~~~~~~~~~~~~~~~~
//
// Copyright (c) 2003-2016 Christopher M. Kohlhoff (chris at kohlhoff dot com)
//
// Distributed under the Boost Software License, Version 1.0. (See accompanying
// file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
//

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
  map<int, Warden*> Users; // dictionary of users connected to this spreadsheet
  map<string, string> CellContents; // key = CellName; value = CellContent
  stack<undo_pak> Undos; // stack of undo-pak's
}Sheet_pak;

// contains all currently open spreadsheets.
map<string, Sheet_pak*> Spreadsheets;

// contains a means for a socket to find it's warden
map<string, Warden*> wardenLookup;

void AddUser(Warden* user);
int save(string spreadsheetName,  map<string, string>* CellContents);

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
    // Warden* newClient = (Warden*)malloc(sizeof(Warden));
    Warden* newClient = new Warden;
    newClient->ID = clientID++; // need to lock this
    newClient->socket = &socket_;
    newClient->spreadsheet = NULL;
    newClient->inCell = NULL;

    wardenLookup[port] = newClient; 


    do_read();
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
	    for(int i = 0; i < max_length; i++)
	      {
		int j = data_[i];
		if(j == 9)
		  {
		    words.push_back(word);
		    word = "";
		  }
		else if(j == 10)
		  {
		    words.push_back(word);
		    break;
		  }
		else
		  word+=data_[i];
		
	      }
	    cout<<"finished parse"<<endl;
	    cout << "words: " << words[0]<<endl;//<<words[1] <<words[2]<<endl;
	    string port = boost::lexical_cast<string>(socket_.remote_endpoint());
	    Warden* user = wardenLookup[port];
	    if(!words[0].compare("Connect"))
	      {
		

		// create a malloc'd string of spreadsheet name
		char* sheetName = new char[words[1].length()+1];
		strcpy(sheetName, words[1].c_str());
		
		// set user's spreadsheet field to new string		
		user->spreadsheet = sheetName;

		// add user to the spreadsheet's users list
		//  calls open if required 
		AddUser(user);

		// Test out the dictionary for proper contents
		cout << "Stored Sheet Name: ";
		cout << (Spreadsheets[sheetName]->Users)[user->ID]->spreadsheet << endl;
		
		// Build the Startup data for spreadsheet
		// 
		// send the data to the user
		//SendMessage(user, "data goes here");
		strcpy(data_, "I want to take you to a gay bar \n");
		do_write(39);
	      }
	      else if(!words[0].compare("Edit"))
	      {
		cout << "Got to Edit.." << endl;
		char* sheetName = user->spreadsheet;
		// Edit\t cellName\t cellContents\t\n
		// update the spreadsheet
		//prep undo
		undo_pak* newUndo = new undo_pak;
		char* word1 = new char[words[1].length()+1];
		strcpy(word1, words[1].c_str());
		newUndo->cellName=word1;


		std::map<string,string>::iterator it;

		it = (Spreadsheets[sheetName]->CellContents).find(word1);
		if (it == (Spreadsheets[sheetName]->CellContents).end())
		  {
		    newUndo->oldContent = new char[1];
		    *(newUndo->oldContent) = 0;
		  }
		else
		  {
		    char* temp55 = new char[(Spreadsheets[sheetName]->CellContents)[word1].length()+1];
		    strcpy(temp55, (Spreadsheets[sheetName]->CellContents)[word1].c_str());
		    cout <<"temp55:" << temp55 <<endl;
		    newUndo->oldContent = temp55;//(Spreadsheets[sheetName]->CellContents)[word1];
		  }
		
		// store undo (previous data in that cell)
	        (Spreadsheets[sheetName]->Undos).push(*newUndo );
		char* word2 = new char[words[2].length()+1];
		strcpy(word2, words[2].c_str());
		Spreadsheets[sheetName]->CellContents[words[1]] = word2;
		// save the updated spreadsheet
		int success;
		success = save(sheetName, &Spreadsheets[sheetName]->CellContents);

		//if(success)
		//{		  
		string message = string("Change\t") + words[1] + "\t" + words[2] + "\t\n";
		for(int i = 0; i<message.length();++i)
		  {
		    data_[i] = message[i];
		  }

		undo_pak* up = &(Spreadsheets[sheetName]->Undos.top());
		  //new char* = new char[strlen];
		  
		  cout << "Checking undos.top()" << endl;
		  cout << up->cellName <<endl;
		  cout << up->oldContent <<endl;


		do_write(message.length());
		cout<<"SentBack: "<< message <<endl;
		// update clients
		//}
	      }
	      else if(!words[0].compare("Undo"))
	    {
		cout << "Got to Undo.." << endl;
		//string port = boost::lexical_cast<string>(socket_.remote_endpoint());

	      //Warden* user = wardenLookup[port];
	      Sheet_pak* sp = Spreadsheets[user->spreadsheet];
	      
	      if(sp->Undos.size()>0)
		{

		  undo_pak* up = &(sp->Undos.top());
		  //new char* = new char[strlen];
		  
		  cout << up->cellName <<endl;
		  cout << up->oldContent <<endl;
		  (sp->CellContents)[up->cellName] = up->oldContent;
		  string message = string("Change\t") + sp->Undos.top().cellName + "\t" +
		    sp->Undos.top().oldContent + "\t\n";
		  sp->Undos.pop();
		  for(int i = 0; i<message.length();++i){
		    data_[i] = message[i];
		  }
		  do_write(message.length());
		}
	    }
	      else if(!words[0].compare("IsTyping"))
		{
		  string port = boost::lexical_cast<string>(socket_.remote_endpoint());
		  Warden* user = wardenLookup[port];
		  string message = string("IsTyping\t") + words[1] + "\t" +
		    words[2] + "\t\n";
		  for(int i = 0; i<message.length();++i){
		    data_[i] = message[i];
		  }
		  do_write(message.length());
		  // IsTyping\t clientID\t cellName\t\n
		    // propogate exact message to all clients
		  }

	    //else if(words[0].compare("DoneTyping"))
		else if(!words[0].compare("DoneTyping"))
		  {
		    string port = boost::lexical_cast<string>(socket_.remote_endpoint());
		    Warden* user = wardenLookup[port];
		    string message = string("DoneTyping\t") + words[1] + "\t" +
		      words[2] + "\t\n";
		    for(int i = 0; i<message.length();++i){
		      data_[i] = message[i];
		    }
		    do_write(message.length());


		    // DoneTyping\t clientID\t cellName\t\n
		    // propogate exact message to all clients
		  }
	    else
	      do_write(length);
	    /*
	    
	    //	bzero(data_, 301);
		
		strcpy(data_, "I want to take you to a gay bar/5/6/70x34A \n");
		do_write(39);
	    
	    */
	    
	    /*
	    int success = 0;
	    switch(words[0])
	      {
	      case"Edit":
		// Edit\t cellName\t cellContents\t\n
		// update the spreadsheet
		// save the updated spreadsheet
		//	int success;
		//	success = save(string[1],string[2]);

		//	if(success)
		  {
		    // store edit in stack incase someone hits undo(maybe store previous data?)
		    // update clients
		  }

		break;
	      case "Undo":
		// Undo\t\n
		// get the top of the stack

		//	success = save(cellname, oldvalue);
		//	if(success)
		  {
		    // pop the stack
		    // update clients
		  }
		break;
	      case "Connect":
		// Connect\t spreadsheetName\t\n
		// send this client the spread sheet
		// if not  created then create it
		// add the client to that spreadsheet to that map

		bzero(data_, 301);
		
		strcpy(data_, "=> I want to take you to a gay bar\n");
		do_write(length);
		break;
	      case "IsTyping":
		// IsTyping\t clientID\t cellName\t\n
		// propogate exact message to all clients 
		break;
	      case "DoneTyping":
		// DoneTyping\t clientID\t cellName\t\n
		// propogate exact message to all clients
		break;


	      }

	    */
	    // do_write(length);
          }
        });
  }

  void do_write(std::size_t length)
  {
    auto self(shared_from_this());
    boost::asio::async_write(socket_, boost::asio::buffer(data_, length),
        [this, self](boost::system::error_code ec, std::size_t /*length*/)
        {
          if (!ec)
          {
	    cout << "write: " << data_ << endl;
            do_read();
          }
        });
  }

  tcp::socket socket_;
  // enum {Edit, Undo, Connect, IsTyping, DoneTyping}
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
    //    (Spreadsheets[sheetName]->Users)[user->ID] = user;
  } else{
    // if not -> create a new (empty) sheet pak and contents
    Sheet_pak* newSheet = new Sheet_pak;
    map<int, Warden*> *newUsers = new map<int, Warden*>; // needs to be in heap
    map<string, string> *newCells = new map<string, string>; // needs to be in the heap
    stack<undo_pak> *newUndos = new stack<undo_pak>; // needs to be in the heap

    // set contents to new allocated structures
    newSheet->Users = *newUsers;
    newSheet->CellContents = *newCells;
    newSheet->Undos = *newUndos;    

    // add the user to the Users dictionary
    newSheet->Users[user->ID] = user;

    // add the Sheet Pak to the Spreadsheet Dictionary
    Spreadsheets[sheetName] = newSheet;
  }
}
