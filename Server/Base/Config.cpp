#include "Config.h"
#include "Ini.h"
#include "FileDef.h"

Config g_Config;

Config::Config(void)
{
}

Config::~Config(void)
{
}

BOOL Config::Init(){
	__ENTER_FUNCTION

	LoadLoginInfo();

	return TRUE ;

	__LEAVE_FUNCTION

	return FALSE ;
}

VOID Config::ReLoad( )
{
	LoadLoginInfo_Reload();
}

VOID Config::LoadLoginInfo( )
{
	LoadLoginInfo_Reload ();
}

VOID Config::LoadLoginInfo_Reload( )
{
	__ENTER_FUNCTION

	Ini ini( FILE_LOGIN_INFO ) ;

	CHAR	dbFlag[] = "DB"; 

	ini.ReadText( dbFlag, "DBDriver", m_LoginInfo.m_DBDriver, DATABASE_STR_LEN ) ;
	ini.ReadText( dbFlag, "DBIP", m_LoginInfo.m_DBIP, DATABASE_STR_LEN ) ;
	m_LoginInfo.m_DBPort = (UINT)(ini.ReadInt(dbFlag,"DBPort"));

	ini.ReadText( dbFlag, "DBName", m_LoginInfo.m_DBName, DATABASE_STR_LEN ) ;
	ini.ReadText( dbFlag, "DBUser", m_LoginInfo.m_DBUser, DB_USE_STR_LEN ) ;
	ini.ReadText( dbFlag, "DBPassword", m_LoginInfo.m_DBPassword, DB_PASSWORD_STR_LEN ) ;

	m_LoginInfo.m_ServerPort	=	(UINT)(ini.ReadInt(dbFlag, "ServerPort"));
	m_LoginInfo.m_MaxConnectCount	=	(UINT)(ini.ReadInt(dbFlag, "MaxConnectCount"));

	// Log::SaveLog( CONFIG_LOGFILE, "Load LoginInfo.ini ...Only OK! " ) ;

	__LEAVE_FUNCTION
}