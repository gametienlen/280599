using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Player
    {
        string userName;
        string avatar;
        public int soTienConLai;
        public int role; // 0: Người xem; 1: Người chơi
        public int pos;
        int soQuanBaiConLai;
        public bool ready;
        public int room;
        public Player(int index)
        {
            //userName = name;
            //avatar = "";
            soTienConLai = 10000;
            role = 0;
            soQuanBaiConLai = 13;
            pos = index;
            ready = false;
            room = -1;

        }
    }
}
