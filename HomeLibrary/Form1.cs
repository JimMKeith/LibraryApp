using System.IO;

namespace HomeLibrary
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
            this.Load += Form1_Load; // attach your handler to the form Load event
            dbBranchCB.SelectedIndexChanged += dbBranchCB_SelectedIndexChanged;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }


        // Example DBMetaData class — make sure properties are public
        public class DBMetaData
        {
            public string? LibName { get; set; }
            public string? DbName { get; set; }
            public string? DbFilePath { get; set; }
            public int DbVersion { get; set; }
            public DateTime DbCreateDt { get; set; }
        }

        // --- In your Form (e.g. Form1) ---

        // 1) Load your list (if from JSON)
        private List<DBMetaData> LoadDbListFromJson(string jsonFilePath)
        {
            var json = File.ReadAllText(jsonFilePath);
            // System.Text.Json
            return System.Text.Json.JsonSerializer.Deserialize<List<DBMetaData>>(json)
                   ?? new List<DBMetaData>();
        }

        // 2) Configure the combo box (call this after InitializeComponent, e.g. in Form_Load)
        private void SetupDbBranchComboBox(List<DBMetaData> dbList)
        {
            // If you want the combo to be read-only selection:
            dbBranchCB.DropDownStyle = ComboBoxStyle.DropDownList;

            // Tell the combo what to display and what the "value" is
            dbBranchCB.DisplayMember = nameof(DBMetaData.LibName);     // shown to user
            dbBranchCB.ValueMember = nameof(DBMetaData.DbFilePath);   // returned by SelectedValue

            // Bind the list (you can use BindingList if you need runtime updates)
            dbBranchCB.DataSource = dbList;
        }

        // 3) Read the selected database path
        private void dbBranchCB_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var meta = dbBranchCB.SelectedItem as DBMetaData;
            dbPathTB.Text = meta?.DbFilePath ?? string.Empty;
        }

        // Example wiring in Form_Load
        private void Form1_Load(object? sender, EventArgs e)
        {
            string metaDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                          "BaseLine", "HomeLibrary");
            string metaPath = Path.Combine(metaDir, "databases.json");

            var dbList = LoadDbListFromJson($"{metaPath}");
            SetupDbBranchComboBox(dbList);

            // Optional: pre-select the first item
            if (dbBranchCB.Items.Count > 0)
            {
                dbBranchCB.SelectedIndex = 0;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // modal (blocks owner until closed)
            using (var dlg = new MainForm())
            {
                var result = dlg.ShowDialog(this); // returns DialogResult
                if (result == DialogResult.OK)
                {
                    // read properties from dlg or handle result
                }
            }
        }
    }
}
