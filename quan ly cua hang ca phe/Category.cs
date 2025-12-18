using System.Data;

namespace DTO
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public Category(DataRow row)
        {
            this.ID = (int)row["MaDM"];
            this.Name = row["TenDM"].ToString();
        }
    }
}