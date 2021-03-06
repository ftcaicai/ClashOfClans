#ifndef __H_RAKNET_MESSAGE_TEST
#define __H_RAKNET_MESSAGE_TEST

#include "stdafx.h"


#include "RakNetTypes.h"
#include "PluginInterface2.h"
#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "FT_ConnectProcess.h"
#include "FT_DataStruct.h"
#include <map>

using namespace RakNet;

class Test_ProcessA : public FT_Node_Process {
public:
	STATIC_FACTORY_DECLARATIONS(Test_ProcessA)

	Test_ProcessA ();
	virtual ~Test_ProcessA();

	virtual FT_MessageTypesNode GetNodeType() { return NODE_FT_UNITDATA_LIST; }

	virtual void OnProcess (const FT_Session session, BitStream* bsIn, const AddressOrGUID systemIdentifier);

};

class Test_ProcessB : public FT_Node_Process {
public:
	STATIC_FACTORY_DECLARATIONS(Test_ProcessB)

		Test_ProcessB ();
	virtual ~Test_ProcessB();

	virtual FT_MessageTypesNode GetNodeType() { return NODE_FT_UNITDATA; }

	virtual void OnProcess (const FT_Session session, BitStream* bsIn, const AddressOrGUID systemIdentifier);

};

#endif


