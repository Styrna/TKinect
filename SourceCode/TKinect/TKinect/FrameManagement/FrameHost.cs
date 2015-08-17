using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TKinect.Data;

namespace TKinect.FrameManagement
{
    public class FrameHost<T> where T : TFrame
    {
        private int _port;
        private List<SocketClient> _clientList;
        private MemoryStream _memoryStream;
        private BinaryWriter _binaryWriter;

        //TODO Try UDP Client
        protected TcpListener Listener { get; set; }

        public FrameHost()
        {
            _clientList = new List<SocketClient>();

            _memoryStream = new MemoryStream();
            _binaryWriter = new BinaryWriter(_memoryStream);
        }

        public void Start(int port)
        {
            _port = port;
            _clientList.Clear();

            Listener = new TcpListener(IPAddress.Any, _port);
            Listener.Start(10);
            Listener.BeginAcceptTcpClient(OnConnection, null);
        }

        private void OnConnection(IAsyncResult ar)
        {
            TcpClient client = Listener.EndAcceptTcpClient(ar);
            var sc = new SocketClient(client);
            lock (_clientList)
            {
                _clientList.Add(sc);
            }
            Listener.BeginAcceptTcpClient(OnConnection, null);
        }

        public void Stop()
        {
            foreach (SocketClient client in _clientList)
                client.Close();
        }

        protected void RemoveClients()
        {
            lock (_clientList)
            {
                for (int i = 0; i < _clientList.Count; i++)
                {
                    if (!_clientList[i].IsConnected)
                        _clientList.Remove(_clientList[i]);
                }
            }
        }

        public void SendFrame(object sender, T frame)
        {
            if (!_clientList.Any()) return;

            _memoryStream.SetLength(0);
            frame.Write(_binaryWriter);


            Parallel.For(0, _clientList.Count, index =>
            {
                SocketClient sc = _clientList[index];
                byte[] data = _memoryStream.ToArray();

                sc.Send(BitConverter.GetBytes(data.Length));
                sc.Send(data);
            });

            RemoveClients();
        }
    }
}
