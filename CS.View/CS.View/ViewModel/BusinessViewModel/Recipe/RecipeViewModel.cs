using CS.IBLL.Basic;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common.Enum;
using CS.View.View.Dlg;
using CS.View.ViewModel.Base;
using CS.View.ViewModel.BusinessViewModel.Recipe;
using DCS.BASE;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    class RecipeViewModel: DataProcess<ProductofLineEntity>
    {
        readonly IRecipeService _recipeService;
        readonly IProductService _productService;

        public RecipeViewModel(IRecipeService recipeService, IProductService productService)
        {
            _recipeService = recipeService;
            _productService = productService;
            //关联命令
            EditRecipeCommand = new RelayCommand<ProductofLineEntity>(EditRecipe);
            //执行初始化、
            this.Init();
        }

        protected override void Reset()
        {
            RecipeName = string.Empty;
        }

        protected async override void GetPageData(int pageInex)
        {
            var result = await _recipeService.GetRecipeListAync(pageInex, PageSize, new ModuleAssembleRecipeCondition() { RecipeName = RecipeName });

            if (result.Data != null)
            {
                DataGridDatas.Clear();

                string Lines = string.Empty;
                foreach (var item in result.Data)
                {
                    //List<ProductEntiry> productLines = JsonHelper.DeserializeObject<List<ProductEntiry>>(item.ProductLine);
                    //foreach (ProductEntiry entiry in productLines)
                    //{
                    //    Lines += entiry.ProductName + ",";
                    //}
                    //item.ProductLine = Lines.Substring(0, Lines.Length - 1);
                    DataGridDatas.Add(item);
                }
            }
        }

        protected async override void Delete(ProductofLineEntity model)
        {
            if (await _recipeService.DeleteRecipeAync(model.Id))
            {
                base.Delete(model);
            }
      
        }


        protected async override void Save()
        {
            bool result = false;

            if (string.IsNullOrEmpty(Model.ProductType))
            {
                HnitText = "工艺类型不能为空";
                return;
            }


            //if (string.IsNullOrEmpty(Model.RecipeCode))
            //{
            //    HnitText = "配方编号不能为空";
            //    return;
            //}

            if (Mode == ActionMode.Add)
            {
                result = await _recipeService.AddRecipeAync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _recipeService.UpdateRecipeAync(Model.Id,Model);
            }

            if (result)
            {
                base.Save();
            }

        }

        /// <summary>
        /// 编辑配方
        /// </summary>
        /// <param name="entity"></param>
        private async void EditRecipe(ProductofLineEntity entity)
        {

            BaseMsgDialog dialog = new BaseMsgDialog();

            RecipeDetailViewModel recipeDetailViewModel = new RecipeDetailViewModel(_recipeService,_productService, entity.ProductLine)
            {
                RecipeName = entity.ProductType
            };
            //绑定视图
            dialog.BindDataContex<RecipeDetailDialog, RecipeDetailViewModel>(new RecipeDetailDialog(), recipeDetailViewModel);
            //显示对话框
            if (await dialog.ShowDialog())
            {
                entity.ProductLine = recipeDetailViewModel.RecipeList;
                //更新到数据库
                await _recipeService.UpdateRecipeAync(entity.Id,entity);
            }
        }

        /// <summary>
        /// 编辑配方命令
        /// </summary>
        public RelayCommand<ProductofLineEntity> EditRecipeCommand { private set; get; }

        /// <summary>
        /// 配方名称
        /// </summary>
        string _recipeName;
        public string RecipeName
        {
            set { _recipeName = value;RaisePropertyChanged();}
            get { return _recipeName; }
        }
    }
}
