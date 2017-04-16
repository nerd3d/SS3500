#include <stdio>
#include <string>
#include <iostream>
#include <sstream>
#include <algorithm>
#include <iterator>
#include <iteratorvector>
#include "networking.h"

using namespace std::

private void RecieveSpreatSheet(SocketState ss){
  string ssName = to_string(ss->sb);
  vector<string> messSplit;
  string buffer;
  stringstream splitter(ssName);
  while(splitter >> buffer){
    messSplit.push_back(buffer);
}
  int a ;
  for(a = 0; a<messSplit.size()-1;a++){
    if(spreadSheets.find(messSplit[1]) != spreadSheets.end()){
      string answer = "Accepted: /n";
      Networking->send(answer);
    }
    else{
      map<string, string> cellAndValue;
      spreadSheets.insert(messSplit[1], cellAndValue);
      string answer = "Accepted: /n";
      Networking->send(answer);
    }
  } 
}

private void HandleNewConnect(SocketState ss){
  ss->theCaller = RecieveSpreadSheet;
  Networking->getData(ss);
}

class sheetServer{
public:
  map <string, map<string, string> > spreadSheets;
Networking->ServerAwaitingClientLoop(HandleNewConnect);
}

class server {
public:
  static void Main(){
    sheetServer newServer = new sheetServer();
    string str;
    getline(cin, str);
}

}
