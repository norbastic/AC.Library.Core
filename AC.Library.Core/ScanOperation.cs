using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AC.Library.Core.Communication;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Protocol;
using Newtonsoft.Json;

namespace AC.Library.Core
{
    public class ScanOperation : BaseCommunication<List<AirConditionerDevice>>
    {
        public ScanOperation(IUdpClientWrapper udpClientWrapper) : base(udpClientWrapper)
        {
        }

        protected override object CreateRequestPack()
        {
            return new ScanRequestPack();
        }

        protected override string Encrypt(string serializedPack)
        {
            return serializedPack;
        }

        protected override byte[] PrepareRequestForSend(string encryptedData)
        {
            return Encoding.ASCII.GetBytes(encryptedData);
        }

        protected override Task<List<UdpReceiveResult>> SendUdpRequest(byte[] bytes, string ipAddress)
        {
            return SendUdpBroadcastRequest(bytes, ipAddress);
        }

        protected override List<AirConditionerDevice> ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var deviceDiscoveryResponses = udpResponses.Select(udpResponse =>
            {
                var responseJson = GetResponsePackFromUdpResponse(udpResponse);
                return new DeviceDiscoveryResponse()
                {
                    Address = udpResponse.RemoteEndPoint.Address.ToString(),
                    Json = responseJson
                };
            }).ToList();
            
            return deviceDiscoveryResponses.Select(deviceDiscoveryResponse =>
                {
                    var deviceInfoResponsePack =
                        JsonConvert.DeserializeObject<DeviceInfoResponsePack>(deviceDiscoveryResponse.Json);
                    return new AirConditionerDevice(deviceInfoResponsePack) { IpAddress = deviceDiscoveryResponse.Address };
                }
                ).ToList();
        }

        public async Task<List<AirConditionerDevice>> Scan(string broadcastAddress)
        {
            return await ExecuteOperation(broadcastAddress);
        }
    }
}