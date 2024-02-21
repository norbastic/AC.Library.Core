using System.Collections.Generic;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using AC.Library.Core.Utils;
using Xunit;

namespace AC.Library.Core.Test;

public class DeviceCommunicationTests
{
    [Fact]
    public async void ScanTest()
    {
        var deviceScanner = new ScanOperation(new UdpClientWrapper());
        var result = await deviceScanner.Scan("192.168.1.255");
        Assert.True(result.Count > 0);
    }
    
    [Fact]
    public async void BindTest()
    {
        var acDevice = new AirConditionerDevice()
        {
            ClientId = "f4911ed36c75",
            IpAddress = "192.168.1.148"
        };
        var bindOperation = new BindOperation(new UdpClientWrapper());
        var privateKey = await bindOperation.Bind(acDevice.ClientId, acDevice.IpAddress);
        Assert.NotNull(privateKey);
    }
    
    [Fact]
    public async void GetStatusTest()
    {
        var acDevice = new AirConditionerDevice()
        {
            ClientId = "f4911ed36c75",
            IpAddress = "192.168.1.148",
            PrivateKey = "4Fg7Ij0Lm3Op6Rs9"
        };
        var parameterList = new List<IParameter>()
        {
            PowerParam.Power,
            TemperatureParam.Temperature
        };

        var statusOperation = new DeviceStatusOperation(new UdpClientWrapper(), acDevice.ClientId, acDevice.PrivateKey);
        var status = await statusOperation.GetDeviceStatus(parameterList, acDevice.IpAddress);
        Assert.True(status.Keys.Count == 2);
    }
    
    [Fact]
    public async void SetParameterTest()
    {
        var acDevice = new AirConditionerDevice()
        {
            ClientId = "f4911ed36c75",
            IpAddress = "192.168.1.148",
            PrivateKey = "4Fg7Ij0Lm3Op6Rs9"
        };
        
        var setParameterOperation = new SetParameterOperation(new UdpClientWrapper(), acDevice.ClientId, acDevice.PrivateKey);
        var result = await setParameterOperation.SetParameter(TemperatureParam.Temperature, new TempParameterValue(TemperatureValues._20), acDevice.IpAddress);

        Assert.Equal(TemperatureParam.Temperature.Value, result);
    }
}