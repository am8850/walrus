using System.Text;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Transcription;

namespace DPSIW.Common.Services
{
    public class AzureSTTService
    {
        private readonly SpeechConfig speechConfig;

        public AzureSTTService(string key, string region, string language = "en-US")
        {
            speechConfig = SpeechConfig.FromSubscription(key, region);
            speechConfig.SpeechRecognitionLanguage = "en-US";
            speechConfig.SetProperty(PropertyId.SpeechServiceResponse_DiarizeIntermediateResults, "true");
        }

        public async Task<string> TranscribeAsync(string filepath, string? targetPath = null)
        {
            if (string.IsNullOrEmpty(targetPath))
            {
                targetPath = Utilities.Utilities.FileGenerator();
            }

            var stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            StringBuilder sb = new();

            var success = true;
            // Create an audio stream from a wav file or from the default microphone
            using (var audioConfig = AudioConfig.FromWavFileInput(filepath))
            {
                // Create a conversation transcriber using audio stream input
                using (var conversationTranscriber = new ConversationTranscriber(speechConfig, audioConfig))
                {
                    conversationTranscriber.Transcribing += (s, e) =>
                    {
                        //Console.WriteLine($"TRANSCRIBING: Text={e.Result.Text} Speaker ID={e.Result.SpeakerId}");
                    };

                    conversationTranscriber.Transcribed += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            //Console.WriteLine();
                            Console.WriteLine($"Speaker: {e.Result.SpeakerId}\n{e.Result.Text}\n");
                            sb.AppendLine($"Speaker: {e.Result.SpeakerId}\n{e.Result.Text}\n");
                            //Console.WriteLine();
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            Console.WriteLine($"NOMATCH: Speech could not be transcribed.");
                        }
                    };

                    conversationTranscriber.Canceled += (s, e) =>
                    {
                        Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                            stopRecognition.TrySetResult(0);
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    conversationTranscriber.SessionStopped += (s, e) =>
                    {
                        Console.WriteLine("\n    Session stopped event.");
                        stopRecognition.TrySetResult(0);
                    };

                    await conversationTranscriber.StartTranscribingAsync();

                    // Waits for completion. Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny([stopRecognition.Task]);

                    if (sb.Length > 0)
                    {
                        Console.WriteLine("Transcript:\n" + sb.ToString());
                        try
                        {
                            await File.WriteAllTextAsync(targetPath, sb.ToString());
                            Console.WriteLine($"Transcription written to file {targetPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unable to write transcription to file {targetPath} with error: {ex.Message}");
                            
                            // Indicate that there was an error
                            success = false;
                        }
                    }
                    else
                    {                        
                        Console.WriteLine("Error: No transcriptions was generated.");
                        // Indicate that there was an error
                        success = false;
                    }

                    await conversationTranscriber.StopTranscribingAsync();
                }
            }

            return success ? targetPath : "";
        }
    }
}
