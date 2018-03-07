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

        internal static bool UpdateRoom(string roomName, string roomType)
        {
            string updateCommand = "Update Roomtype Set roomType = " + "'" + roomType + "' where roomname = '" + roomName + "')";
            return RunSqlCommand(updateCommand);
        }

        internal static bool DeleteRoom(string roomName)
        {
            string deleteCommand = "DELETE FROM Roomtype where roomName = " + "'" + roomName + "'";
            return RunSqlCommand(deleteCommand);
        }

        internal static bool AddRoom(string roomName, string roomType)
        {
            string insertCommand = "INSERT INTO Roomtype (RoomName, RoomType, isReplicated) VALUES (" + "'" + roomName + "','" + roomType + "', '1')";
            return RunSqlCommand(insertCommand);
        }

        private static bool RunSqlCommand(string sqlCmd)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlCmd, conn);
                int num = sqlCommand.ExecuteNonQuery();
                return num == 1 ? true : false;
            }
        }
    }
}