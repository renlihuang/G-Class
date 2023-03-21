using CS.IBLL.Basic;
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
    class ModuleRecipeDialog : IBaseModel
    {
        UserControl _view;

        RecipeViewModel _viewModel;

        public ModuleRecipeDialog(IRecipeService recipeService, IProductService productService)
        {
            _viewModel = new RecipeViewModel(recipeService, productService);
        }

        public void BindDefaultViewModel()
        {
            GetView().DataContext = _viewModel;
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {

        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new ModuleRecipeView();
            }

            return _view;
        }
    }
}
