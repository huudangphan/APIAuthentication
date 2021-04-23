using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Forms;

namespace Schedule
{
    public partial class Info : Form
    {
        private Session sess;
        Session Sess
        {
            get { return sess; }
            set { sess = value; }
        }
        public Info(Session sess)
        {
            InitializeComponent();
            this.sess = sess;
        }
        public void ChangePass(string username,string newPass)
        {

        }
        public bool checkPass(string username,string curpass)
        {
            try
            {
                Session s = new Session();
                string Urlbase = "https://localhost:44390/Authentication";
                UserModel user = new UserModel();

                user.username = username;
                user.password = curpass;
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
                       
                        s.token = data["token"].Value<string>();
                        if (s.token != null)
                        {
                            return true;
                        }
                    }
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Please type correct curently password");
            }
            return false;
        }
        public void Sua(string username,string password )
        {
            try
            {
                string strUrl = String.Format("https://localhost:44390/api/Account?username=" + username+"&password="+password );
                WebRequest request = WebRequest.Create(strUrl);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + sess.token);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var reponse = request.GetResponse();
                    using (var streamReader = new StreamReader(reponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        MessageBox.Show("Change password success");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Login Session expies, please login again");
                this.Close();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string newPass = txtNewPass.Text;
            string conPass = txtConPass.Text;
            string curPass = txtCurPass.Text;
            string username = sess.username;
            if(string.IsNullOrEmpty(newPass)||string.IsNullOrWhiteSpace(conPass))
            {
                MessageBox.Show("Pls type new password");
            }
            else
            {
                if (newPass != conPass)
                {
                    MessageBox.Show("New password must be equal confirm password");
                }
                else
                {
                    if (checkPass(username, curPass))
                    {
                        Sua(username, newPass);
                        
                    }
                }
               
            }
           


        }
    }
}
