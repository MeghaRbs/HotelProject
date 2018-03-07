using Hotel.HotelManager;
using Hotel.Models;
using System;
using System.Web.Http;

namespace Hotel.Controllers
{
    public class RoomController : ApiController
    {
        /*[HttpGet]
        public Room GetRoomDetails(string id)
        {
            return RoomManager.GetRoom(id);            
        }*/

        [HttpPost]
        public bool AddRoomDetails([FromBody] Room room)
        {
            return RoomManager.AddRoom(room.RoomName, room.RoomType);
        }

        [HttpDelete]
        public bool DeleteRoomDetails(string id)
        {
            return RoomManager.DeleteRoom(id);
        }

        [HttpPut]
        public bool UpdateRoomDetails(string roomName, string roomType)
        {
            return RoomManager.UpdateRoom(roomName, roomType);
        }
    }

}

