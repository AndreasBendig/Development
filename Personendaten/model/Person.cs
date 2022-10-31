using System.Collections.Generic;

namespace Personendaten.model
{
    /// <summary>
    /// Verwaltet und steuert die Datenstruktur einer Person
    /// </summary>
    internal partial class Person
    {
        public string strFirstName; // Vorname
        private string strLastName; // Nachname
        private string strCity;     // Ort
        private int intID;          // ID in der Datenbank
        private bool isValid;       // Objektinhalt überprüft und gültig


        private List<PersonFields> UnvalidFields; // Liste der fehlerhaften Felder

       
        /// <summary>
        /// Initiallisiert ein neues Personen-Objekt
        /// </summary>
        public Person()
        {
            this.SetIntID(-1);
            SetUnvalidFields(new List<PersonFields>());

            GetUnvalidFields().Add(PersonFields.FirstName);
            GetUnvalidFields().Add(PersonFields.LastName);
            GetUnvalidFields().Add(PersonFields.City);

            this.SetIsValid(false);
        }

        /// <summary>
        /// Initiallisiert ein neues Personen-Objekt und übernimmt Personenwerte
        /// </summary>
        /// <param name="strFirstName"></param>
        /// <param name="strLastName"></param>
        /// <param name="strCity"></param>
        public Person(string strFirstName, string strLastName, string strCity) : this()
        {
            this.SetStrFirstName(strFirstName);
            this.SetStrLastName(strLastName);
            this.SetStrCity(strCity);
        }

        /// <summary>
        /// Initiallisiert ein neues Personen-Objekt und übernimmt Personenwerte und setzt zusätzlich die ID
        /// </summary>
        /// <param name="intID"></param>
        /// <param name="strFirstName"></param>
        /// <param name="strLastName"></param>
        /// <param name="strCity"></param>
        public Person(int intID, string strFirstName, string strLastName, string strCity) : this(strFirstName, strLastName, strCity)
        {
            this.SetIntID(intID);
        }

        // Setter und Getter *******************************************************************************************

        /// <summary>
        /// Liefert die Liste der ungültigen Felder
        /// </summary>
        /// <returns></returns>
        public List<PersonFields> GetUnvalidFields()
        {
            return (UnvalidFields);
        }
      
        private void SetUnvalidFields(List<PersonFields> value)
        {
            UnvalidFields = value;
        }

        public string GetStrFirstName()
        {
            return strFirstName;
        }

        public void SetStrFirstName(string value)
        {
            if (CheckString(value,50, PersonFields.FirstName))
            {
                strFirstName = value;
              
            }
        }

        public string GetStrLastName()
        {
            return strLastName;
        }

        public void SetStrLastName(string value)
        {
            if (CheckString(value,50, PersonFields.LastName))
            {
                strLastName = value;
            }
        }

        public string GetStrCity()
        {
            return strCity;
        }

        public void SetStrCity(string value)
        {
            if (CheckString(value,30, PersonFields.City))
            {
                strCity = value;
            }
        }

        public int GetIntID()
        {
            return intID;
        }

        public void SetIntID(int value)
        {
            intID = value;
        }

        public bool GetIsValid()
        {
            return isValid;
        }

        private void SetIsValid(bool value)
        {
            isValid = value;
        }


        /// <summary>
        /// Überprüft eine Zeichenkette auf Inhalt und maxinale Länge.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="MaxLength"></param>
        /// <param name="personfield"></param>
        /// <returns></returns>
        private bool CheckString(string value,int MaxLength, PersonFields personfield)
        {
            if (value != null)
            {
                if ((value.Length > 0) && (value.Length< MaxLength))
                {
                    GetUnvalidFields().Remove(personfield);
                    if (GetUnvalidFields().Count == 0)
                    {
                        // Inhalt ist gültig
                        SetIsValid(true);
                    }
                    return (true);
                }

            }
            // Wenn das Feld noch nicht als fehlerhaft definiert ist, dann jetzt...
            if (!GetUnvalidFields().Contains(personfield))
            {
                GetUnvalidFields().Add(personfield);
            }

            // Wenn es min. ein fehlerhaftes Feld gibt, ist das Objekt ungültig
            if (GetUnvalidFields().Count > 0)
            {
                if(isValid) { SetIsValid(false); };
            }

            return (false);
        }
    }
}
