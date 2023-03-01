using CSViewAdminService.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSViewAdminService.DAL
{
    internal class UpdateModel<TPrimaryKeyType>
    {
        public List<IEntity<TPrimaryKeyType>> UpdateBatchEntityList { get; set; }

        public IEntity<TPrimaryKeyType> UpdateEntity { get; set; }
        public IEntity<TPrimaryKeyType> SaveEntity { get; set; }

        public List<IEntity<TPrimaryKeyType>> SaveBatchEntityList { get; set; }

        public UpdatePartFieldModel<TPrimaryKeyType> UpdatePartField { get; set; }

        public UpdateSingleFieldByIdsModel<TPrimaryKeyType> UpdateSingleFieldByIds { get; set; }

        public UpdateTypeEnum UpdateType { get; set; }
    }


    internal enum UpdateTypeEnum
    {
        UpdatePartField,
        UpdateBatch,
        Update,
        UpdateSingleFieldByIds,
        Save,
        SaveBatch
    }
    internal class UpdatePartFieldModel<TPrimaryKeyType>
    {

        public IEntity<TPrimaryKeyType> Entity { get; set; }

        public IList<string> Fields { get; set; }

    }


    internal class UpdateSingleFieldByIdsModel<TPrimaryKeyType>
    {

        public IList<TPrimaryKeyType> IdList { get; set; }

        public KeyValuePair<string, object> Column { get; set; }

    }



    internal class UpsertDeleteModel<TPrimaryKeyType>
    {
        public List<IEntity<TPrimaryKeyType>> NewEntityList { get; set; }
        public List<TPrimaryKeyType> OldIdList { get; set; }
    }
}
