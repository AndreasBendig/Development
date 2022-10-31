using System;
using Personendaten.controller;

namespace Personendaten
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Controller MyController = new Controller();
        }
    }
}
