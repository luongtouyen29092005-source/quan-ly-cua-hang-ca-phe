
using DTO; 
using quan_ly_cua_hang_ca_phe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quan_ly_cua_hang_ca_phe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public fAdmin()
        {
            InitializeComponent();
            LoadState();
        }

        void LoadState()
        {
            dgvFood.DataSource = foodList;
            dgvCategory.DataSource = categoryList;
            dgvTable.DataSource = tableList;
            dgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            LoadListFood();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();

            LoadListCategory();
            AddCategoryBinding();

            LoadListTable();
            AddTableBinding();

            LoadListAccount();
            AddAccountBinding();
        }

        #region TAB 1: DOANH THU
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            string query = "SELECT t.TenBan AS [Tên Bàn], b.TongTien AS [Tổng Tiền], b.NgayLap AS [Ngày Lập], b.TrangThai AS [Trạng Thái] FROM HoaDon AS b, Ban AS t WHERE b.MaBan = t.MaBan AND b.TrangThai = 1 AND b.NgayLap >= @checkIn AND b.NgayLap <= @checkOut";
            dgvBill.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] { checkIn, checkOut });
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        #endregion

        #region TAB 2: THỨC ĂN
        void LoadListFood()
        {
            foodList.DataSource = DataProvider.Instance.ExecuteQuery("SELECT * FROM DoUong");
        }
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "TenDoUong", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "MaDoUong", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "DonGia", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = DataProvider.Instance.ExecuteQuery("SELECT * FROM DanhMuc");
            cb.DisplayMember = "TenDM";
            cb.ValueMember = "MaDM";
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["MaDM"].Value;
                    cbFoodCategory.SelectedValue = id;
                }
            }
            catch { }
        }
        private void btnShowFood_Click(object sender, EventArgs e) => LoadListFood();
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (int)cbFoodCategory.SelectedValue;
            float price = (float)nmFoodPrice.Value;
            string query = string.Format("INSERT INTO DoUong (TenDoUong, MaDM, DonGia) VALUES (N'{0}', {1}, {2})", name, categoryID, price);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Thêm thành công"); LoadListFood(); }
            else MessageBox.Show("Lỗi khi thêm");
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (int)cbFoodCategory.SelectedValue;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);
            string query = string.Format("UPDATE DoUong SET TenDoUong = N'{0}', MaDM = {1}, DonGia = {2} WHERE MaDoUong = {3}", name, categoryID, price, id);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Sửa thành công"); LoadListFood(); }
            else MessageBox.Show("Lỗi khi sửa");
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            string query = string.Format("DELETE DoUong WHERE MaDoUong = {0}", id);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Xóa thành công"); LoadListFood(); }
            else MessageBox.Show("Lỗi khi xóa");
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            string query = string.Format("SELECT * FROM DoUong WHERE TenDoUong LIKE N'%{0}%'", txbSearchFood.Text);
            foodList.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }
        #endregion

        #region TAB 3: DANH MỤC
        void LoadListCategory()
        {
            categoryList.DataSource = DataProvider.Instance.ExecuteQuery("SELECT * FROM DanhMuc");
        }
        void AddCategoryBinding()
        {
            txtCategoryID.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "MaDM", true, DataSourceUpdateMode.Never));
            txtCategoryName.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "TenDM", true, DataSourceUpdateMode.Never));
        }
        private void btnShowCategory_Click(object sender, EventArgs e) => LoadListCategory();
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string query = string.Format("INSERT INTO DanhMuc (TenDM) VALUES (N'{0}')", txtCategoryName.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Thêm thành công"); LoadListCategory(); LoadCategoryIntoCombobox(cbFoodCategory); }
            else MessageBox.Show("Thêm thất bại");
        }
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string query = string.Format("UPDATE DanhMuc SET TenDM = N'{0}' WHERE MaDM = {1}", txtCategoryName.Text, txtCategoryID.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Sửa thành công"); LoadListCategory(); LoadCategoryIntoCombobox(cbFoodCategory); }
            else MessageBox.Show("Sửa thất bại");
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("DELETE DanhMuc WHERE MaDM = {0}", txtCategoryID.Text);
                if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Xóa thành công"); LoadListCategory(); LoadCategoryIntoCombobox(cbFoodCategory); }
                else MessageBox.Show("Xóa thất bại");
            }
            catch { MessageBox.Show("Không thể xóa danh mục này vì đang chứa thức ăn!"); }
        }
        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            string query = string.Format("SELECT * FROM DanhMuc WHERE TenDM LIKE N'%{0}%'", txtSearchCategoryName.Text);
            categoryList.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }
        #endregion

        #region TAB 4: BÀN ĂN
        void LoadListTable()
        {
            tableList.DataSource = DataProvider.Instance.ExecuteQuery("SELECT * FROM Ban");
        }
        void AddTableBinding()
        {
            txtTableID.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "MaBan", true, DataSourceUpdateMode.Never));
            txtTableName.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "TenBan", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "TrangThai", true, DataSourceUpdateMode.Never));
        }
        private void btnShowTable_Click(object sender, EventArgs e) => LoadListTable();
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string query = string.Format("INSERT INTO Ban (TenBan) VALUES (N'{0}')", txtTableName.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Thêm thành công"); LoadListTable(); }
            else MessageBox.Show("Thêm thất bại");
        }
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string query = string.Format("UPDATE Ban SET TenBan = N'{0}', TrangThai = N'{1}' WHERE MaBan = {2}", txtTableName.Text, cbTableStatus.Text, txtTableID.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Sửa thành công"); LoadListTable(); }
            else MessageBox.Show("Sửa thất bại");
        }
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            string query = string.Format("DELETE Ban WHERE MaBan = {0}", txtTableID.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Xóa thành công"); LoadListTable(); }
            else MessageBox.Show("Xóa thất bại");
        }
        private void btnSeachTable_Click(object sender, EventArgs e)
        {
            string query = string.Format("SELECT * FROM Ban WHERE TenBan LIKE N'%{0}%'", txtSearchTableName.Text);
            tableList.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }
        #endregion

        #region TAB 5: TÀI KHOẢN
        void LoadListAccount()
        {
            accountList.DataSource = DataProvider.Instance.ExecuteQuery("SELECT TenDangNhap, TenHienThi, LoaiTaiKhoan FROM TaiKhoan");
        }
        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "TenDangNhap", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "TenHienThi", true, DataSourceUpdateMode.Never));
        }
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccount.SelectedCells.Count > 0)
                {
                    int type = (int)dgvAccount.SelectedCells[0].OwningRow.Cells["LoaiTaiKhoan"].Value;
                    cbAccountType.SelectedIndex = (type == 1) ? 0 : 1;
                }
            }
            catch { }
        }
        private void btnShowAccount_Click(object sender, EventArgs e) => LoadListAccount();
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            int type = cbAccountType.Text == "Admin" ? 1 : 0;
            string query = string.Format("INSERT INTO TaiKhoan (TenDangNhap, TenHienThi, LoaiTaiKhoan, MatKhau) VALUES (N'{0}', N'{1}', {2}, N'0')", txtUserName.Text, txtDisplayName.Text, type);
            try
            {
                if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Thêm thành công"); LoadListAccount(); }
            }
            catch { MessageBox.Show("Tên đăng nhập này đã tồn tại!"); }
        }
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            int type = cbAccountType.Text == "Admin" ? 1 : 0;
            string query = string.Format("UPDATE TaiKhoan SET TenHienThi = N'{0}', LoaiTaiKhoan = {1} WHERE TenDangNhap = N'{2}'", txtDisplayName.Text, type, txtUserName.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Sửa thành công"); LoadListAccount(); }
            else MessageBox.Show("Sửa thất bại");
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string query = string.Format("DELETE TaiKhoan WHERE TenDangNhap = N'{0}'", txtUserName.Text);
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0) { MessageBox.Show("Xóa thành công"); LoadListAccount(); }
            else MessageBox.Show("Xóa thất bại");
        }
        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            // 1. Lấy tên tài khoản đang chọn
            string userName = txtUserName.Text;

            // 2. Lấy mật khẩu mới từ ô nhập liệu
            string newPass = txbResetPass.Text;

            // Kiểm tra xem đã nhập mật khẩu chưa
            if (string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới vào ô bên cạnh!", "Thông báo");
                return;
            }

            // 3. Câu lệnh SQL cập nhật mật khẩu
            string query = string.Format("UPDATE TaiKhoan SET MatKhau = N'{0}' WHERE TenDangNhap = N'{1}'", newPass, userName);

            // 4. Thực thi
            if (DataProvider.Instance.ExecuteNonQuery(query) > 0)
            {
                MessageBox.Show("Đặt lại mật khẩu thành công! Mật khẩu mới là: " + newPass);
                txbResetPass.Text = ""; // Xóa ô nhập sau khi xong cho sạch
            }
            else
            {
                MessageBox.Show("Lỗi khi đặt lại mật khẩu!");
            }
        }
        private void btnSearchAccount_Click(object sender, EventArgs e)
        {
            string query = string.Format("SELECT TenDangNhap, TenHienThi, LoaiTaiKhoan FROM TaiKhoan WHERE TenHienThi LIKE N'%{0}%'", txtSearchAccount.Text);
            accountList.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }
        #endregion

        // Event rác designer
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void txtSearchTableName_TextChanged(object sender, EventArgs e) { }
    }
}