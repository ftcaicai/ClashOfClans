#ifndef __H_MYSQL_TEST
#define __H_MYSQL_TEST

#include "stdafx.h"
#include "Assert.h"
#include "OtlDefine.h"

class row{
public:
	int f1;
	std::string f2;

	// default constructor
	row(){f1=0;}

	// destructor
	~row(){}

	// copy constructor
	row(const row& row)
	{
		f1=row.f1;
		f2=row.f2;
	}

	// assignment operator
	row& operator=(const row& row)
	{
		f1=row.f1;
		f2=row.f2;
		return *this;
	}

	VOID SetValueX (otl_stream& write){
		write << f1;
		write << f2;
	}

	VOID GetValueX (otl_stream& read){
		read >> f1;
		read >> f2;
	}

};

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

	VOID DeleteAll ();

	VOID InsertByStruct ();

	VOID SelectByStruct ();

private:
	otl_connect db;
};


#endif

