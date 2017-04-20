#include <iostream>
#include <string>
#include <fstream>
#include <map>


using namespace std;
//using namespace ifstream;

int main()
{
  
  map<string,string> mymap;//::iterator in;// = new map<string,string
  map<string,string>::iterator it;// = new map<string,string

  mymap["cellName"]="65";
  mymap["A2"] = "666";
  mymap["Z3"] = "67 + 8585 * a (b- c) / 8";

  // open a file file to write to
  ofstream save("test.txt");


  // iterate through the map and write them to the .txt file
  for(it = mymap.begin(); it != mymap.end(); it++)
    {
      save << it->first << "\t" << it->second << "\n";


    }
  
}
