using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Server : Form
    {
        private TCPModel tcp;
        public SocketModel[] socketList1;

        private SocketModel[] socketList2;
        private int numberOfPlayers = 200;
        private int currentClient;
        private object thislock;

        List<Room> danhSachPhong;
        List<Player> danhSachNguoiChoi=new List<Player>();
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            thislock = new object();
            danhSachPhong = new List<Room>(10);
          //  for (int i = 0; i < 10; i++)
                danhSachPhong.Add(new Room());
        }

        public void StartServer()
        {
            string ip = txbIP.Text;
            int port = int.Parse(txbPort.Text);            
            tcp = new TCPModel(ip, port);
            tcp.Listen();
            btnStart.Enabled = false;
        }
        public void ServeClients()
        {
            socketList1 = new SocketModel[numberOfPlayers];
            socketList2 = new SocketModel[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                ServeAClient();
            }
        }
        public void Accept1()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            socketList1[currentClient] = new SocketModel(s);

            string str = socketList1[currentClient].GetRemoteEndpoint();           
            txbConnectionManager.AppendText("\nNew connection from: " + str +"id"+  currentClient+"\n" );

        }
        public void Accept2()
        {
            int status = -1;
            Socket s = tcp.SetUpANewConnection(ref status);
            socketList2[currentClient] = new SocketModel(s);
            string str = socketList2[currentClient].GetRemoteEndpoint();
            txbConnectionManager.AppendText("\nNew connection from: " + str + "id" + currentClient + "\n" );
        }

        public void ServeAClient()
        {
            int num = -1;
            lock (thislock)
            {
                Accept1();
                Accept2();
                currentClient++;
                num = currentClient - 1;
            }
            Thread t = new Thread(Commmunication);
            t.Start(num);
        }

        void DangNhap(int index)
        {           
            danhSachNguoiChoi.Add(new Player(index));               
        }

        int TimPhong(int index)
        {
            int j = 0;
            foreach (var i in danhSachPhong)
            {
                if (i.isFull() == false)
                {
                    i.players.Add(danhSachNguoiChoi[index]);
                    i.players[i.players.Count - 1].room = j;
                    if (i.isFull() == true)
                        danhSachPhong.Add(new Room());
                    return j;
                }
                j++;
            }
            return -1;
            //   danhSachPhong.Add(new Room(danhSachNguoiChoi[index]));
        }
        void ChiaBai(int sophong)
        {
            if (danhSachPhong[sophong].Ready() == danhSachPhong[sophong].players.Count){
                danhSachPhong[sophong].chiaBai(socketList1);
                for (int j = 0; j < danhSachPhong[sophong].players.Count; j++)
                    danhSachPhong[sophong].players[j].ready = false;
            }         
        }

        void SetBoLuot(int sophong)
        {
            while (danhSachPhong[sophong].DanhSachBoLuot.Contains(danhSachPhong[sophong].turn))
                danhSachPhong[sophong].turn = (danhSachPhong[sophong].turn + 1) % danhSachPhong[sophong].players.Count();

        }
        public void Commmunication(object obj)
        {         
            int pos = (Int32)obj;
            while (true)
            {
                string str = socketList1[pos].ReceiveData();
                //int sophong = -1;
                //int soLuongNguoiChoiTrongPhong = 0;

                //Nếu người chơi đã đăng nhập và đã vào phong thì set 2 biên bên dưới để cho tiện sử dụng
                //if (danhSachNguoiChoi.Count != 0 && danhSachNguoiChoi[pos].room != -1)
                //{
                //    sophong = danhSachNguoiChoi[pos].room;
                //    soLuongNguoiChoiTrongPhong = danhSachPhong[sophong].players.Count;
                //}

                if (str == "dangnhap")
                {
                    DangNhap(pos);
                    socketList1[pos].SendData("Đăng nhập thành công!");
                }
                if (str == "timphong")
                {
                    int room=TimPhong(pos)+1;
                    socketList1[pos].SendData("Bạn đã được thêm vào phòng số "+room);
                }
                if(str=="chiabai")
                {

                    danhSachNguoiChoi[pos].ready = true;// Khi người chơi sẳn sàng nhận bài thì cờ ready được bật lên và khi số cờ ready trong phòng bằng với số người chơi hiện tại trong phòng thì bài sẽ được chia
                    ChiaBai(danhSachNguoiChoi[pos].room);                   
                }
                //Khi Server nhận bài đánh ra từ các người chơi, Server sẽ broadcast cho các người chơi còn lại
                if(char.IsDigit(str[0])&&!str.Contains("win"))
                {
                    int sophong = danhSachNguoiChoi[pos].room;
                    int soLuongNguoiChoiTrongPhong = danhSachPhong[sophong].players.Count;
                    //set turn
                    danhSachPhong[sophong].turn = (danhSachPhong[sophong].turn + 1) % soLuongNguoiChoiTrongPhong;
                   
                    SetBoLuot(sophong);
                    int turn = danhSachPhong[sophong].turn;
                    //Gửi bài kèm theo lượt cho người chơi
                    socketList2[danhSachPhong[sophong].players[turn].pos].SendData(str + "turn");
                    //Gửi cho người chơi còn lại trong phòng
                    for (int i = 0; i < soLuongNguoiChoiTrongPhong; i++){
                        if (danhSachPhong[sophong].players[i].pos != pos || danhSachPhong[sophong].players[i].pos != turn )                   
                                socketList2[danhSachPhong[sophong].players[i].pos].SendData(str);
                    }
                }
                if(str=="boluot")
                {
                    int sophong = danhSachNguoiChoi[pos].room;
                    //Thêm ID của người chơi hiện tại về danh sách bỏ lượt
                    danhSachPhong[sophong].DanhSachBoLuot.Add(danhSachPhong[sophong].turn);
                    danhSachPhong[sophong].turn=(danhSachPhong[sophong].turn+1)% danhSachPhong[sophong].players.Count();

                    //Nếu tất cả các người chơi khác đã bỏ lượt thì set lượt mới
                    if (danhSachPhong[sophong].DanhSachBoLuot.Count() == danhSachPhong[sophong].players.Count() - 1){                       
                        danhSachPhong[sophong].DanhSachBoLuot.Clear();
                        socketList2[danhSachPhong[sophong].players[danhSachPhong[sophong].turn].pos].SendData("newturn");
                    }
                    else{
                        SetBoLuot(sophong);
                        socketList2[danhSachPhong[sophong].players[danhSachPhong[sophong].turn].pos].SendData("turn");
                    }
                }
                if(str.Contains("win"))
                {
                    int sophong = danhSachNguoiChoi[pos].room;
                    danhSachPhong[sophong].ResetRoom(danhSachPhong[sophong].turn);
                    for (int i = 0; i < danhSachPhong[sophong].players.Count(); i++)
                        if (danhSachPhong[sophong].players[i].pos != pos)
                            socketList2[danhSachPhong[sophong].players[i].pos].SendData(str);
                }
   
            }
        }
        public void BroadcastResult(int pos, string result)
        {
            socketList2[pos].SendData(result);
            //if (pos == 0)
            //{
            //    socketList2[1].SendData(result);
            //    return;
            //}
            //socketList2[0].SendData(result);
        }

      
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServer();
            Thread t = new Thread(ServeClients);
            t.Start();


            //BoBai x = new BoBai();
            //x.xaoBai();
            //textBox1.Text = x.inbai() + "fsdfsdfsdfsdf\r\n" + x.boBai[0].LayBai();

            //txbPort.Text = x.ktra();

            //Room r = new Room();
            //r.chiaBai();
            //r.xembai();
        }


    }
}
