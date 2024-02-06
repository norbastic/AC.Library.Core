using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Communication;
using AC.Library.Core.Utils;
using Newtonsoft.Json;

namespace AC.Library.Core.Templates
{
    public abstract class DeviceCommunicationTemplate
    {
        protected readonly IUdpClientWrapper _udpClientWrapper;
        protected readonly AirConditionerModel _airConditionerModel;
        protected readonly string _broadcastAddress;

        protected DeviceCommunicationTemplate(
            IUdpClientWrapper udpClientWrapper,
            AirConditionerModel airConditionerModel = null,
            string broadcastAddress = null)
        {
            _udpClientWrapper = udpClientWrapper ?? throw new ArgumentNullException(nameof(udpClientWrapper));
            _airConditionerModel = airConditionerModel;
            _broadcastAddress = broadcastAddress;
        }

        public async Task<object> ExecuteOperationAsync()
        {
            // ValidatePrivateKey();
            var request = CreateRequest();
            //var encryptedData = EncryptUsingPrivateKey(request);
            var udpResponses = await SendUdpRequest(encryptedData);
            var json = DecryptResponse(udpResponses);
            return ProcessResponseJson(json);
        }

        public async Task PerformScan()
        {

        }

        private void ValidatePrivateKey(string privateKey)
        {
            if(string.IsNullOrEmpty(privateKey))
            {
                throw new InvalidOperationException("Device [PrivateKey] is required for GetStatus or SetParameter operations.");
            }   
        }

        protected string EncryptUsingGenericKey<T>(T request)
        {
            var packJson = JsonConvert.SerializeObject(request);
            return Crypto.EncryptGenericData(packJson) ?? throw new InvalidOperationException("Could not encrypt generic data.");
        }

        private string EncryptUsingPrivateKey<T>(T request, string privateKey)
        {
            var packJson = JsonConvert.SerializeObject(request);
            return Crypto.EncryptData(packJson, privateKey) ?? throw new InvalidOperationException("Could not encrypt pack json.");
        }

        private async Task<List<UdpReceiveResult>> SendBroadcastRequest(Request request, string broadcastAddress)
        {
            var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(request));
            var udpHandler = new UdpHandler(_udpClientWrapper);
            return await udpHandler.SendReceiveBroadcastRequest(bytes, broadcastAddress);
        }

        private async Task<List<UdpReceiveResult>> SendUdpRequest(string encryptedData)
        {
            Request request;
            switch (_operation)
            {
                case Operation.Scan:
                    request = Request.CreateScan();
                    break;
                case Operation.Bind:
                    request = Request.Create(_airConditionerModel.Id, encryptedData, 1);
                    break;
                default:
                    request = Request.Create(_airConditionerModel.Id, encryptedData);
                    break;
            }


            return _operation == Operation.Scan
                ? 
                : await udpHandler.SendReceiveRequest(bytes, _airConditionerModel.Address);
        }

        private string DecryptResponse(List<UdpReceiveResult> udpResponses)
        {
            if (_operation == Operation.Scan)
                return DecryptScanResponses(udpResponses);

            var udpResponse = udpResponses.FirstOrDefault();
            if (udpResponse == null) return null;

            var responseJson = Encoding.ASCII.GetString(udpResponse.Buffer);
            var response = JsonConvert.DeserializeObject<ResponsePackInfo>(responseJson);
            return _operation == Operation.Bind ?
                Crypto.DecryptGenericData(response.Pack) :
                Crypto.DecryptData(response.Pack, _airConditionerModel.PrivateKey);
        }

        private string DecryptScanResponses(List<UdpReceiveResult> udpResponses)
        {
            var decryptedResponses = udpResponses.Select(udpResponse =>
            {
                var responsePackJson = Encoding.ASCII.GetString(udpResponse.Buffer);
                var responsePack = JsonConvert.DeserializeObject<ResponsePackInfo>(responsePackJson);
                var decryptedData = Crypto.DecryptGenericData(responsePack.Pack);
                
                return new DeviceDiscoveryResponse
                {
                    Address = udpResponse.RemoteEndPoint.Address.ToString(),
                    Json = decryptedData
                };
            }).ToList();
            
            return JsonConvert.SerializeObject(decryptedResponses);
        }

        protected abstract object CreateRequest();
        protected abstract object ProcessResponseJson(string json);
    }
}

