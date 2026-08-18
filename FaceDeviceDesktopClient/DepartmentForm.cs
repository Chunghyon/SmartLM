namespace FaceDeviceDesktopClient;

public partial class DepartmentForm : Form
{
    public DepartmentInfo Department { get; private set; }

    public DepartmentForm()
    {
        InitializeComponent();
        Department = new DepartmentInfo();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtDepartmentID.Text))
        {
            MessageBox.Show("Department ID is required.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDepartmentID.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
        {
            MessageBox.Show("Department Name is required.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDepartmentName.Focus();
            return;
        }

        Department.DepartmentID = txtDepartmentID.Text.Trim();
        Department.DepartmentName = txtDepartmentName.Text.Trim();

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
