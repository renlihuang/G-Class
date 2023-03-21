using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;

namespace CS.View.Common
{
    /// <summary>
    /// 皮肤管理器
    /// </summary>
    internal class ServiceFiguration
    {
        private static string skinIniFilePath = AppDomain.CurrentDomain.BaseDirectory + "Config\\";

        /// <summary>
        /// 获取皮肤资源名称
        /// </summary>
        /// <returns></returns>
        public static string GetSkinName()
        {
            string skinName = "";
            //读取文件
            IniFile iniFile = new IniFile("Skin.ini");
            skinName = iniFile.IniReadValue("Skin", "SkinName");

            return skinName;
        }

        /// <summary>
        /// 加载皮肤
        /// </summary>
        public static void LoadSkin()
        {
            string SkinName = GetSkinName();

            if (SkinName == "")
                return;

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

        /// <summary>
        /// 设置皮肤名称
        /// </summary>
        /// <param name="skinName"></param>
        public static void SetSkinName(string skinName)
        {
            IniFile iniFile = new IniFile("Skin.ini");
            iniFile.IniWriteValue("Skin", "SkinName", skinName);
        }
    }
}