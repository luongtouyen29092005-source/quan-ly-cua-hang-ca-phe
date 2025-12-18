using System;
using System.Data;

namespace DTO
{
    public class Menu
    {
        public string FoodName { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }

        public Menu(string foodName, int count, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            this.FoodName = row["TenDoUong"].ToString();
            this.Count = (int)row["SoLuong"];
            this.Price = (float)Convert.ToDouble(row["DonGia"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["ThanhTien"].ToString());
        }
    }
}