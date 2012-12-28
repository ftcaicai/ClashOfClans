cmake_minimum_required(VERSION 2.6)

project(RakNet)

if( NOT APPLE )
# check 64 bit
if( CMAKE_SIZEOF_VOID_P MATCHES "4" )
  set( HAVE_64_BIT 0 )
else(  CMAKE_SIZEOF_VOID_P MATCHES "4")
  set( HAVE_64_BIT 1 )
endif(  CMAKE_SIZEOF_VOID_P MATCHES "4")
endif( NOT APPLE )

IF (WIN32 AND NOT UNIX)
	IF (HAVE_64_BIT)
		set (PROGRAMFILESX86 $ENV{PROGRAMFILESX86})
	ELSE(HAVE_64_BIT)
		set (PROGRAMFILESX86 $ENV{PROGRAMFILES})
	ENDIF(HAVE_64_BIT)
	string(REPLACE "\\" "/" PROGRAMFILESX86 ${PROGRAMFILESX86})
ENDIF(WIN32 AND NOT UNIX)

IF (WIN32 AND NOT UNIX)
	set(RAKNET_LIBRARY_LIBS ws2_32.lib)
ELSE(WIN32 AND NOT UNIX)
	set(RAKNET_LIBRARY_LIBS pthread)
ENDIF(WIN32 AND NOT UNIX)

set(RAKNETHEADERFILES ${RakNet_SOURCE_DIR}/Source)
include(./CmakeIncludes/CmakeMacros.txt)
FIXLINKOPTIONS()
FIXCOMPILEOPTIONS()

#add_subdirectory(Source)
#add_subdirectory(DependentExtensions)
add_subdirectory(Lib)

set(RAKNET_COMMON_LIBS LibStatic)

add_subdirectory(Samples)
