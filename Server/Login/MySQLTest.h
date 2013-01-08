#ifndef __H_MYSQL_TEST
#define __H_MYSQL_TEST

#include "LoginDefines.h"
#include "DB_Struct.h"

class MySQLTest
{
public:
	MySQLTest(void);
	~MySQLTest(void);

	BOOL Connect (
		const char* driver, 
		const char* ip, 
		const int	port,
		const char* userName, 
		const char* pwd, 
		const char* dbname
	);

	VOID CreateTable ();

	VOID Insert ();

	VOID TestSelect ();

	VOID Delete ();

private:
	otl_connect db;
};

#endif

