using IpScanApp.Clases;
using IpScanApp.Classes;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadConfiguration();
        }        
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
        private void ClassifyIp(string ip)
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

        private List<string> SortIps(string[] unsortedIps)
        {
            //var unsortedIps =
            //    new[]
            //    {
            //        "192.168.1.4",
            //        "192.168.1.5",
            //        "192.168.2.1",
            //        "10.152.16.23",
            //        "69.52.220.44"
                //};

            return unsortedIps
                .Select(Version.Parse)
                .OrderBy(arg => arg)
                .Select(arg => arg.ToString())
                .ToList();
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
                    string line = reader.ReadLine();
                    string[] values = line.Split(';');

                    ipRanges.Add(new IpRange()
                    {
                        IpBegin = new IpAddress(values[0]),
                        IpEnd = new IpAddress(values[1]),
                    });
                }
            }

            return ipRanges;
        }

        private void SaveConfiguration()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //Guardo datos introducidos en los textinput
            config.AppSettings.Settings["IpStart"].Value = txt_IpStart.Text;
            config.AppSettings.Settings["IpEnd"].Value = txt_IpEnd.Text;
        }        

        private void LoadConfiguration()
        {
            //Tomo los datos guardados y los muestro en los textinput
            txt_IpStart.Text = ConfigurationManager.AppSettings["IpStart"];
            txt_IpEnd.Text = ConfigurationManager.AppSettings["IpEnd"];
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

        private void RescanSavedCameras(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            FileStream fileStream = File.Open(filePath, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();

            //new Thread(() => new Bot("defeway", "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1", 60001).ScanList(lines)).Start();
            new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", "admin", "admin").ScanList(lines)).Start();

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

        private void btnAsyncScan_Click(object sender, RoutedEventArgs e)
        {            
            //new Thread(() => new Bot("newvision", "/tmpfs/auto.jpg", new NetworkCredential("admin", "admin")).RunAsync()).Start();
            new Thread(() => new Bot("defeway", "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1").AsyncScanFrom(new IpAddress("152.168.0.0"))).Start();
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
}
