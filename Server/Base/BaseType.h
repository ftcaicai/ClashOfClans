#ifndef __BASETYPE_H__
#define __BASETYPE_H__

#include <stdio.h>
#include <iostream>
#include <vector>
#include <iterator>
#include <string>
#include <cstring>
#include <cstdlib>
#include <fstream>
#include <time.h>
#include <math.h>
#include <stdarg.h>
#include <signal.h>

using namespace std;

///////////////////////////////////////////////////////////////////////
//标准数据类型定义
///////////////////////////////////////////////////////////////////////
#define VOID			void			//标准空
typedef unsigned char	UCHAR;			//标准无符号CHAR
typedef char			CHAR;			//标准CHAR
typedef unsigned int	uint;			//标准无符号INT
typedef int				INT;			//标准INT
typedef unsigned short	USHORT;			//标准无符号short
typedef short			SHORT;			//标准short
typedef unsigned long	ULONG;			//标准无符号LONG(不推荐使用)
typedef long			LONG;			//标准LONG(不推荐使用)
typedef float			FLOAT;			//标准float

typedef UCHAR			uchar;
typedef USHORT			ushort;
typedef uint			UINT;
typedef ULONG			ulong;
typedef ULONG			IP_t;
typedef USHORT			PacketID_t;
typedef INT				BOOL;
typedef UCHAR			BYTE;

//无效的句柄
#define INVALID_HANDLE	-1
//无效的ID值
#define INVALID_ID		-1
//真
#ifndef TRUE
#define TRUE 1
#endif
//假
#ifndef FALSE
#define FALSE 0
#endif
//文件路径的字符最大长度
#ifndef _MAX_PATH
#define _MAX_PATH 260
#endif

//根据指针值删除内存
#ifndef SAFE_DELETE
#define SAFE_DELETE(x)	if( (x)!=NULL ) { delete (x); (x)=NULL; }
#endif
//根据指针值删除数组类型内存
#ifndef SAFE_DELETE_ARRAY
#define SAFE_DELETE_ARRAY(x)	if( (x)!=NULL ) { delete[] (x); (x)=NULL; }
#endif
//根据指针调用free接口
#ifndef SAFE_FREE
#define SAFE_FREE(x)	if( (x)!=NULL ) { free(x); (x)=NULL; }
#endif
//根据指针调用Release接口
#ifndef SAFE_RELEASE
#define SAFE_RELEASE(x)	if( (x)!=NULL ) { (x)->Release(); (x)=NULL; }
#endif

#define __ENTER_FUNCTION 
#define __LEAVE_FUNCTION

#define _MY_TRY try
#define _MY_CATCH catch(...)

#endif