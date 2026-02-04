using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trados.GenAI.LMStudio.Models;

namespace Trados.GenAI.LMStudio.Extensions
{
    public static class ChatCompletionRequestExtensions
    {
        public static void AddBase64Image(this ChatCompletionRequest request, string base64)
        {
            var lastItem = request.Messages.LastOrDefault();
            if (lastItem != null)
            {
                var messageImagePart = MessageItemPart.CreateMessageImageItemPart(base64);
                lastItem.Content.Add(messageImagePart);
            }
        }
    }
}
