using System.Data;

namespace DTO
{
    public class Table
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Table(int id, string name, string status)
        {
            this.ID = id;
            this.Name = name;
            this.Status = status;
        }

        // Hàm chuyển từ dòng dữ liệu SQL sang Object Table
        public Table(DataRow row)
        {
            this.ID = (int)row["MaBan"];
            this.Name = row["TenBan"].ToString();
            this.Status = row["TrangThai"].ToString();
        }
    }
}