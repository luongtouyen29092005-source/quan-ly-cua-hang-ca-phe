using System.Data;

namespace DAO
    public class Account
    {
        // Các thuộc tính tương ứng với cột trong bảng TaiKhoan ở SQL
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public int Type { get; set; } // 1: Admin, 0: Staff

        public Account(string userName, string displayName, int type, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        // Hàm này quan trọng: Giúp chuyển 1 dòng dữ liệu SQL thành đối tượng Account
        public Account(DataRow row)
        {
            this.UserName = row["TenDangNhap"].ToString();
            this.DisplayName = row["TenHienThi"].ToString();
            this.Type = (int)row["LoaiTaiKhoan"];
            this.Password = row["MatKhau"].ToString();
        }
    }
}
