using DTO; 
using System;
using System.Collections.Generic;
using System.Data;

namespace quan_ly_cua_hang_ca_phe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;
        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            private set { instance = value; }
        }

        private MenuDAO() { }
        public List<DTO.Menu> GetListMenuByTable(int id)
        {
            List<DTO.Menu> listMenu = new List<DTO.Menu>();

            string query = "SELECT f.TenDoUong, bi.SoLuong, f.DonGia, f.DonGia*bi.SoLuong AS ThanhTien " +
                           "FROM ChiTietHoaDon AS bi, HoaDon AS b, DoUong AS f " +
                           "WHERE bi.MaHD = b.MaHD AND bi.MaDoUong = f.MaDoUong AND b.TrangThai = 0 AND b.MaBan = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                DTO.Menu menu = new DTO.Menu(item);
                listMenu.Add(menu);
            }
            return listMenu;
        }
    }
}