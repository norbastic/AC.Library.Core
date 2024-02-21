using System;
using System.Linq;
using System.Text;
using Xunit;

namespace AC.Library.Core.Test;

public class BindOperationTest {
    private const string ScanResultBase64 =
        "eyJ0IjoicGFjayIsImkiOjEsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJMUDI0RWswT2FZb2d4czNpUUxqTDRKQmwvWlhoRjBQR2J6eFpTb1hpZExDTWE5bE0yUnFJL0t5dHZKMzJJc0dTWlhyT3IrTWFrVnp6WEhiZ2hQZXlpam5XTXphTFFhYXcxYUZYbEU5azcxTDBjTW04YnNyL3k0Rmt4dW1wUmcxdEtzLzM0eGhCdU1TeFhmTmZ2RWdTNTZnb3NlV2NVYWFTdWVCdFNPZDBjN0tEaDRNVEtZd1QxQndOak4yaXIrMGVuS1lidDBpSURzZHA4L2Z0WGxBOUh2eHd3aUNJelN5MWIzWi9QaVFrN0JlODBncTlIeEs4TG9hOFdYVmpnWmNQNFZmNU1qS3hhNjBYdDVKMW9JK2xzeFV1WFRIa2d1bkxnNzZXV0d5K2V1bz0ifQ==";

    private const string BindResultBase64 =
        "eyJ0IjoicGFjayIsImkiOjEsInVpZCI6MCwiY2lkIjoiZjQ5MTFlZDM2Yzc1IiwidGNpZCI6IiIsInBhY2siOiJUMnRHdTlKVHNaUExNaG9QTy9tQmNrcHVXbnV2ejhMUUJ5TzZ3SitBaUFyblVEbENDREhpbFNUSHFTbG5qRkljc3NQcGk4WXFteGFoVkZ3bjNzS2xxWXpEMWs2amp4aXdFMnJNclRnN1huaz0ifQ==";

    private const string BindSend = "eyJ0Ijoic2NhbiJ9";
    
    private const string BindCommand =
        "{\"i\":1,\"tcid\":\"f4911ed36c75\",\"uid\":0,\"pack\":\"KMAcSuiBACBDHRReu/TdXlOGAWG3fyMJNBdv22JNXkBpOma5GRS/34RKDj8oZv\\u002Bt\",\"t\":\"pack\",\"cid\":\"app\"}";
    private const string TargetIp = "192.168.1.148";
    
    [Fact]
    public async void BindSuccessful()
    {
        var mockUdp = TestSetup.CreateBroadcastMock(Convert.FromBase64String(ScanResultBase64), TargetIp);
        var bytesToSend = Convert.FromBase64String(BindSend);
        var bytesToReceive = Convert.FromBase64String(BindResultBase64);
        var mockBindUdp = TestSetup.CreateBindUdpMock(bytesToSend, bytesToReceive, TargetIp);
    
        var scanner = new ScanOperation(mockUdp);
        var acDevices = await scanner.Scan("192.168.1.255");
        Assert.True(acDevices.Count > 0);
        
        var toBind = acDevices.FirstOrDefault();
        var binder = new BindOperation(mockBindUdp);
        var receivedKey = await binder.Bind(toBind?.ClientId, TargetIp);
        Assert.True(!string.IsNullOrEmpty(receivedKey));
    }
    
}