using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.LMStudio.Models;

namespace Trados.GenAI.LMStudio.Extensions
{
    public static class TranslationContextExtensions
    {
        public static ChatCompletionRequest ToChatCompletionRequest(this ITranslationContext context)
        {
            var chatCompletionRequest = new ChatCompletionRequest();
            chatCompletionRequest.Model = context.Model;
            if (!string.IsNullOrEmpty(context.SystemInstructions))
            {
                chatCompletionRequest.Messages.Add(MessageItem.CreateSystemMessageItem(context.SystemInstructions));
            }

            if (!string.IsNullOrEmpty(context.UserPrompt))
            {
                chatCompletionRequest.Messages.Add(MessageItem.CreateUserMessageItem(context.UserPrompt));
            }

            return chatCompletionRequest;
        }

        public static CompletionRequest ToCompletionRequest(this ITranslationContext context)
        {
            return new CompletionRequest()
            {
                Model = context.Model,
                Prompt = context.UserPrompt,
            };
        }
    }
}
