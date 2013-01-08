#include "MySQLTest.h"


MySQLTest::MySQLTest(void)
{
	otl_connect::otl_initialize();
}


MySQLTest::~MySQLTest(void)
{
	db.logoff();
}

BOOL MySQLTest::Connect (
	const char* driver, 
	const char* ip, 
	const int	port,
	const char* userName, 
	const char* pwd, 
	const char* dbname){

	try
	{
		char strConn[2048];

		sprintf_s(strConn, 
			"Driver=%s;Server=%s;Port=%d;Database=%s;"
			"User=%s;Password=%s;Option=3;",
			driver, ip, port, dbname,
			userName, pwd
		); 

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

VOID MySQLTest::CreateTable(){

	long rpc = db.direct_exec("CREATE TABLE IF NOT EXISTS test_tab (f1 int(16) not null primary key, f2 varchar(32) )");
	if (rpc	== -1){
		printf_s("create table error");
		Assert(TRUE);
	}

	/*
	long rpc = otl_cursor::direct_exec(
		db,
		"CREATE TABLE IF NOT EXISTS test_tab (f1 int(16) not null primary key, f2 varchar(32) )",
		otl_exception::disabled
	);
	*/
}

VOID MySQLTest::Insert() {
	otl_stream writeStream (
		1,
		"INSERT INTO test_tab VALUES (:f1<int>,:f2<char[33]>)",
		db);

	char cF2[33];

	for (int i = 0; i < 100; i++)
	{
		sprintf_s(cF2, "FtCaiCai%d",i);
		writeStream << i << cF2;
	}
}

VOID MySQLTest::TestSelect(){
	
	CreateTable ();

	otl_stream readStream(
		50,
		"SELECT * FROM test_tab ",
		db
	);

	// readStream << 10;

	int reaf1;
	char readf2[32];

	while (!readStream.eof())
	{
		readStream >> reaf1 >> readf2;

		printf_s(" f1 = %d, f2 = %s \n", reaf1, readf2);
	}
	
}

VOID MySQLTest::Delete(){
	/*
	otl_stream writeStream (
		1,
		"DELETE FROM test_tab WHERE f1 = :f1<int> ",
		db);
	*/

	char sql[1024];

	for (int i = 0; i < 100; i++)
	{
		sprintf_s(sql, "DELETE FROM test_tab WHERE f1 = %d", i);
		db.direct_exec(sql);
	}
}