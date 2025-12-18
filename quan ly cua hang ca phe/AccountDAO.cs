using DTO;
using System;
using System.Data; // Cần thiết để dùng DataTable

//  Thêm .DAO vào sau để phân biệt đây là thư mục DAO
namespace quan_ly_cua_hang_ca_phe.DAO
{
    public class AccountDAO
    {
        // 1. Tạo Singleton (Để chỉ có 1 instance duy nhất chạy trong chương trình)
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return instance;
            }
            private set { instance = value; }
        }
        // Hàm lấy thông tin tài khoản từ Username
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM TaiKhoan WHERE TenDangNhap = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }
        // Hàm cập nhật thông tin tài khoản
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            // Logic: Cập nhật Tên hiển thị và Mật khẩu mới
            // ĐIỀU KIỆN: Tên đăng nhập phải đúng VÀ Mật khẩu cũ phải đúng
            string query = "UPDATE TaiKhoan SET TenHienThi = N'" + displayName + "', MatKhau = N'" + newPass + "' WHERE TenDangNhap = N'" + userName + "' AND MatKhau = N'" + pass + "'";

            // Thực thi lệnh
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            // Nếu số dòng bị ảnh hưởng > 0 nghĩa là cập nhật thành công
            return result > 0;
        }
        private AccountDAO() { }

        // 2. Hàm xử lý Đăng Nhập
        public bool Login(string userName, string passWord)
        {
            // Câu lệnh SQL lấy dữ liệu
            string query = "SELECT * FROM TaiKhoan WHERE TenDangNhap = @userName AND MatKhau = @passWord";

            // Gọi DataProvider để thực thi lệnh (Nhớ đảm bảo file DataProvider cũng cùng namespace này)
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });

            // Nếu số dòng tìm thấy > 0 nghĩa là có tài khoản đó -> Return True
            return result.Rows.Count > 0;
        }
    }
}
