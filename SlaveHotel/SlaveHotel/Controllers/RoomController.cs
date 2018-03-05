using Hotel.HotelManager;
using Hotel.Models;
using System;
using System.Web.Http;

namespace Hotel.Controllers
{
    public class RoomController : ApiController
    {
        [HttpGet]
        public Room GetRoomDetails(string id)
        {
            return RoomManager.GetRoom(id);            
        }

        [HttpPost]
        public bool AddRoomDetails([FromBody] Room room)
        {
            return RoomManager.AddRoom(room.RoomName, room.RoomType);
        }

        /*[HttpDelete]
        public string DeleteRoomDetails(string id)
        {
            return null;
        }
        [HttpPut]
        public string UpdateRoomDetails(string Name, ***)
        {
         return null;   
        }*/
    }

}

