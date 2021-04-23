using DevExpress.XtraBars;
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
using System.Windows.Forms;

namespace ThoiKhoaBieu
{
    public partial class ThoiKhoaBieu : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Session sess;
        Session Sess
        {
            get { return sess; }
            set { sess = value; }
        }
        public ThoiKhoaBieu(Session sess)
        {
            InitializeComponent();
            this.sess = sess;
            loadData();
        }
        void loadData()
        {
            string baseURL = "https://localhost:44390/api/Schedule?username=" + sess.username + "&password=" + sess.password;
            using (WebClient wc = new WebClient())
            {

                try
                {
                    wc.Headers.Add("Authorization", "Bearer " + sess.token);
                    var json = wc.DownloadString(baseURL);

                    var data = JsonConvert.DeserializeObject<List<ScheduleModel>>(json);

                    dataGridView1.DataSource = data;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        public void Action(string method,string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + sess.token);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var reponse = request.GetResponse();
                using (var streamReader = new StreamReader(reponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }
        public void Sua(string id, string userid, string day, string thoigian, string viec)
        {
            try
            {
                string strUrl = String.Format("https://localhost:44390/api/Schedule?id=" + id + "&userid=" + userid + "&day=" + day + "&time=" + thoigian + "&job=" + viec);                
                Action("PUT", strUrl);
            }
            catch (Exception)
            {
                MessageBox.Show("Login Session expies, please login again");
                this.Close();
            }
        }
        public void ThemDongTrong(string userid)
        {
            try
            {
                string strUrl = String.Format("https://localhost:44390/api/Schedule?userid=" + userid);
                WebRequest request = WebRequest.Create(strUrl);                
                Action("POST", strUrl);
            }
            catch (Exception)
            {
                MessageBox.Show("Login Session expies, please login again");
                this.Close();
            }
        }
        public void Xoa(string id)
        {
            try
            {
                string strUrl = String.Format("https://localhost:44390/api/Schedule?id=" + id);
                WebRequest request = WebRequest.Create(strUrl);               
                Action("DELETE", strUrl);
            }
            catch (Exception)
            {
                MessageBox.Show("Login Session expies, please login again");
                this.Close();
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            string id, day, time, job;
            string userid;
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    day = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    time = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    job = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    userid = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    Sua(id, userid, day, time, job);
                }
                loadData();
            }
            catch (Exception)
            {
                MessageBox.Show("Login Session expies, please login again");
                this.Close();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string userid = dataGridView1.Rows[1].Cells[1].Value.ToString();
                if (e.KeyData == Keys.Enter)
                {
                    ScheduleModel lich = new ScheduleModel();
                    lich.userId = userid;
                    lich.time = "";
                    lich.job = "";
                    lich.day = "";
                    ThemDongTrong(lich.userId);
                    loadData();
                }
                if (e.KeyData == Keys.Delete)
                {
                    string id = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
                    Xoa(id);
                    loadData();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Login Session expies, please login again");
                this.Close();
            }
        }

        private void ThoiKhoaBieu_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Info f = new Info(Sess);
            this.Hide();
            f.ShowDialog();
            this.Show();

        }
    }
}