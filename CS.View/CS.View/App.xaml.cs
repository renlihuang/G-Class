using CS.Base.AppSet;
using CS.BLL;
using CS.View.Common;
using CS.View.ViewDlg;
using CS.View.ViewDlg.UICoreViewDlg;
using DCS.BASE;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows;

namespace CS.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 倒计时关闭系统
        /// </summary>
        public static CountDownApplication countDownAppcation = new CountDownApplication();

        /// <summary>
        /// 激活程序
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(System.EventArgs e)
        {
            base.OnActivated(e);

            countDownAppcation.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDeactivated(System.EventArgs e)
        {
            base.OnDeactivated(e);

            countDownAppcation.SetActive(false);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //注册程序集
            OnRegisterService();

         

            //设置appSettings
            AppConfig.SetAppSettings(ConfigurationManager.AppSettings);
            //设置皮肤
            ServiceFiguration.LoadSkin();
            //模式选择对话框
            ShowLoginModeDialog();
            //ShowSanCardLoginDialog();
        }

        /// <summary>
        /// 显示刷卡登录对话框
        /// </summary>
        private void ShowSanCardLoginDialog()
        {
            SanCardDlg sanCardDlg = new SanCardDlg();
            sanCardDlg.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowLoginModeDialog()
        {

            //显示模式选中对话框
            LoginModeDlg loginModeDlg = new LoginModeDlg();
            loginModeDlg.ShowDialog();
            ////获取对话框值
            var result = loginModeDlg.GetDialogResult();
        }


        /// <summary>
        /// 依赖注入服务
        /// </summary>

        private void OnRegisterService()
        {
            //创建依赖注入容器
            ServiceCollection serviceCollection = new ServiceCollection();
            //添加HTTP工厂类
            serviceCollection.AddHttpClient();
            //添加HHTTP类
            serviceCollection.AddScoped<RequestToHttpHelper>();
            //创建自动注册服务
            AutoInjectService autoInjectService = new AutoInjectService(serviceCollection);
            //注册BLL程序集
            autoInjectService.RegisterAssembly(RegisterBLLService.GetAssembly());
            //设置依赖注入容器的引用
            DialogManage.SetServiceCollection(serviceCollection);
        }
    }
}