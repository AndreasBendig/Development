using System;
using System.Drawing;
using System.Windows.Forms;
using static Personendaten.Formular;

namespace Personendaten
{
    // Übergeordnete nutzbare Ereignisse

    public delegate void OnButtonEventHandler(object sender, OnButtonClickedEventArgs e);       // Schaltfläche wurde verwendet
    public delegate void OnMenueEventHandler(object sender, OnMenueEventHandlerEventArgs e);    // Menueintrag wurde gewählt

    /// <summary>
    /// Verwaltet das im UI sichtbare Formular
    /// </summary>
    public partial class Formular : Form
    {
        public event OnButtonEventHandler OnClicked;
        public event OnMenueEventHandler OnImportClicked;
        public event OnMenueEventHandler OnExportClicked;


       /// <summary>
       /// Klasse für die Übergabeparameter für das Click-Event des Speichern-Buttons
       /// </summary>
        public class OnButtonClickedEventArgs : EventArgs
        {
            public String firstName;
            public String lastName;
            public String city;
        }

        /// <summary>
        /// Klasse für die Übergabeparameter für das Click-Event eines Menüeintrags
        /// </summary>
        public class OnMenueEventHandlerEventArgs : EventArgs
        {
            public String path;
        }


        // Getter und Setter **********************************************************************************

        public String GetFirstName()
        {
            return (txtFirstName.Text);
        }
        public String GetLastName()
        {
            return (txtLastName.Text);
        }
        public String GetCity()
        {
            return (txtCity.Text);
        }


        /// <summary>
        /// Konstuktor des Formuklars
        /// </summary>
        public Formular()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        /// <summary>
        /// Wird ausgeführt, wenn im UI ein anderer Datensatz in der Tabelle ausgewählt wird.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtFirstName.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtLastName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtCity.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            }
        }

        /// <summary>
        /// Initialsiert das Formular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Formular_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "persondata.tbl_persondata". Sie können sie bei Bedarf verschieben oder entfernen.
            this.tbl_persondataTableAdapter.Fill(this.persondata.tbl_persondata);

        }
        /*
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            OnClicked?.Invoke(sender, new OnButtonClickedEventArgs { firstName = GetFirstName(), lastName = GetLastName(), city = GetCity() });

        }
        */

        /// <summary>
        /// Aktualisiert die Tabelle in der GUI und leert die Eingabefelder
        /// </summary>
        public void RefreshDataGrid()
        {
            this.tbl_persondataTableAdapter.Fill(this.persondata.tbl_persondata);
            ClearFields();
        }

        /// <summary>
        /// Leert die Eigabefelder und setzt die Hintergrundfarbe der Label zurück
        /// </summary>
        public void ClearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCity.Text = "";

            ResetLabel();
        }

        /// <summary>
        /// Setzt die Hintergrundfarbe der Label zurück
        /// </summary>
        public void ResetLabel()
        {
            lbl_FistName.BackColor = Control.DefaultBackColor;
            lbl_LastName.BackColor = Control.DefaultBackColor;
            lbl_City.BackColor = Control.DefaultBackColor;

        }

        /// <summary>
        /// markiert die fehlerhaften Eingabefelder durch Veränderung der Hintergrundfarbe des Labels
        /// </summary>
        /// <param name="errFieldName"></param>
        internal void MarkLabel(string errFieldName)
        {
            Color ErrorColor = Color.LightPink;
            switch (errFieldName)
            {
                case "firstname":
                    lbl_FistName.BackColor = ErrorColor;
                    break;
                case "lastname":
                    lbl_LastName.BackColor = ErrorColor;
                    break;
                case "city":
                    lbl_City.BackColor = ErrorColor;
                    break;
                default:

                    break;
            }
        }

        /// <summary>
        /// Wird ausgeführt, wenn im Menue der Export ausgewählt wird.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datenExportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = exportFolderBrowserDialogBox.ShowDialog();
            if (result == DialogResult.OK)
            {

                OnMenueEventHandlerEventArgs args = new OnMenueEventHandlerEventArgs();
                args.path = exportFolderBrowserDialogBox.SelectedPath;
                OnExportClicked?.Invoke(sender, args);
            }

        }

        /// <summary>
        /// Wird ausgeführt, wenn im Menue der Import ausgewählt wird.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datenImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openImportFileDialog.ShowDialog();


            if (result == DialogResult.OK)
            {

                OnMenueEventHandlerEventArgs args = new OnMenueEventHandlerEventArgs();
                args.path = openImportFileDialog.FileName;
                OnImportClicked?.Invoke(sender, args);

            }


        }


        /// <summary>
        /// Wird ausgeführt, wenn die Schaltfläche "Abbruch" angewählt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            ClearFields();
        }


        /// <summary>
        /// Wird ausgeführt, wenn im Formular der Eintrag "Neu" gewählt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripLabel_New_Click(object sender, EventArgs e)
        {
            activateFormular();
        }

        /// <summary>
        /// Wird ausgeführt, wenn im Formular der Eintrag "Speichern" gewählt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripLabelSave_Click(object sender, EventArgs e)
        {
            OnClicked?.Invoke(sender, new OnButtonClickedEventArgs { firstName = GetFirstName(), lastName = GetLastName(), city = GetCity() });
            deactivateFormular();

        }
        
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }
        

        /// <summary>
        /// Wird ausgeführt, wenn im Formular der Eintrag "Abbrechen" gewählt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripLabelCancel_Click(object sender, EventArgs e)
        {
            deactivateFormular();
        }

        /// <summary>
        /// Setzt UI-Eigenschaften für den Zustand, dass das Formular nicht ausgefüllt werden kann
        /// </summary>
        private void deactivateFormular()
        {
            ClearFields();

            txtFirstName.ReadOnly = true;
            txtLastName.ReadOnly = true;
            txtCity.ReadOnly = true;

            toolStripLabelSave.Enabled = false;
            toolStripLabelCancel.Enabled = false;
            toolStripLabel_New.Enabled = true;
        }

        /// <summary>
        /// Setzt UI-Eigenschaften für den Zustand, dass das Formular ausgefüllt werden kann
        /// </summary>
        private void activateFormular()
        {
            txtFirstName.ReadOnly = false;
            txtLastName.ReadOnly = false;
            txtCity.ReadOnly = false;

            toolStripLabelSave.Enabled = true;
            toolStripLabelCancel.Enabled = true;
            toolStripLabel_New.Enabled = false;

            ClearFields();
            txtFirstName.Focus();
        }
    }
}
