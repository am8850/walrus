using DPSIW.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPSIW.Common.Services
{
    public class MockProducer
    {
        private readonly SBService sbservice;

        public MockProducer(SBService sbService)
        {
            sbservice = sbService;
        }

        public async Task ProduceAsync(int number)
        {
            try
            {
                for (var i = 0; i < number; i++)
                {
                    var metadata = new MedicalNotesMetadata("url", "", "", "", "https://storage/notes/jmdoe-123.wav", "notes/jmdoe-123.wav");
                    var note = new MedicalNotes(Guid.NewGuid().ToString(), "jmdoe", Guid.NewGuid().ToString(), "medicalnotes", metadata, DateTime.UtcNow);
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
