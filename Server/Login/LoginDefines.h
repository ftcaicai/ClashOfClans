#ifndef __LOGIN_DEFINES_H
#define __LOGIN_DEFINES_H

#include "RakPeerInterface.h"
#include "RakNetTypes.h"
#include "RakSleep.h"
#include "MessageIdentifiers.h"

#define	SERVER_PORT  60000
#define MAX_CONNECT_COUNT 1000

using namespace RakNet;

enum LoginMessageIDTypes {
	ID_SEND_TEST_1 = ID_USER_PACKET_ENUM + 1,
};


#endif

