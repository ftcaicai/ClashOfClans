#include "RakNetMessageTest.h"
#include "RakPeerInterface.h"
#include "FT_DataStruct.h"

STATIC_FACTORY_DEFINITIONS(Test_ProcessA, Test_ProcessA);

Test_ProcessA::Test_ProcessA (){

}

Test_ProcessA::~Test_ProcessA (){

}

void Test_ProcessA::OnProcess(const FT_Session session, BitStream* bsIn, const AddressOrGUID systemIdentifier){
	printf_s("Test_ProcessA::OnProcess\n");
	FT_UnitDataList unitDataList;
	unitDataList.session = session;
	unitDataList.Serialize(false, bsIn);

	FT_Node_Plugin::GetInstance()->Send(session, &unitDataList, systemIdentifier);
}
