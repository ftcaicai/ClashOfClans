using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;

namespace Server
{
    class Test
    {
        private RakPeerInterface _peer;
        private SocketDescriptor _Socket;
        private Packet _packet;

        private bool _bStop;

        public Test()
        {
            _peer = RakPeerInterface.GetInstance();
            _bStop = false;
        }

        ~Test()
        {
            _bStop = true;

        }

        public StartupResult Startup(ushort port, ushort maxConnect)
        {
            _Socket = new SocketDescriptor(port, "");
            StartupResult result = _peer.Startup(maxConnect, _Socket, 1);
            _peer.SetMaximumIncomingConnections(maxConnect);
            Log.Info("RakPeerInterface.Startup(" + port + ")" + result);
            if (result == StartupResult.RAKNET_STARTED)
            {
                ProcessMessage();
            }
            return result;
        }

        public void ProcessMessage()
        {
            while (!_bStop)
            {
                _packet = _peer.Receive();
                while (_packet != null)
                {
                    byte messageType = _packet.data[0];
                    if (messageType < (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM)
                    {
                        DefaultMessageIDTypes idType = (DefaultMessageIDTypes)messageType;
                        switch (idType)
                        {
                            case DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                                Log.Info("In a client/server environment, our connection request to the server has been accepted.\n");
                                break;
                            case DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                                Log.Info("Sent to the player when a connection request cannot be completed due to inability to connect.\n ");
                                break;
                            case DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                                Log.Info("Sent a connect request to a system we are currently connected to.\n ");
                                break;
                            case DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                                Log.Info("A remote system has successfully connected.\n");
                                break;
                            case DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                                Log.Info(" A remote system has disconnected. \n");
                                break;
                            case DefaultMessageIDTypes.ID_CONNECTION_LOST:
                                Log.Info(" The connection to that system has been closed. \n");
                                break;
                        }
                    }
                    else
                    {
                        if (messageType == (byte)FT_MessageTypes.ID_SERVER_LOGIN)
                        {
                            byte dataType = _packet.data[1];
                            RakNet.BitStream bsIn = new RakNet.BitStream(_packet.data, _packet.length, false);
                            bsIn.IgnoreBytes(2);
                            FT_UnitData unitData = new FT_UnitData();
                            unitData.session.Serialize(false, bsIn);
                            unitData.Serialize(false, bsIn);
                            Log.Info("" + unitData.sInfo.C_String());

                            byte serverLogin = (byte)RakNet.FT_MessageTypes.ID_SERVER_LOGIN;
                            
                            RakNet.BitStream bsOut = new RakNet.BitStream();
                            bsOut.Serialize(true, ref serverLogin);
                            bsOut.Serialize(true, ref dataType);
                            unitData.session.Serialize(true, bsOut);
                            unitData.Serialize(true, bsOut);

                            uint sendLength =  _peer.Send(bsOut,
                                RakNet.PacketPriority.IMMEDIATE_PRIORITY,
                                RakNet.PacketReliability.RELIABLE_ORDERED, (char)0,
                                _packet.systemAddress,
                                false);

                            Log.Info("SendLength:" + sendLength);
                        }
                    }
                    Log.Info(string.Format("Receive Data. [0] = {0}, Length = {1}", _packet.data[0], _packet.data.Length));
                    _peer.DeallocatePacket(_packet);
                    _packet = _peer.Receive();
                }
                System.Threading.Thread.Sleep(30);
            }
        }

        public void Stop()
        {
            _bStop = true;
        }
    }
}
