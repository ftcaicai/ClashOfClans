#include "NativeFeatureIncludes.h"

#include "FT_ConnectProcess.h"
#include "BitStream.h"
#include "RakPeerInterface.h"
#include "MessageIdentifiers.h"

using namespace RakNet;

STATIC_FACTORY_DEFINITIONS(FT_ConnectProcess,FT_ConnectProcess);
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

}

FT_Node_Process::~FT_Node_Process (){

}

FT_Node_Plugin::FT_Node_Plugin (){

}

FT_Node_Plugin::~FT_Node_Plugin() {
	std::map<FT_MessageTypesNode,FT_Node_Process*>::iterator it;
	for (it = _Handler.begin(); it != _Handler.end(); it++)
	{
		FT_Node_Process::DestroyInstance(it->second);
	}
}

PluginReceiveResult FT_Node_Plugin::OnReceive(Packet *packet){

	if (packet->data[0] == ID_SERVER_LOGIN) {
		FT_MessageTypesNode  typeNode = (FT_MessageTypesNode)packet->data[1];

		std::map<FT_MessageTypesNode,FT_Node_Process*>::iterator it = _Handler.find(typeNode);
		if (it != _Handler.end()){
			BitStream bsIn(packet->data, packet->length, false);
			bsIn.IgnoreBits(sizeof(RakNet::MessageID));
			bsIn.IgnoreBits(sizeof(RakNet::FT_MessageTypesNode));
			FT_Node_Process* hand = it->second;
			hand->OnProcess(&bsIn);

			return RR_STOP_PROCESSING;
		}
	}

	return RR_CONTINUE_PROCESSING;
}

void FT_Node_Plugin::RegisterProcess(FT_Node_Process* handler){
	_Handler.insert(std::make_pair(handler->GetNodeType(), handler));
}