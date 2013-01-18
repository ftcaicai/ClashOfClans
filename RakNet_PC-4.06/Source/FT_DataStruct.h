#ifndef _H_DATASTRUCT_FT
#define _H_DATASTRUCT_FT

#include "RakNetDefines.h"
#include "MessageIdentifiers.h"
#include "BitStream.h"

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
	};

	struct FT_UnitData {
	public:
		unsigned int	iID;
		unsigned char	iLevel;
		FT_EUnit		eUnit;
		unsigned char	nGridSize;
		unsigned char	nGrid_x;
		unsigned char	nGrid_y;

		void Serialize(bool writeToBitstream, RakNet::BitStream* bs);
	};
}

#endif