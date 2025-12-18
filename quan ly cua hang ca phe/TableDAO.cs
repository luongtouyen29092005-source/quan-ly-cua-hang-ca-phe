using DTO; // Gọi DTO để dùng class Table
using quan_ly_cua_hang_ca_phe.DAO;
using System.Collections.Generic;
using System.Data;

namespace quan_ly_cua_hang_ca_phe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set { instance = value; }
        }
        // Hàm gọi thủ tục chuyển bàn
        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });
        }

        // Định nghĩa chiều rộng/cao của nút Bàn (để vẽ lên giao diện)
        public static int TableWidth = 100;
        public static int TableHeight = 100;

        private TableDAO() { }

        // Hàm lấy danh sách bàn
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Ban");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
    }
}