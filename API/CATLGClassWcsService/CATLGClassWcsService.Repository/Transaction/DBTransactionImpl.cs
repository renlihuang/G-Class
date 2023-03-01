﻿using CATLGClassWcsService.Core;
using NPoco;
using System.Data;

namespace CATLGClassWcsService.Repository
{
    internal class DBTransactionImpl : IDBTransaction
    {
        IDatabase db;

        public DBTransactionImpl(IDatabase db)
        {
            this.db = db;
            this.db.BeginTransaction();
        }

        public virtual void Complete()
        {
            db.CompleteTransaction();
            db = null;
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.AbortTransaction();
            }
        }


    }
}
