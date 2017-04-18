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
#include <unordered_map>

using boost::asio::ip::tcp;
using namespace std;

static unordered_map<int, tcp::socket> mymap;
static int clientID = 0;


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
	    cout << "read: " << data_ << endl;

	    
	    vector<string> words;
	    string word;
	    //string checked;
	    for(int i = 0; i < max_length; i++)
	      {
		int j = data_[i];

		//	checked += data_[i];
		//checked += data_[i+1];

		if(j == 9)//(checked == "  ")
		  {
		    words.push_back(word);
		    word = "";
		    i++;

		    //  checked = "";
		  }
		else if(j == 10)
		  {
		    words.push_back(word);
		    break;
		  }
		else
		  word+=data_[i];
		
	      }

	    if(!words[0].compare("Connect"))
	      {
		bzero(data_, 301);
		
		strcpy(data_, "I want to take you to a gay bar \n");
		do_write(39);
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
	    //  mymap.insert( std::pair<int, tcp::socket>(clientID, socket_));
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
int save(string cellName, string cellContents)
{

}

int updateClients()
{

}
