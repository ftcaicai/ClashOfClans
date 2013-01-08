#ifndef __H_DB_STRUCT
#define __H_DB_STRUCT

#include "BaseType.h"
#include "OtlDefine.h"

class _TEST_TAB {
public:
	int f1;
	string f2;

	_TEST_TAB (){
		f1 = 0;
	}
	~_TEST_TAB(){

	}

	_TEST_TAB(const _TEST_TAB& test_tab){
		f1 = test_tab.f1;
		f2 = test_tab.f2;
	}

	_TEST_TAB& operator=(const _TEST_TAB& test_tab){
		f1 = test_tab.f1;
		f2 = test_tab.f2;
		return *this;
	}
};

#endif