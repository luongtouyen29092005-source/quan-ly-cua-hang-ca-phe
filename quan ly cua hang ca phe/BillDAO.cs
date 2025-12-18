using DTO; // Gọi thư viện DTO để dùng class Bill
using quan_ly_cua_hang_ca_phe.DAO;
using System.Data;

namespace quan_ly_cua_hang_ca_phe.DAO
{
    public class BillDAO
    {
        // Singleton
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        // 1. Hàm lấy ID hóa đơn chưa thanh toán
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM HoaDon WHERE MaBan = " + id + " AND TrangThai = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }

        // 2. Thêm hóa đơn mới (Gọi thủ tục SQL để vừa thêm hóa đơn, vừa đổi màu bàn)
        public void InsertBill(int id)
        {
            // USP_InsertBill trong SQL phải có lệnh: UPDATE Ban SET TrangThai = 'Có người'...
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable", new object[] { id });
        }

        // 3. Hàm thanh toán (Cập nhật tiền và trả bàn về trống)
        public void CheckOut(int idBill, int idTable, float totalPrice)
        {
            // USP_CheckOut trong SQL phải có lệnh: UPDATE Ban SET TrangThai = 'Trống'...
            string query = "EXEC USP_CheckOut @idBill , @totalPrice , @idTable";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { idBill, totalPrice, idTable });
        }

        // 4. Lấy ID của hóa đơn lớn nhất
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(MaHD) FROM HoaDon");
            }
            catch
            {
                return 1;
            }
        }
    }
}