﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSViewAdminService.Core
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MyIgnoreAttribute : Attribute
    {
    }
}
