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

	ini.ReadText( "System", "DBDSNName", m_LoginInfo.m_DBDSNName, DATABASE_STR_LEN ) ;
	ini.ReadText( "System", "DBUser", m_LoginInfo.m_DBUser, DB_USE_STR_LEN ) ;
	ini.ReadText( "System", "DBPassword", m_LoginInfo.m_DBPassword, DB_PASSWORD_STR_LEN ) ;

	m_LoginInfo.m_ServerPort	=	(UINT)(ini.ReadInt("System","ServerPort"));
	m_LoginInfo.m_MaxConnectCount	=	(UINT)(ini.ReadInt("System","MaxConnectCount"));

	// Log::SaveLog( CONFIG_LOGFILE, "Load LoginInfo.ini ...Only OK! " ) ;

	__LEAVE_FUNCTION
}