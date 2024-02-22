# AC.Library.Core
Simple library to manage Gree Air Conditioner devices.

Only tested with Gree G-Tech.(https://global.gree.com/contents/169/290.html)

All credits go to tomikaa87 who made an amazing work. Check out his work: [gree-remote](https://github.com/tomikaa87/gree-remote).

## How to use
### Scanning local network

Local discovery sends a special UDP package to the broadcast address and the available devices on the network reply with device related information.
Eg.: brand, mac address, name, etc.

```CS
    var deviceScanner = new ScanOperation(new UdpClientWrapper());
    // The IP address must be the broadcast address of  local network
    var devices = await deviceScanner.Scan("192.168.1.255");
```

### Binding device

When we want to communicate with an AC, first we have to bind the device to the actual computer.
After a successful bind, the device will return with a private key. This private key must be used for the communication, so it's a good idea to store in a secure way.

```cs
    // From the scanning let's take the first device
    var device = devices.FirstOrDefault();
    var bindOperation = new BindOperation(new UdpClientWrapper());
    var privateKey = await bindOperation.Bind(device.ClientId, device.IpAddress);
    // We can now add the private key to the device object
    device.PrivateKey = privateKey;
```

### Set a parameter

Setting a parameter or more can be done the following way:

- Creating an instance of **SetParameterOperation** :

```cs
    // Let's use the previously bound device
    var setParameterOperation = new SetParameterOperation(
        new UdpClientWrapper(),
        device.ClientId,
        device.PrivateKey
    );
```

- Then call the SetParameter with the correct parameter and value:

```cs
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

It is also possible to query the actual value of a parameter or parameters.

- First create an instance of **DeviceStatusOperation** with the correct parameters:

```cs
    var statusOperation = new DeviceStatusOperation(
        new UdpClientWrapper(),
        device.ClientId,
        device.PrivateKey
    );
```

- Create a list of parameters and call the **GetDeviceStatus** method:

```cs
    var parameterList = new List<IParameter>()
    {
        PowerParam.Power,
        TemperatureParam.Temperature
    };

   var status = await statusOperation.GetDeviceStatus(parameterList, device.IpAddress);

```
