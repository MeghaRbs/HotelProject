using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hotel.HotelManager
{
    public static class RoomManager
    {
        static string connectionString = "Server=DESKTOP-NTICP67;Database=Hotel;Trusted_Connection=true";
        static public Room GetRoom(string roomName)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();

                string queryString = "SELECT * FROM RoomType where Roomname=" + "'" + roomName + "'";
                SqlCommand command = new SqlCommand(queryString, conn);
                
                Room room = new Room();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        room.RoomName = reader[0].ToString();
                        room.RoomType = reader[1].ToString();
                    }
                }
                return room;
            }
        }

        static public bool AddRoom(string roomName, string roomType)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();

                SqlCommand insertCommand = new SqlCommand("INSERT INTO Roomtype (RoomName, RoomType, isReplicated) VALUES ("  +"'" + roomName + "','" +  roomType + "', '1')", conn);
                int num = insertCommand.ExecuteNonQuery();
                return num == 1 ? true : false ;
            }
        }
    }
}