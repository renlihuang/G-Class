using NPoco;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using CSViewAdminService.Utility;
using CSViewAdminService.Core;

namespace CSViewAdminService.Repository.Repositories
{
    internal class CustomDatabase : Database
    {
        public CustomDatabase(string connection, DatabaseType databaseType, DbProviderFactory dbProviderFactory) : base(connection, databaseType, dbProviderFactory)
        {


        }
        protected override bool OnUpdating(UpdateContext updateContext)
        {
            var entity = updateContext.Poco as BaseField;
            if (entity != null)
            {
                var userInfo = CustomHeaderHelper.GetCustomHeader();
                entity.ModifyTime = DateTime.Now;
                entity.ModifierId = userInfo.Id;
                entity.CreateTime = null;
                entity.CreaterId = null;
            }
            return base.OnUpdating(updateContext);
        }

        protected override bool OnInserting(InsertContext insertContext)
        {
            var entity = insertContext.Poco as BaseField;
            if (entity != null)
            {
                var userInfo = CustomHeaderHelper.GetCustomHeader();
                entity.CreateTime = DateTime.Now;
                entity.CreaterId = userInfo.Id;
                entity.ModifyTime = DateTime.Now;
                entity.ModifierId = userInfo.Id;
            }
            return base.OnInserting(insertContext);
        }


    }
}
