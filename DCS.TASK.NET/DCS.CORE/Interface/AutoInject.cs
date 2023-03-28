using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.CORE
{
    /// <summary>
    /// 作用域
    /// </summary>
    public interface IAutoInjectScoped { }

    /// <summary>
    /// 瞬时
    /// </summary>
    public interface IAutoInjectTransient { }

    /// <summary>
    /// 单例
    /// </summary>
    public interface IAutoInjectSingleton { }
}
