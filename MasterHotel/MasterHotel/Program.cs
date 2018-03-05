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
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    ExecuteCapture(conn);
                    IEnumerable<Room> rooms = GetUnreplicatedData(conn);
                    Replicate(rooms);

                }
            }
        }

        private static void Replicate(IEnumerable<Room> rooms)
        {
            foreach (Room r in rooms)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:56290/");
                    var response = client.PostAsJsonAsync("api/Room", r).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                        //UpdateRoomType To mark isreplicated true
                    }
                    else
                        Console.Write("Error");
                }
            }
        }

        private static List<Room> GetUnreplicatedData(SqlConnection conn)
        {
            List<Room> unReplicatedRooms = new List<Room>();
            SqlCommand command = new SqlCommand("select roomType.RoomName, roomType.RoomType from cdc.dbo_roomtype_CT roomType,cdc.lsn_time_mapping timing" +
                                                "where roomType.Isreplicated = 0" +
                                                "and roomType.__$operation = 2" +
                                                "and roomType.__$start_lsn = timing.start_lsn" +
                                                "order by timing.tran_end_time, timing.tran_begin_time", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    unReplicatedRooms.Add(new Room(reader[0].ToString(), reader[1].ToString()));
                }
            }
            return unReplicatedRooms;
        }

        private static void ExecuteCapture(SqlConnection conn)
        {
            conn.ConnectionString = "Server=DESKTOP-NTICP67;Database=Hotel;Trusted_Connection=true";
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_MScdc_capture_job", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteReader();
        }
    }
}
