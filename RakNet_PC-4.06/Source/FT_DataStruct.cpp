#include "FT_DataStruct.h"
#include "GetTime.h"

using namespace RakNet;

FT_Session::FT_Session (){
	startTime = RakNet::GetTimeMS();
}

FT_Session::FT_Session(RakNet::RakNetGUID g){
	guid = g;
	startTime = RakNet::GetTimeMS();
}

FT_Session::~FT_Session(){

}

void FT_Session::Serialize(bool writeToBitstream, RakNet::BitStream* bs){
	bs->Serialize(writeToBitstream, guid);
	bs->Serialize(writeToBitstream, startTime);
}

bool FT_Session::IsOutTime(){
	return (startTime + 6000) < RakNet::GetTimeMS();
}

bool FT_Session::operator==(const FT_Session &c){
	return c.guid == guid;
}

void FT_DataBase::Serialize(bool writeToBitstream, RakNet::BitStream* bs){
	bs->Serialize(writeToBitstream, session);
}

FT_UnitData::FT_UnitData (){

}

FT_UnitData::FT_UnitData(FT_Session s){
	session = s;
}

void FT_UnitData::Serialize(bool writeToBitstream, RakNet::BitStream* bs){
	bs->Serialize(writeToBitstream, iID);
	bs->Serialize(writeToBitstream, iLevel);
	bs->Serialize(writeToBitstream, eUnit);
	bs->Serialize(writeToBitstream, nGridSize);
	bs->Serialize(writeToBitstream, nGrid_x);
	bs->Serialize(writeToBitstream, nGrid_y);
	bs->Serialize(writeToBitstream, sName);
	bs->Serialize(writeToBitstream, sInfo);
}

FT_UnitDataList::FT_UnitDataList(){
	iDataLength = 0;
}

void FT_UnitDataList::PushUnitData2List (const FT_UnitData data) {
	_dataList.Push(data, _FILE_AND_LINE_);
	iDataLength++;
}

void FT_UnitDataList::Serialize(bool writeToBitstream, RakNet::BitStream* bs){
	bs->Serialize(writeToBitstream, iDataLength);
	for (unsigned int i = 0; i < iDataLength; i++)
	{
		if (writeToBitstream){
			FT_UnitData unitData = _dataList.Get(i);
			unitData.Serialize(writeToBitstream, bs);
		}
		else{
			FT_UnitData unitData;
			unitData.Serialize(writeToBitstream, bs);
			_dataList.Insert(unitData, _FILE_AND_LINE_);
		}
	}
}