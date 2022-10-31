using System;
using System.Data;
using System.Security.Policy;
using Devart.Data.SQLite;

namespace Personendaten.model
{
    /// <summary>
    /// Klasse zur Verwaltung der Zugriffe auf die SQLite Datenbank
    /// </summary>
    internal class SQLDataHandler
    {
        private SQLiteConnection dbConnection;  // Speichert die Datenbankverbindung
        private String strQuery;                // Speichert die auszuführende Query-Anweisung
        /// </summary>
            
        // Getter & Setter ****************************************************************
        public string StrQuery { get => strQuery; set => strQuery = value; }


        /// <summary>
        /// Erstellt ein neues Objekt der Klasse und initialisiert die Grundelemente. 
        /// </summary>
        /// <param name="strStartupPath">Enthält den Dateipfad zur Anwendung</param>
        public SQLDataHandler(string strStartupPath)
        {
            String connectionString = "data source=C:\\Users\\andreas.bendig\\source\\repos\\Personendaten\\Personendaten\\db\\Personendaten.db"; // TODO: ist noch anzupassen !!!
            //String connectionString = "data source="+ strStartupPath +"\\db\\Personendaten.db";
            dbConnection = new SQLiteConnection(connectionString); 
            dbConnection.Open();
        }

        /// <summary>
        /// Liest eine Sammlung aller Personenobjekte aus der Datenbank aus
        /// </summary>
        /// <returns>Sammlung von Personen-Ojekten aus der Datenbank.</returns>
        public PersonCollection<Person> ReadData()
        {

            PersonCollection<Person> newCollection = new PersonCollection<Person>();

            SQLiteCommand myCommand = dbConnection.CreateCommand();
            myCommand.CommandText = strQuery;

            SQLiteDataReader myDataReader = myCommand.ExecuteReader();

            while (myDataReader.Read())
            {
                Person newPerson = new Person();
                newPerson.SetIntID(Int32.Parse(myDataReader.GetString(0)));
                newPerson.SetStrFirstName(myDataReader.GetString(1));
                newPerson.SetStrLastName(myDataReader.GetString(2));
                newPerson.SetStrCity(myDataReader.GetString(3));

                if (newPerson.GetIsValid())
                {
                    newCollection.Add(newPerson);
                }
                else
                {
                    Console.WriteLine("Unvalid Entry ID: " + newPerson.GetIntID());
                }

            }
            //dbConnection.Close();   ??HowTo
            return newCollection;
        }

        /// <summary>
        /// Schreibt Informationen über ein SQLite Statement in die Datenbank
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public Boolean WriteData(String strQuery)
        {
            // dbConnection.Open();
            SQLiteCommand myCommand = dbConnection.CreateCommand();
            myCommand.CommandText = strQuery;
            try
            {
                myCommand.ExecuteNonQuery();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           

            // dbConnection.Close();    ??HowTo
            return true;
        }

        /// <summary>
        /// Exportiert die gesamte Datenbank im XML-Format.
        /// </summary>
        /// <param name="path">Der Export-Pfad der Datei</param>
        /// <returns>Liefert wahr, ween der Export erfolgreich war</returns>
        public bool ExportXML(string path)
        {
            PersonCollection<Person> ExportCollection = new PersonCollection<Person>();
            StrQuery = "SELECT * FROM tbl_persondata";
            ExportCollection = ReadData();

            DataTable myDataTable = new DataTable();
            myDataTable.TableName = "persondata";
            myDataTable.Columns.Add("id", typeof(int));
            myDataTable.Columns.Add("firstname", typeof(string));
            myDataTable.Columns.Add("lastname", typeof(string));
            myDataTable.Columns.Add("city", typeof(string));

            foreach (Person person in ExportCollection)
            {
                myDataTable.Rows.Add(person.GetIntID(), person.GetStrFirstName(), person.GetStrLastName(), person.GetStrCity());
            }

            myDataTable.WriteXml(path + @"\export.xml");


            return true;
        }

        /// <summary>
        /// Importiert eine XML-Datei und fügt die Einträge der Datenbank hinzu
        /// </summary>
        /// <param name="path">Dateipfad der zu importiernden XML-Datei</param>
        /// <returns>Liefert wahr, wenn der Import erfolgreich war</returns>
        public bool ImportXML(String path)
        {
            DataTable myDataTable = new DataTable();
            myDataTable.TableName = "persondata";
            myDataTable.Columns.Add("id", typeof(int));
            myDataTable.Columns.Add("firstname", typeof(string));
            myDataTable.Columns.Add("lastname", typeof(string));
            myDataTable.Columns.Add("city", typeof(string));

            myDataTable.ReadXml(path);



            for (int intCounter = 0; intCounter < myDataTable.Rows.Count; intCounter++)
            {
               // Person person = new Person((int)myDataTable.Rows[intCounter]["id"],
                    Person person = new Person(myDataTable.Rows[intCounter]["firstname"].ToString(),
                    myDataTable.Rows[intCounter]["lastname"].ToString(),
                    myDataTable.Rows[intCounter]["city"].ToString());

                if (person.GetIsValid())
                {
                    //strQuery = "INSERT INTO tbl_persondata (id,firstname,lastname,city) VALUES ('" + person.GetIntID() + "','" + person.GetStrFirstName() + "','" + person.GetStrLastName() + "','" + person.GetStrCity() + "')";
                    strQuery = "INSERT INTO tbl_persondata (firstname,lastname,city) VALUES ('" + person.GetStrFirstName() + "','" + person.GetStrLastName() + "','" + person.GetStrCity() + "')";
                    WriteData(strQuery);
                }
            }
            return true;
        }

    }
}
