#include <iostream>
#include <string>
#include <fstream>
#include <sstream>
#include <map>
#include <vector>

using namespace std;

int main()
{
  
  map<string,string> mymap;


  string line;
  ifstream inputFile;
  string buff;
  string s[2];
  int index = 0;
  inputFile.open("test.txt");

  while(getline(inputFile, line))
    {

      istringstream iss(line);
      string token;

      while(getline(iss,token, '\t'))
	{
	  cout << token << endl;
	  s[index++] = token;
	}
      
      mymap[s[0]] = s[1];
      //cout << "s0 " << s[0] << " s1 " << s[1] << endl;

      for(auto it = mymap.cbegin(); it != mymap.cend(); ++it)
	{
	  cout<< "key: " << it->first << "\t\t " << "value: " << it->second << endl;

	}

      index = 0;
    }
  


  return 0;
  
}


