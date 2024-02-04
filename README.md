# AC.Library.Core
Simple library to manage Gree Air Conditioner devices.

Only tested with Gree G-Tech.(https://global.gree.com/contents/169/290.html)

All credits go to tomikaa87 who made an amazing work. Check out his work: [gree-remote](https://github.com/tomikaa87/gree-remote).

## How to use
### Scanning local network
```CS
    var scanOperation = new ScanOperation(new UdpClientWrapper(), Operation.Scan, "192.168.1.255");
    var scanResult = (List<ScannedDevice>) await scanOperation.ExecuteOperationAsync();
```

### Binding device
```cs
    var device = scanResult.FirstOrDefault();
    var acDevice = new AirConditionerModel
    {
        Id = device.Id
        Address = device.Address
    };
    var bindOperation = new BindOperation(new UdpClientWrapper(), Operation.Bind, acDevice);
    var privateKey = (string) await bindOperation.ExecuteOperationAsync();
```
PrivateKey should be stored.

### Set a parameter
```cs
    var device = scanResult.FirstOrDefault();
    var acDevice = new AirConditionerModel
    {
        Id = device.Id
        Address = device.Address
        PrivateKey = privateKey
    };
    
    var setParameterOperation = new SetDeviceParameterOperation<IParameter, IParameterValue>(
        new UdpClientWrapper(),
        Operation.SetParameter,
        acDevice,
        TemperatureParam.Temperature,
        new TempParameterValue(TemperatureValues._20));
    
    var changedParam = (string) await setParameterOperation.ExecuteOperationAsync();
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
