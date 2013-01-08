#include "MySQLTest.h"


MySQLTest::MySQLTest(void)
{
	otl_connect::otl_initialize();
}


MySQLTest::~MySQLTest(void)
{
	db.logoff();
}

BOOL MySQLTest::Connect (const char* userName, const char* pwd, const char* dsn){

	try
	{
		char strConn[1024];
		sprintf(strConn, "UID=%s;PWD=%s;DSN=%s", userName, pwd, dsn);
		
		db.rlogon(strConn);

		return TRUE;
	}
	catch (otl_exception& p)
	{
		cerr<<p.msg<<endl;   
		cerr<<p.stm_text<<endl; 
		cerr<<p.var_info<<endl;
	}
	return FALSE;
}

VOID MySQLTest::TestSelect(){
	
	otl_stream readStream(
		1,
		"SELECT * FROM test_tab WHERE f1 = :f1<int> ",
		db
	);

	readStream << 10;

	int reaf1;
	char readf2[32];

	while (!readStream.eof())
	{
		readStream >> reaf1 >> readf2;

		printf_s(" f1 = %d, f2 = %s \n", reaf1, readf2);
	}
	
}