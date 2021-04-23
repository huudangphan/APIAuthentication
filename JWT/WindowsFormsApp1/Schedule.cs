using DevExpress.XtraBars;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Schedule : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Session sess;
        Session Sess
        {
            get { return sess; }
            set { sess = value; }
        }
        public Schedule(Session sess)
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


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}