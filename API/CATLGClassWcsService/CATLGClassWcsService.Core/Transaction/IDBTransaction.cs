using System;

namespace CATLGClassWcsService.Core
{
    public interface IDBTransaction : IDisposable
    {
        void Complete();
    }
}
