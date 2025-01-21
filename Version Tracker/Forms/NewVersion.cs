using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Version_Tracker.Models;

namespace Version_Tracker.Forms
{
    public partial class NewVersion : DevExpress.XtraEditors.XtraForm
    {
        private string filePath;

        Main main;

        public NewVersion(Main main)
        {
            InitializeComponent();
            this.main = main;
 
        }
      
        public  void LoadAppVersions()
        {
            var res = main.GetAppVersions();

            foreach (var version in res)
            {
                be_AppName.Properties.Items.Add(version.Name);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (be_AppName.EditValue == null || be_AppName.EditValue.ToString() == "")
                {
                    XtraMessageBox.Show("Please select application name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtVerName.Text == "")
                {
                    XtraMessageBox.Show("Please enter a version name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dtReleaseDate.Text == "")
                {
                    XtraMessageBox.Show("Please select a release date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                AppVersion appVersion = new AppVersion
                {
                    Id = be_AppName.SelectedIndex,
                    Name = be_AppName.EditValue.ToString() + " " + "Version:" + " "+ txtVerName.Text,
                    ReleaseDate = dtReleaseDate.Text,
                    DeployDate = "0000-00-00"
                };

                main.AddOrUpdateAppVersion(appVersion);
                main.LoadAppVersions();
                this.Close();


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while saving the version", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cencel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtVerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true; // Stop the character from being entered into the control
                }

            }catch(Exception ex)
            {

            }
        }

        private void NewVersion_Load(object sender, EventArgs e)
        {
            try
            {
                dtReleaseDate.EditValue = DateTime.Now;
            }
            catch(Exception ex)
            {

            }

           
        }
    }
}