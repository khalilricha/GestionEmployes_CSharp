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

namespace GestionEmployes
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Data Source=DESKTOP-83RTEL3;Initial Catalog=Users;Integrated Security=True";
        private const string TableName = "Users";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nomUtilisateur = textBox1.Text;
            string motDePasse = textBox2.Text;

            if (ValiderConnexion(nomUtilisateur, motDePasse))
            {
               
                MessageBox.Show("Connexion réussie !");
                Form2 form2 = new Form2();
                form2.Show();
             
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect !");
            }

        }
        private bool ValiderConnexion(string nomUtilisateur, string motDePasse)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    
                    string query = "SELECT COUNT(*) FROM User1 WHERE username = @NomUtilisateur AND password = @MotDePasse";
                    Console.WriteLine("Query: " + query); 
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NomUtilisateur", nomUtilisateur);
                    command.Parameters.AddWithValue("@MotDePasse", motDePasse);

                    int nombreUtilisateurs = (int)command.ExecuteScalar();

                    return nombreUtilisateurs > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la validation de la connexion : " + ex.Message);
                return false;
            }
        }
    }   
}
