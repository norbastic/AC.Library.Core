using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AC.Library.Core.Communication;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Protocol;
using AC.Library.Core.Utils;
using Newtonsoft.Json;

namespace AC.Library.Core
{
    public class ScanOperation : BaseCommunication<List<ScannedDevice>>
    {
        public ScanOperation(IUdpClientWrapper udpClientWrapper) : base(udpClientWrapper)
        {
        }

        internal override object CreateRequestPack()
        {
            return new ScanRequestPack();
        }

        internal override string Encrypt(string serializedPack)
        {
            return serializedPack;
        }

        internal override byte[] PrepareRequestForSend(object request)
        {
            return Encoding.ASCII.GetBytes((string) request);
        }

        internal override string Decrypt(string stringToDecrypt)
        {
            return Crypto.DecryptGenericData(stringToDecrypt);
        }

        internal override List<ScannedDevice> ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var deviceDiscoveryResponses = udpResponses.Select(udpResponse =>
            {
                var responseJson = GetResponsePackFromUdpResponse(udpResponse);
                return new DeviceDiscoveryResponse()
                {
                    Address = udpResponse.RemoteEndPoint.ToString(),
                    Json = responseJson
                };
            }).ToList();
            
            return deviceDiscoveryResponses.Select(deviceDiscoveryResponse =>
                {
                    var deviceInfoResponsePack =
                        JsonConvert.DeserializeObject<DeviceInfoResponsePack>(deviceDiscoveryResponse.Json);
                    return new ScannedDevice(deviceInfoResponsePack) { IpAddress = deviceDiscoveryResponse.Address };
                }
                ).ToList();
        }
        
        public async Task<List<ScannedDevice>> Scan(string broadcastAddress)
        {
            var scanRequestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(scanRequestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpBroadcastRequest(bytesToSend, broadcastAddress);
            return ProcessUdpResponses(udpResponses);
        }
    }
}