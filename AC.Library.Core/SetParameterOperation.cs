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

        protected override object CreateRequestPack()
        {
            return CommandRequestPack.Create(_macAddress, _parameter, _value);
        }

        protected override string Encrypt(string serializedPack) => Crypto.EncryptData(serializedPack, _privateKey);

        protected override byte[] PrepareRequestForSend(string encryptedData)
        {
            var requestToSend = Request.Create(_macAddress, encryptedData);
            return Encoding.ASCII.GetBytes(SerializeRequestPack(requestToSend));
        }

        protected override string Decrypt(string stringToDecrypt) => Crypto.DecryptData(stringToDecrypt, _privateKey);

        protected override string ProcessUdpResponses(List<UdpReceiveResult> udpResponses)
        {
            var response = udpResponses.FirstOrDefault();
            var decryptedPack = GetResponsePackFromUdpResponse(response, _privateKey);
            
            var commandResponse = JsonConvert.DeserializeObject<CommandResponsePack>(decryptedPack);
            return string.Join(";", commandResponse.Columns);
        }

        private async Task<string> SetDeviceParameter(IParameter parameter, IParameterValue value, string ipAddress)
        {
            _parameter = parameter.Value;
            _value = value.Value;
            return await ExecuteOperation(ipAddress);
        }
        
        public async Task<string> SetParameter(PowerParam parameter, PowerParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(ModeParam parameter, ModeParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(TemperatureParam parameter, TempParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(FanSpeedParam parameter, FanSpeedParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(AirModeParam parameter, AirParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(XfanModeParam parameter, SwingHorizontalParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(HealthModeParam parameter, HealthParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(SleepModeParam parameter, SleepParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(LightParam parameter, LightParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(VerticalSwingParam parameter, SwingVerticalParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(QuietModeParam parameter, QuietParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(TurboModeParam parameter, TurboParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(EnergySavingModeParam parameter, PowerSaveParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);

        public async Task<string> SetParameter(TemperatureParam parameter, TempUnitParameterValue value, string ipAddress) => await SetDeviceParameter(parameter, value, ipAddress);
    }
}
