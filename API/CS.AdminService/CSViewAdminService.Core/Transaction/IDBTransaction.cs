using System;

namespace CSViewAdminService.Core
{
    public interface IDBTransaction : IDisposable
    {
        void Complete();
    }
}
