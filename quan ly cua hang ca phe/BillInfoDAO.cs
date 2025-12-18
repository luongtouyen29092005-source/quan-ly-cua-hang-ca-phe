using DTO; // Nhớ using
using quan_ly_cua_hang_ca_phe.DAO;
using System.Data;

namespace quan_ly_cua_hang_ca_phe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
            private set { instance = value; }
        }
        private BillInfoDAO() { }

        // Hàm thêm món ăn vào hóa đơn
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            // Gọi thủ tục vừa tạo ở trên
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
