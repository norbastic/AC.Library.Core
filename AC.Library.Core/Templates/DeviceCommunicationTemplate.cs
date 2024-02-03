using System;
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
        protected AirConditionerModel _airConditionerModel;
        protected IUdpClientWrapper _udpClientWrapper;

        protected DeviceCommunicationTemplate(AirConditionerModel airConditionerModel, IUdpClientWrapper udpClientWrapper)
        {
            _airConditionerModel = airConditionerModel;
            _udpClientWrapper = udpClientWrapper;
        }

        public async Task<object> ExecuteOperationAsync()
        {
            ValidateDevice();
            var request = CreateRequest();
            var encryptedData = EncryptData(request);
            var udpResponse = await SendUdpRequest(encryptedData);
            var json = DecryptResponse(udpResponse);
            return ProcessResponseJson(json);
        }

        private void ValidateDevice()
        {
            if (_airConditionerModel.PrivateKey == null)
                throw new Exception("Device [PrivateKey] is required!");
        }

        private string EncryptData(object request)
        {
            var packJson = JsonConvert.SerializeObject(request);
            return Crypto.EncryptData(packJson, _airConditionerModel.PrivateKey) ?? throw new Exception("Could not encrypt pack json.");
        }

        private async Task<UdpReceiveResult> SendUdpRequest(string encryptedData)
        {
            var request = Request.Create(_airConditionerModel.Id, encryptedData);
            var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(request));
            var udpHandler = new UdpHandler(_udpClientWrapper);
            return (await udpHandler.SendReceiveRequest(bytes, _airConditionerModel.Address)).FirstOrDefault();
        }

        private string DecryptResponse(UdpReceiveResult udpResponse)
        {
            var responseJson = Encoding.ASCII.GetString(udpResponse.Buffer);
            var response = JsonConvert.DeserializeObject<ResponsePackInfo>(responseJson);
            if (response == null)
            {
                return null;
            }
            var decryptedJson = Crypto.DecryptData(response.Pack, _airConditionerModel.PrivateKey);
            return decryptedJson;
        }

        protected abstract object CreateRequest();
        protected abstract object ProcessResponseJson(string json);
    }
}

