using DPSIW.Common.Models;
using System.Text.Json;



namespace DPSIW.Common.Agents
{
    public class MedicalNotesAgent : IAgent
    {

        Tuple<string,bool> PreValidate(MedicalNotes data)
        {

            if (string.IsNullOrEmpty(data.pid) || string.IsNullOrEmpty(data.cid) || string.IsNullOrEmpty(data.type) || data.metadata is null || string.IsNullOrEmpty(data.metadata.fileUrl) || string.IsNullOrEmpty(data.metadata.blobName))
            {
                return new Tuple<string, bool>("One or more missing required fields: pid, cid, fileUrl and blobName", false);
            }

            return new Tuple<string, bool>("", true);
        }

        public async Task ProcessAsync(CancellationToken token, string message)
        {
            Console.WriteLine($"Processing message: {message}");
            await Task.Delay(10);
            var msg = JsonSerializer.Deserialize<MedicalNotes>(message)!;
                
            var result = PreValidate(msg);
            var (error, isValid) = result;
            if (!isValid)
            {
                Console.WriteLine(error);
                throw new ApplicationException(error);
            }

            // Download blob

            // Speech to Text

            // LLM Summarize
        }
    }
}
