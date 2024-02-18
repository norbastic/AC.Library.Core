using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AC.Library.Core.Communication;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models.Protocol;
using AC.Library.Core.Utils;
using Newtonsoft.Json;


namespace AC.Library.Core
{
    public class DeviceStatusOperation : BaseCommunication<Dictionary<string, int>>
    {
        private readonly string _macAddress;
        private readonly string _privateKey;
        private List<IParameter> _columns;
        
        public DeviceStatusOperation(IUdpClientWrapper udpClientWrapper, string macAddress, string privateKey) : base(udpClientWrapper)
        {
            _macAddress = macAddress;
            _privateKey = privateKey;
        }

        internal override object CreateRequestPack()
        {
            return new StatusRequestPack
            {
                Type = "status",
                MAC = _macAddress,
                Columns = _columns.Select(x => x.Value).ToList()
            };
        }

        internal override string Encrypt(string serializedPack)
        {
            return Crypto.EncryptData(serializedPack, _privateKey);
        }

        internal override byte[] PrepareRequestForSend(object request)
        {
            var requestToSend = Request.Create(_macAddress, (string) request);
            return Encoding.ASCII.GetBytes(SerializeRequestPack(requestToSend));
        }

        internal override string Decrypt(string stringToDecrypt)
        {
            return Crypto.DecryptData(stringToDecrypt, _privateKey);
        }

        internal override Dictionary<string, int> ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var response = udpResponses.FirstOrDefault();
            var decryptedPack = GetResponsePackFromUdpResponse(response);            
            var statusResponsePack = JsonConvert.DeserializeObject<StatusResponsePack>(decryptedPack);
            
            return statusResponsePack.Columns.Zip(statusResponsePack.Data, (key, value) => new { key, value })
                .ToDictionary(item => item.key, item => item.value);
        }

        public async Task<Dictionary<string, int>> GetDeviceStatus(List<IParameter> parameterList, string ipAddress)
        {
            _columns = parameterList;
            var statusRequestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(statusRequestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpBroadcastRequest(bytesToSend, ipAddress);
            return ProcessUdpResponses(udpResponses);
        }
        
    }
}