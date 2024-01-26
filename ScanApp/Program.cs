// See https://aka.ms/new-console-template for more information

//186.147.241.108
// http://37.228.239.91:80
// lastone 15 ene 2024 - 186.148.207.96:80
//while (ip.CompareTo(ipAddressEndAsString) == -1)

using RestSharp;
using System.Diagnostics;
using System.Net;

Console.WriteLine("Started!");

var ip = IPAddress.Parse("186.147.241.153");
var port = 8080;

while (true)
{
    string url = $"http://{ip}:{port}";

    Scan(url);

    ip = ip.Next();
}

void Scan(string url)
{
    try
    {
        var options = new RestClientOptions(url)
        {
            ThrowOnAnyError = true,
            MaxTimeout = 1000,
            FollowRedirects = false //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
        };

        var client = new RestClient(options);
        var request = new RestRequest("/home", Method.Get);

        var response = client.Execute(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine($"URL: {url} StatusCode: OK");
            Process.Start("C:\\Program Files\\Firefox Developer Edition\\firefox.exe", url); //"C:\Program Files\Firefox Developer Edition\firefox.exe"
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"URL: {url} failed. Message: {ex.Message}");
    }
}

public static class IPAddressExtensions
{
    public static IPAddress Next(this IPAddress ipAddress, uint increment = 1)
    {
        byte[] addressBytes = ipAddress.GetAddressBytes().Reverse().ToArray();
        uint ipAsUint = BitConverter.ToUInt32(addressBytes, 0);
        byte[] nextAddress = BitConverter.GetBytes(ipAsUint + increment);
        string result = string.Join('.', nextAddress.Reverse());
        return IPAddress.Parse(result);
    }
}



//http://186.148.73.228/Findex.htm admin:empty
//http://186.148.146.154/Findex.htm admin:12345678
//http://186.148.148.200/Findex.htm
//http://186.148.158.184/Findex.htm

//https://stackoverflow.com/questions/2245040/how-can-i-display-an-rtsp-video-stream-in-a-web-page