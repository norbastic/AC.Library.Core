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
        
        private async Task<string> ExecuteOperation(IParameter parameter, IParameterValue value, string ipAddress)
        {
            _parameter = parameter.Value;
            _value = value.Value;
            var statusRequestPack = CreateRequestPack();
            var packJson = SerializeRequestPack(statusRequestPack);
            var encryptedData = Encrypt(packJson);
            var bytesToSend = PrepareRequestForSend(encryptedData);
            var udpResponses = await SendUdpBroadcastRequest(bytesToSend, ipAddress);
            return ProcessUdpResponses(udpResponses);
        }
        
        public async Task<string> SetParameter(PowerParam parameter, PowerParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(ModeParam parameter, ModeParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(TemperatureParam parameter, TempParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(FanSpeedParam parameter, FanSpeedParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(AirModeParam parameter, AirParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(XfanModeParam parameter, SwingHorizontalParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(HealthModeParam parameter, HealthParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(SleepModeParam parameter, SleepParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(LightParam parameter, LightParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(VerticalSwingParam parameter, SwingVerticalParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(QuietModeParam parameter, QuietParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(TurboModeParam parameter, TurboParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(EnergySavingModeParam parameter, PowerSaveParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);

        public async Task<string> SetParameter(TemperatureParam parameter, TempUnitParameterValue value, string ipAddress) => await ExecuteOperation(parameter, value, ipAddress);
    }
}
