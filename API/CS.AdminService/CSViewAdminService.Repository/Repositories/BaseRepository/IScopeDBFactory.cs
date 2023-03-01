using CSViewAdminService.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSViewAdminService.Repository
{
    internal interface IScopeDBFactory
    {
        CustomDatabase GetScopeDb();
    }
}
