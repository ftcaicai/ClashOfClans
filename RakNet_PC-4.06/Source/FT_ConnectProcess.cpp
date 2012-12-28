#include "NativeFeatureIncludes.h"

#include "FT_ConnectProcess.h"
#include "BitStream.h"
#include "RakPeerInterface.h"
#include "MessageIdentifiers.h"

using namespace RakNet;

STATIC_FACTORY_DEFINITIONS(FT_ConnectProcess,FT_ConnectProcess);

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

	return RR_CONTINUE_PROCESSING;
}

void FT_ConnectProcess::OnRakPeerShutdown() {

}