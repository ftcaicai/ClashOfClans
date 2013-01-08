#ifndef __H_CONFIG
#define __H_CONFIG

#include "BaseType.h"
#include "Define.h"

struct _LOGIN_INFO 
{
	CHAR			m_DBDriver[DATABASE_STR_LEN];
	CHAR			m_DBIP[IP_SIZE];
	UINT			m_DBPort;
	CHAR			m_DBName[DATABASE_STR_LEN];
	CHAR			m_DBUser[DB_USE_STR_LEN];
	CHAR			m_DBPassword[DB_PASSWORD_STR_LEN];

	UINT			m_ServerPort;
	UINT			m_MaxConnectCount;

	_LOGIN_INFO (){

	}
	~_LOGIN_INFO (){

	}
};

class Config
{
public:
	Config(void);
	~Config(void);

	BOOL					Init( ) ;
	VOID					ReLoad( ) ;

	VOID					LoadLoginInfo( );
	VOID					LoadLoginInfo_Reload( );

public:

	_LOGIN_INFO m_LoginInfo;
};

extern Config g_Config;


#endif


