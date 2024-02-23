using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Utils;

namespace AC.Library.Core.Examples
{
    public class Example
    {
        private AirConditionerDevice _acDevice;
        
        public async Task LocalNetworkDiscovery()
        {
            using (var udpClient = new UdpClientWrapper())
            {
                var deviceScanner = new ScanOperation(udpClient);
                var result = await deviceScanner.Scan("192.168.1.255");
                _acDevice = result.FirstOrDefault();
            }
        }

        public async Task BindingDevice()
        {
            using (var udpClient = new UdpClientWrapper())
            {
                var bindOperation = new BindOperation(udpClient);
                var privateKey = await bindOperation.Bind(_acDevice.ClientId, _acDevice.IpAddress);
                _acDevice.PrivateKey = privateKey;
            }
        }

        public async Task GetStatus()
        {
            var parameterList = new List<IParameter>()
            {
                PowerParam.Power,
                TemperatureParam.Temperature
            };

            using (var udpClient = new UdpClientWrapper())
            {
                var statusOperation = new DeviceStatusOperation(udpClient, _acDevice.ClientId, _acDevice.PrivateKey);
                var status = await statusOperation.GetDeviceStatus(parameterList, _acDevice.IpAddress);

                foreach (var item in status)
                {
                    Console.WriteLine($"{item.Key} : {item.Value}");
                }
            }
        }
        
        public async void SetParameter()
        {
            using (var udpClient = new UdpClientWrapper())
            {
                var setParameterOperation = new SetParameterOperation(udpClient, _acDevice.ClientId, _acDevice.PrivateKey);
                var result = await setParameterOperation.SetParameter(
                    TemperatureParam.Temperature,
                    new TempParameterValue(TemperatureValues._20),
                    _acDevice.IpAddress);
                result = await setParameterOperation.SetParameter(
                    TemperatureParam.Temperature,
                    new TempParameterValue(TemperatureValues._22),
                    _acDevice.IpAddress);
            }
        }
    }
}

