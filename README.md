# AC.Library.Core
Simple library to manage Gree Air Conditioner devices.

Only tested with Gree G-Tech.(https://global.gree.com/contents/169/290.html)

All credits go to tomikaa87 who made an amazing work. Check out his work: [gree-remote](https://github.com/tomikaa87/gree-remote).

## How to use
### Scanning local network
```CS
    var deviceScanner = new ScanOperation(new UdpClientWrapper());
    // The IP address must be the broadcast address of  local network
    var devices = await deviceScanner.Scan("192.168.1.255");
```

### Binding device
```cs
    // From the scanning let's take the first device
    var device = devices.FirstOrDefault();
    var bindOperation = new BindOperation(new UdpClientWrapper());
    var privateKey = await bindOperation.Bind(device.ClientId, device.IpAddress);
    // We can now add the private key to the device object
    device.PrivateKey = privateKey;
```
PrivateKey and the object should be stored, because the private key is used for the device other device operations.

### Set a parameter
```cs
    // Let's use the previously bound device
    var setParameterOperation = new SetParameterOperation(new UdpClientWrapper(), device.ClientId, device.PrivateKey);

    // Turn on the device
    await setParameterOperation.SetParameter(
        PowerParam.Power,
        new PowerParameterValue(PowerValues.On),
        acDevice.IpAddress
    );

    // Let's set the temperature to 20°C
    var result = await setParameterOperation.SetParameter(
        TemperatureParam.Temperature,
        new TempParameterValue(TemperatureValues._20),
        device.IpAddress
    );
    
```

### Query status
```cs
        var device = scanResult.FirstOrDefault();
        var acDevice = new AirConditionerModel
        {
            Id = device.Id
            Address = device.Address
            PrivateKey = privateKey
        };
        var parameterList = new List<IParameter>()
        {
            PowerParam.Power,
            TemperatureParam.Temperature
        };
        
        var getStatusOperation = new GetDeviceStatusOperation<IParameter>(new UdpClientWrapper(), Operation.GetStatus, acDevice, parameterList);
        var status = (StatusResponsePack) await getStatusOperation.ExecuteOperationAsync();

```
