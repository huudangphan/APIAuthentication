using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Session s = new Session();
        public void Login(string username, string password)
        {

            try
            {
                string Urlbase = "https://localhost:44390/Authentication";
                UserModel user = new UserModel();
                user.username = username;
                user.password = password;
                string postData = JsonConvert.SerializeObject(user);
                string url = string.Format(Urlbase);
                WebRequest request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                    streamWriter.Flush();
                    streamWriter.Close();
                    var response = request.GetResponse();
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var data = (JObject)JsonConvert.DeserializeObject(result);
                        s.id = data["userID"].Value<int>();
                        s.username = data["username"].Value<string>();
                        s.password = data["password"].Value<string>();
                        s.token = data["token"].Value<string>();
                        if (s.token != null)
                        {
                            Schedule f = new Schedule(s);
                            this.Hide();
                            f.ShowDialog();
                            this.Show();
                        }
                    }
                }

            }


            catch (Exception)
            {

                MessageBox.Show("Username or Password invalid");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtusername.Text;
            string password = txtpassword.Text;
            Login(username, password);
        }
    }
}
