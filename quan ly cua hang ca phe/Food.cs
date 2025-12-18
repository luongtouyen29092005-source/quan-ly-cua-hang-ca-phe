using System;
using System.Data;

namespace DTO
{
    public class Food
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public float Price { get; set; }

        public Food(int id, string name, int categoryID, float price)
        {
            this.ID = id;
            this.Name = name;
            this.CategoryID = categoryID;
            this.Price = price;
        }

        public Food(DataRow row)
        {
            this.ID = (int)row["MaDoUong"];
            this.Name = row["TenDoUong"].ToString();
            this.CategoryID = (int)row["MaDM"];
            this.Price = (float)Convert.ToDouble(row["DonGia"].ToString());
        }
    }
}