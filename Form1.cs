using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Activity_8___API
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                txtOutput.Clear();
                HttpResponseMessage response = await client.GetAsync("http://localhost/api.php");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize JSON response
                dynamic data = JsonConvert.DeserializeObject(responseBody);

                // Display users
                txtOutput.Text += "Users:\n";
                foreach (var user in data.users)
                {
                    txtOutput.Text += $" ID: {user.id}, Username: {user.username}, Email: {user.email}\n";
                }

                // Display products
                txtOutput.Text += "\n Products:\n";
                foreach (var product in data.products)
                {
                    txtOutput.Text += $" ID: {product.id}, Name: {product.name}, Price: {product.price}\n\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void btnPost_Click(object sender, EventArgs e)
        {
            var userData = new
            {
                username = txtUsername.Text,
                pass = txtPassword.Text,
                email = txtEmail.Text,
                product_name = txtProductName.Text,
                product_price = Convert.ToDecimal(txtProductPrice.Text)
            };
            string json = JsonConvert.SerializeObject(userData);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost/api.php", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                txtOutput.Text = responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
