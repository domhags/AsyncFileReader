class FileManager
{
    private static string filePath = @"C:\Export\Hagspiel\AsyncFileReader\text.txt"; // Beispielpfad zur Testdatei
    private static string fileContent; // Variable für den Dateiinhalt
    private static int lineCount; // Variable für die Anzahl der Zeilen

    public static async Task ReadFileAsync()
    {
        try
        {
            // Asynchrones Einlesen der Datei
            using var cts = new CancellationTokenSource(); // Erstellen eines Cancellation-Token Objekts
            var progressTask = ProgressReporter.ShowProgressAsync(cts.Token); // Startet die Fortschritt-Methode

            // Dateiinhalt asynchron einlesen
            fileContent = await ReadFileContentAsync(filePath);

            /* Environment.NewLine steht für die Zeilenendesequenz.
            * Die Split-Methode teilt den Dateiinhalt (fileContent) an jeder neuen Zeile auf und gibt ein Array zurück.
            * StringSplitOptions.None bedeutet, dass auch leere Zeilen im Array enthalten bleiben.
            * Length gibt die Anzahl der Elemente im Array zurück, was der Anzahl der Zeilen im Dateiinhalt entspricht.*/
            lineCount = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length; // Zeilen zählen


            cts.Cancel(); // Stoppe die Fortschrittsanzeige
            Console.WriteLine("Datei wurde erfolgreich eingelesen.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Fehler: Die Datei konnte nicht gefunden werden. {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Fehler: Ein I/O-Fehler ist aufgetreten. {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: Ein unerwarteter Fehler ist aufgetreten. {ex.Message}");
        }
    }

    private static async Task<string> ReadFileContentAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Die angegebene Datei existiert nicht.", filePath);
        }

        using var reader = new StreamReader(filePath);
        return await reader.ReadToEndAsync();
    }

    public static void ShowLineCount()
    {
        Console.WriteLine($"Die Datei enthält {lineCount} Zeilen.");
    }

    public static void ShowFileContent()
    {
        if (string.IsNullOrEmpty(fileContent))
        {
            Console.WriteLine("Die Datei wurde noch nicht eingelesen.");
        }
        else
        {
            Console.WriteLine("Dateiinhalt:");
            Console.WriteLine(fileContent);
        }
    }
}
