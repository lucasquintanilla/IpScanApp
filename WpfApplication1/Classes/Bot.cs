using RestSharp;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using IpScanApp.Classes;

namespace IpScanApp.Clases
{
    class Bot
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private string uri;
        private string manufacturer;
        private string username;
        private string password;
        private ICredentials credentials;
        private bool stop = false;
        private const string Jpeg = "image/jpeg";
        private int timeout = 2000;        

        public Bot(string manufacturer, string uri, string username, string password)
        {
            this.manufacturer = manufacturer;
            this.uri = uri;
            this.username = username;
            this.password = password;
            this.credentials = new NetworkCredential(username, password);            
        }

        public Bot(string manufacturer, string uri)
        {
            this.manufacturer = manufacturer;
            this.uri = uri;
        }
        
        public static void ShowActiveTcpConnections()
        {
            //Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            Console.WriteLine("Active TCP Connections: " + connections.Length);
            //foreach (TcpConnectionInformation c in connections)
            //{
            //    Console.WriteLine("{0} <==> {1}",
            //                      c.LocalEndPoint.ToString(),
            //                      c.RemoteEndPoint.ToString());
            //}
        }

        public void Stop()
        {
            this.stop = true;
        }

        public void ScanRange(IpAddress ipAddressBegin, IpAddress ipAddressEnd, int port = 80)
        {
            var ip = ipAddressBegin;
                           
            while (ip.CompareTo(ipAddressEnd) == -1)
            {
                try
                {
                    RestClient client = new RestClient($"http://{ip}:{port}");
                    client.Timeout = timeout;
                    client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
                        
                    RestRequest request = new RestRequest(this.uri, Method.GET);
                    request.Credentials = this.credentials; //solo newvision  

                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg  && response.ContentLength > 0)
                    {
                        SaveCamera(response.ResponseUri.Host, response.ResponseUri.Port, this.manufacturer);
                    }

                    ip = ip.Increment(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Source + " " + ex.Message);
                }
            }

            Console.WriteLine("END");
        }

        public void ScanList(string[] list)
        {
            foreach (string ipAddress in list)
            {
                if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Length > 0)
                {
                    if (IsCamera(ipAddress))
                    { 
                        SaveCamera(ipAddress, this.manufacturer);
                    }
                }
            }

            Console.WriteLine("DONE!");
        }

        public void DeepScan(int[] ports)
        {
            foreach (var port in ports)
            {                
                AsyncScanRange(new IpAddress("152.168.0.0"), new IpAddress("152.171.255.255"), port);
            }

            Console.WriteLine("DEEP SCAN DONE!");
        }

        public void AsyncScanFrom(IpAddress ipAddress, int port = 80)
        {
            while (!stop)
            {
                Console.WriteLine("start");

                var holdingpoint = ipAddress.Increment(255 * 5); //ESTABLECE UNA IP DE ESPERA PARA MEJORAR EL RENDIMIENTO

                while (ipAddress.CompareTo(holdingpoint) == -1)
                {                    
                    try
                    {
                        AsyncScan(ipAddress, port);

                        ipAddress = ipAddress.Increment(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Source + " " + ex.Message);
                    }
                }

                //ESTE CICLO PERMITE LA RECEPCION DE RESPUESTAS SIN GENERAR CONFLICTOS EN LA RED
                //TENIENDO LA MEJOR PERFORMANCE EN LAS PRUEBAS REALIZADAS
                int count = 0;

                while (count < 50)
                {
                    Thread.Sleep(250);
                    count++;
                }
            }

            Console.WriteLine("DONE!");
        }

        public void AsyncScanRange(IpAddress ipAddressBegin, IpAddress ipAddressEnd, int port = 80)
        {
            IpAddress ip = ipAddressBegin;

            while (ip.CompareTo(ipAddressEnd) == -1)
            {
                var holdingpoint = ip.Increment(255 * 5); //HOLDINGPOINT ES UN PUNTO DE ESPERA PARA RECIBIR PAQUETES ENVIADOS

                while (ip.CompareTo(holdingpoint) == -1) //Agregar comparacion con ipend, porque sigue un poco de largo.
                {
                    try
                    {
                        Console.WriteLine(ip);
                        AsyncScan(ip, port);

                        ip = ip.Increment(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Source + " " + ex.Message);
                    }
                }

                //ESTE CICLO PERMITE LA RECEPCION DE RESPUESTAS SIN GENERAR CONFLICTOS EN LA RED
                //TENIENDO LA MEJOR PERFORMANCE EN LAS PRUEBAS REALIZADAS
                int count = 0;

                while (count < 50)
                {
                    Thread.Sleep(250);
                    count++;
                }
            }

            Console.WriteLine("DONE!");
        }

        private void AsyncScan(IpAddress ipAddress, int port)
        {
            int timeout = 4000;

            RestClient client = new RestClient($"http://{ipAddress}:{port}");
            client.Timeout = timeout;
            client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            RestRequest request = new RestRequest(this.uri, Method.GET);
            request.Credentials = this.credentials; //solo newvision & dahua

            client.ExecuteAsync(request, response =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.ResponseUri.Host + " OK");

                    if (IsCamera(response.ResponseUri.Host, response.ResponseUri.Port))
                    {

                        if (!string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.password))
                        {
                            Process.Start("firefox.exe", $"http://{this.username}:{this.password}@{response.ResponseUri.Host}:{response.ResponseUri.Port}{this.uri}");
                        }
                        else
                        {
                            Process.Start("firefox.exe", $"http://{response.ResponseUri.Host}:{response.ResponseUri.Port}{this.uri}");
                        }

                        SaveCamera(response.ResponseUri.Host, response.ResponseUri.Port, this.manufacturer);
                    }
                }
            });
        }

        private bool IsCamera(string ipAddress, int port)
        {            
            RestClient client = new RestClient($"http://{ipAddress}:{port}");
            client.Timeout = 10000;
            client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            RestRequest request = new RestRequest(this.uri, Method.GET);           
            request.Credentials = this.credentials;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
            {
                return true;
            }

            return false;
        }

        private bool IsCamera(string ipAddress)
        {
            RestClient client = new RestClient($"http://{ipAddress}");
            client.Timeout = 10000;
            client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            RestRequest request = new RestRequest(this.uri, Method.GET);
            request.Credentials = this.credentials;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
            {
                return true;
            }

            return false;
        }

        private void SaveCamera(string ipAddress, int port, string manufacturer)
        {
            try
            {
                string filename = $"{manufacturer}.txt";

                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                }

                var file = File.ReadAllText(filename);

                if (!file.Contains(ipAddress))
                {
                    using (StreamWriter txt = new StreamWriter(filename, true))
                    {
                        if (port == 80)
                        {
                            txt.WriteLine(ipAddress);
                        }
                        else
                        {                            
                            txt.WriteLine($"{ipAddress}:{port}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }            
        }

        private void SaveCamera(string ipAddress, string manufacturer)
        {
            try
            {
                string filename = $"{manufacturer}.txt";

                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                }

                var file = File.ReadAllText(filename);

                if (!file.Contains(ipAddress))
                {
                    using (StreamWriter txt = new StreamWriter(filename, true))
                    {
                        txt.WriteLine(ipAddress);                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private List<IpRange> GetIpRanges()
        {
            List<IpRange> ipRanges = new List<IpRange>();

            //Leo CSV con datos de Ips de Argentina
            using (var reader = new StreamReader(@"Resources\ip_range_argentina.csv"))
            {
                reader.ReadLine(); //Leo encabezado

                //Leo el resto de la tabla
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    ipRanges.Add(new IpRange()
                    {
                        IpBegin = new IpAddress(values[0]),
                        IpEnd = new IpAddress(values[1])
                    });
                }
            }

            return ipRanges;
        }
    }    
}