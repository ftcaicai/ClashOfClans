#include "stdafx.h"
#include "LoginIncludes.h"
#include "RakPeerInterface.h"
#include "BitStream.h"
#include "RakSleep.h"
#include "FT_ConnectProcess.h"
#include "FT_DataStruct.h"
#include "RakNetMessageTest.h"

using namespace RakNet;

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
				{
					FT_UnitData ft_data;
					RakNet::BitStream bsWrite;

					ft_data.eUnit = FT_EUnit_Trees;
					ft_data.iID = 1;
					ft_data.iLevel = 1;
					ft_data.nGrid_x = 5;
					ft_data.nGrid_y = 5;
					ft_data.nGridSize = 5;
					ft_data.sName = "x";
					ft_data.sInfo = "xx";

					bsWrite.Write((RakNet::MessageID)ID_SERVER_LOGIN);
					bsWrite.Write((RakNet::MessageID)ID_FT_TEST1);
					ft_data.Serialize(true, &bsWrite);
					peer->Send(&bsWrite, HIGH_PRIORITY,RELIABLE_ORDERED, 0, packet->systemAddress,false);
				}

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

DWORD take;

VOID _TestMySQLConnect (){

	MySQLTest mysqlTest;
	 
	BOOL bRet = 
		mysqlTest.Connect(
			g_Config.m_LoginInfo.m_DBDriver,
			g_Config.m_LoginInfo.m_DBIP,
			g_Config.m_LoginInfo.m_DBPort,
			g_Config.m_LoginInfo.m_DBUser, 
			g_Config.m_LoginInfo.m_DBPassword, 
			g_Config.m_LoginInfo.m_DBName);
	if (!bRet){
		printf_s("MysqlConnect Error...\n");
		Assert(TRUE);
		return;
	}
	mysqlTest.CreateTable();
	mysqlTest.DeleteAll();

	printf_s("Insert...\n");
	take = GetTickCount();

	mysqlTest.Insert();

	printf_s("Insert...OK! Use Tick =[%ld]\n", GetTickCount () - take);

	printf_s("InsertByStruct...\n");
	take = GetTickCount();

	mysqlTest.InsertByStruct();

	printf_s("InsertByStruct...OK! Use Tick =[%ld]\n", GetTickCount () - take);

	take = GetTickCount();
	/*
	printf_s("After Insert Select...\n");
	
	mysqlTest.TestSelect();

	printf_s("After Insert Select...OK! Use Tick =[%ld]\n", GetTickCount () - take);
	*/

	take = GetTickCount();

	printf_s("Delete...\n");

	mysqlTest.Delete();

	printf_s("Delete...OK! Use Tick =[%ld]\n",  GetTickCount () - take);

	printf_s("After Delete Select...\n");

	mysqlTest.SelectByStruct();

	printf_s("After Delete Select...OK\n"); 
}


void RakNetMessageTestInit (RakPeerInterface *peer){
	FT_Node_Plugin* test = FT_Node_Plugin::GetInstance();
	FT_Node_Process* processa = Test_ProcessA::GetInstance();
	test->RegisterProcess(processa);
	peer->AttachPlugin(test);
}

int main (void){

	Init (); 

	// _TestMySQLConnect ();

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

	RakNetMessageTestInit(peer);

	printf_s("Server Listen At : %d , MaxConnect: %d \n", g_Config.m_LoginInfo.m_ServerPort, g_Config.m_LoginInfo.m_MaxConnectCount);
	
	ProcessRakNetMessage(peer);

	peer->Shutdown(300);

	RakPeerInterface::DestroyInstance(peer);

	// delete(pluginHandler);
	
	return 0;
}