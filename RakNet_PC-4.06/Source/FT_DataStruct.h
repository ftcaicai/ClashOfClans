#ifndef _H_DATASTRUCT_FT
#define _H_DATASTRUCT_FT

#include "RakNetDefines.h"
#include "MessageIdentifiers.h"
#include "BitStream.h"
#include "DS_List.h"

namespace RakNet{

	enum FT_EUnit {
		FT_EUnit_None = 0,
		FT_EUnit_Trees = 1,
		FT_EUnit_Stone = 2,
	};

	enum FT_MessageTypes {
		ID_FT_TEST1 = ID_USER_PACKET_ENUM + 1,
		ID_SERVER_LOGIN,
	};

	enum FT_MessageTypesNode {
		NODE_FT_None = 0,
		NODE_FT_TEST1,
		NODE_FT_UNITDATA,
		NODE_FT_UNITDATA_LIST,
	};

	class FT_Session {
	public:
		RakNet::RakNetGUID	guid;
		RakNet::TimeMS		startTime;	

		bool			IsOutTime ();

		FT_Session ();
		FT_Session (RakNet::RakNetGUID	g);
		virtual ~FT_Session ();

		bool operator==(const FT_Session &c);
	};

	struct FT_DataBase {
	public:
		FT_DataBase () {}
		virtual ~FT_DataBase () {}

		virtual FT_MessageTypesNode NodeType (){ return NODE_FT_None; }

		virtual void Serialize (bool writeToBitstream, RakNet::BitStream* bs);

	public:
		FT_Session	session;
	};

	struct FT_UnitData : public FT_DataBase {
	public:
		unsigned int	iID;
		unsigned char	iLevel;
		FT_EUnit		eUnit;
		unsigned char	nGridSize;
		signed char		nGrid_x;
		signed char		nGrid_y;
		RakString		sName;
		RakString		sInfo;

	public:

		FT_UnitData();
		FT_UnitData(FT_Session session);

		FT_MessageTypesNode NodeType (){ return NODE_FT_UNITDATA; }

		void Serialize(bool writeToBitstream, RakNet::BitStream* bs);
	};

	struct FT_UnitDataList : public FT_DataBase {
	public: 
		unsigned int	iDataLength;

		FT_UnitDataList();

		void PushUnitData2List (const FT_UnitData data);
		DataStructures::List<FT_UnitData> GetUnitDataList (){ return _dataList; }
		FT_MessageTypesNode NodeType (){ return NODE_FT_UNITDATA_LIST; }

		void Serialize(bool writeToBitstream, RakNet::BitStream* bs);
	private:
		DataStructures::List<FT_UnitData>	_dataList;
	};
}

#endif