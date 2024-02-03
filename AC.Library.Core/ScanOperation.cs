using System.Collections.Generic;
using System.Linq;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Models.Communication;
using AC.Library.Core.Templates;
using Newtonsoft.Json;

namespace AC.Library.Core
{
    public class ScanOperation : DeviceCommunicationTemplate 
    {
        public ScanOperation(IUdpClientWrapper udpClientWrapper, Operation operation, string broadcastAddress)
            : base(udpClientWrapper, operation, null, broadcastAddress)
        {
        }

        protected override object CreateRequest()
        {
            return Request.CreateScan();
        }

        protected override object ProcessResponseJson(string json)
        {
            var foundUnits = JsonConvert.DeserializeObject<List<DeviceDiscoveryResponse>>(json);
            var scannedDevices = foundUnits.Select(foundUnit =>
            {
                var deviceInfo = JsonConvert.DeserializeObject<DeviceInfoResponsePack>(foundUnit.Json);
                return new ScannedDevice
                {
                    Id = deviceInfo.ClientId,
                    Name = deviceInfo.FriendlyName,
                    Address = foundUnit.Address,
                    Type = deviceInfo.Model
                };
            }).ToList();
            return scannedDevices;
        }
    }
}