class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Willkommen beim Datei-Reader!");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1. Datei einlesen");
            Console.WriteLine("2. Anzahl der Zeilen anzeigen");
            Console.WriteLine("3. Datei-Inhalt anzeigen");
            Console.WriteLine("4. Beenden");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await FileManager.ReadFileAsync();
                    break;
                case "2":
                    FileManager.ShowLineCount();
                    break;
                case "3":
                    FileManager.ShowFileContent();
                    break;
                case "4":
                    Console.WriteLine("Programm wird beendet. Auf Wiedersehen!");
                    return;
                default:
                    Console.WriteLine("Ungültige Auswahl. Bitte versuchen Sie es erneut.");
                    break;
            }

            Console.WriteLine("Drücken Sie eine beliebige Taste, um fortzufahren...");
            Console.ReadKey();
        }
    }
}
