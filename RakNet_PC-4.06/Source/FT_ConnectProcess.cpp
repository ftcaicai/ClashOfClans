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
	return RR_CONTINUE_PROCESSING;
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
	resultHandler = 0;
}

FT_Node_Plugin::~FT_Node_Plugin() {

}

void FT_Node_Plugin::SetRakPeer( RakPeerInterface *ptr ){
	_rakPeerInterface = ptr;
}

PluginReceiveResult FT_Node_Plugin::OnReceive(Packet *packet){

	if (resultHandler){
		RakString strLog;
		strLog.Set ( "FT_Node_Plugin::OnReceive :%d",  packet->data[0]);
		resultHandler->ReceiveLog(strLog);
	}

	if (packet->data[0] == ID_SERVER_LOGIN) {
		FT_MessageTypesNode  typeNode = (FT_MessageTypesNode)packet->data[1];

		if (resultHandler){
			RakString str;
			str.Set("FT_Node_Plugin::OnReceive data[0]: %d, data[1]: %d, typeNode: %d",packet->data[0],packet->data[1], typeNode );
			resultHandler->ReceiveLog(str);
		}

		UINT i = 0;
		for (; i < _Handlers.Size(); i++)
		{
			FT_Node_Process* handler = _Handlers[i];
			if (resultHandler){
				RakString str;
				str.Set("FT_Node_Plugin::OnReceive _Handler[%d].NodeType = %d", i, handler->GetNodeType() );
				resultHandler->ReceiveLog(str);
			}
			if (handler && handler->GetNodeType() == typeNode){
				BitStream bsIn(packet->data, packet->length, false);
				bsIn.IgnoreBits(sizeof(RakNet::MessageID));
				bsIn.IgnoreBits(sizeof(RakNet::FT_MessageTypesNode));
				FT_DataBase dataBase;
				dataBase.Serialize(false, &bsIn);
				if (!dataBase.session.IsOutTime()){
					handler->OnProcess(dataBase.session, &bsIn, packet->systemAddress);
				}
				else{
					handler->OnOutTime(dataBase.session);
				}
				return RR_STOP_PROCESSING;
			}
		}
	}

	return RR_CONTINUE_PROCESSING;
}

void FT_Node_Plugin::SetResultHandler(FT_ConnectProcessResultHandler *rh) {
	resultHandler = rh;
}

void FT_Node_Plugin::RegisterProcess(FT_Node_Process* handler){
	
	handler->SetRakPeerInterface(rakPeerInterface);
	_Handlers.Insert(handler, _FILE_AND_LINE_);
}

void FT_Node_Plugin::RegisterProcess(FT_MessageTypesNode type) {
	int removeIdenx = -1;
	int i = 0;
	for (; i < _Handlers.Size(); i++)
	{
		FT_Node_Process* handler = _Handlers[i];
		if (handler->GetNodeType() == type){
			removeIdenx = i;
			break;
		}
	}
	if (removeIdenx != -1){
		_Handlers.RemoveAtIndexFast(removeIdenx);
	}
}

uint32_t FT_Node_Plugin::Send(const FT_Session session,FT_DataBase* data, const AddressOrGUID systemIdentifier){
	return Send(session, data, MEDIUM_PRIORITY,RELIABLE_ORDERED, 0, systemIdentifier ); 
}

uint32_t FT_Node_Plugin::Send(const FT_Session session,FT_DataBase* data, PacketPriority priority, PacketReliability reliability, char orderingChannel, const AddressOrGUID systemIdentifier){
	RakNet::MessageID messageID = ID_SERVER_LOGIN;
	RakNet::FT_MessageTypesNode nodeID = data->NodeType();
	BitStream bsOut;
	bsOut.Serialize(true, messageID);
	bsOut.Serialize(true, nodeID);
	bsOut.Serialize(true, session);
	data->Serialize(true, &bsOut);
	if (_rakPeerInterface) {
		return _rakPeerInterface->Send(&bsOut, priority, reliability, orderingChannel, systemIdentifier, false);
	}
	return 0;
}