#ifndef __H_MYSQL_TEST
#define __H_MYSQL_TEST

#include "LoginDefines.h"

class MySQLTest
{
public:
	MySQLTest(void);
	~MySQLTest(void);

	BOOL Connect (const char* userName, const char* pwd, const char* dsn);

	VOID TestSelect ();

private:
	otl_connect db;
};

#endif

