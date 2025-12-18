
using quan_ly_cua_hang_ca_phe.DAO; // Sử dụng namespace DAO ngắn gọn
using DTO;
using quan_ly_cua_hang_ca_phe.DAO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace quan_ly_cua_hang_ca_phe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount; // Biến lưu tài khoản đăng nhập
        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.loginAccount = acc; // Lưu tài khoản lại

            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable); // Load danh sách bàn để chuyển

            ChangeAccount(loginAccount.Type); // Phân quyền Admin/Staff
        }

        #region Method

        // Hàm phân quyền: 1 = Admin, 0 = Staff
        void ChangeAccount(int type)
        {
            // Nếu là Admin (1) thì enable = true, Staff (0) thì false
            adminToolStripMenuItem.Enabled = type == 1;

            // Hiển thị tên người dùng lên menu cho đẹp
            thongTinTaiKhoanToolStripMenuItem.Text += " (" + loginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống": btn.BackColor = Color.Aqua; break;
                    default: btn.BackColor = Color.LightPink; break;
                }
                flpTable.Controls.Add(btn);
            }
        }

        // Hàm load ComboBox chuyển bàn (Bạn bị thiếu hàm này trong code gửi lên)
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;

            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        #endregion

        #region Events

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category;
            if (selected == null) return; // Tránh lỗi null

            int id = selected.ID;
            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn trước khi thêm món!");
                return;
            }

            if (cbFood.SelectedItem == null) return; // Kiểm tra chọn món

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                int billMaxID = BillDAO.Instance.GetMaxIDBill();
                BillInfoDAO.Instance.InsertBillInfo(billMaxID, foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) return;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            // Xử lý chuỗi tiền tệ
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", "").Replace("₫", "").Trim());
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - Giảm giá ({1}%) = {2}", table.Name, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, table.ID, (float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) { MessageBox.Show("Hãy chọn bàn cần chuyển!"); return; }

            int id1 = table.ID;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;

            if (MessageBox.Show(string.Format("Bạn muốn chuyển bàn {0} qua bàn {1}?", table.Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void thongTinCaNhanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);

            // Đăng ký sự kiện: Khi fAccountProfile cập nhật xong thì chạy hàm f_UpdateAccount
            f.UpdateAccount += f_UpdateAccount;

            f.ShowDialog();
        }

        // Hàm xử lý cập nhật lại thông tin trên Form chính
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thongTinTaiKhoanToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void dangXuatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e) { }

        #endregion
    }
}