using Azure.AI.OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPSIW.Common.Services
{
    public class OpenAIService
    {
        private readonly AzureOpenAIClient azureClient;
        private readonly string chatModel;

        public OpenAIService(Settings settings)
        {
            azureClient = new AzureOpenAIClient(
                new Uri(settings.OpenAIEndpoint),
                new ApiKeyCredential(settings.OpenAIKey));

            chatModel = settings.OpenAIChatModel;
        }

        public async Task<string> ChatCompletion(ChatMessage[] messages, string? model = "gpt-4o")
        {
            try
            {
                Console.WriteLine("Processing LLM completion");
                if (string.IsNullOrEmpty(model))
                {
                    model = chatModel;
                }
                ChatClient chatClient = azureClient.GetChatClient(model);
                ChatCompletion completion = await chatClient.CompleteChatAsync(messages);
                //[
                //    // System messages represent instructions or other guidance about how the assistant should behave
                //    new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
                //    // User messages represent user input, whether historical or the most recent input
                //    new UserChatMessage("Hi, can you help me?"),
                //    // Assistant messages in a request represent conversation history for responses
                //    new AssistantChatMessage("Arrr! Of course, me hearty! What can I do for ye?"),
                //    new UserChatMessage("What's the best way to train a parrot?"),
                //]);

                return completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get chat completion: {ex.Message}");
                return "";
            }
        }
    }
}
