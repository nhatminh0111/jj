using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace buoi6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var context = new Model1();
                // Lấy danh sách sinh viên, bao gồm thông tin khoa
                var listStudents = context.Students
                    .Select(s => new
                    {
                        s.StudentID,
                        s.FullName,
                        FacultyName = s.Faculty.FacultyName, // Lấy tên khoa từ Faculty
                        s.DiemTrungBinh
                    }).ToList();

                dgvStudent.DataSource = listStudents; // Gán dữ liệu trực tiếp vào DataGridView
                var listFaculties = context.Faculties.ToList(); // Lấy danh sách khoa
                FillFacultyCombobox(listFaculties); // Gọi hàm điền combobox
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFacultyCombobox(List<Faculty> listFaculties)
        {
            cmbFaculty.DataSource = listFaculties;
            cmbFaculty.DisplayMember = "FacultyName";
            cmbFaculty.ValueMember = "FacultyID";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new Model1())
                {
                    var student = new Student
                    {
                        StudentID = int.Parse(txtStudentID.Text),
                        FullName = txtFullName.Text,
                        DiemTrungBinh = float.Parse(txtDiemTB.Text),
                        FacultyID = (int)cmbFaculty.SelectedValue
                    };

                    context.Students.Add(student); // Thêm vào database
                    context.SaveChanges(); // Lưu thay đổi

                    MessageBox.Show("Thêm sinh viên thành công!");
                    LoadData(); // Tải lại dữ liệu lên giao diện
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudent.CurrentRow != null) // Kiểm tra xem có hàng nào đang được chọn
                {
                    // Lấy StudentID từ hàng được chọn trong DataGridView
                    int studentID = Convert.ToInt32(dgvStudent.CurrentRow.Cells["StudentID"].Value);

                    using (var context = new Model1())
                    {
                        var student = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                        if (student != null)
                        {
                            context.Students.Remove(student); // Xóa sinh viên
                            context.SaveChanges(); // Lưu thay đổi

                            MessageBox.Show("Xóa sinh viên thành công!");
                            LoadData(); // Tải lại dữ liệu lên giao diện
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để xóa.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudent.CurrentRow != null) // Kiểm tra xem có hàng nào đang được chọn
                {
                    // Lấy StudentID từ hàng được chọn
                    int studentID = Convert.ToInt32(dgvStudent.CurrentRow.Cells["StudentID"].Value);

                    using (var context = new Model1())
                    {
                        // Tìm sinh viên trong database dựa vào StudentID
                        var student = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                        if (student != null)
                        {
                            // Cập nhật thông tin sinh viên
                            student.FullName = txtFullName.Text;
                            student.DiemTrungBinh = float.Parse(txtDiemTB.Text);
                            student.FacultyID = (int)cmbFaculty.SelectedValue;

                            context.SaveChanges(); // Lưu thay đổi vào database

                            MessageBox.Show("Cập nhật sinh viên thành công!");
                            LoadData(); // Tải lại dữ liệu lên DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để sửa.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
        private void LoadData()
        {
            using (var context = new Model1())
            {
                var listStudents = context.Students
                    .Select(s => new
                    {
                        s.StudentID,
                        s.FullName,
                        FacultyName = s.Faculty.FacultyName,
                        s.DiemTrungBinh
                    }).ToList();

                dgvStudent.DataSource = listStudents;
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtDiemTB.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return false;
            }

            if (!int.TryParse(txtStudentID.Text, out _) || !float.TryParse(txtDiemTB.Text, out _))
            {
                MessageBox.Show("Dữ liệu không hợp lệ.");
                return false;
            }

            return true;
        }

        private void dgvStudent_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Kiểm tra nếu hàng được nhấp hợp lệ
                {
                    // Lấy hàng hiện tại
                    DataGridViewRow row = dgvStudent.Rows[e.RowIndex];

                    // Gán giá trị từ DataGridView vào các TextBox và ComboBox
                    txtStudentID.Text = row.Cells["StudentID"].Value?.ToString();
                    txtFullName.Text = row.Cells["FullName"].Value?.ToString();
                    txtDiemTB.Text = row.Cells["DiemTrungBinh"].Value?.ToString();

                    // Lấy FacultyName từ DataGridView và chọn trong ComboBox
                    string facultyName = row.Cells["FacultyName"].Value?.ToString();
                    cmbFaculty.SelectedIndex = cmbFaculty.FindStringExact(facultyName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
