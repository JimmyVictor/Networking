#include "pch.h"
#include <iostream>
#include <WinSock2.h>
#pragma comment(lib, "Ws2_32.lib")

using namespace std;

int main()
{
	WSADATA WASData; // data about the windows socket implementation

	SOCKET serverSocket, clientSocket;

	SOCKADDR_IN serverAddr, clientAddr; // addresses of the server and the client we will connect to

	WSAStartup(MAKEWORD(2, 0), &WASData); // Initializes the winsock DLL

	serverSocket = socket(AF_INET, SOCK_STREAM, 0); // create a new socket
													// AF_INET = address family internet
													// SOCK_SREAM = streaming data
													// protocol, 0 as we dont care what protocol we use

	serverAddr.sin_addr.s_addr = INADDR_ANY; //try any
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = htons(1234); // set the port number

	bind(serverSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)); // bind the socket and the address
	listen(serverSocket, 0); // places the socket in a state where it is listening for incoming connections

	while (true) // forever
	{
		cout << "Listening for incoming Connections..." << endl;

		char buffer[1024]; // make a buffer to store data in
		int clientAddrSize = sizeof(clientAddr); // get the size of the client adress object

		if ((clientSocket = accept(serverSocket, (SOCKADDR*)&clientAddr, &clientAddrSize)) != INVALID_SOCKET)
		{
			memset(buffer, 0, sizeof(buffer)); // set buffer to 0, to be safe
			cout << "Client connected!" << endl;
			recv(clientSocket, buffer, sizeof(buffer), 0); // read data from the incoming connection
			cout << "Client says: " << buffer << endl; // print the client's message

			string responseText = "Response from server: ";
			responseText += buffer; // add our response to the client's message

			memcpy(buffer, responseText.c_str(), responseText.length());

			send(clientSocket, buffer, sizeof(buffer), 0); // send the new response back to the client

			closesocket(clientSocket); // close the connection
			cout << "Client disconnected." << endl;
		}
	}

	return 0;
}

