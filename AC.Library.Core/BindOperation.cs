using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Communication;
using AC.Library.Core.Templates;
using Newtonsoft.Json;

namespace AC.Library.Core
{
    public class BindOperation : DeviceCommunicationTemplate
    {
        public BindOperation(IUdpClientWrapper udpClientWrapper, Operation operation, AirConditionerModel airConditionerModel = null, string broadcastAddress = null)
            : base(udpClientWrapper, operation, airConditionerModel, broadcastAddress)
        {
        }
        
        protected override object CreateRequest()
        {
            return new BindRequestPack() { MAC = _airConditionerModel.Id };
        }

        protected override object ProcessResponseJson(string json)
        {
            var bindResponse = JsonConvert.DeserializeObject<BindResponsePack>(json);
            return bindResponse?.Key;
        }
    }
}