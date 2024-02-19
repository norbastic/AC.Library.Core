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
    public class SetParameterOperation : BaseCommunication<string>
    {
        private readonly string _macAddress;
        private readonly string _privateKey;
        private string _parameter;
        private int _value;
        
        public SetParameterOperation(IUdpClientWrapper udpClientWrapper, string macAddress, string privateKey) : base(udpClientWrapper)
        {
            _macAddress = macAddress;
            _privateKey = privateKey;
        }

        internal override object CreateRequestPack()
        {
            return CommandRequestPack.Create(_macAddress, _parameter, _value);
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

        internal override string ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var response = udpResponses.FirstOrDefault();
            var decryptedPack = GetResponsePackFromUdpResponse(response);
            
            var commandResponse = JsonConvert.DeserializeObject<CommandResponsePack>(decryptedPack);
            return string.Join(";", commandResponse.Columns);
        }
        
        private async Task<string> SetParameter(string parameter, int value, string ipAddress)
        {
            var statusRequestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(statusRequestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpBroadcastRequest(bytesToSend, ipAddress);
            return ProcessUdpResponses(udpResponses);
        }
        
        public async Task<string> SetParameter(PowerParam parameter, PowerParameterValue value, string ipAddress)
        {
            _parameter = parameter.Value;
            _value = value.Value;
            
            return await SetParameter(parameter.Value, value.Value, ipAddress);
        }
        
        public async Task<string> SetParameter(TemperatureParam parameter, TempParameterValue value, string ipAddress)
        {
            _parameter = parameter.Value;
            _value = value.Value;

            return await SetParameter(parameter.Value, value.Value, ipAddress);
        }
    }
    
    /*
    public class SetDeviceParameterOperation<TParam, TValue> : DeviceCommunication
        where TParam : IParameter
        where TValue : IParameterValue
    {
        private TParam _param;
        private TValue _value;
        
        public SetDeviceParameterOperation(IUdpClientWrapper udpClientWrapper, Operation operation, AirConditionerModel airConditionerModel, TParam param, TValue value, string broadcastAddress = null) : base(udpClientWrapper, operation, airConditionerModel, broadcastAddress)
        {
            _param = param;
            _value = value;
        }

        protected override object CreateRequest()
        {
            return CommandRequestPack.Create(_airConditionerModel.Id, _param.Value, _value.Value);
        }

        protected override object ProcessResponseJson(string json)
        {
            var setParameterResponse = JsonConvert.DeserializeObject<CommandResponsePack>(json);
            if (setParameterResponse == null) {
                return null;
            }

            return _param.Value.Equals(setParameterResponse.Columns.First()) ? setParameterResponse.Columns.First() : null;
        }
    }*/
}
