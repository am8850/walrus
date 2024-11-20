using DPSIW.Common.Models;
using System.Text.Json;

namespace DPSIW.Common.Services
{
    public class MockProducerService
    {
        private readonly AzureServiceBusService sbservice;

        public MockProducerService(AzureServiceBusService sbService)
        {
            sbservice = sbService;
        }

        public async Task ProduceAsync(int number)
        {
            try
            {
                for (var i = 0; i < number; i++)
                {
                    var metadata = new MedicalNotesMetadata("url", "", "", "", "https://stdipsdevcus.blob.core.windows.net/medical-notes-in/jmdoe-0632e6482a.wav", "jmdoe-0632e6482a.wav");
                    var note = new MedicalNotes(Guid.NewGuid().ToString(), "jmdoe", Guid.NewGuid().ToString(), "medicalnotesagent", metadata, DateTime.UtcNow);

                    // Serialize note to JSON
                    Console.WriteLine("Producing Message: " + JsonSerializer.Serialize(note));

                    await sbservice.SendMessage(note);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to produce message: {ex.Message}");
            }
        }
    }
}
