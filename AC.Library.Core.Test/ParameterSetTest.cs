using System;
using AC.Library.Core.Models;
using Xunit;

namespace AC.Library.Core.Test;

public class ParameterSetTest
{
    private const string sendCommandResponse =
        "eyJ0IjoicGFjayIsImkiOjAsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJ6dFVIRkdmQ1BnclJCelVtK1BWSzdIa0ZrRW1GYVJsOFZJd3FPMVNjV3l1N05obnNVRFYzYWVyVE5ZbkRFbHlhbmN5YVVMNExqMHJGUkxuZ2JyanBXUWxacG1LcUNrT01PSHlvV3Y2eTAzYz0ifQ==";
    private const string toSend =
        "eyJpIjowLCJ0Y2lkIjoiZjQ5MTFlZDM2Yzc1IiwidWlkIjowLCJwYWNrIjoia0hUemp6RnNGVEpuRmc4amN2SE93MkJTM1R5VE9RRXhKLzZjdm1HUzJSbVd6NHI2dEYwa1BzZ1FIZnhDTGZvbXFzWVV1RUoxeGhPUS9PR1R3M254NVE9PSIsInQiOiJwYWNrIiwiY2lkIjoiYXBwIn0=";
    
    [Fact]
    public async void SendTest()
    {
        var acDevice = new AirConditionerDevice()
        {
            IpAddress = "192.168.1.148",
            FriendlyName = "1ed36c75",
            ClientId = "f4911ed36c75",
            PrivateKey = "4Fg7Ij0Lm3Op6Rs9"
        };
        var udpClientWrapper =
            TestSetup.CreateSendParameterUdpWrapper(
                Convert.FromBase64String(toSend),
                Convert.FromBase64String(sendCommandResponse),
                acDevice.IpAddress);

        var setParameterOperation = new SetParameterOperation(udpClientWrapper, acDevice.ClientId, acDevice.PrivateKey);
        var response = await setParameterOperation.SetParameter(
            TemperatureParam.Temperature,
            new TempParameterValue(TemperatureValues._20),
            acDevice.IpAddress);

        Assert.Equal(TemperatureParam.Temperature.Value, response);
    }
    

}