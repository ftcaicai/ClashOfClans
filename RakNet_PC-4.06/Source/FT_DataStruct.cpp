#include "FT_DataStruct.h"

using namespace RakNet;

void FT_UnitData::Serialize(bool writeToBitstream, RakNet::BitStream* bs){
	bs->Serialize(writeToBitstream, iID);
	bs->Serialize(writeToBitstream, iLevel);
	bs->Serialize(writeToBitstream, eUnit);
	bs->Serialize(writeToBitstream, nGridSize);
	bs->Serialize(writeToBitstream, nGrid_x);
	bs->Serialize(writeToBitstream, nGrid_y);
}