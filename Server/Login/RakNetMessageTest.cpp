#include "RakNetMessageTest.h"
#include "RakPeerInterface.h"

STATIC_FACTORY_DEFINITIONS(FT_Node_Process, Test_ProcessA);

Test_ProcessA::Test_ProcessA (){

}

Test_ProcessA::~Test_ProcessA (){

}

void Test_ProcessA::OnProcess(BitStream* bsIn){
	printf_s("Test_ProcessA::OnProcess\n");
}
