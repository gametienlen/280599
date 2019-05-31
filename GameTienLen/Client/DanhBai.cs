using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class DanhBai : Form
    {
        private TCPModel tcpForPlayer;
        private TCPModel tcpForOpponent;
        private List<double> BaiHienTai = new List<double>();
        private List<double> BaiCuaDoiThu = new List<double>();
        int Y;
        int X;

        List<PictureBox> pictureBoxList1 = new List<PictureBox>();
        List<PictureBox> pictureBoxList2 = new List<PictureBox>();


        string path = @"C:\Users\DELL\OneDrive\Desktop\Game2\GameTienLen\Client\Resources\";
        public DanhBai(TCPModel player, TCPModel Opponent)
        {
            InitializeComponent();
            tcpForPlayer = player;
            tcpForOpponent = Opponent;
            btnDanh.Enabled = false;
            btnBoLuot.Enabled = false;
            if (tcpForPlayer.ReadData() == "isplaying")
                btnReady.Enabled = false;

            Y = pictureBox1.Location.Y;
            Thread t = new Thread(NhanBai);
            t.Start();
        }

        private void btnReady_Click(object sender, EventArgs e)
        {          
            tcpForPlayer.SendData("chiabai");
            string result = tcpForPlayer.ReadData();
            pictureBoxList1.Add(pictureBox1);
            pictureBoxList1.Add(pictureBox2);
            pictureBoxList1.Add(pictureBox3);
            pictureBoxList1.Add(pictureBox4);
            pictureBoxList1.Add(pictureBox5);
            pictureBoxList1.Add(pictureBox6);
            pictureBoxList1.Add(pictureBox7);
            pictureBoxList1.Add(pictureBox8);
            pictureBoxList1.Add(pictureBox9);
            pictureBoxList1.Add(pictureBox10);
            pictureBoxList1.Add(pictureBox11);
            pictureBoxList1.Add(pictureBox12);
            pictureBoxList1.Add(pictureBox13);
            pictureBoxList2.Add(pictureBox14);
            pictureBoxList2.Add(pictureBox15);
            pictureBoxList2.Add(pictureBox16);
            pictureBoxList2.Add(pictureBox17);
            pictureBoxList2.Add(pictureBox18);
            pictureBoxList2.Add(pictureBox19);
            pictureBoxList2.Add(pictureBox20);
            pictureBoxList2.Add(pictureBox21);
            pictureBoxList2.Add(pictureBox22);
            pictureBoxList2.Add(pictureBox23);
            pictureBoxList2.Add(pictureBox24);
            pictureBoxList2.Add(pictureBox25);
            pictureBoxList2.Add(pictureBox26);
            //Nếu người chơi nhận được lượt đánh thì enable btbDanh
            if (result.Contains("turn")){
                result = result.Remove(result.Length - 4);//Xóa 4 kí tự cuối chuối(là string "turn")
                btnDanh.Enabled = true;
            }   
            
            ConvertToListDouble(BaiHienTai, result);//Convert từ string sang List<double>
            //Kiểm tra xem bài của người cho có tới trắng hay không
            if (XuLyBai.KiemTraToiTrang(BaiHienTai) == true)
                tcpForPlayer.SendData(ConvertToString(BaiHienTai) + "wintrang");

            txbBai.Text = ConvertToString(BaiHienTai);//gọi hàm load hình LoadHinh(BaiHienTai)  
            for(int i=0;i<BaiHienTai.Count();i++)
            {
                pictureBoxList1[i].Image = Image.FromFile(path + BaiHienTai[i].ToString() + ".png");
                pictureBoxList1[i].BackColor = Color.White;
                pictureBoxList1[i].BorderStyle = BorderStyle.FixedSingle;
                pictureBoxList1[i].Tag = BaiHienTai[i];
            }
            btnReady.Enabled = false;
        }
       
        void NhanBai()//Dùng để nhận bài của đối thủ
        {
            while (true)
            {
                string result = tcpForOpponent.ReadData();//Bài của đối thủ vừa đánh
                //Nếu nhận được "newturn" thì người chơi có quyền đánh những lá tùy ý.
                if (result == "newturn"){
                    BaiCuaDoiThu.Clear();
                    btnDanh.Enabled = true;
                }
                //Trường người chơi trước bỏ lượt, và lượt hiện tại của người chơi này
                else if (result == "turn"){
                    btnDanh.Enabled = true;
                    btnBoLuot.Enabled = true;
                }
                //Trường hợp tới lượt đánh của người chơi
                else if (result.Contains("turn")){                   
                    result = result.Remove(result.Length - 4);//Xóa 4 kí tự cuối chuối(là string "turn")
                    btnDanh.Enabled = true;
                    btnBoLuot.Enabled = true;
                    // Gọi hàm load hình: LoadHinh(BaiCuaDoiThu);
                    //for (int i = 0; i < 13; i++)
                    //{
                    //    pictureBoxList2[i].Image = null;
                    //    pictureBoxList2[i].BackColor = DefaultBackColor;
                    //    pictureBoxList2[i].BorderStyle = BorderStyle.None;
                    //    pictureBoxList2[i].Tag = null;
                    //}

                    //for (int i=0;i<BaiCuaDoiThu.Count();i++)
                    //{
                    //    pictureBoxList2[i].Image = Image.FromFile(path + BaiCuaDoiThu[i].ToString() + ".png");
                    //    pictureBoxList2[i].BackColor = Color.White;
                    //    pictureBoxList2[i].BorderStyle = BorderStyle.FixedSingle;
                    //    pictureBoxList2[i].Tag = BaiCuaDoiThu[i];
                    //}
                }
                //Đây là trường hợp đã có người chơi trong bàn win. Tiến hành Load bài đó và set lại game
                else if (result.Contains("win")){
                    result = result.Remove(result.Length - 3);                   
                    ConvertToListDouble(BaiCuaDoiThu, result);
                    txtBaiCuaDoiThu.Text = ConvertToString(BaiCuaDoiThu);//Load bài của đối thủ

                    //for (int i = 0; i < 13; i++)
                    //{
                    //    pictureBoxList2[i].Image = null;
                    //    pictureBoxList2[i].BackColor = DefaultBackColor;
                    //    pictureBoxList2[i].BorderStyle = BorderStyle.None;
                    //    pictureBoxList2[i].Tag = null;
                    //}

                    //for (int i = 0; i<BaiCuaDoiThu.Count();i++)
                    //{
                    //    pictureBoxList2[i].Image = Image.FromFile(path + BaiCuaDoiThu[i].ToString() + ".png");
                    //    pictureBoxList2[i].BackColor = Color.White;
                    //    pictureBoxList2[i].BorderStyle = BorderStyle.FixedSingle;
                    //    pictureBoxList2[i].Tag = BaiCuaDoiThu[i];
                    //}
                    BaiHienTai.Clear();
                    BaiCuaDoiThu.Clear();
                    btnDanh.Enabled = false;
                    btnBoLuot.Enabled = false;
                    btnReady.Enabled = true;
                }
                //Đây là trường hợp người chơi không có lượt đánh. Chỉ nhận được bài của đối thủ
                else if (char.IsDigit(result[0])){
                    ConvertToListDouble(BaiCuaDoiThu, result);
                    txtBaiCuaDoiThu.Text = ConvertToString(BaiCuaDoiThu);//Load bài của đối thủ
                    //for (int i = 0; i < 13; i++)
                    //{
                    //    pictureBoxList2[i].Image = null;
                    //    pictureBoxList2[i].BackColor = DefaultBackColor;
                    //    pictureBoxList2[i].BorderStyle = BorderStyle.None;
                    //    pictureBoxList2[i].Tag = null;
                    //}

                    //for (int i = 0; i < BaiCuaDoiThu.Count(); i++)
                    //{
                    //    pictureBoxList2[i].Image = Image.FromFile(path + BaiCuaDoiThu[i].ToString() + ".png");
                    //    pictureBoxList2[i].BackColor = Color.White;
                    //    pictureBoxList2[i].BorderStyle = BorderStyle.FixedSingle;
                    //    pictureBoxList2[i].Tag = BaiCuaDoiThu[i];
                    //}
                }
            }
        }
        private void btnBoLuot_Click(object sender, EventArgs e)
        {
            btnDanh.Enabled = false;
            tcpForPlayer.SendData("boluot");
            btnBoLuot.Enabled = false;
        }
        private void btnDanh_Click(object sender, EventArgs e)
        {
            Danhbai();
        }
       
        void Danhbai()
        {
            List<double> Buffer = new List<double>();
            //Viết xử lý ở đây: 
            //Khi bấm vào 1 quân bài thì sẽ có 1 double được add về buffer
            string input = txtDanh.Text;
            input = input.Trim();
            string[] Input = input.Split(' ');
            foreach (var i in Input)
                Buffer.Add(Convert.ToDouble(i));          
            string flag = "";
           if (XuLyBai.KiemTraHopLe(Buffer, BaiCuaDoiThu) == true)//Nếu những quân bài đánh ra hợp lệ thì làm 3 viêc: 1. Xóa những quân bài đã đánh ra khỏi List BaiHienTai; 2.Nếu số quân bài còn lại ==0 thì gán thêm flag win cho server; 3. Trả về kết quả đánh
            {
                
                for (int i = 0; i < Buffer.Count(); i++)
                {
                    pictureBoxList2[i].Image = Image.FromFile(path + Buffer[i].ToString() + ".png");
                    pictureBoxList2[i].BackColor = Color.White;
                    pictureBoxList2[i].BorderStyle = BorderStyle.FixedSingle;
                }



                foreach (var i in Buffer)
                    BaiHienTai.Remove(i);
                BaiHienTai.Sort();

                for (int i = 0; i < 13; i++)
                {
                    pictureBoxList1[i].Image = null;
                    pictureBoxList1[i].BackColor = DefaultBackColor;
                    pictureBoxList1[i].BorderStyle = BorderStyle.None;
                    pictureBoxList1[i].Location = new Point(pictureBoxList1[i].Location.X, Y);
                    pictureBoxList1[i].Tag = null;
                    txtDanh.Clear();
                }

                for (int i = 0; i < BaiHienTai.Count(); i++)
                {

                    pictureBoxList1[i].Image = Image.FromFile(path + BaiHienTai[i].ToString() + ".png");
                    pictureBoxList1[i].BackColor = Color.White;
                    pictureBoxList1[i].BorderStyle = BorderStyle.FixedSingle;
                    pictureBoxList1[i].Tag = BaiHienTai[i];

                }
                //Gọi hàm load hình LoadHinh(Buffer)

                txbBai.Text = ConvertToString(BaiHienTai);// gọi hàm load hinh; LoadHinh(BaiHienTai)
                if (BaiHienTai.Count == 0){
                    flag = "win";
                    BaiCuaDoiThu.Clear();
                    btnReady.Enabled = true;
                }
                tcpForPlayer.SendData(ConvertToString(Buffer)+flag);
                Buffer.Clear();
                btnDanh.Enabled = false;
                btnBoLuot.Enabled = false;
            }            
        }

        private void fClick_pBox1(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Location.Y == Y)
            {
                ((PictureBox)sender).Location = new Point(((PictureBox)sender).Location.X, ((PictureBox)sender).Location.Y - 20);
                txtDanh.Text = txtDanh.Text + ((PictureBox)sender).Tag.ToString() + ' ';
                return;
            }
            if (((PictureBox)sender).Location.Y < Y)
            {
                int lengthLaBai = ((PictureBox)sender).Tag.ToString().Count() + 1;
                ((PictureBox)sender).Location = new Point(((PictureBox)sender).Location.X, ((PictureBox)sender).Location.Y + 20);
                txtDanh.Text = txtDanh.Text.Remove(txtDanh.Text.Length - lengthLaBai);
                return;
            }
        }

        void ConvertToListDouble(List <double> Cards, string String)
        {
            BaiCuaDoiThu.Clear();
            String = String.Trim();
            string[] Result = String.Split('\t');
            for (int i = 0; i < Result.Length; i++){
                Cards.Add(Convert.ToDouble(Result[i]));
            }
           Cards.Sort();
        }
        string ConvertToString(List<double> Cards)
        {
            string String = "";
            foreach (var i in Cards)
                String += i.ToString() + "\t";
            return String;

        }
    }
}
