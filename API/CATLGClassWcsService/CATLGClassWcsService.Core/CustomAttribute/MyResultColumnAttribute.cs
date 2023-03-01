﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CATLGClassWcsService.Core
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MyResultColumnAttribute : MyColumnAttribute
    {
        public MyResultColumnAttribute() { }
        public MyResultColumnAttribute(string name) : base(name) { }
    }
}
