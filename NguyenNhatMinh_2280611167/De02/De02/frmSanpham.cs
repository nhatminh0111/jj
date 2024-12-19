using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De02
{
    public partial class frmSanpham : Form
    {
        public frmSanpham()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmSanpham_Load(object sender, EventArgs e)
        {
            dgvSanpham.Columns.Clear();
            dgvSanpham.Columns.Add("MaSP", "Mã SP");
            dgvSanpham.Columns.Add("TenSP", "Tên SP");
            dgvSanpham.Columns.Add("NgayNhap", "Ngày Nhập");
            dgvSanpham.Columns.Add("LoaiSP", "Loại SP");
            LoadSanpham();
            LoadLoaiSP();
            dgvSanpham.CellValueChanged += dgvSanpham_CellValueChanged;
            dgvSanpham.CurrentCellDirtyStateChanged += dgvSanpham_CurrentCellDirtyStateChanged;

            SetButtonState(false); // Mặc định không chỉnh sửa
        }
        private void LoadSanpham()
        {
            using (var context = new Model1())
            {
                var sanphams = from sp in context.Sanphams
                               join loai in context.LoaiSPs on sp.MaLoai equals loai.MaLoai
                               select new
                               {
                                   sp.MaSP,
                                   sp.TenSP,
                                   sp.Ngaynhap,
                                   TenLoai = loai.TenLoai
                               };

                dgvSanpham.Rows.Clear();
                foreach (var sp in sanphams)
                {
                    dgvSanpham.Rows.Add(sp.MaSP, sp.TenSP, sp.Ngaynhap.ToString("dd/MM/yyyy"), sp.TenLoai);
                }
            }
        }



        // Load danh sách loại sản phẩm
        private void LoadLoaiSP()
        {
            using (var context = new Model1())
            {
                var loaiSPs = context.LoaiSPs.ToList();

                if (loaiSPs.Count == 0)
                {
                    MessageBox.Show("Không có loại sản phẩm nào trong cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cboLoaiSP.DataSource = loaiSPs;
                cboLoaiSP.DisplayMember = "TenLoai"; // Hiển thị tên loại sản phẩm
                cboLoaiSP.ValueMember = "MaLoai";   // Giá trị là mã loại
                cboLoaiSP.SelectedIndex = -1;       // Không chọn mục nào mặc định
            }
        }

        private void dgvSanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanpham.Rows[e.RowIndex];

                txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? string.Empty;
                txtTenSP.Text = row.Cells["TenSP"].Value?.ToString() ?? string.Empty;

                if (row.Cells["NgayNhap"].Value != null && !string.IsNullOrWhiteSpace(row.Cells["NgayNhap"].Value.ToString()))
                {
                    if (DateTime.TryParse(row.Cells["NgayNhap"].Value.ToString(), out DateTime ngayNhap))
                    {
                        dtNgaynhap.Value = ngayNhap;
                    }
                    else
                    {
                        MessageBox.Show("Ngày nhập không hợp lệ. Vui lòng kiểm tra dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dtNgaynhap.Value = DateTime.Now; // Gán giá trị mặc định nếu lỗi
                    }
                }
                else
                {
                    dtNgaynhap.Value = DateTime.Now; // Gán giá trị mặc định nếu dữ liệu null hoặc rỗng
                }


                cboLoaiSP.Text = row.Cells["LoaiSP"].Value?.ToString() ?? string.Empty;

                // Kích hoạt các nút Sửa, Xóa, Lưu, K.Lưu
                SetButtonState(false);
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Model1())
                {
                    // Kiểm tra dữ liệu đầu vào
                    if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                    {
                        MessageBox.Show("Mã sản phẩm không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtTenSP.Text))
                    {
                        MessageBox.Show("Tên sản phẩm không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Kiểm tra ComboBox
                    if (cboLoaiSP.SelectedValue == null || cboLoaiSP.SelectedIndex == -1)
                    {
                        MessageBox.Show("Vui lòng chọn loại sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Tạo đối tượng sản phẩm mới
                    var sanpham = new Sanpham
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        Ngaynhap = dtNgaynhap.Value,
                        MaLoai = cboLoaiSP.SelectedValue.ToString()
                    };

                    // Thêm sản phẩm vào DbSet
                    context.Sanphams.Add(sanpham);
                    context.SaveChanges();

                    MessageBox.Show("Thêm sản phẩm thành công!");

                    // Tải lại danh sách sản phẩm
                    LoadSanpham();

                    // Làm sạch dữ liệu trong các trường nhập liệu
                    ClearInputFields();
                }

                // Tắt trạng thái chỉnh sửa
                SetButtonState(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kích hoạt các nút Lưu và Không Lưu
                btLuu.Enabled = true;
                btKLuu.Enabled = true;

                // Cho phép chỉnh sửa thông tin sản phẩm
                txtTenSP.Enabled = true;
                dtNgaynhap.Enabled = true;
                cboLoaiSP.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                using (var context = new Model1())
                {
                    var sanpham = context.Sanphams.Find(txtMaSP.Text);
                    if (sanpham != null)
                    {
                        context.Sanphams.Remove(sanpham);
                        context.SaveChanges();
                        MessageBox.Show("Xóa sản phẩm thành công!");
                        LoadSanpham();

                        // Chuyển sang trạng thái chỉnh sửa
                        SetButtonState(true);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm!");
                    }
                }
            }
        }

        private void btTim_Click(object sender, EventArgs e)
        {
            using (var context = new Model1())
            {
                // Khởi tạo truy vấn ban đầu
                var query = from sp in context.Sanphams
                            join loai in context.LoaiSPs on sp.MaLoai equals loai.MaLoai
                            select new
                            {
                                sp.MaSP,
                                sp.TenSP,
                                sp.Ngaynhap,
                                TenLoai = loai.TenLoai
                            };

                // Kiểm tra nếu người dùng nhập tên sản phẩm
                if (!string.IsNullOrWhiteSpace(txtSearch.Text)) // Đảm bảo TextBox tên là txtSearch
                {
                    query = query.Where(sp => sp.TenSP.Contains(txtSearch.Text));
                }

                // Chuyển query thành danh sách
                var results = query.ToList();

                // Hiển thị kết quả trong DataGridView
                dgvSanpham.Rows.Clear();
                foreach (var sp in results)
                {
                    dgvSanpham.Rows.Add(sp.MaSP, sp.TenSP, sp.Ngaynhap.ToString("dd/MM/yyyy"), sp.TenLoai);
                }

                // Kiểm tra nếu không có kết quả
                if (!results.Any())
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
        private void SetButtonState(bool isEditing)
        {
            // Các nút Thêm, Xóa, Sửa luôn bật
            btThem.Enabled = true;
            btSua.Enabled = true;
            btXoa.Enabled = true;

            // Nút Lưu và Không Lưu được bật khi đang chỉnh sửa
            btLuu.Enabled = isEditing;
            btKLuu.Enabled = isEditing;
        }



    

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Model1())
                {
                    var maSP = txtMaSP.Text.Trim();

                    // Tìm sản phẩm theo mã
                    var sanpham = context.Sanphams.Find(maSP);
                    if (sanpham == null)
                    {
                        MessageBox.Show($"Không tìm thấy sản phẩm với Mã SP: {maSP}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cập nhật thông tin sản phẩm
                    sanpham.TenSP = txtTenSP.Text.Trim();
                    sanpham.Ngaynhap = dtNgaynhap.Value;
                    sanpham.MaLoai = cboLoaiSP.SelectedValue?.ToString();

                    // Lưu thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();

                    MessageBox.Show("Cập nhật sản phẩm thành công!");

                    // Tải lại danh sách sản phẩm
                    LoadSanpham();
                }

                // Tắt trạng thái chỉnh sửa
                SetButtonState(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btKLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Hủy bỏ thay đổi, tải lại dữ liệu gốc từ cơ sở dữ liệu
                LoadSanpham();

                MessageBox.Show("Hủy bỏ các thay đổi!");

                // Tắt trạng thái chỉnh sửa
                SetButtonState(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy thay đổi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSanpham_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Bật nút Lưu và Không Lưu khi có thay đổi trong DataGridView
            SetButtonState(true);
        }

        private void dgvSanpham_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSanpham.IsCurrentCellDirty)
            {
                dgvSanpham.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void ClearInputFields()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = -1; // Xóa chọn trong ComboBox
            dtNgaynhap.Value = DateTime.Now; // Đặt lại ngày nhập
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận trước khi thoát
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Thoát toàn bộ ứng dụng
            }
        }
    }
}
