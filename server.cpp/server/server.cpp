#include "pch.h"
#define _WINSOCK_DEPRECATED_NO_WARNINGS // makes VS shut up
#include <iostream>
#include <sstream>
#include <WinSock2.h>
#pragma comment(lib, "Ws2_32.lib")

using namespace std;

int main()
{
	WSADATA WSAData; // data about the windows socket implementation

	SOCKET serverSocket; // the socket for the server we will connect to

	SOCKADDR_IN addr; // the address of the server we will connect to

	WSAStartup(MAKEWORD(2, 0), &WSAData); // init the winsock DLL

	serverSocket = socket(AF_INET, SOCK_STREAM, 0); // create the socket for the server

	addr.sin_addr.s_addr = inet_addr("10.128.3.110"); // set the server address, local host in our case as well

	addr.sin_family = AF_INET; // set the adress family, so the socket knows its for an itnernet adress
	addr.sin_port = htons(1234); // set the port to listen to

	connect(serverSocket, (SOCKADDR*)&addr, sizeof(addr)); // attempt a connection to the server
														   // can fail, wrap it up in an if statement

	cout << "Connected to the server!" << endl;

	char buffer[1024]; // make a data buffer for our message
	memset(buffer, 0, sizeof(buffer)); // set buffer to 0

	string msg;
	getline(cin, msg); // read a line of text from the console
	//msg+= "<EOF>";
	memcpy(buffer, msg.c_str(), msg.length()); // copy the message to the buffer

	send(serverSocket, msg.c_str(), msg.length(), 0); // send the buffer to the connected socket
	cout << "Message sent!" << endl;

	cout << "Server response..." << endl;
	recv(serverSocket, buffer, sizeof(buffer), 0); // wait for response, this method is blocking
	cout << buffer << endl; // print response from server

	closesocket(serverSocket); // close the socket
	WSACleanup(); // clean up
	cout << "Socket closed." << endl;

	system("pause"); // lets you read the message

	return 0;
}
