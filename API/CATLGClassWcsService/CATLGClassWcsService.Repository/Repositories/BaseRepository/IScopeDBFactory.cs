using CATLGClassWcsService.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CATLGClassWcsService.Repository
{
    internal interface IScopeDBFactory
    {
        CustomDatabase GetScopeDb();
    }
}
