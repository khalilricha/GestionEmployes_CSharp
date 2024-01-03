using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GestionEmployes
{
    public partial class Form2 : Form
    {
        private const string ConnectionString = "Data Source=DESKTOP-83RTEL3;Initial Catalog=GestionEmploye;Integrated Security=True";
        private const string TableName = "Employe";
        public Form2()
        {
            InitializeComponent();
           
            LoadData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }
      
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM {TableName}";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = $"INSERT INTO {TableName} (id,EmpNom, Genre, Departement, DateNaissance, DateEmbauche, Salaire) " +
                               $"VALUES (@id,@EmpNom, @Genre, @Departement, @DateNaissance, @DateEmbauche, @Salaire)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", textBox1.Text);
                command.Parameters.AddWithValue("@EmpNom", textBox2.Text);
                command.Parameters.AddWithValue("@Genre", comboBox1.Text);
                command.Parameters.AddWithValue("@Departement", comboBox2.Text);
                command.Parameters.AddWithValue("@DateNaissance", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@DateEmbauche", dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Salaire", decimal.Parse(textBox3.Text));

                command.ExecuteNonQuery();
                LoadData();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int employeeId;

            try
            {
                if (int.TryParse(textBox1.Text, out employeeId))
                {
                    Console.WriteLine("ID à supprimer : " + employeeId);

                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM " + TableName + " WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", employeeId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Suppression réussie");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Aucune ligne supprimée. Assurez-vous que l'ID existe.");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ID non valide : " + textBox1.Text);
                    MessageBox.Show("Veuillez entrer une valeur d'ID valide.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedIndex];
                textBox1.Text = selectedRow.Cells["id"].Value.ToString();
                textBox2.Text = selectedRow.Cells["EmpNom"].Value.ToString();
                comboBox1.Text = selectedRow.Cells["Genre"].Value.ToString();
                comboBox2.Text = selectedRow.Cells["Departement"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["DateNaissance"].Value);
                dateTimePicker2.Value = Convert.ToDateTime(selectedRow.Cells["DateEmbauche"].Value);
                textBox3.Text = selectedRow.Cells["Salaire"].Value.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedIndex];

                    int id = Convert.ToInt32(selectedRow.Cells["id"].Value);
                    string nouvelEmpNom = textBox1.Text;
           
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string query = $"UPDATE {TableName} SET EmpNom = @EmpNom WHERE id = @id";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@EmpNom", nouvelEmpNom);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Modification réussie");
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez taper 2 fois sur la ligne à modifier.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
