#include "RakNetMessageTest.h"
#include "RakPeerInterface.h"

STATIC_FACTORY_DEFINITIONS(Test_ProcessA, Test_ProcessA);

Test_ProcessA::Test_ProcessA (){

}

Test_ProcessA::~Test_ProcessA (){

}

void Test_ProcessA::OnProcess(BitStream* bsIn){
	printf_s("Test_ProcessA::OnProcess\n");
}
