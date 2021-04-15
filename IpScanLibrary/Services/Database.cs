using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpScanLibrary.Services
{
    public class Database
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        SqlConnection sqlConnection = null;

        public bool IsConnected { get; private set; } = false;

        public void Connect(string connetionString)
        {
            //string connetionString = @"Data Source=RAUL-WORK\A1SQLEXPRESS;Initial Catalog=A1MobileAccess;User ID=sa;Password=Alsina911";
            sqlConnection = new SqlConnection(connetionString);
            sqlConnection.Open();
            IsConnected = true;
        }

        public void Disconect()
        {
            if (sqlConnection != null)
            {
                sqlConnection.Close();
            }
        }

        public void InsertNew()
        {
            if (sqlConnection?.State != ConnectionState.Open)
            {
                Debug.WriteLine("No conectado!");
                return;
            }

            string saveAccess = "INSERT into dbo.Cameras " +
            "(id, found_timestamp, ip_address, port, manufacturer, username, password, status) " +
            "VALUES (newid(), @found_timestamp, @ip_address, @port, @manufacturer, @username, @password, @status)";

            using (SqlCommand query = new SqlCommand(saveAccess))
            {
                query.Connection = sqlConnection;

                query.Parameters.AddWithValue("@found_timestamp", DateTime.Now);
                query.Parameters.AddWithValue("@ip_address", "190.190.190.190");
                query.Parameters.AddWithValue("@port", 80);
                query.Parameters.AddWithValue("@manufacturer", "Lucas");
                query.Parameters.AddWithValue("@username", "admin");
                query.Parameters.AddWithValue("@password", "admin");
                query.Parameters.AddWithValue("@status", 1);                

                int result = query.ExecuteNonQuery();

                if (result < 0)
                {
                    //log.Error("Error inserting data into Database!");
                    Debug.WriteLine("Error inserting data into Database!");
                }
            }
        }



        //public AccessModel GetAccessToActivity(DateTime timeStamp, string doorSide)
        //{
        //    AccessModel access = null; ;

        //    if (sqlConnection?.State != ConnectionState.Open)
        //    {
        //        //log.Debug("No conectado");
        //        return access;
        //    }

        //    string getAccess = "SELECT qr_code, image, face_match, latitude, longitude " +
        //    "FROM Access " +
        //    "WHERE status = @status and timestamp = @timestamp and door_side = @door_side";

        //    using (SqlCommand query = new SqlCommand(getAccess))
        //    {
        //        query.Connection = sqlConnection;

        //        query.Parameters.AddWithValue("@door_side", doorSide);
        //        query.Parameters.AddWithValue("@timestamp", timeStamp);
        //        query.Parameters.AddWithValue("@status", "2");

        //        SqlDataReader reader = query.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            access = new AccessModel(reader["qr_code"].ToString(),
        //            ReadImageSafe(reader, 1),
        //            //(FaceMatch) Enum.Parse(typeof(FaceMatch), reader.GetValue(2).ToString())
        //            (FaceMatch)int.Parse(reader["face_match"].ToString()),
        //            reader["latitude"].ToString(),
        //            reader["longitude"].ToString());
        //        }

        //        reader.Close();
        //    }

        //    return access;
        //}

        private byte[] ReadImageSafe(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetValue(colIndex) as byte[];
            return new byte[0];
        }
    }
}
