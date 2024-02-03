using System.Linq;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Communication;
using AC.Library.Core.Templates;
using Newtonsoft.Json;

namespace AC.Library.Core
{
    public class SetDeviceParameterOperation<TParam, TValue> : DeviceCommunicationTemplate
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
    }
}

