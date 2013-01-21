#include "RakNetMessageTest.h"
#include "RakPeerInterface.h"

STATIC_FACTORY_DEFINITIONS(Test_ProcessA, Test_ProcessA);

Test_ProcessA::Test_ProcessA (){

}

Test_ProcessA::~Test_ProcessA (){

}

void Test_ProcessA::OnProcess(BitStream* bsIn, const AddressOrGUID systemIdentifier){
	printf_s("Test_ProcessA::OnProcess\n");
	FT_UnitData unitData;
	unitData.Serialize(false, bsIn);
	FT_Node_Plugin::GetInstance()->Send(&unitData, systemIdentifier);
}
