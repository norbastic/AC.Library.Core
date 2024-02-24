# AC.Library.Core
AC.Library.Core is a streamlined library designed for managing Gree Air Conditioner devices. This library has been specifically tested with the Gree G-Tech model. For more information on this model, please visit [Gree Global](https://global.gree.com/contents/169/290.html).

We extend our gratitude to tomikaa87 for his exceptional contributions, from which this project draws inspiration and methodology. To explore his innovative work, which served as a foundational resource for our development, visit [gree-remote](https://github.com/tomikaa87/gree-remote).

## Usage Instructions
### Local network discovery

This functionality initiates a local network discovery by dispatching a specialized UDP packet to the broadcast address. Devices within the network respond with pertinent information such as brand, MAC address, and device name, among others.

```CS
    using var udpWrapper = new UdpClientWrapper();    
    var deviceScanner = new ScanOperation(udpWrapper);
    // The IP address must be the broadcast address of  local network
    var devices = await deviceScanner.Scan("192.168.1.255");
```

### Binding device

To communicate with an air conditioner (AC), the device must first be bound to the computer. Successful binding yields a private key essential for subsequent communications. It is advisable to store this key securely.

```cs
    // From the scanning results let's take the first one
    var device = devices.FirstOrDefault();

    using var udpWrapper = new UdpClientWrapper();
    var bindOperation = new BindOperation(udpWrapper);
    var privateKey = await bindOperation.Bind(device.ClientId, device.IpAddress);
    // We can now add the private key to the device object
    device.PrivateKey = privateKey;    
```

### Set a parameter

To modify a device parameter, follow these steps:

Instantiate a **SetParameterOperation**.
When creating an instance of `UdpClientWrapper`, it is crucial to employ the `using` statement as shown below. This approach ensures that the `udpWrapper` object is properly disposed of once it is no longer in use, thereby managing resources efficiently and preventing potential memory leaks.

```cs
    // Let's use the previously bound device
    using var udpWrapper = new UdpClientWrapper(); 
    var setParameterOperation = new SetParameterOperation(
        udpWrapper,
        device.ClientId,
        device.PrivateKey
    );
```

Execute the **SetParameter** method with the desired parameter and value:

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

You can also retrieve the current settings of one or more parameters:

Begin by creating a **DeviceStatusOperation** instance with the necessary parameters:

```cs
    using var udpWrapper = new UdpClientWrapper(); 
    var statusOperation = new DeviceStatusOperation(
        udpWrapper,
        device.ClientId,
        device.PrivateKey
    );
```

Formulate a list of parameters and invoke the **GetDeviceStatus** method:

```cs
    var parameterList = new List<IParameter>()
    {
        PowerParam.Power,
        TemperatureParam.Temperature
    };

   var status = await statusOperation.GetDeviceStatus(parameterList, device.IpAddress);
```
This method returns a `Dictionary<string, int>`, where each key-value pair represents a parameter and its corresponding value. The `string` key denotes the name of the queried parameter, and the `int` value signifies the parameter's actual value. This structure allows for efficient access to the status of multiple device parameters in a type-safe manner.

## Examples
For practical implementation examples and to better understand how to utilize the features of this library, please refer to the example code provided in the repository at `.\AC.Library.Core\Examples`. This directory contains sample application and usage scenarios that demonstrate the library's capabilities in real-world contexts.