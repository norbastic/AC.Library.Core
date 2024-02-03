using System.Collections.Generic;
using AC.Library.Core.Models;
using AC.Library.Core.Utils;
using Xunit;

namespace AC.Library.Core.Test;

public class DeviceCommunicationTests
{
    [Fact]
    public async void ScanTest()
    {
        var scanOperation = new ScanOperation(new UdpClientWrapper(), Operation.Scan, "192.168.1.255");
        var scanResult = (List<ScannedDevice>) await scanOperation.ExecuteOperationAsync();
        Assert.NotNull(scanResult);
    }
    
    [Fact]
    public async void BindTest()
    {
        var acDevice = new AirConditionerModel
        {
            Id = "f4911ed36c75",
            Address = "192.168.1.148"
        };
        var bindOperation = new BindOperation(new UdpClientWrapper(), Operation.Bind, acDevice);
        var privateKey = (string) await bindOperation.ExecuteOperationAsync();
        Assert.NotNull(privateKey);
    }
}