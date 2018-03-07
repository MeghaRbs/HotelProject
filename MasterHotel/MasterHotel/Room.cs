using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterHotel
{
    class Room
    {
        public Room(string v1, string v2, string v3)
        {
            RoomName = v1;
            RoomType = v2;
            Operation = v3;
        }

        public string RoomName { get; set; }
        public string RoomType { get; set; }
        public string Operation { get; set; }
    }
}
