using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScanApp
{
    //[TestFixture]
    //public class GetNextIpAddressTest
    //{
    //    [Test]
    //    public void ShouldGetNextIp()
    //    {
    //        Assert.AreEqual("0.0.0.1", GetNextIpAddress("0.0.0.0", 1));
    //        Assert.AreEqual("0.0.1.0", GetNextIpAddress("0.0.0.255", 1));
    //        Assert.AreEqual("0.0.0.11", GetNextIpAddress("0.0.0.1", 10));
    //        Assert.AreEqual("123.14.1.101", GetNextIpAddress("123.14.1.100", 1));
    //        Assert.AreEqual("0.0.0.0", GetNextIpAddress("255.255.255.255", 1));
    //    }

    //    private static string GetNextIpAddress(string ipAddress, uint increment)
    //    {
    //        byte[] addressBytes = IPAddress.Parse(ipAddress).GetAddressBytes().Reverse().ToArray();
    //        uint ipAsUint = BitConverter.ToUInt32(addressBytes, 0);
    //        var nextAddress = BitConverter.GetBytes(ipAsUint + increment);
    //        return String.Join(".", nextAddress.Reverse());
    //    }
    //}
}
