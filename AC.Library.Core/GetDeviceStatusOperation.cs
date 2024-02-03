using System.Collections.Generic;
using System.Linq;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Communication;
using AC.Library.Core.Templates;
using Newtonsoft.Json;

namespace AC.Library.Core
{
    public class GetDeviceStatusOperation<T> : DeviceCommunicationTemplate where T : IParameter
    {
        private readonly List<T> _columns;
        
        public GetDeviceStatusOperation(
            IUdpClientWrapper udpClientWrapper,
            Operation operation,
            AirConditionerModel airConditionerModel,
            List<T> columns,
            string broadcastAddress = null)
            : base(udpClientWrapper, operation, airConditionerModel, broadcastAddress)
        {
            _columns = columns;
        }
        
        protected override object CreateRequest()
        {
            return new StatusRequestPack
            {
                Type = "status",
                MAC = _airConditionerModel.Id,
                Columns = _columns.Select(x => x.Value).ToList()
            };
        }

        protected override object ProcessResponseJson(string json)
        {
            return JsonConvert.DeserializeObject<StatusResponsePack>(json);
        }
    }
}

