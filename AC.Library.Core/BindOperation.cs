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
    public class BindOperation : BaseCommunication<string>
    {
        private string _macAddress;
        
        public BindOperation(IUdpClientWrapper udpClientWrapper) : base(udpClientWrapper)
        {
        }

        internal override object CreateRequestPack()
        {
            return new BindRequestPack { MAC = _macAddress };
        }

        internal override string Encrypt(string serializedPack)
        {
            return Crypto.EncryptGenericData(serializedPack);
        }
        
        private Request CreateRequest(string macAddress, string encryptedPack)
        {
            return Request.Create(macAddress, encryptedPack, 1);
        }

        internal override byte[] PrepareRequestForSend(string encryptedData)
        {
            var requestToSend = CreateRequest(_macAddress, encryptedData);
            return Encoding.ASCII.GetBytes(SerializeRequestPack(requestToSend));
        }

        internal override string Decrypt(string stringToDecrypt)
        {
            return Crypto.DecryptGenericData(stringToDecrypt);
        }

        internal override string ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var responseJson = Encoding.ASCII.GetString(udpResponses.FirstOrDefault().Buffer);
            var responsePackInfo = JsonConvert.DeserializeObject<ResponsePackInfo>(responseJson);
            if (!ResponseChecker.IsReponsePackInfoValid(responsePackInfo)) return null;
            var decryptedData = Decrypt(responsePackInfo?.Pack);
            var bindResponse = JsonConvert.DeserializeObject<BindResponsePack>(decryptedData);
            return bindResponse?.Key;
        }

        public async Task<string> Bind(string macAddress, string ipAddress)
        {
            _macAddress = macAddress;
            
            var requestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(requestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpBroadcastRequest(bytesToSend, ipAddress);
            return ProcessUdpResponses(udpResponses);
        }
    }
}