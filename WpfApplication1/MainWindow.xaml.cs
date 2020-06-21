using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using IpScanApp.Clases;
using SnmpSharpNet;
using RestSharp;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Drawing;
using IpScanApp.Classes;

namespace WpfApplication1
{
    #region Classes
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Tomo los datos guardados y los muestro en los textinput
            txt_IpStart.Text = ConfigurationManager.AppSettings["IpStart"];
            txt_IpEnd.Text = ConfigurationManager.AppSettings["IpEnd"];
        }        
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
         

              
                        
        private void FilterIp(string ip)
        {
            try
            {
                //GET TITLE
                WebClient x = new WebClient();
                string source = x.DownloadString("http://" + ip);

                string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                Console.WriteLine(title);

                if (title == "WEB SERVICE")
                {
                    //SAVE IP
                    Console.WriteLine("Filtrado Dahua " + ip);
                    string fileNameDahua = "dahua.txt";
                    using (StreamWriter txt = new StreamWriter(fileNameDahua, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "NETSurveillance WEB")
                {
                    //SAVE IP
                    string fileNameNET = "NETSurveillance.txt";
                    Console.WriteLine("Filtrado NETSurveillance " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameNET, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "RouterOS router configuration page")
                {
                    //SAVE IP
                    string fileNameMikro = "mikrotik.txt";
                    Console.WriteLine("Filtrado mikrotik " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameMikro, true))
                    {
                        txt.WriteLine(ip);
                    }
                }

                else if (title == "Technicolor Gateway - Login")
                {
                    //SAVE IP
                    string fileNameTechnicolorGateway = "technicolor-gateway.txt";
                    Console.WriteLine("Filtrado Technicolor Gateway" + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameTechnicolorGateway, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "Index" || title == "index")
                {
                    //SAVE IP
                    string fileNameAbrirSesion = "index.txt";
                    Console.WriteLine("Filtrado Index " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameAbrirSesion, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "Index of /")
                {
                    //SAVE IP
                    string fileNameIndexOf = "indexOf.txt";
                    Console.WriteLine("Filtrado Index OF" + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameIndexOf, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "Vigor Login Page")
                {
                    //SAVE IP
                    string fileNameVigorLoginPage = "VigorLoginPage.txt";
                    Console.WriteLine("Filtrado Vigor Login Page" + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameVigorLoginPage, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "NetDvrV3")
                {
                    //SAVE IP
                    string fileNameNetDvrV3 = "netdvrv3.txt";
                    Console.WriteLine("Filtrado NetDvrV3 " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameNetDvrV3, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "Apache2 Ubuntu Default Page: It works")
                {
                    //SAVE IP
                    string fileNameUbuntu = "ubuntu.txt";
                    Console.WriteLine("Filtrado Ubuntu " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameUbuntu, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "Login")
                {
                    //SAVE IP
                    string fileNameLogin = "login.txt";
                    Console.WriteLine("Filtrado Login " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameLogin, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else if (title == "Abrir Sesión")
                {
                    //SAVE IP
                    string fileNameAbrirSesion = "AbrirSesion.txt";
                    Console.WriteLine("Filtrado AbrirSesion " + ip);
                    using (StreamWriter txt = new StreamWriter(fileNameAbrirSesion, true))
                    {
                        txt.WriteLine(ip);
                    }
                }
                else
                {
                    try
                    {
                        string technicolorTag = Regex.Match(source, @"<small>([\s\S]*?)<\/small>", RegexOptions.Singleline).Value;
                        if (technicolorTag == "<small>Â© 2015 Technicolor. All rights reserved. </small>")
                        {
                            Console.WriteLine("Filtrado Technicolor " + ip);
                            string fileNametechnicolor = "technicolor.txt";
                            using (StreamWriter txt = new StreamWriter(fileNametechnicolor, true))
                            {
                                txt.WriteLine(ip);
                            }
                        }
                        else
                        {
                            //string HikvisionTag = Regex.Match(source, @"<label>([\s\S]*?)</label>", RegexOptions.Singleline).Value;
                            //Console.WriteLine(HikvisionTag);
                            //if (HikvisionTag == "©Hikvision Digital Technology Co., Ltd. All Rights Reserved.")
                            //{
                            //    Console.WriteLine("HIKVISION" + ip);
                            //    string fileNametechnicolor = "technicolor.txt";
                            //    using (StreamWriter txt = new StreamWriter(fileNametechnicolor, true))
                            //    {
                            //        txt.WriteLine(ip);
                            //    }
                            //}
                            //else
                            //{
                            //SAVE IP
                            string fileName = "open.txt";
                            using (StreamWriter txt = new StreamWriter(fileName, true))
                            {
                                txt.WriteLine(ip);
                            }

                            //OPEN BROWSER
                            Process.Start("http://" + ip);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("EX: " + ex.Message + ip);

                        //SAVE IP
                        string fileName = "open.txt";
                        using (StreamWriter txt = new StreamWriter(fileName, true))
                        {
                            txt.WriteLine(ip);
                        }

                        //OPEN BROWSER
                        Process.Start("http://" + ip);
                    }
                }
            }
            catch (Exception exx)
            {
                Console.WriteLine("EXX: " + exx.Message + ip);

                if (exx.Message == "")
                {
                    //OPEN BROWSER
                    Process.Start("http://" + ip);
                }

                //OPEN BROWSER
                Process.Start("http://" + ip);

                //SAVE IP
                string fileName = "open.txt";
                using (StreamWriter txt = new StreamWriter(fileName, true))
                {
                    txt.WriteLine(ip);
                }
            }
        }                

        private void SetConfiguration()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //Guardo datos introducidos en los textinput
                config.AppSettings.Settings["IpStart"].Value = txt_IpStart.Text;
                config.AppSettings.Settings["IpEnd"].Value = txt_IpEnd.Text;

                Globals.IpStart = txt_IpStart.Text;
                Globals.IpEnd = txt_IpEnd.Text;
            }
            catch (Exception e)
            {
                log.Info(e.ToString());                
            }
            
        }        

        private void btnTcpConnections_Click(object sender, RoutedEventArgs e)
        {

            //while (!stop) {

                //Console.WriteLine("Active TCP Connections");
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
                txt_TcpConnections.Text = connections.Length.ToString();
                //Console.WriteLine("Active TCP Connections: " + connections.Length);
                //Thread.Sleep(1000);
            //}


        }

        

        private void btn_RescanExistingCams_Click(object sender, RoutedEventArgs e)
        {
            string FolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            try
            {                
                foreach (string filePath in Directory.EnumerateFiles(FolderPath, "*.txt"))
                {
                    string FileName = System.IO.Path.GetFileNameWithoutExtension(filePath); 
                    switch (FileName)
                    {
                        case "vivotek":
                            Console.WriteLine("vivotek");
                            break;
                        case "defeway":
                            Console.WriteLine("defeway");
                            RescanDefeway(filePath);
                            break;                        
                        case "newvision":
                            Console.WriteLine("Case 2");
                            RescanNewvision(filePath);
                            break;
                        case "tplink":
                            Console.WriteLine("Case 2");
                            break;
                        case "bosch":
                            Console.WriteLine("Case 2");
                            break;
                        default:
                            Console.WriteLine("Camara no la tuve en cuenta jaja");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);                
            }
        }

        private void RescanDefeway(string FilePath)
        {
            string[] lines = File.ReadAllLines(FilePath);

            List<string> OldCamaras = new List<string>();
            foreach (string camara in lines)
            {
                OldCamaras.Add(camara);
            }
            foreach (var OldCam in OldCamaras)
            {
                string fileNameDefeway = "defeway-old.txt";
                using (StreamWriter txt = new StreamWriter(fileNameDefeway, true))
                {
                    txt.WriteLine(OldCam);
                }
            }
            
            List<string> CamarasFiltradas = new List<string>();

            foreach (string ip in lines)
            {
                if (ip != null & ip.Length > 1)
                {
                    try
                    {
                        string URL = "http://" + ip + "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1";
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                        //request.Credentials = new NetworkCredential("admin", "admin");
                        request.Timeout = 6000;
                        request.AllowAutoRedirect = false;
                        string Jpeg = "image/jpeg";

                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                            if (response.StatusCode == HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
                            {
                                Console.WriteLine(ip);
                                Console.WriteLine("Content leght: " + response.ContentLength);
                                CamarasFiltradas.Add(ip);
                            }
                            else
                            {
                                Console.WriteLine(ip + " No xiste una camara");
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

            //Borro todo el contenido delegate archivo
            File.WriteAllText(FilePath, String.Empty);

            //Guardo las CamarasFiltradas filtradas
            foreach (var ip in CamarasFiltradas)
	        {		
                string fileNameDefeway = "defeway.txt";
                using (StreamWriter txt = new StreamWriter(fileNameDefeway, true))
                {
                    txt.WriteLine(ip);
                }
	        }

            Console.WriteLine("FIN");
            
        }

        private void RescanSavedCameras(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            FileStream fileStream = File.Open(filePath, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();

            //new Thread(() => new Bot("defeway", "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1", 60001).ScanList(lines)).Start();
            new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", "admin", "admin").ScanList(lines)).Start();

        }

        private void RescanNewvision(string FilePath)
        {
            string[] lines = File.ReadAllLines(FilePath);

            List<string> OldCamaras = new List<string>();
            foreach (string camara in lines)
            {
                OldCamaras.Add(camara);
            }
            foreach (var OldCam in OldCamaras)
            {
                string fileNameDefeway = "newvision-old.txt";
                using (StreamWriter txt = new StreamWriter(fileNameDefeway, true))
                {
                    txt.WriteLine(OldCam);
                }
            }
            
            List<string> CamarasFiltradas = new List<string>();

            foreach (string ip in lines)
            {
                if (ip != null & ip.Length > 1)
                {
                    try
                    {
                        string URL = "http://" + ip + "/tmpfs/auto.jpg";
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                        request.Credentials = new NetworkCredential("admin", "admin");
                        request.Timeout = 4000;
                        request.AllowAutoRedirect = false;
                        string Jpeg = "image/jpeg";

                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();                            
                            
                            if (response.StatusCode == HttpStatusCode.OK && response.ContentType == Jpeg && response.ContentLength > 0)
                            {
                                Console.WriteLine(ip);
                                Console.WriteLine("Content leght: " + response.ContentLength);
                                CamarasFiltradas.Add(ip);                                
                            }
                            else
                            {
                                Console.WriteLine(ip + " No xiste una camara");
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

            //Borro todo el contenido delegate archivo
            File.WriteAllText(FilePath, String.Empty);

            //Guardo las CamarasFiltradas filtradas
            foreach (var ip in CamarasFiltradas)
	        {		
                string fileNameDefeway = "newvision.txt";
                using (StreamWriter txt = new StreamWriter(fileNameDefeway, true))
                {
                    txt.WriteLine(ip);
                }
	        }

            Console.WriteLine("FIN");
        }        

        private void btnReadData_Click(object sender, RoutedEventArgs e)
        {
            var ipRange = GetIpRanges()[0];

            new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", "admin", "admin").AsyncScanRange(ipRange.IpBegin, ipRange.IpEnd)).Start();

            //foreach (var ipRange in GetIpRanges())
            //{
            //    new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", "admin", "admin").AsyncScanRange(ipRange.IpBegin, ipRange.IpEnd)).Start();
            //}
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
                    string  line = reader.ReadLine();
                    string[] values = line.Split(';');

                    ipRanges.Add(new IpRange()
                    {
                        IpBegin = new IpAddress(values[0]),
                        IpEnd = new IpAddress(values[1]),
                    }) ;
                }
            }

            return ipRanges;
        }

        private void btnAsyncScan_Click(object sender, RoutedEventArgs e)
        {            
            //new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", new NetworkCredential("admin", "admin")).RunAsync()).Start();
            new Thread(() => new Bot("defeway", "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1").AsyncScanFrom("152.168.0.0")).Start();
        }

        private void btnScanRange_Click(object sender, RoutedEventArgs e)
        {            
            new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", "admin", "admin").AsyncScanRange(new IpAddress("152.168.0.0"), new IpAddress("152.171.255.255"))).Start();
        }

        private void btnScanSavedCameras_Click(object sender, RoutedEventArgs e)
        {
            RescanSavedCameras("newvision.txt");
        }
    }
    
    #endregion
}
