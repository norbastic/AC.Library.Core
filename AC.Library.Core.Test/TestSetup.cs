using System.Net;
using System.Net.Sockets;
using AC.Library.Core.Interfaces;
using Moq;

namespace AC.Library.Core.Test;

public class TestSetup {
    public static IUdpClientWrapper CreateBroadcastMock(byte[] toReceive, string ipAddress)
    {
        var mock = new Mock<IUdpClientWrapper>();
        mock.SetupSequence(x => x.Available)
            .Returns(1)
            .Returns(0);
        mock.Setup(x => x.EnableBroadcast)
            .Returns(true);
        mock.Setup(x => x.ReceiveAsync())
            .ReturnsAsync(new UdpReceiveResult(
                toReceive,
                IPEndPoint.Parse($"{ipAddress}:7000"))
            );
        return mock.Object;
    }

    public static IUdpClientWrapper CreateBindUdpMock(byte[] toSend, byte[] toReceive, string ipAddress)
    {
        var mockBindUdp = new Mock<IUdpClientWrapper>();
        mockBindUdp.SetupSequence(x => x.Available)
            .Returns(1)
            .Returns(0);
        mockBindUdp.Setup(x => x.EnableBroadcast)
            .Returns(false);
        mockBindUdp.Setup(x => x.SendAsync(toSend, toSend.Length, ipAddress, 7000))
            .ReturnsAsync(toSend.Length);
        mockBindUdp.Setup(x => x.ReceiveAsync())
            .ReturnsAsync(new UdpReceiveResult(
                toReceive,
                IPEndPoint.Parse($"{ipAddress}:7000"))
            );
        return mockBindUdp.Object;
    }

    public static IUdpClientWrapper CreateSendParameterUdpWrapper(byte[] toSend, byte[] toReceive, string ipAddress)
    {
        var mockBindUdp = new Mock<IUdpClientWrapper>();
        mockBindUdp.Setup(x => x.EnableBroadcast)
            .Returns(false);
        mockBindUdp.Setup(x => x.ReceiveAsync())
            .ReturnsAsync(new UdpReceiveResult(
                toReceive,
                IPEndPoint.Parse($"{ipAddress}:7000"))
            );
        mockBindUdp.Setup(x => x.SendAsync(toSend, toSend.Length, ipAddress, 7000))
            .ReturnsAsync(toSend.Length);
        mockBindUdp.SetupSequence(x => x.Available)
            .Returns(1)
            .Returns(0);
    
        return mockBindUdp.Object;
    }
    
}