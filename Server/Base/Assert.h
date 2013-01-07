#ifndef __ASSERT__H__
#define __ASSERT__H__


#include "BaseType.h"

VOID __assert__ (const CHAR* file, UINT line, const CHAR* func, const CHAR* expr) ;
VOID __assertex__ (const CHAR* file, UINT line, const CHAR* func, const CHAR* expr, const CHAR* msg) ;
VOID __assertspecial__ (const CHAR* file, UINT line, const CHAR* func, const CHAR* expr, const CHAR* msg) ;
VOID __messagebox__(const CHAR*msg ) ;

#if defined __WIN_CONSOLE__ || __WIN32__ || __WINDOWS__ 
	#define Assert(expr) ((VOID)((expr)?0:(__assert__(__FILE__,__LINE__,__FUNCTION__,#expr),0)))
	#define AssertEx(expr,msg) ((VOID)((expr)?0:(__assertex__(__FILE__,__LINE__,__FUNCTION__,#expr,msg),0)))
	#define AssertSpecial(expr,msg) ((VOID)((expr)?0:(__assertspecial__(__FILE__,__LINE__,__FUNCTION__,#expr,msg),0)))
	#define MyMessageBox(msg) __messagebox__(msg)
#endif

#endif