using System;
using System.Windows.Forms;
using Personendaten.controller;
using Personendaten.model;
using static Personendaten.Formular;
using static Personendaten.view.UIControl;

namespace Personendaten.view
{
    // Übergeordnete nutzbare Ereignisse
    public delegate void AppEventHandler(object sender, ApplicationEventHandlerEventArgs e);

    /// <summary>
    /// Übergabeparameter für UI-Ereignisse
    /// </summary>
    public class ApplicationEventHandlerEventArgs : EventArgs
    {
        public String startupPath;
    }

    /// <summary>
    /// Klasse zur Steuerung der Funktionalitäten zwischen Formular und Controller
    /// </summary>
    internal class UIControl

    {
        public event AppEventHandler OnStartUp; // Ereignis wird beim Starten der Anwendung ausgelöst

        private Controller myController; // Referenz auf das Controller-Objekt

        public Formular myFormular; // Referenz auf das Formualar-objekt

        /// <summary>
        /// Erstellung des UIControl-Objektes
        /// </summary>
        /// <param name="Controller"></param>
        public UIControl(Controller Controller)
        {
            myController = Controller;
            myController.OnDbAction_Changed += MyController_OnDbAction_Changed;
            myController.OnValidation_isUnvalid += MyController_OnValidation_isUnvalid;
            
            initUiControl();
        }

        /// <summary>
        /// Ereignisbehandlung, wenn eine Überprüfung des Personen-Obejktes ungültig ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyController_OnValidation_isUnvalid(object sender, OnValidationHandlerEventArgs e)
        {
            myFormular.ResetLabel();
            string errMsg = "";

            if (e.InValidUiFields.Contains(PersonFields.FirstName))
            {
                myFormular.MarkLabel("firstname");
                errMsg += "Bitte geben Sie einen Vornamen mit max. 50 Zeichen ein!\n";
            }
            if (e.InValidUiFields.Contains(PersonFields.LastName))
            {
                myFormular.MarkLabel("lastname");
                errMsg += "Bitte geben Sie einen Nachnamen mit max. 50 Zeichen ein!\n";
            }
            if (e.InValidUiFields.Contains(PersonFields.City))
            {
                myFormular.MarkLabel("city");
                errMsg += "Bitte geben Sie einen Ort mit max. 50 Zeichen ein!\n";
            }
            if (errMsg != "")
            {
                MessageBox.Show(errMsg, "Eingabefehler", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Ereignisbehandlung, wenn eine Veränderung in der Datenbank auftrat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyController_OnDbAction_Changed(object sender, EventArgs e)
        {
            myFormular.RefreshDataGrid();
        }

        /// <summary>
        /// Initialisierung des UIControl-Objektes
        /// </summary>
        private void initUiControl()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myFormular = (new Formular());

            ApplicationEventHandlerEventArgs args = new ApplicationEventHandlerEventArgs();
            args.startupPath= Application.StartupPath;
            MessageBox.Show(args.startupPath);
            OnStartUp?.Invoke(this, args);

        }

        /// <summary>
        /// Startet das Formular in der GUI
        /// </summary>
        public void Start()
        {
            Application.Run(myFormular);
        }
    }
}
