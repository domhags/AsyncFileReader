class ProgressReporter
{
    // Methode zur asynchronen Ausgabe des Fortschritts auf der Konsole
    public static async Task ShowProgressAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)  // Prüft ob der Cancellation Token eine Abbruch-Abfrage bekommt
        {
            Console.WriteLine("Datei wird geladen...");
            await Task.Delay(500);
        }
    }
}
