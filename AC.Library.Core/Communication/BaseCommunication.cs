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

        internal virtual string Encrypt(string serializedPack) => Crypto.EncryptGenericData(serializedPack);

        internal abstract byte[] PrepareRequestForSend(string encryptedData);
        
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

        internal virtual string Decrypt(string stringToDecrypt) => Crypto.DecryptGenericData(stringToDecrypt);

        protected string GetResponsePackFromUdpResponse(UdpReceiveResult udpResponse, string privateKey = null)
        {
            var responsePackJson = Encoding.ASCII.GetString(udpResponse.Buffer);
            var responsePack = JsonConvert.DeserializeObject<ResponsePackInfo>(responsePackJson);
            if (!ResponseChecker.IsReponsePackInfoValid(responsePack))
            {
                return null;
            }
            return Decrypt(responsePack.Pack);
        }

        internal abstract T ProcessUdpResponses(List<UdpReceiveResult> udpResponses);

        protected async Task<T> ExecuteOperation(string ipAddress)
        {
            var requestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(requestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpBroadcastRequest(bytesToSend, ipAddress);
            return ProcessUdpResponses(udpResponses);

            /*
             *             var bindRequestPack = CreateRequestPack();
            var serializedRequestPack = SerializeRequestPack(bindRequestPack);
            var encryptedPack = Encrypt(serializedRequestPack);
            var request = CreateRequest(_macAddress, encryptedPack);
            var toSend = PrepareRequestForSend(request);
            var udpResponses = await SendUdpRequest(toSend, ipAddress);
            return ProcessUdpResponses(udpResponses);
             */
        }
    }
}