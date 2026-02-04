using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Interfaces
{
    public interface IPipelineStep : IDisposable
    {
        StepStatus Status { get; }

        Task ExecuteAsync();
    }
}
