using System;
using System.Collections.Generic;
using Personendaten.model;
using Personendaten.view;
using static Personendaten.Formular;

namespace Personendaten.controller
{
    public delegate void OnDbActionHandler(object sender, EventArgs e);
    public delegate void OnValitationHandler(object sender, OnValidationHandlerEventArgs e);

    public class OnValidationHandlerEventArgs : EventArgs
    {
        public List<PersonFields> InValidUiFields;
    }

    /// <summary>
    /// Klasse zur Steuerung der Anwendung
    /// Vermittelt den Informationsaustausch zwischen Daten und Eingabeoberfläche
    /// </summary>
    internal class Controller
    {
        public event OnDbActionHandler OnDbAction_Changed; //wird geworfen, wenn eine Veränderung an der Datenbank durchgeführt wurde
        public event OnValitationHandler OnValidation_isUnvalid; //wird geworfen, wenn die eingegebenen Daten nicht der Erwartung entsprechen



        private UIControl UiControl; // Frontendklasse
        private PersonCollection<Person> CurrentCollection; // Aktuelle Liste der Personenobjekte
        private SQLDataHandler SqlDataHandler; // Austausch zwischen SQLite-DB und Anwendung

        private string strStartupPath; // Anwendungspfad der Installation

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Controller()
        {

            Init();
            UiControl = new UIControl(this);

            // Zuweisung der zu überwachenden Ereignisse im UI
            UiControl.myFormular.OnClicked += MyFormular_OnClicked;
            UiControl.myFormular.OnImportClicked += MyFormular_OnImportClicked;
            UiControl.myFormular.OnExportClicked += MyFormular_OnExportClicked;
            UiControl.OnStartUp += UiControl_OnStartUp;
         
            // Anwendungs-GUI starten
            UiControl.Start();
        }

        /// <summary>
        /// Ermittlung des Anwendungspfads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiControl_OnStartUp(object sender, ApplicationEventHandlerEventArgs e)
        {
            strStartupPath = e.startupPath;
            
        }

        /// <summary>
        /// Initialisiert die benötgten Objekte für die Anwendung.
        /// </summary>
        /// <returns></returns>
        private bool Init()
        {
            CurrentCollection = new PersonCollection<Person>();
            SqlDataHandler = new SQLDataHandler(strStartupPath);


            return (true);
        }

        /// <summary>
        /// Ermittelt die aktuelle Liste der Personen in der Datenbank.
        /// </summary>
        private void GetCurrentCollection()
        {
            CurrentCollection = ReadSQL("SELECT * FROM tbl_persondata");
        }

        /// <summary>
        /// Schreibt über eine SQL-Anweisung Daten in die SQLite Datenbank
        /// </summary>
        /// <param name="strQuery"></param>
        private void WriteSQL(String strQuery)
        {
            SqlDataHandler.WriteData(strQuery);
        }

        /// <summary>
        /// Liest über eine SQL-Anweisung die Daten aus der SQLite Datenbank aus.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        private PersonCollection<Person> ReadSQL(String strQuery)
        {
            SqlDataHandler.StrQuery = strQuery;
            return (SqlDataHandler.ReadData());

        }

        /// <summary>
        /// Wird beim Klick auf Speichern ausgelöst. Führt eine Überprüfung der Inhalte durch und wirft im Fehlerfall das 
        /// Event OnValidation_isUnvalid. Wenn die Daten korrekt sind, werden sie in die SQLite DB übernommen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyFormular_OnClicked(object sender, Formular.OnButtonClickedEventArgs e)
        {
            Person newPerson = new Person(e.firstName, e.lastName, e.city);
            if (!newPerson.GetIsValid())
            {
                OnValidationHandlerEventArgs onValitadtiionHandlerEventArgs = new OnValidationHandlerEventArgs();

                onValitadtiionHandlerEventArgs.InValidUiFields = newPerson.GetUnvalidFields();
                OnValidation_isUnvalid?.Invoke(sender, onValitadtiionHandlerEventArgs);
                return;
            }

            //s chreiben der Daten in die Datenbank
            String strQuery = "INSERT INTO tbl_persondata (firstname,lastname,city) VALUES ('" + e.firstName + "','" + e.lastName + "','" + e.city + "')";

            WriteSQL(strQuery);

            this.GetCurrentCollection();

            // Änderung in der Datebank durchgefüht -> Event auslösen
            OnDbAction_Changed?.Invoke(sender, EventArgs.Empty);

        }

        /// <summary>
        /// Wird ausgeführt, ween der DatenExport gestartet worden ist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyFormular_OnExportClicked(object sender, OnMenueEventHandlerEventArgs e)
        {
            SqlDataHandler.ExportXML(e.path);
        }

        /// <summary>
        /// Wird gestartet, enn der Datenimport gestartet worden ist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyFormular_OnImportClicked(object sender, OnMenueEventHandlerEventArgs e)
        {

            SqlDataHandler.ImportXML(e.path);
        }
    }
}
