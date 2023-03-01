using System;
using System.Collections.Generic;
using System.Text;

namespace CSViewAdminService.Core
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MyTableNameAttribute : Attribute
    {
        public MyTableNameAttribute(string tableName)
        {
            Value = tableName;
        }
        public string Value { get; private set; }
    }
}
