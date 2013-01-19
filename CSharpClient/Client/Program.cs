using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;

namespace Client
{
    class FT_Node_Process_Test1 : FT_Node_Process {

        public override FT_MessageTypesNode GetNodeType()
        {
            return FT_MessageTypesNode.NODE_FT_TEST1;
        }

        public override void OnProcess(BitStream bsIn)
        {
            FT_UnitData unitData = new FT_UnitData();
            unitData.Serialize(false, bsIn);
            Log.Debug("FT_Node_Process_Test1.OnProcess :");
        }

    }

    class FT_Node_Pluginx : FT_Node_Plugin{
        public override void PrintLog(string msg)
        {
            Log.Debug(msg);
        }
    }

    class Program
    {
        public static string serverIP = "127.0.0.1";
        public static ushort serverPort = 60000;
        public static string password = "";

        private static XNetWork2 _client2;
        private static RakNetClient client;
        public static FT_Node_Pluginx nodePlugin;
        private static FT_Node_Process_Test1 processTest1;

        static void Main(string[] args)
        {
            _RakNetClient();

            Console.ReadKey();

            client.Dispose();
        }

        static void _RakNetClient (){
            client = new RakNetClient(serverIP, serverPort, ProcessMessage);
            // client = new RakNetClient(serverIP, serverPort, null);
            client.Start();
            nodePlugin = new FT_Node_Pluginx();
            // processTest1 = new FT_Node_Process_Test1();
            // nodePlugin.RegisterProcess(processTest1);
            client.AttachInterface2(nodePlugin);
        }

        static void _Connect2()
        {
            _Init2();
            RakNet.ConnectionAttemptResult result = _client2.Connect(serverIP, serverPort, password);
            Console.WriteLine("ConnectionAttemptResult result = " + result);
        }

        static void _Init2()
        {
            if (_client2 == null)
            {
                _client2 = new XNetWork2();
                _client2._Init();
            }
        }

        static void ProcessMessage(RakPeerInterface peer, RakNet.Packet packet)
        {
            if (packet != null)
            {
                if (packet.data[0] == (byte)(FT_MessageTypes.ID_SERVER_LOGIN))
                {
                    BitStream receiveBitStream = new BitStream();
                    receiveBitStream.Write(packet.data, packet.length);
                    receiveBitStream.IgnoreBytes(2);
                    FT_UnitData data = new FT_UnitData();
                    data.Serialize(false, receiveBitStream);
                    Log.Debug(" data.nGrid_x: " + data.nGrid_x);

                    BitStream writeBitStream = new BitStream();
                    writeBitStream.Write((byte)FT_MessageTypes.ID_SERVER_LOGIN);
                    writeBitStream.Write((byte)FT_MessageTypesNode.NODE_FT_TEST1);
                    data.Serialize(true, writeBitStream);
                    uint sendLength = peer.Send(writeBitStream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, false);
                    Log.Debug("SendLength = " + sendLength);
                }
            }
        }
    }


}
