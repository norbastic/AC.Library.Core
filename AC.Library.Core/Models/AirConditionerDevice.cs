using AC.Library.Core.Models.Protocol;

namespace AC.Library.Core.Models
{
    public class AirConditionerDevice : DeviceInfoResponsePack
    {
        public string IpAddress { get; set; }
        public string PrivateKey { get; set; }

        public AirConditionerDevice() { }
        public AirConditionerDevice(DeviceInfoResponsePack deviceInfo)
        {
            BrandCode = deviceInfo.BrandCode;
            Brand = deviceInfo.Brand;
            Catalog = deviceInfo.Catalog;
            ClientId = deviceInfo.ClientId;
            ModelId = deviceInfo.ModelId;
            Model = deviceInfo.Model;
            FriendlyName = deviceInfo.FriendlyName;
            Series = deviceInfo.Series;
            Vendor = deviceInfo.Vendor;
            FirmwareVersion = deviceInfo.FirmwareVersion;
            LockState = deviceInfo.LockState;
        }
    }
}