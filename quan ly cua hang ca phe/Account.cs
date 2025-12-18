using System.Data;

namespace DTO  
{
    public class Account
    {
        // 1. Các thuộc tính
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public int Type { get; set; } // 1: Admin, 0: Staff

        // 2. Constructor thường (Tạo đối tượng thủ công)
        public Account(string userName, string displayName, int type, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        // 3. Constructor từ DataRow (QUAN TRỌNG: Để chuyển dữ liệu từ SQL thành Object)
        public Account(DataRow row)
        {
            this.UserName = row["TenDangNhap"].ToString();
            this.DisplayName = row["TenHienThi"].ToString();
            this.Type = (int)row["LoaiTaiKhoan"];
            this.Password = row["MatKhau"].ToString();
        }
    }
}