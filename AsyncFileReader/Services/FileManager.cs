using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class FileManager
{
    private static string filePath; // Variable für den Dateipfad
    private static string fileContent; // Variable für den Dateiinhalt
    private static int lineCount; // Variable für die Anzahl der Zeilen

    // Öffentliche Eigenschaft für den Dateipfad
    public static string FilePath
    {
        get { return filePath; }
    }

    // Methode zum Öffnen eines Dateidialogs
    public static void OpenFileDialog()
    {
        Thread staThread = new Thread(() =>  // SingleThread da es Probleme gibt mit Dialogfenstern in der Konsole
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\"; // Standard-Startverzeichnis
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Filter für Dateitypen
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)  // Beim Klicken auf "OK"
                {
                    // Speichert den ausgewählten Dateipfad
                    filePath = openFileDialog.FileName;
                    Console.WriteLine($"Datei ausgewählt: {filePath}");
                }
                else
                {
                    Console.WriteLine("Es wurde keine Datei ausgewählt.");
                }
            }
        });

        staThread.SetApartmentState(ApartmentState.STA);  // Setzt auf den STA Zustand - Single Thread Apartment
        staThread.Start(); // Startet den Thread
        staThread.Join(); // Warten, bis der Thread beendet ist
    }

    public static async Task ReadFileAsync()
    {
        try
        {
            // Überprüfen, ob der Dateipfad gesetzt wurde
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Es wurde keine Datei ausgewählt.");
                return;
            }

            using var cts = new CancellationTokenSource(); // Erstellen eines Cancellation-Token Objekts
            var progressTask = ProgressReporter.ShowProgressAsync(cts.Token); // Startet die Fortschritt-Methode

            // Dateiinhalt asynchron einlesen
            fileContent = await ReadFileContentAsync(filePath);

            // Zeilen zählen - \r\n sind Windows spezifisch (Environment.NewLine - allgemein), 
            lineCount = fileContent.Split(new[] { "\r\n" }, StringSplitOptions.None).Length;

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
        return await reader.ReadToEndAsync();  // Eigene Methode vom StreamReader - liest Bis zum Ende ein
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
