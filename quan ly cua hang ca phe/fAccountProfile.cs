
using DTO;
using quan_ly_cua_hang_ca_phe.DAO;
using System;
using System.Windows.Forms;

namespace quan_ly_cua_hang_ca_phe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        // 👇 ĐÂY LÀ PHẦN BẠN ĐANG THIẾU 👇
        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        // 👆 --------------------------- 👆

        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txbUserName.Text = loginAccount.UserName;
            txbDisplayName.Text = loginAccount.DisplayName;
        }

        void UpdateAccountInfo()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassWord.Text;
            string newpass = txbNewPass.Text;
            string reenterPass = txbReEnterPass.Text;
            string userName = txbUserName.Text;

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
                return;
            }

            if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newpass))
            {
                MessageBox.Show("Cập nhật thành công!");
                if (updateAccount != null)
                    updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
            }
            else
            {
                MessageBox.Show("Vui lòng điền đúng mật khẩu cũ!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // 👇 CLASS NÀY CŨNG CẦN THIẾT
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }
        // Thêm hàm này vào để thỏa mãn Designer
        private void fAccountProfile_Load(object sender, EventArgs e)
        {

        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}