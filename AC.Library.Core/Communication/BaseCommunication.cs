using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models.Protocol;
using AC.Library.Core.Utils;
using Newtonsoft.Json;

namespace AC.Library.Core.Communication
{
    public abstract class BaseCommunication<T>
    {
        protected readonly IUdpClientWrapper _udpClientWrapper;

        protected BaseCommunication(IUdpClientWrapper udpClientWrapper)
        {
            _udpClientWrapper = udpClientWrapper ?? throw new ArgumentNullException(nameof(udpClientWrapper));
        }

        internal abstract object CreateRequestPack();

        protected string SerializeRequestPack(object requestPack)
        {
            return JsonConvert.SerializeObject(requestPack);
        }

        internal virtual string Encrypt(string serializedPack, string privateKey = null)
        {
            return Crypto.EncryptData(serializedPack, privateKey);
        }

        internal abstract byte[] PrepareRequestForSend(object request);
        
        protected async Task<List<UdpReceiveResult>> SendUdpBroadcastRequest(byte[] bytes, string broadcastAddress)
        {
            var udpHandler = new UdpHandler(_udpClientWrapper);
            return await udpHandler.SendReceiveBroadcastRequest(bytes, broadcastAddress);
        }
        
        protected async Task<List<UdpReceiveResult>> SendUdpRequest(byte[] bytes, string ipAddress)
        {
            var udpHandler = new UdpHandler(_udpClientWrapper);
            return await udpHandler.SendReceiveBroadcastRequest(bytes, ipAddress);
        }

        internal virtual string Decrypt(string stringToDecrypt, string privateKey = null) => Crypto.DecryptData(stringToDecrypt, privateKey);

        protected string GetResponsePackFromUdpResponse(UdpReceiveResult udpResponse, string privateKey = null)
        {
            var responsePackJson = Encoding.ASCII.GetString(udpResponse.Buffer);
            var responsePack = JsonConvert.DeserializeObject<ResponsePackInfo>(responsePackJson);
            if (!ResponseChecker.IsReponsePackInfoValid(responsePack))
            {
                return null;
            }
            return Decrypt(responsePack.Pack, privateKey);
        }

        internal abstract T ProcessUdpResponses(List<UdpReceiveResult> udpResponses);
    }
}