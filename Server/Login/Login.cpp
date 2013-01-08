#include "LoginDefines.h"
#include "FT_ConnectProcess.h"
#include "Config.h"
#include "MySQLTest.h"

using namespace std;

bool _bQuit = false;

void Quit (int k){
	_bQuit = true;
}

void ProcessRakNetMessage (RakPeerInterface *peer){
	Packet *packet;
	while (!_bQuit)
	{
		for( packet = peer->Receive(); packet; peer->DeallocatePacket(packet), packet = peer->Receive()){
			switch (packet->data[0])
			{
			case ID_CONNECTION_REQUEST_ACCEPTED:
				printf_s("In a client/server environment, our connection request to the server has been accepted.\n");
				break;
			case ID_CONNECTION_ATTEMPT_FAILED:
				printf_s("Sent to the player when a connection request cannot be completed due to inability to connect.\n ");
				break;
			case ID_ALREADY_CONNECTED:
				printf_s("Sent a connect request to a system we are currently connected to.\n ");
				break;
			case ID_NEW_INCOMING_CONNECTION:
				printf_s("A remote system has successfully connected.\n");
				break;
			case ID_DISCONNECTION_NOTIFICATION:
				printf_s(" A remote system has disconnected. \n");
				break;
			case ID_CONNECTION_LOST:
				printf_s(" The connection to that system has been closed. \n");
				break;
			default:
				printf_s("others");
				break;
			}
		}

		RakSleep(30);
	}
}

VOID Init (){
	BOOL bRet = g_Config.Init();
	Assert(bRet);
	printf_s("Load Config...OK!\n");
}



VOID _TestMySQLConnect (){

	MySQLTest mysqlTest;

	if (!mysqlTest.Connect(g_Config.m_LoginInfo.m_DBUser, g_Config.m_LoginInfo.m_DBPassword, g_Config.m_LoginInfo.m_DBDSNName)){
		printf_s("MysqlConnect Error...\n");
		return;
	}

	mysqlTest.TestSelect();
}


int main (void){

	Init ();

	_TestMySQLConnect ();

	signal(SIGINT, Quit);

	RakPeerInterface *peer = RakPeerInterface::GetInstance();
	SocketDescriptor sd(g_Config.m_LoginInfo.m_ServerPort, 0);

	peer->Startup( g_Config.m_LoginInfo.m_MaxConnectCount, &sd, 1);
	peer->SetMaximumIncomingConnections( g_Config.m_LoginInfo.m_MaxConnectCount );

	/*
	FT_ConnectProcessResultHandlerTest *pluginHandler = new FT_ConnectProcessResultHandlerTest();
	FT_ConnectProcess *process = FT_ConnectProcess::GetInstance();
	process->SetResultHandler( pluginHandler );
	peer->AttachPlugin( process );
	*/

	printf_s("Server Listen At : %d , MaxConnect: %d \n", g_Config.m_LoginInfo.m_ServerPort, g_Config.m_LoginInfo.m_MaxConnectCount);
	
	ProcessRakNetMessage(peer);

	peer->Shutdown(300);

	RakPeerInterface::DestroyInstance(peer);

	// delete(pluginHandler);
	
	return 0;
}