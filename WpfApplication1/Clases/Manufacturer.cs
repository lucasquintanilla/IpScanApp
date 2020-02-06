using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpScanApp.Clases
{
    class Manufacturer
    {
        //Puertos comunes
        //80,23,8080,8081 and 8082 , 82, 82

        //Camaras querys por marca
        public string UriAxis = "/mjpg/video.mjpg"; //8001
        public string UriBosch = "/snap.jpg?JpegSize=M&JpegCam=1&r=156297"; //8080
        public string UriDefeway = "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1";
        public string UriNewvision = "/tmpfs/auto.jpg";
        public string UriPanasonic = "/cgi-bin/camera?resolution=640&amp;quality=1&amp;Language=0&amp;1562997319";
        public string UriVivotek = "/cgi-bin/viewer/video.jpg"; //8081 8080 81
        public string UriStramer = "/?action=stream";
        public string UriLinksys = "/img/video.mjpeg"; //1024
        public string UriFoscam = "/videostream.cgi?user=admin&pwd="; //8082 85 81 8080
        public string UriDlink = "/mjpeg.cgi";
        public string UriHi3516 = "/webcapture.jpg?command=snap&channel=1?15629973";
        public string UriMegapixel = "/jpgmulreq/1/image.jpg?key=1516975535684&lq=1&15629973";
        public string UriNetcam = "/IMAGE.JPG";
        public string UriTpLink = "/jpg/image.jpg?15739570";
        public string UriSony = "/image?speed=0"; //2000
    }

    public class Newvision
    {
        public static string Name = "newvision";
        public static string Query = "/tmpfs/auto.jpg";
    }

    public class Defeway
    {
        public static string Name = "defeway";
        public static string Query = "/cgi-bin/snapshot.cgi?chn=0&u=admin&p=&q=1";
    }

    public class Vivotek
    {
        public static string Name = "vivotek";
        public static string Query = "/cgi-bin/viewer/video.jpg";
    }
}
