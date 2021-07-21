using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceExample
{
    internal interface IScopedProcessingService
    {
        void DoWork();
    }
}
