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

        protected abstract object CreateRequestPack();

        protected string SerializeRequestPack(object requestPack)
        {
            return JsonConvert.SerializeObject(requestPack);
        }

        protected virtual string Encrypt(string serializedPack) => Crypto.EncryptGenericData(serializedPack);

        protected abstract byte[] PrepareRequestForSend(string encryptedData);
        
        protected virtual string Decrypt(string stringToDecrypt) => Crypto.DecryptGenericData(stringToDecrypt);

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

        protected async Task<List<UdpReceiveResult>> SendUdpBroadcastRequest(byte[] bytes, string broadcastAddress)
        {
            var udpHandler = new UdpHandler(_udpClientWrapper);
            return await udpHandler.SendReceiveBroadcastRequest(bytes, broadcastAddress);
        }

        protected virtual async Task<List<UdpReceiveResult>> SendUdpRequest(byte[] bytes, string ipAddress)
        {
            var udpHandler = new UdpHandler(_udpClientWrapper);
            return await udpHandler.SendReceiveRequest(bytes, ipAddress);
        }

        protected abstract T ProcessUdpResponses(List<UdpReceiveResult> udpResponses);

        protected async Task<T> ExecuteOperation(string ipAddress)
        {
            var requestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(requestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpRequest(bytesToSend, ipAddress);
            return ProcessUdpResponses(udpResponses);
        }
    }
}