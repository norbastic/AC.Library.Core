using AC.Library.Core.Models.Protocol;

namespace AC.Library.Core.Models
{
    public class ScannedDevice : DeviceInfoResponsePack
    {
        // Scanned device has an IpAddress
        public string IpAddress { get; set; }
        
        public ScannedDevice() { }
        public ScannedDevice(DeviceInfoResponsePack deviceInfo)
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