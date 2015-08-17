using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using TKinect.Data;

namespace TKinect.FrameManagement
{
    public class FrameClient<T> where T : TFrame
    {
        public event EventHandler<T> FrameReady;

        protected ThreadStart ThreadProcessor { get; set; }
        protected TcpClient Client { get; set; }
        protected SynchronizationContext Context { get; set; }

        public FrameClient()
        {
            Context = SynchronizationContext.Current;
        }

        public void Connect(string address, int port)
        {
            Client = new TcpClient();
            Client.BeginConnect(address, port, OnSocketConnectCompleted, null);
        }

        private void OnSocketConnectCompleted(IAsyncResult ar)
        {
            bool connected = Client.Connected;

            if (!connected)
                return;

            var thread = new Thread(ReceiveFrame) { IsBackground = true };
            thread.Start();
        }

        public void Disconnect()
        {
            if (Client != null)
                Client.Close();
        }

        private void ReceiveFrame()
        {
            try
            {
                NetworkStream ns = Client.GetStream();
                BinaryReader reader = new BinaryReader(ns);

                while (Client.Connected)
                {
                    int size = reader.ReadInt32();
                    byte[] data = reader.ReadBytes(size);

                    MemoryStream ms = new MemoryStream(data);
                    BinaryReader br = new BinaryReader(ms);

                    var frame = Activator.CreateInstance<T>();
                    frame.Read(br);

                    Context.Send(delegate
                    {
                        if (FrameReady != null)
                            FrameReady(this, frame);

                    }, null);
                }
            }
            catch (IOException)
            {
                Client.Close();
            }
        }
    }
}
