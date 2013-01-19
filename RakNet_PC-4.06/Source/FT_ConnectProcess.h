#ifndef __H_CONNECT_PROCESS
#define __H_CONNECT_PROCESS
#include <stdio.h>
#include <vector>
#include "Export.h"
#include "RakNetTypes.h"
#include "PluginInterface2.h"
#include "FT_DataStruct.h"

namespace RakNet{

class RakPeerInterface;
class FT_ConnectProcess;

class FT_ConnectProcessResultHandler {

public:
	FT_ConnectProcessResultHandler (void){}
	virtual ~FT_ConnectProcessResultHandler (void){}

	virtual void OnConnectedToServer () = 0;

	virtual void OnFailedToConnect () = 0;

	virtual void OnLostConnection () = 0;

	virtual void OnDisconnectedFromServer () = 0;

	virtual void DebugReceive (int flag) = 0;

	virtual void ReceiveLog () = 0;

	virtual void ReceiveLog2 () = 0;

	virtual PluginReceiveResult OnReceive(Packet *packet) = 0;

	virtual PluginReceiveResult OnRead(BitStream *bsIn) = 0;
};

class RAK_DLL_EXPORT FT_ConnectProcess : public PluginInterface2
{
public:

	STATIC_FACTORY_DECLARATIONS(FT_ConnectProcess)

	FT_ConnectProcess(void);
	~FT_ConnectProcess(void);

	void SetResultHandler(FT_ConnectProcessResultHandler *rh);

	virtual void Update(void) {};
	virtual PluginReceiveResult OnReceive(Packet *packet);
	virtual void OnRakPeerShutdown(void);

private:
	FT_ConnectProcessResultHandler *resultHandler;
};

class FT_Node_Process {

public:
	STATIC_FACTORY_DECLARATIONS(FT_Node_Process)

	FT_Node_Process ();
	virtual ~FT_Node_Process ();

	virtual FT_MessageTypesNode GetNodeType() { return NODE_FT_None; }

	virtual void OnProcess (BitStream* bsIn) {}

	void SetRakPeerInterface( RakPeerInterface *ptr );

private:
	RakPeerInterface*	_peer;
};

class RAK_DLL_EXPORT FT_Node_Plugin : public PluginInterface2
{
public:

	STATIC_FACTORY_DECLARATIONS(FT_Node_Plugin)

	FT_Node_Plugin(void);
	~FT_Node_Plugin(void);

	virtual PluginReceiveResult OnReceive(Packet *packet);

	void RegisterProcess(FT_Node_Process* handler);
	
	uint32_t Send(FT_DataBase* data, const AddressOrGUID systemIdentifier);

	uint32_t Send(FT_DataBase* data, PacketPriority priority, PacketReliability reliability, char orderingChannel, const AddressOrGUID systemIdentifier);

	virtual void PrintLog (const char* msg){}

private:
	std::vector<FT_Node_Process*>	_Handler;

};

}
#endif
