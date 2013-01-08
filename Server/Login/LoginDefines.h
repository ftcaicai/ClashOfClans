#ifndef __LOGIN_DEFINES_H
#define __LOGIN_DEFINES_H

#include "BaseType.h"
#include "Config.h"
#include "RakPeerInterface.h"
#include "RakNetTypes.h"
#include "RakSleep.h"
#include "MessageIdentifiers.h"

#include "OtlDefine.h"

using namespace RakNet;

enum LoginMessageIDTypes {
	ID_SEND_TEST_1 = ID_USER_PACKET_ENUM + 1,
};

// typedef VOID (*SetSQLParams)(otl_stream write);


#endif

