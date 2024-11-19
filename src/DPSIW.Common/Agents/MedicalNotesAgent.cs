using DPSIW.Common.Exceptions;
using DPSIW.Common.Models;
using DPSIW.Common.Services;
using OpenAI.Chat;
using System.Text.Json;



namespace DPSIW.Common.Agents
{
    public class MedicalNotesAgent : IAgent
    {
        private readonly AzureBlobStorageService storageService;
        private readonly OpenAIService llmService;
        private readonly AzureSTTService ttsService;

        public MedicalNotesAgent(AzureBlobStorageService? blobStorage = null, OpenAIService? openai = null, AzureSTTService? tts=null)
        {
            Settings settings = new();
            //storageService = blobStorage;
            if (blobStorage is null)
            {
                storageService = new AzureBlobStorageService(settings.storageConnectionString);
            }
            else
            {
                storageService = blobStorage;
            }

            if (openai is null)
            {
                llmService = new OpenAIService(settings);
            }
            else
            {
                llmService = openai;
            }
            if (tts is null)
            {
                ttsService = new AzureSTTService(settings.speechKey, settings.speechRegion);
            }
            else
            {
                ttsService = tts;
            }
        }


        Tuple<string, bool> PreValidate(MedicalNotes data)
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
                throw new DeadLetterException(error);
            }

            // Download the blob in the message
            var (_, ext) = Utilities.Utilities.GetFileNameAndExtension(msg.metadata.fileUrl!);
            var outFile = Utilities.Utilities.FileGenerator(ext);
            await storageService.DownloadBlob(msg.metadata.fileUrl!, outFile);

            // Transcribe the audio file and save it to a temp txt file
            var transcriptFile = await ttsService.TranscribeAsync(outFile, Utilities.Utilities.FileGenerator());


            // LLM Summarize the transcription
            ChatMessage[] messages = [
                new SystemChatMessage("You are an AI that can help summarize a patiend doctor conversation."),
                new UserChatMessage(File.ReadAllText(transcriptFile))
                ];
            var completion = await llmService.ChatCompletion(messages);
            Console.WriteLine("Summary:\n" + completion);

            // Delete all temp file
            Utilities.Utilities.DeleteFile(outFile);
            Utilities.Utilities.DeleteFile(transcriptFile);


        }
    }
}
