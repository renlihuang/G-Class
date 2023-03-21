using CS.IBLL.Business;
using CS.View.View.BusinessView;
using CS.View.ViewModel.BusinessViewModel;
using CS.View.ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CS.View.ViewDlg.BusinessViewDlg
{
    public class GlueRulesDlg : IBaseModel
    {
        private UserControl _view;

        private GlueRulesViewModel _viewModel;

        public GlueRulesDlg(IPalletRulesService palletRulesService)
        {
            _viewModel = new GlueRulesViewModel(palletRulesService);
        }

        public void BindDefaultViewModel()
        {
            if (_viewModel != null)
            {
                GetView().DataContext = _viewModel;
            }
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {
            GetView().DataContext = viewModel;
        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new GlueRulesView();
            }

            return _view;
        }
    }
}
