using System;
using System.Collections.Generic;
using AC.Library.Core.Interfaces;
using AC.Library.Core.Models;
using Xunit;

namespace AC.Library.Core.Test;

public class ParameterStatusTest
{
    private const string toSend =
        "eyJpIjowLCJ0Y2lkIjoiZjQ5MTFlZDM2Yzc1IiwidWlkIjowLCJwYWNrIjoidzc5NTYzRUVaSUdTQTY0ODBJY0F6Q0xKUWxZcFhFbHBVaVFORFkwOVJBa0EzcGRsaktTR05IU2xCaGlKbTUyYnhwdndlc1cyUWE4Z25FMFhNQ05rMVE9PSIsInQiOiJwYWNrIiwiY2lkIjoiYXBwIn0=";
    private const string toReceive =
        "eyJ0IjoicGFjayIsImkiOjAsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJCWGJxOGFBNVhoWTBaSjJFRE9ndk1Ya0ZrRW1GYVJsOFZJd3FPMVNjV3lzV1JnU0ZNaVpyLzd2T3ZOemQ1SmtQaEE1UUdLL0JhYlNRMjVwS25BRjArczFsQzNCZlhXWFV4L013c084czh0TT0ifQ==";
    
    [Fact]
    public async void GetStatusTest()
    {
        var acDevice = new AirConditionerDevice()
        {
            IpAddress = "192.168.1.148",
            FriendlyName = "1ed36c75",
            ClientId = "f4911ed36c75",
            PrivateKey = "4Fg7Ij0Lm3Op6Rs9"
        };
        
        var toQuery = new List<IParameter>()
        {
            PowerParam.Power,
            TemperatureParam.Temperature
        };
        
        var udpWrapperMock = TestSetup.CreateSendParameterUdpWrapper(
            Convert.FromBase64String(toSend),
            Convert.FromBase64String(toReceive),
            acDevice.IpAddress);
        
        var statusOperation = new DeviceStatusOperation(
            udpWrapperMock,
            acDevice.ClientId,
            acDevice.PrivateKey);
        var result = await statusOperation.GetDeviceStatus(toQuery, acDevice.IpAddress);

        Assert.True(result.ContainsKey(PowerParam.Power.Value));
        Assert.Equal(1, result[PowerParam.Power.Value]);
        Assert.True(result.ContainsKey(TemperatureParam.Temperature.Value));
        Assert.Equal(22, result[TemperatureParam.Temperature.Value]);
    }
}