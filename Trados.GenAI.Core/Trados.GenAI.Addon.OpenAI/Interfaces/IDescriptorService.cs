using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Trados.GenAI.Addon.OpenAI.Interfaces
{
    /// <summary>
    /// Used to return the app descriptor
    /// </summary>
    public interface IDescriptorService
    {
        /// <summary>
        /// Gets the descriptor.
        /// </summary>
        /// <returns></returns>
        JsonNode GetDescriptor();

        /// <summary>
        /// Gets the secret configurations ids.
        /// </summary>
        /// <returns></returns>
        List<string> GetSecretConfigurations();
    }
}
