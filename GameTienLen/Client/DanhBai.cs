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
        string path = @"C:\Users\ASUS\Desktop\GameTienLen\GameTienLen\Client\Resources\";
        public DanhBai(TCPModel player, TCPModel Opponent)
        {
            InitializeComponent();
            tcpForPlayer = player;
            tcpForOpponent = Opponent;
            btnDanh.Enabled = false;
            btnBoLuot.Enabled = false;
            Thread t = new Thread(NhanBai);
            t.Start();
            Y = pictureBox1.Location.Y;
        }

        private void btnReady_Click(object sender, EventArgs e)
        {          
            tcpForPlayer.SendData("chiabai");
            string result = tcpForPlayer.ReadData();

            //Nếu người chơi nhận được lượt đánh thì enable btbDanh
            if(result.Contains("turn")){
                result = result.Remove(result.Length - 4);//Xóa 4 kí tự cuối chuối(là string "turn")
                btnDanh.Enabled = true;
            }   
            
            ConvertToListDouble(BaiHienTai, result);//Convert từ string sang List<double>
            //Kiểm tra xem bài của người cho có tới trắng hay không
            if (XuLyBai.KiemTraToiTrang(BaiHienTai) == true)
                tcpForPlayer.SendData(ConvertToString(BaiHienTai) + "wintrang");

            txbBai.Text = ConvertToString(BaiHienTai);//gọi hàm load hình LoadHinh(BaiHienTai)   
            pictureBox1.Image = Image.FromFile(path + BaiHienTai[0].ToString() + ".png");
            pictureBox1.BackColor = Color.White;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Tag = BaiHienTai[0];
            pictureBox2.Image = Image.FromFile(path + BaiHienTai[1].ToString() + ".png");
            pictureBox2.BackColor = Color.White;
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Tag = BaiHienTai[1];
            pictureBox3.Image = Image.FromFile(path + BaiHienTai[2].ToString() + ".png");
            pictureBox3.BackColor = Color.White;
            pictureBox3.BorderStyle = BorderStyle.FixedSingle;
            pictureBox3.Tag = BaiHienTai[2];
            pictureBox4.Image = Image.FromFile(path + BaiHienTai[3].ToString() + ".png");
            pictureBox4.BackColor = Color.White;
            pictureBox4.BorderStyle = BorderStyle.FixedSingle;
            pictureBox4.Tag = BaiHienTai[3];
            pictureBox5.Image = Image.FromFile(path + BaiHienTai[4].ToString() + ".png");
            pictureBox5.BackColor = Color.White;
            pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            pictureBox5.Tag = BaiHienTai[4];
            pictureBox6.Image = Image.FromFile(path + BaiHienTai[5].ToString() + ".png");
            pictureBox6.BackColor = Color.White;
            pictureBox6.BorderStyle = BorderStyle.FixedSingle;
            pictureBox6.Tag = BaiHienTai[5];
            pictureBox7.Image = Image.FromFile(path + BaiHienTai[6].ToString() + ".png");
            pictureBox7.BackColor = Color.White;
            pictureBox7.BorderStyle = BorderStyle.FixedSingle;
            pictureBox7.Tag = BaiHienTai[6];
            pictureBox8.Image = Image.FromFile(path + BaiHienTai[7].ToString() + ".png");
            pictureBox8.BackColor = Color.White;
            pictureBox8.BorderStyle = BorderStyle.FixedSingle;
            pictureBox8.Tag = BaiHienTai[7];
            pictureBox9.Image = Image.FromFile(path + BaiHienTai[8].ToString() + ".png");
            pictureBox9.BackColor = Color.White;
            pictureBox9.BorderStyle = BorderStyle.FixedSingle;
            pictureBox9.Tag = BaiHienTai[8];
            pictureBox10.Image = Image.FromFile(path + BaiHienTai[9].ToString() + ".png");
            pictureBox10.BackColor = Color.White;
            pictureBox10.BorderStyle = BorderStyle.FixedSingle;
            pictureBox10.Tag = BaiHienTai[9];
            pictureBox11.Image = Image.FromFile(path + BaiHienTai[10].ToString() + ".png");
            pictureBox11.BackColor = Color.White;
            pictureBox11.BorderStyle = BorderStyle.FixedSingle;
            pictureBox11.Tag = BaiHienTai[10];
            pictureBox12.Image = Image.FromFile(path + BaiHienTai[11].ToString() + ".png");
            pictureBox12.BackColor = Color.White;
            pictureBox12.BorderStyle = BorderStyle.FixedSingle;
            pictureBox12.Tag = BaiHienTai[11];
            pictureBox13.Image = Image.FromFile(path + BaiHienTai[12].ToString() + ".png");
            pictureBox13.BackColor = Color.White;
            pictureBox13.BorderStyle = BorderStyle.FixedSingle;
            pictureBox13.Tag = BaiHienTai[12];
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
                }
                //Đây là trường hợp đã có người chơi trong bàn win. Tiến hành Load bài đó và set lại game
                else if (result.Contains("win")){
                    result = result.Remove(result.Length - 3);                   
                    ConvertToListDouble(BaiCuaDoiThu, result);
                    txtBaiCuaDoiThu.Text = ConvertToString(BaiCuaDoiThu);//Load bài của đối thủ

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
            string[] Input = input.Split('\t');
            foreach (var i in Input)
                Buffer.Add(Convert.ToDouble(i));          
            string flag = "";
           if (XuLyBai.KiemTraHopLe(Buffer, BaiCuaDoiThu) == true)//Nếu những quân bài đánh ra hợp lệ thì làm 3 viêc: 1. Xóa những quân bài đã đánh ra khỏi List BaiHienTai; 2.Nếu số quân bài còn lại ==0 thì gán thêm flag win cho server; 3. Trả về kết quả đánh
            {
                List<PictureBox> pictureBoxList = new List<PictureBox>();

                pictureBoxList.Add(pictureBox1);
                pictureBoxList.Add(pictureBox2);
                pictureBoxList.Add(pictureBox3);
                pictureBoxList.Add(pictureBox4);
                pictureBoxList.Add(pictureBox5);
                pictureBoxList.Add(pictureBox6);
                pictureBoxList.Add(pictureBox7);
                pictureBoxList.Add(pictureBox8);
                pictureBoxList.Add(pictureBox9);
                pictureBoxList.Add(pictureBox10);
                pictureBoxList.Add(pictureBox11);
                pictureBoxList.Add(pictureBox12);
                pictureBoxList.Add(pictureBox13);
                pictureBoxList.Add(pictureBox14);
                pictureBoxList.Add(pictureBox15);
                pictureBoxList.Add(pictureBox16);
                pictureBoxList.Add(pictureBox17);
                pictureBoxList.Add(pictureBox18);
                pictureBoxList.Add(pictureBox19);
                pictureBoxList.Add(pictureBox20);
                pictureBoxList.Add(pictureBox21);
                pictureBoxList.Add(pictureBox22);
                pictureBoxList.Add(pictureBox23);
                pictureBoxList.Add(pictureBox24);
                pictureBoxList.Add(pictureBox25);
                pictureBoxList.Add(pictureBox26);

                int j = 25;
                for (int i = 0; i < Buffer.Count(); i++)
                {

                    pictureBoxList[j].Image = Image.FromFile(path + Buffer[0].ToString() + ".png");
                    pictureBoxList[j].BackColor = Color.White;
                    pictureBoxList[j].BorderStyle = BorderStyle.FixedSingle;
                    --j;
                }



                foreach (var i in Buffer)
                    BaiHienTai.Remove(i);

                for (int i = 0; i < BaiHienTai.Count(); i++)
                {

                    pictureBoxList[i].Image = Image.FromFile(path + BaiHienTai[0].ToString() + ".png");
                    pictureBoxList[i].BackColor = Color.White;
                    pictureBoxList[i].BorderStyle = BorderStyle.FixedSingle;

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
