//Adam Bennion
//4-9-2017
//
//Client Networking Controller

/*
*Two Classes are in this file:
*	-NetworkWarden
*		Keeps track of connection information, including messages from server
*	-Networking
*		All the methods that network warden will work through in order to send and receive info from server.
*/

/*
*How Client utilizes this controller:
*	1. Initialize a NetworkWarden in client program. (NetworkWarden networkSocket)
*	2. use command: Networking.ConnectToServer(/*method for handling successful tcp connection goes here*/

/*  server ip goes here
*	3. inside local successful connection command (which needs a NetworkWarden parameter):
*		update parameter NetworkWarden with networkWardenVariableName.callNext = /*local method expecting first message from server*/
/*		use command: Networking.Send(networkwarden.socket, /* hello message + \n */
/*	4. inside the callNext function assigned in step 3, we again need the NetworkWarden parameter.
*		set your local NetworkWarden to the incoming parameter.
*		make a buffer for collecting message from network, and parse it. message will be inside incoming NetworkWarden parameter.message
*/
using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatClient
{

    /// <summary>
    // Holds the info (socket) necessary to handle a client connection to server.
    // Keeps track of incoming messages from server.
    // Keeps track of which method to call next when server responds (callNext).
    /// </summary>
    public class NetworkWarden
    {
        public Socket socket;
        public int ID; //assigned by server for server socket connection tracking
        public byte[] buffer = new byte[1024]; // byte string, used for recieving
        public string buffString = null; // string version of buffer, for message extraction
        public StringBuilder message = new StringBuilder(); // used for sending
        public delegate void callBack(NetworkWarden warden);//call back functions must have a NetworkWarden parameter
        public callBack callNext;//holds a method to be executed once reply arrives
        /// <summary>
        // Holds the info (socket) necessary to handle a client connection to server.
        // Keeps track of incoming messages from server.
        // Keeps track of which method to call next when server responds (callNext).
        /// </summary>
        public NetworkWarden(Socket s, int id)
        {
            socket = s;
            ID = id;
        }
    }

    /// <summary>
    /// All networking methods that client needs.
    /// </summary>
    public static class Networking
    {

        public const int PORT = 2113;
        /*
        *ConnectToServer
        *give callback function for first parameter.
        *give ip of server as 'hostname' parameter.
        */
        public static Socket ConnectToServer(NetworkWarden.callBack callbackFunction, string hostname)
        {
            System.Diagnostics.Debug.WriteLine("connecting to " + hostname);
            //Connect to server
            try
            {
                //Establish remote endpoint for socket
                IPHostEntry ipHostInfo;
                IPAddress ipAddress = IPAddress.None;
                //determine if the server address is a URL or IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostname);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                    {
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    }
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid address: " + hostname);
                        return null;
                    }
                }
                catch (Exception e1)
                {
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostname);
                }

                //create a TCP/IP socket
                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);
                NetworkWarden warden = new NetworkWarden(socket, -1);

                warden.callNext = callbackFunction;
                warden.socket.BeginConnect(ipAddress, PORT, handleSuccessfulConnection, warden);
                return socket;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return null;
            }
        }

        /// <summary>
        /// If connection with server successful, complete connection.
        /// Begin listening for server data.
        /// </summary>
        /// <param name="ARSTATE"></param>
        static private void handleSuccessfulConnection(IAsyncResult ARSTATE)
        {
            NetworkWarden warden = (NetworkWarden)ARSTATE.AsyncState;

            try
            {
                //stops the attempt to connect routine
                warden.socket.EndConnect(ARSTATE);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }
            //begin receiving data.
            warden.callNext(warden);
            warden.socket.BeginReceive(warden.buffer, 0, warden.buffer.Length, SocketFlags.None, receiveMessage, warden);
        }

        /// <summary>
        /// Receives state and server's transfered bytes and appends UTF8 conversion to warden's message variable
        /// </summary>
        /// <param name="ARSTATE"></param>
        public static void receiveMessage(IAsyncResult ARSTATE)
        {
            try
            {
              NetworkWarden warden = (NetworkWarden)ARSTATE.AsyncState;
              int numBytes = warden.socket.EndReceive(ARSTATE);
    
              if (numBytes > 0) {
                string newMessage = Encoding.UTF8.GetString(warden.buffer, 0, numBytes);
                newMessage = newMessage.Trim();
                newMessage = newMessage.TrimEnd();
                //string[] lines = ss.sb.ToString().Split(new string[] {"\n"}, StringSPlitOptions.None);
                //warden.message.Clear();
                //warden.message.Append(newMessage);
                Console.WriteLine("Server said: " + newMessage);
                warden.buffString = newMessage;
              }
              warden.callNext(warden);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Receive callback failed. Error occured: " + e);
                return;
            }
        }

        /// <summary>
        /// Allows a program to send data over a socket. 
        /// Stores message into byte array and begins sending bytes.
        /// Helper function 'StopSend' will be called once all bytes have sent.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(NetworkWarden ward, String data)
        {
            //data = data.TrimEnd();//removes any leftover spaces
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            try
            {
                ward.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, StopSend, ward);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Send Failed. Error occured: " + e);
                throw new Exception("Send Failed. Error occured: " + e);
            }
        }


        /// <summary>
        /// This function "assists" the Send function. If all the data has been sent, end sending procedure.
        /// </summary>
        static void StopSend(IAsyncResult ARSTATE)
        {
            NetworkWarden warden = (NetworkWarden)ARSTATE.AsyncState;
            warden.socket.EndSend(ARSTATE);
            //Console.WriteLine("warden callnext: " + (warden.callNext.Method.Name));
            warden.callNext(warden);
        }

        public static void getData(NetworkWarden ward)
        {
            ward.socket.BeginReceive(ward.buffer, 0, ward.buffer.Length, SocketFlags.None, receiveMessage, ward);
        }
    }

}