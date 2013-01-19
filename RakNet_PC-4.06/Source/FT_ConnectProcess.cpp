#include "NativeFeatureIncludes.h"
#include "FT_ConnectProcess.h"
#include "BitStream.h"
#include "RakPeerInterface.h"
#include "MessageIdentifiers.h"

using namespace RakNet;

STATIC_FACTORY_DEFINITIONS(FT_ConnectProcess,FT_ConnectProcess);
STATIC_FACTORY_DEFINITIONS(FT_Node_Process,FT_Node_Process);
STATIC_FACTORY_DEFINITIONS(FT_Node_Plugin,FT_Node_Plugin);

FT_ConnectProcess::FT_ConnectProcess(){
	resultHandler = 0;
}

FT_ConnectProcess::~FT_ConnectProcess (){

}

void FT_ConnectProcess::SetResultHandler(FT_ConnectProcessResultHandler *rh){
	resultHandler = rh;
}

PluginReceiveResult FT_ConnectProcess::OnReceive(Packet *packet){

	if(resultHandler) {
		switch (packet->data[0])
		{
		case ID_CONNECTION_ATTEMPT_FAILED:
			resultHandler->OnFailedToConnect();
			break;
		case ID_CONNECTION_REQUEST_ACCEPTED:
			resultHandler->OnConnectedToServer();
			break;
		case ID_CONNECTION_LOST:
			resultHandler->OnLostConnection();
			break;
		case ID_DISCONNECTION_NOTIFICATION:
			resultHandler->OnDisconnectedFromServer();
			break;
		}
		resultHandler->DebugReceive(packet->data[0]);
	}

	resultHandler->ReceiveLog();

	return resultHandler->OnReceive(packet) ;
}

void FT_ConnectProcess::OnRakPeerShutdown() {

}

FT_Node_Process::FT_Node_Process (){
	_peer = 0;
}

FT_Node_Process::~FT_Node_Process (){

}

void FT_Node_Process::SetRakPeerInterface( RakPeerInterface *ptr ){
	_peer = ptr;
}

FT_Node_Plugin::FT_Node_Plugin (){

}

FT_Node_Plugin::~FT_Node_Plugin() {
	std::vector<FT_Node_Process*>::iterator it;
	for (it = _Handler.begin(); it != _Handler.end(); it++)
	{
		FT_Node_Process::DestroyInstance( *it );
	}
	
}

PluginReceiveResult FT_Node_Plugin::OnReceive(Packet *packet){
	PrintLog("xx");
	if (packet->data[0] == ID_SERVER_LOGIN) {
		FT_MessageTypesNode  typeNode = (FT_MessageTypesNode)packet->data[1];
		
		std::vector<FT_Node_Process*>::iterator it;
		for (it = _Handler.begin(); it != _Handler.end(); it++)
		{
			if((*it)->GetNodeType() == typeNode){
				BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBits(sizeof(RakNet::MessageID));
				bsIn.IgnoreBits(sizeof(RakNet::FT_MessageTypesNode));
				(*it)->OnProcess(&bsIn);
				return RR_STOP_PROCESSING;
			}
		}
	}

	return RR_CONTINUE_PROCESSING;
}

bool find_node (std::vector<FT_Node_Process*>::value_type va, FT_MessageTypesNode nodetype ){
	return va->GetNodeType() == nodetype;
}

void FT_Node_Plugin::RegisterProcess(FT_Node_Process* handler){
	
	handler->SetRakPeerInterface(rakPeerInterface);
	_Handler.push_back(handler);	
}

uint32_t FT_Node_Plugin::Send(FT_DataBase* data, const AddressOrGUID systemIdentifier){
	return Send(data, MEDIUM_PRIORITY,RELIABLE_ORDERED, 0, systemIdentifier ); 
}

uint32_t FT_Node_Plugin::Send(FT_DataBase* data, PacketPriority priority, PacketReliability reliability, char orderingChannel, const AddressOrGUID systemIdentifier){
	RakNet::MessageID messageID = ID_SERVER_LOGIN;
	RakNet::FT_MessageTypesNode nodeID = data->NodeType();
	BitStream bsOut;
	bsOut.Serialize(true, messageID);
	bsOut.Serialize(true, nodeID);
	data->Serialize(true, &bsOut);
	if (rakPeerInterface) {
		return rakPeerInterface->Send(&bsOut, priority, reliability, orderingChannel, systemIdentifier, false);
	}
	return 0;
}