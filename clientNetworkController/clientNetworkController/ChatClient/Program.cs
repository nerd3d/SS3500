using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

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
        static NetworkWarden listenWarden;
        static void Main(string[] args)
        {
            //string hostname = "lab2-24.eng.utah.edu";
            string hostname = "lab1-7.eng.utah.edu";
            Networking.ConnectToServer(sendName, hostname);
            while (true) { };
        }
        static void sendName(NetworkWarden ward)
        {
            ward.callNext = getNameBack;
            Networking.Send(ward, "Praise the Sun" + "\n");
        }
        static void getNameBack(NetworkWarden ward)
        {
            warden = ward;
            Console.WriteLine("server sent spreadsheet: " + warden.message);
            ward.callNext = getNameBack;
            Networking.Send(ward, "trying to send chats" + "\n");
            //Networking.Send(ward.socket, "#");
            listenWarden = new NetworkWarden(ward.socket, ward.ID);
            getData(listenWarden);
            sendMessage(warden);
            //Thread ctThread = new Thread(getData(warden));
            //ctThread.Start();
        }
        static void sendMessage(NetworkWarden ward)
        {
            /*while(warden == null)
            {
            }*/
            ward.callNext = sendMessage;
            Networking.Send(ward, Console.ReadLine() + "\n");
        }
        static void cleanBuffer(NetworkWarden ward)
        {
            warden = ward;
            warden.message.Clear();
            sendMessage(warden);
        }
        static void getData(NetworkWarden ward)
        {
            listenWarden = ward;
            listenWarden.callNext = getData;
            Networking.getData(listenWarden);
        }
    }
}
