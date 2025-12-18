using System;
using System.Windows.Forms;
using quan_ly_cua_hang_ca_phe.DAO; 
using DTO;

namespace quan_ly_cua_hang_ca_phe
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        // Sự kiện nút Đăng Nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string passWord = txbPassWord.Text;

            if (Login(userName, passWord))
            {
                // 1. Lấy thông tin tài khoản
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);

                // 2. Truyền vào fTableManager
                fTableManager f = new fTableManager(loginAccount); // 👈 QUAN TRỌNG

                this.Hide();
                f.ShowDialog();
                this.Show();
        }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }
        }

        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        // Sự kiện nút Thoát
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Sự kiện đóng form
        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}