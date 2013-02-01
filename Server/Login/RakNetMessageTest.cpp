#include "RakNetMessageTest.h"
#include "RakPeerInterface.h"
#include "FT_DataStruct.h"

STATIC_FACTORY_DEFINITIONS(Test_ProcessA, Test_ProcessA);
STATIC_FACTORY_DEFINITIONS(Test_ProcessB, Test_ProcessB);

Test_ProcessA::Test_ProcessA (){

}

Test_ProcessA::~Test_ProcessA (){

}

void Test_ProcessA::OnProcess(const FT_Session session, BitStream* bsIn, const AddressOrGUID systemIdentifier){
	printf_s("Test_ProcessA::OnProcess\n");
	FT_UnitDataList unitDataList;
	unitDataList.session = session;
	unitDataList.Serialize(false, bsIn);

	Send(session, &unitDataList, systemIdentifier);

}

Test_ProcessB::Test_ProcessB (){

}

Test_ProcessB::~Test_ProcessB (){

}

void Test_ProcessB::OnProcess(const FT_Session session, BitStream* bsIn, const AddressOrGUID systemIdentifier){
	printf_s("Test_ProcessB::OnProcess\n");

	FT_UnitData unitData;
	unitData.session = session;
	unitData.Serialize(false, bsIn);

	printf_s(unitData.sName.C_String());

	Send(session, &unitData, systemIdentifier);

}
