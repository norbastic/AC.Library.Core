using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using AC.Library.Core.Interfaces;

namespace AC.Library.Core.Utils
{
    public class UdpHandler
    {
        private readonly IUdpClientWrapper _udpClientWrapper;

        public UdpHandler(IUdpClientWrapper udpClientWrapper)
        {
            _udpClientWrapper = udpClientWrapper;
        }

        private bool SkipTheResponse(UdpReceiveResult udpReceiveResult, string ipAddress)
        {
            if (_udpClientWrapper.EnableBroadcast)
            {
                return false;
            }
            
            return !udpReceiveResult.RemoteEndPoint.Address.ToString().Equals(ipAddress);
        }

        private async Task<List<UdpReceiveResult>> WaitForUdpResponse(IUdpClientWrapper udp, string ipAddress, int timeOut)
        {
            var responses = new List<UdpReceiveResult>();
            var loopMax = timeOut / 100;

            for (int i = 0; i < loopMax; i++)
            {
                if (udp.Available > 0)
                {
                    var response = await udp.ReceiveAsync();
                    if (SkipTheResponse(response, ipAddress))
                    {
                        continue;
                    }
                    
                    responses.Add(response);
                }
                
                await Task.Delay(100);
            }

            return responses;
        }
        
        public async Task<List<UdpReceiveResult>> SendReceiveBroadcastRequest(byte[] bytes, string ipAddress, int timeOut = 2000)
        {
            _udpClientWrapper.EnableBroadcast = true;
            await _udpClientWrapper.SendAsync(bytes, bytes.Length, ipAddress, 7000);
            
            return await WaitForUdpResponse(_udpClientWrapper, ipAddress, timeOut);
        }
        
        public async Task<List<UdpReceiveResult>> SendReceiveRequest(byte[] bytes, string ipAddress, int timeOut = 2000)
        {
            var sent = await _udpClientWrapper.SendAsync(bytes, bytes.Length, ipAddress, 7000);
            if (sent != bytes.Length)
            {
                throw new Exception("UDP request could not be sent.");
            }
            
            return await WaitForUdpResponse(_udpClientWrapper, ipAddress, timeOut);
        }
    }
}

