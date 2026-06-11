namespace FaceDeviceDesktopClient;

public partial class PersonForm : Form
{
    public PersonInfo Person { get; private set; }

    public PersonForm()
    {
        InitializeComponent();
        Person = new PersonInfo();

        cmbAccessType.Items.AddRange(new object[] 
        { 
            "Normal User", 
            "Administrator", 
            "Blacklist" 
        });
        cmbAccessType.SelectedIndex = 0;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUserID.Text))
        {
            MessageBox.Show("User ID is required.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUserID.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Name is required.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }

        Person.UserID = txtUserID.Text.Trim();
        Person.Name = txtName.Text.Trim();
        Person.DepartmentID = txtDepartmentID.Text.Trim();
        Person.Job = txtJob.Text.Trim();
        Person.CardNum = txtCardNum.Text.Trim();
        Person.Password = txtPassword.Text.Trim();
        Person.AccessType = cmbAccessType.SelectedIndex;

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
