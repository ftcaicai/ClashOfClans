#ifndef __LOGIN_DEFINES_H
#define __LOGIN_DEFINES_H

#include "BaseType.h"
#include "Config.h"
#include "RakPeerInterface.h"
#include "RakNetTypes.h"
#include "RakSleep.h"
#include "MessageIdentifiers.h"

#define	SERVER_PORT  60000
#define MAX_CONNECT_COUNT 1000

#define OTL_ODBC // Compile OTL 4.0/ODBC
// The following #define is required with MyODBC 5.1 and higher
#define OTL_ODBC_SELECT_STM_EXECUTE_BEFORE_DESCRIBE
#include "otlv4.h"

using namespace RakNet;

enum LoginMessageIDTypes {
	ID_SEND_TEST_1 = ID_USER_PACKET_ENUM + 1,
};

// typedef VOID (*SetSQLParams)(otl_stream write);


#endif

