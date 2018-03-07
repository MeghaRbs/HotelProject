using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MasterHotel
{
    class MasterHotel
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ExecuteCapture();
                IEnumerable<Room> rooms = GetUnreplicatedData();
                Replicate(rooms);
            }
        }        

        private static void Replicate(IEnumerable<Room> rooms)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56290/");
                foreach (Room r in rooms.Where( s => s.Operation == "2"))
                {
                    HttpResponseMessage response = client.PostAsJsonAsync("api/Room", r).Result;
                    MarkReplicated(r.RoomName, response);//perf optimisation can be done
                }

                foreach (Room r in rooms.Where(s => s.Operation == "1"))
                {
                    HttpResponseMessage response = client.DeleteAsync("api/Room/" + r.RoomName).Result;
                    MarkReplicated(r.RoomName, response);
                }

                foreach (Room r in rooms.Where(s => s.Operation == "4"))
                {
                    HttpResponseMessage response = client.PutAsync("api/Room/" + r.RoomName + "/" + r.RoomType, null).Result;
                    MarkReplicated(r.RoomName, response);
                }
            }
        }

        private static void MarkReplicated(string roomName, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                MarkIsReplicated(roomName);
            }
            else
                Console.Write("Error");
        }
        
        private static void MarkIsReplicated(string roomName)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=DESKTOP-NTICP67;Database=Hotel;Trusted_Connection=true";
                conn.Open();
                string sqlCmd = "Update Roomtype Set isreplicated = 1 where roomname = '" + roomName + "'";
                SqlCommand sqlCommand = new SqlCommand(sqlCmd, conn);
                sqlCommand.ExecuteNonQuery();
            }
        }

        private static List<Room> GetUnreplicatedData()
        {
            List<Room> unReplicatedRooms = new List<Room>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=DESKTOP-NTICP67;Database=Hotel;Trusted_Connection=true";
                conn.Open();
                SqlCommand command = new SqlCommand("select roomType.RoomName, roomType.RoomType, roomType.__$operation from cdc.dbo_roomtype_CT roomType,cdc.lsn_time_mapping timing" +
                                                " where roomType.Isreplicated = 0" +
                                                " and roomType.__$start_lsn = timing.start_lsn" +
                                                " order by timing.tran_end_time, timing.tran_begin_time", conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        unReplicatedRooms.Add(new Room(reader[0].ToString(), reader[1].ToString(), reader[1].ToString()));
                    }
                }
            }
            return unReplicatedRooms;
        }

        private static void ExecuteCapture()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=DESKTOP-NTICP67;Database=Hotel;Trusted_Connection=true";
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_MScdc_capture_job", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteReader();
            }
        }
    }
}
