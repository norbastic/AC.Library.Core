using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AC.Library.Core.Interfaces
{
    public interface IUdpClientWrapper : IDisposable
    {
        Task<int> SendAsync(byte[] datagram, int bytes, string hostname, int port);
        Task<UdpReceiveResult> ReceiveAsync();
        bool EnableBroadcast { get; set; }
        int Available { get; }
        void Close();
    }
}

