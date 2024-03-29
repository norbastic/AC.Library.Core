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

        protected override object CreateRequestPack()
        {
            return new BindRequestPack { MAC = _macAddress };
        }
        
        private Request CreateRequest(string macAddress, string encryptedPack)
        {
            return Request.Create(macAddress, encryptedPack, 1);
        }

        protected override byte[] PrepareRequestForSend(string encryptedData)
        {
            var requestToSend = CreateRequest(_macAddress, encryptedData);
            return Encoding.ASCII.GetBytes(SerializeRequestPack(requestToSend));
        }

        protected override string ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var decryptedData = GetResponsePackFromUdpResponse(udpResponses.FirstOrDefault());
            var bindResponse = JsonConvert.DeserializeObject<BindResponsePack>(decryptedData);
            return bindResponse?.Key;
        }

        public async Task<string> Bind(string macAddress, string ipAddress)
        {
            _macAddress = macAddress;
            return await ExecuteOperation(ipAddress);
        }
    }
}