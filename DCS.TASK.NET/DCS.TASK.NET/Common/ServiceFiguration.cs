using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
    internal class ServiceFiguration
    {
        /// <summary>
        /// 加载皮肤
        /// </summary>
        public static void LoadSkin()
        {
            string SkinName = "indigo";
            SwatchesProvider swatchesProvider = new SwatchesProvider();
            //获取皮肤列表
            Swatch swatch = null;
            foreach (var swatche in swatchesProvider.Swatches)
            {
                if (swatche.Name == SkinName)
                {
                    swatch = swatche;
                    break;
                }
            }

            if (swatch != null)
            {
                SetSkin(swatch);
            }
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        /// <param name="swatch"></param>
        public static void SetSkin(Swatch swatch)
        {
            //皮肤管理器
            PaletteHelper paletteHelper = new PaletteHelper();

            ITheme theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor(swatch.ExemplarHue.Color);

            paletteHelper.SetTheme(theme);
        }
    }

}
