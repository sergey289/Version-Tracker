using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Version_Tracker.Models;

namespace Version_Tracker.Forms
{
    public partial class EditForm : DevExpress.XtraEditors.XtraForm
    {
        AppVersion appVersion = null;

        Main main;

        int versionID;

        public EditForm(AppVersion appVersion,Main main)
        {
            try
            {
                InitializeComponent();
                this.main = main;
                this.appVersion = appVersion;
                LoadData(appVersion);

            }catch(Exception ex)
            {
                XtraMessageBox.Show("אירעה שגיאה בהצגת נתנוים", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData(AppVersion appVersion)
        {
            versionID = appVersion.Id;
            txtVerName.Text = appVersion.Name;
            dtReleaseDate.Text = appVersion.ReleaseDate;
        }

        private void btn_cencel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateVersion();
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("אירעה שגיאה בשמירת נתנוים", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
        }

        private void UpdateVersion()
        {

            List<AppVersion> versions = main.GetAppVersions();

            // Check if the version already exists
            var existingVersion = versions.Find(v => v.Id == versionID);

            if (existingVersion != null)
            {
                // Update existing version
                existingVersion.Name = txtVerName.Text;
                existingVersion.ReleaseDate = dtReleaseDate.Text;
                DateTime selectedDateTime = DateTime.Now;
                existingVersion.DeployDate = selectedDateTime.ToString();
            }
          
            main.SaveAppVersions(versions);
            main.ShowAppVersionsByVersionID(versionID);
            main.LoadAppVersions();
            this.Close();

        }
    }
}