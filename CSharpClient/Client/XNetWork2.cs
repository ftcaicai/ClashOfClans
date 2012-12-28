using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Client
{
    public class XNetWork2
    {

        internal class XConnectProcessResultHandler : RakNet.FT_ConnectProcessResultHandler
        {

            public override void OnConnectedToServer()
            {
                Console.WriteLine("OnConnectedToServer");
            }

            public override void OnDisconnectedFromServer()
            {
                Console.WriteLine("OnDisconnectedFromServer");
            }

            public override void OnFailedToConnect()
            {
                Console.WriteLine("OnFailedToConnect");
            }

            public override void OnLostConnection()
            {
                Console.WriteLine("OnLostConnection");
            }

            public override void ReceiveLog()
            {
                Console.WriteLine("ReceiveLog");
            }

            public override void DebugReceive(int flag)
            {
                Console.WriteLine("DebugReceive");
            }

        }

        private RakNet.RakPeerInterface _peer;
        private RakNet.SocketDescriptor _socketDes;
        private string _ServerIP;
        private ushort _ServerPort;
        private string _ConnectPassword;
        private RakNet.FT_ConnectProcess _process;
        private RakNet.FT_ConnectProcessResultHandler _resultHandler;
        private bool _bHasConnectSuccess;
        private Thread _threadRead;
        private RakNet.Packet _packet;

        internal virtual void _Init()
        {
            if (_peer == null)
            {
                _peer = RakNet.RakPeerInterface.GetInstance();
                _socketDes = new RakNet.SocketDescriptor();
                _peer.Startup(1, _socketDes, 1);

                _process = new RakNet.FT_ConnectProcess();
                _resultHandler = new XConnectProcessResultHandler();
                _process.SetResultHandler(_resultHandler);

                _peer.AttachPlugin(_process);

                _threadRead = new Thread(new ThreadStart(Read));
                _threadRead.IsBackground = true;
                _threadRead.Start();
            }
        }

        private void Read()
        {
            while (true)
            {
                _packet = _peer.Receive();
                while (_packet != null)
                {
                    _peer.DeallocatePacket(_packet);
                    _packet = _peer.Receive();
                }
                Thread.Sleep(10);
            }
        }

        public RakNet.ConnectionAttemptResult Connect(string ip, ushort remotePort, string password)
        {
            _ServerIP = ip;
            _ServerPort = remotePort;
            _ConnectPassword = password;

            RakNet.ConnectionAttemptResult result = _peer.Connect(_ServerIP, _ServerPort, _ConnectPassword, _ConnectPassword.Length);
            if (result == RakNet.ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED)
            {
                _bHasConnectSuccess = true;
            }
            return result;
        }

        public void Stop()
        {
            if (_peer != null && _bHasConnectSuccess)
            {
                _peer.Shutdown(300);
            }
        }
    }
}
