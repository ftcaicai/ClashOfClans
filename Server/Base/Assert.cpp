#pragma comment(lib,"User32.lib")

#include "Assert.h"


VOID __show__( const CHAR* szTemp )
{

	//保存日志
#ifndef GAME_CLIENT
	FILE* f = fopen( "./Log/assert.log", "a" ) ;
	fwrite( szTemp, 1, strlen(szTemp), f ) ;
	fwrite( "\r\n", 1, 2, f ) ;
	fclose(f) ;
#endif

	// INT iRet = MessageBox( NULL, szTemp, "异常", MB_OK ) ;

#ifdef GAME_CLIENT
	throw(std::string(szTemp));
#else
	throw(1);
#endif
}

VOID __messagebox__(const CHAR*msg )
{
	// MessageBox( NULL, msg, "信息", MB_OK ) ;
}

//--------------------------------------------------------------------------------
//
// __assert__
//
//
//--------------------------------------------------------------------------------
VOID __assert__ ( const CHAR * file , UINT line , const CHAR * func , const CHAR * expr )
{
	CHAR szTemp[1024] = {0};

	sprintf( szTemp, "[%s][%d][%s][%s]", file, line, func, expr ) ;

	__show__(szTemp) ;
}

VOID __assertex__ ( const CHAR * file , UINT line , const CHAR * func , const CHAR * expr ,const CHAR* msg)
{
	CHAR szTemp[1024] = {0};

	sprintf( szTemp, "[%s][%d][%s][%s]\n[%s]", file, line, func, expr ,msg ) ;

	__show__(szTemp) ;
}

VOID __assertspecial__ ( const CHAR * file , UINT line , const CHAR * func , const CHAR * expr ,const CHAR* msg)
{
	CHAR szTemp[1024] = {0};

	sprintf( szTemp, "S[%s][%d][%s][%s]\n[%s]", file, line, func, expr ,msg ) ;

	__show__(szTemp) ;
}
//--------------------------------------------------------------------------------
//
//
//--------------------------------------------------------------------------------
VOID __protocol_assert__ ( const CHAR * file , UINT line , const CHAR * func , const CHAR * expr )
{
	printf( "[%s][%d][%s][%s]", file, line, func, expr ) ;
}
