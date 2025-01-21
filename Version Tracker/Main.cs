using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Version_Tracker.Forms;
using Version_Tracker.Models;

namespace Version_Tracker
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {

        private string filePath;

        private bool blnButtonDown = false;

        public Main()
        {
            try
            {
                InitializeComponent();
                filePath = @"\\10.0.0.6\Public\Development\צוות אפליקציה\כלים\VersionTracker\AppVersions.txt";
                LoadAppVersions();

            }catch(Exception ex)
            {
                XtraMessageBox.Show("An error occurred while displaying data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_AddNewVer_Click(object sender, EventArgs e)
        {
            try
            {
                NewVersion newVersion = new NewVersion(this);
                newVersion.ShowDialog();

            }catch(Exception ex)
            {

            }

        }

        public List<AppVersion> GetAppVersions()
        {
            if (File.Exists(filePath))
            {
                // Read JSON string from file
                string json = File.ReadAllText(filePath);

                // Deserialize JSON string to list of AppVersion objects
                return JsonConvert.DeserializeObject<List<AppVersion>>(json) ?? new List<AppVersion>();
            }
            else
            {
                return new List<AppVersion>();
            }
        }

        public void LoadAppVersions()
        { 
            if (InvokeRequired)
            {
                Invoke(new Action(LoadAppVersions));
                return;
            }

            try
            {

                listVersions.Items.Clear(); 

                var res = GetAppVersions();

                BindingList<VersionItem> versionList = new BindingList<VersionItem>();

                foreach (var version in res)
                {
                    versionList.Add(new VersionItem { Id = version.Id, Name = version.Name });
                }

                listVersions.DisplayMember = "Name";  // Property to display
                listVersions.ValueMember = "Id";
                listVersions.DataSource = versionList;
            }
            catch (Exception ex)
            {
               
            }
        }

        public void ShowAppVersionsByVersionID(int versionID)
        {
            try
            {
                List<AppVersion> versions = GetAppVersions();
                versions = versions.Where(v => v.Id == versionID).ToList();
                gridVerStatus.DataSource = null;  // Clear the current data source binding (optional but often necessary)
                gridVerStatus.DataSource = versions; // Reassign the new data source

            }catch(Exception ex)
            {

            }
        }

        public void SaveAppVersions(List<AppVersion> versions)
        {
            string json = JsonConvert.SerializeObject(versions, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void DeleteAppVersion(int versionId)
        {
            List<AppVersion> versions = GetAppVersions();

            // Find and remove the version
            versions.RemoveAll(v => v.Id == versionId);

            // Save updated list to file
            SaveAppVersions(versions);
        }

        public void AddOrUpdateAppVersion(AppVersion newVersion)
        {
            List<AppVersion> versions = GetAppVersions();

            // Check if the version already exists
            var existingVersion = versions.Find(v => v.Id == newVersion.Id);

            if (existingVersion != null)
            {
                // Update existing version
                existingVersion.Name = newVersion.Name;
                existingVersion.ReleaseDate = newVersion.ReleaseDate;
                existingVersion.DeployDate = newVersion.DeployDate;
            }
            else
            {
                // Add new version
                versions.Add(newVersion);
            }

            // Save updated list to file
            SaveAppVersions(versions);
        }

        private void listVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listVersions.SelectedItem != null)
                {
                    int selectedID = (int)listVersions.SelectedValue;
                    ShowAppVersionsByVersionID(selectedID);
                }

            }catch(Exception ex)
            {

            }
        }

        private void listVersions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {

                int index = listVersions.IndexFromPoint(e.Location);

                if (index != ListBox.NoMatches)
                {
                    // Get the selected item
                    var selectedItem = (int)listVersions.SelectedValue;

                    List<AppVersion> versions = GetAppVersions();
                    var version = versions.Where(v => v.Id == selectedItem).First();
                    EditForm editForm = new EditForm(version,this);
                    editForm.ShowDialog();
                }

            }catch(Exception ex)
            {

            }
        }

        private void listVersions_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                DialogResult result = XtraMessageBox.Show("Are you sure you want to delete this Version?",
                                         "Delete Confirmation",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        int selectedVersionID = (int)listVersions.SelectedValue;
                        DeleteAppVersion(selectedVersionID);
                        LoadAppVersions();
                        e.Handled = true;
                        XtraMessageBox.Show("Version deleted successfully.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
           
            }catch(Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (listVersions.SelectedItem != null)
                {
                    int selectedID = (int)listVersions.SelectedValue;

                    ShowAppVersionsByVersionID(selectedID);
                }
            }
            catch (Exception ex)
            {

            }

           
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {

                Assembly assembly = Assembly.GetExecutingAssembly();
                Version version = assembly.GetName().Version;
                this.Text = this.Text + "  " +  version.ToString();

                timer1.Interval = 2000;// Set interval to 2 seconds
                timer1.Start();


                timer2.Interval = 1 * 60 * 1000; // Set interval to 1 minutes
                timer2.Start();

            }
            catch(Exception ex)
            {

            }

           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                LoadAppVersions();
            }
            catch(Exception ex)
            {

            }            
        }

        private void btn_AddNewVer_Paint(object sender, PaintEventArgs e)
        {

            try
            {

                if (blnButtonDown == false)
                {
                    ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Outset,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Outset,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Outset,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Outset);
                }
                else
                {
                    ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Inset,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Inset,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Inset,
                        System.Drawing.SystemColors.ControlLightLight, 2, ButtonBorderStyle.Inset);
            }

            }catch(Exception ex)
            {

            }

        }

        private void btn_AddNewVer_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                blnButtonDown = true;
                (sender as System.Windows.Forms.Button).Invalidate();
            }          
        }

        private void btn_AddNewVer_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                blnButtonDown = false;
                (sender as System.Windows.Forms.Button).Invalidate();
            }
            catch (Exception ex)
            {

            }

           
        }
    }
}
