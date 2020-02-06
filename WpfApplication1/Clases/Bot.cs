using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnmpSharpNet;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;

namespace IpScanApp.Clases
{
    class Bot
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IpAddress IpBegin;
        IpAddress IpFinal = new IpAddress(Globals.IpEnd);
        //IpAddress Ip= new IpAddress(Globals.IpStart);
        //IpAddress IpEnd = new IpAddress(Globals.IpEnd);
        public int Port = 80;
        public bool Stop = false;
        public string Query { get; set; }
        public string Manufacturer { get; set; }
        public const string Jpeg = "image/jpeg";
        private int RequestPartialCount = 0, RequestCount = 0, ResponseCount = 0;
        public ICredentials Credentials { get; set; }

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

        public void Update()
        {
            // Read a text file line by line.
            string[] lines = File.ReadAllLines(Manufacturer + ".txt");

            foreach (string ipAddress in lines)
            {
                if (ipAddress != null & ipAddress.Length > 1)
                {
                    try
                    {
                        string BaseUrl = "http://" + ipAddress;
                        RestClient client = new RestClient(BaseUrl);
                        client.Timeout = 3000;
                        client.FollowRedirects = false; //Cuando una petición devuelve una respuesta de tipo redirección, el cliente no la sigue automáticamente.
                        RestRequest request = new RestRequest(Query, Method.GET);
                        request.Credentials = Credentials; //solo newvision  
                        IRestResponse response = client.Execute(request);
                        string file = "";
                        if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg)
                        {
                            Console.WriteLine(response.ResponseUri.Host + " " + Manufacturer + " Actualizado");
                            string FileName = Manufacturer + ".new.txt";
                            if (File.Exists(FileName))
                            {

                                file = File.ReadAllText(FileName);
                            }
                            else
                            {
                                var FileCreado = File.Create(FileName);
                                FileCreado.Close();
                            }

                            if (!file.Contains(response.ResponseUri.Host))
                            {
                                using (StreamWriter txt = new StreamWriter(FileName, true))
                                {

                                    if (response.ResponseUri.Port == 80)
                                    {
                                        txt.WriteLine(response.ResponseUri.Host);
                                    }
                                    else
                                    {
                                        txt.WriteLine(response.ResponseUri.Host + ":" + response.ResponseUri.Port);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ipAddress + ex.Message);
                    }
                }
            }

            Console.WriteLine("Done!");
        
        }

        public void Run()
        {
            IpAddress Ip = IpBegin;
            IpAddress IpEnd = Ip.Increment(65535);
            try
            {
                log.Info("Start " + Manufacturer + " Ip:" + Ip  + " Port:" + Port);
                while (!Stop && Ip != IpEnd)
                {
                    try
                    {
                        RequestCount++;
                        RequestPartialCount++;
                        string BaseUrl = "http://" + Ip + ":" + Port;
                        RestClient client = new RestClient(BaseUrl);
                        client.Timeout = 3000;
                        client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
                        RestRequest request = new RestRequest(Query, Method.GET);
                        request.Credentials = Credentials; //solo newvision  
                        IRestResponse response = client.Execute(request);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg  && response.ContentLength > 0)
                        {
                            Console.WriteLine(response.ResponseUri.Host + " " + Manufacturer);

                            log.Info("Encontrado " + Manufacturer + " " + response.ResponseUri.Host + ":" + response.ResponseUri.Port.ToString());

                            string FileName = Manufacturer + ".txt";
                            if (File.Exists(FileName))
                            {
                                var file = File.ReadAllText(FileName);

                                if (!file.Contains(response.ResponseUri.Host))
                                {
                                    using (StreamWriter txt = new StreamWriter(FileName, true))
                                    {

                                        if (Port == 80)
                                        {
                                            txt.WriteLine(response.ResponseUri.Host);
                                        }
                                        else
                                        {
                                            txt.WriteLine(response.ResponseUri.Host + ":" + Port);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                File.Create(FileName).Close();
                                using (StreamWriter txt = new StreamWriter(FileName, true))
                                {
                                    if (Port == 80)
                                    {
                                        txt.WriteLine(response.ResponseUri.Host);
                                    }
                                    else
                                    {
                                        txt.WriteLine(response.ResponseUri.Host + ":" + Port);
                                    }
                                }
                            }
                        }

                        Ip = Ip.Increment(1);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Source + " " + ex.Message);
                    }
                }

                log.Info("Finalizó " + Manufacturer);

            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw;
            }
        }

        ///<summary>
        ///This method run async The Bot (Under Testing)
        ///</summary>
        //public void RunAsync()
        //{
        //    //IpAddress Ip = IpStart;

        //    while (!Stop && Ip != IpEnd)
        //    {
        //        try
        //        {
        //            RequestCount++;
        //            RequestPartialCount++;

        //            //Console.WriteLine("Request " + RequestCount);


        //            //Console.WriteLine(System.Net.ServicePointManager.DefaultConnectionLimit.ToString() + " Conexiones ");
        //            string BaseUrl = "http://" + Ip + ":" + Port;
        //            RestClient client = new RestClient(BaseUrl);
                    
        //            client.Timeout = 3000;
        //            client.FollowRedirects = false; //Cuando una petición devuelve una respuesta de tipo redirección, el cliente no la sigue automáticamente.
        //            RestRequest request = new RestRequest(Query, Method.GET);
        //            request.Credentials = Credentials; //solo newvision    
        //            client.ExecuteAsync(request, response =>
        //            {
        //                ResponseCount++;
        //                //ShowActiveTcpConnections();
        //                //Console.WriteLine("Response " + ResponseCount);

        //                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                //{
        //                //    //OPEN BROWSER
        //                //    Process.Start("http://" + response.ResponseUri.Host);
        //                //}
        //                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg)
        //                {

        //                    //Console.WriteLine(res);
        //                    //Console.WriteLine(Manufacturer + " " + response.ResponseUri.Host);
        //                    string FileName = Manufacturer + ".txt";

        //                    if (File.Exists(FileName))
        //                    {                                
        //                        var file = File.ReadAllText(FileName);

        //                        if (!file.Contains(response.ResponseUri.Host))
        //                        {
        //                            using (StreamWriter txt = new StreamWriter(FileName, true))
        //                            {
        //                                if (Port == 80)
        //                                {
        //                                    txt.WriteLine(response.ResponseUri.Host);
        //                                }
        //                                else
        //                                {
        //                                    txt.WriteLine(response.ResponseUri.Host + ":" + Port);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else 
        //                    {
        //                        if (Port == 80)
        //                        {
        //                            System.IO.File.WriteAllText(FileName, response.ResponseUri.Host);                                    
        //                        }
        //                        else
        //                        {
        //                            System.IO.File.WriteAllText(FileName, response.ResponseUri.Host + ":" + Port);                                    
        //                        }                                                         
        //                    }
        //                }
        //            });

        //            Ip = Ip.Increment(1);

        //            Thread.Sleep(50);

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Source + " " + ex.ToString());
        //        }
                
        //    }

        //    Console.WriteLine("DONE!");
        //}

        public Thread thRunInParallel { get; set; }

        public void RunInParallel(){

            IpBegin = new IpAddress(Globals.IpStart);

            try
            {
                int length = 4;
                for (int i = 0; i < length; i++)
                {
                    new Thread(new ThreadStart(new Parallel(IpBegin, Query, Manufacturer, Port, Credentials).Run)).Start();                   

                    IpBegin = IpBegin.Increment(65536);                    
                }

                while(!Stop);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw;
            }
           
            
        }

       
    }

}