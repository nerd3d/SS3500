using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace ChatClient
{
    /*How Client utilizes this controller:
    *	1. Initialize a NetworkWarden in client program. (NetworkWarden networkSocket)
    *	2. use command: Networking.ConnectToServer(/*method for handling successful tcp connection goes here*/

    /*
    *	3. inside local successful connection command (which needs a NetworkWarden parameter):
    *		update parameter NetworkWarden with networkWardenVariableName.callMe = /*local method expecting first message from server*/
    /*		use command: Networking.Send(networkwarden.socket, /* hello message + \n */
    /*	4. inside the callMe function assigned in step 3, we again need the NetworkWarden parameter.
    *		set your local NetworkWarden to the incoming parameter.
    *		make a buffer for collecting message from network, and parse it. message will be inside incoming NetworkWarden parameter.message
    */
    class Program
    {
        static NetworkWarden warden;
        static string usr;
        static void Main(string[] args)
        {
            string hostname = "lab2-24.eng.utah.edu";
            //string hostname = "lab1-7.eng.utah.edu";
            Networking.ConnectToServer(sendName, hostname);
            Console.WriteLine("Read to chat... give a usrname.");
            string usr = Console.ReadLine();
            while(true)
            {
                sendMessage();
                cleanBuffer(warden);
                Networking.getData(warden);
            }
        }
        static void sendName(NetworkWarden ward)
        {
            ward.callNext = getNameBack;
            Networking.Send(ward.socket, usr + ": " + "Praise the Sun" + "\n");
        }
        static void getNameBack(NetworkWarden ward)
        {
            warden = ward;
            Console.WriteLine("server sent spreadsheet: " + warden.message);
            ward.callNext = getNameBack;
            Networking.Send(ward.socket, usr + ": " + "trying to send chats" + "\n");
            //ward.callNext = listen;
            //warden.socket.BeginReceive(warden.buffer, 0, warden.buffer.Length, SocketFlags.None, listen, warden);
            //Networking.receiveMessage(ward);
        }
        static void sendMessage()
        {
            warden.callNext = cleanBuffer;
            Networking.Send(warden.socket, usr + ": " + Console.ReadLine() + "\n");
        }
        static void cleanBuffer(NetworkWarden ward)
        {
            warden = ward;
            warden.message.Clear();
        }

    }
}
