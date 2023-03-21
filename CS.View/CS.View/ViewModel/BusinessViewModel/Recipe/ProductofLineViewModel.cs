using CS.IBLL.Basic;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel.Recipe
{
    class ProductofLineViewModel : ViewModelBase
    {
        public ProductofLineViewModel(IWorkStationService workStationService, IProductService productService, string recipeList)
        {
            _workStationService = workStationService;
            _productService = productService;
            //模组明细列表
            RecipeDetails = new ObservableCollection<ProductofLineEntity>();
            //模组列表
            ModuleList = new ObservableCollection<ProductofLineEntity>();
            //关联命令
            RemoveItemCommand = new RelayCommand(RemoveItem);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            AddRecipeCommand = new RelayCommand(AddRecipe);

            LoadRecipeDetails(recipeList);
        }

        /// <summary>
        /// 加载配方明细
        /// </summary>
        private async void LoadRecipeDetails(string recipeList)
        {
            //循环添加
            var recipeDetails = await _workStationService.GetRecipeListAsync();

            recipeDetails.ForEach(x =>
            {
                ModuleList.Add(x);
            });

            var ids = LoadIdList(recipeList, recipeDetails);

            if (ids != null && ids.Count > 0)
            {
                var result = await _workStationService.GetRecipeListByIDAsync(ids);

                if (result.IsSuccess)
                {
                    result.BackResult.ForEach(x =>
                    {
                        RecipeDetails.Add(x);
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeList"></param>
        /// <returns></returns>
        private List<long> LoadIdList(string recipeList, List<ProductofLineEntity> recipeDetails)
        {
            List<long> ids = new List<long>();
            List<ProductofLineEntity> productEntiries = null;
            if (!string.IsNullOrEmpty(recipeList))
            {
                try
                {
                    productEntiries = JsonConvert.DeserializeObject<List<ProductofLineEntity>>(recipeList);
                    foreach (ProductofLineEntity entiry in productEntiries)
                    {
                        var productEntity = recipeDetails.Find(x => x.ProductType == entiry.ProductType);
                        ids.Add((long)productEntity.Id);
                    }
                }
                catch (Exception ex)
                {

                }

            }

            return ids;
        }



        private void RemoveItem()
        {
            if (RecipeDetails.Contains(SelectedRecipe))
            {
                RecipeDetails.Remove(SelectedRecipe);
                SelectedRecipe = null;
            }
        }

        private void RemoveAll()
        {
            RecipeDetails.Clear();
            SelectedRecipe = null;
        }

        private void AddRecipe()
        {
            if (SelectedModule == null)
                return;



            if (RecipeDetails.Count == 0 ||
               SelectedRecipe == null)
            {
                //RecipeDetails.Add(SelectedModule);
                List<ProductofLineEntity> productEntities = new List<ProductofLineEntity>();
                productEntities = RecipeDetails.ToList();
                productEntities.Add(SelectedModule);
                productEntities.Sort((a, b) => {
                    return a.Id <= b.Id ? -1 : 1;
                });
                RecipeDetails.Clear();
                for (int i = 0; i < productEntities.Count; i++)
                {
                    RecipeDetails.Add(productEntities[i]);
                }
            }
            else
            {
                int index = RecipeDetails.IndexOf(SelectedRecipe);

                if (index != -1)
                {
                    RecipeDetails.Insert(index, SelectedModule);
                    SelectedRecipe = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RecipeList
        {
            get
            {
                string recipeList = string.Empty;
                if (RecipeDetails.Count > 0)
                {
                    List<ProductofLineEntiry> ids = new List<ProductofLineEntiry>();
                    foreach (var item in RecipeDetails)
                    {
                        ids.Add(new ProductofLineEntiry { ProductType = item.ProductType });
                    }

                    recipeList = JsonConvert.SerializeObject(ids);
                }

                return recipeList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        readonly IRecipeService _recipeService;
        readonly IWorkStationService _workStationService;

        readonly IProductService _productService;

        /// <summary>
        /// 当前选中的配方
        /// </summary>
        ProductofLineEntity _selectedRecipe;
        public ProductofLineEntity SelectedRecipe
        {
            set
            {
                if (_selectedRecipe != value)
                {
                    _selectedRecipe = value;
                    RaisePropertyChanged();
                }
            }
            get { return _selectedRecipe; }
        }


        public string RecipeName { set; get; }

        /// <summary>
        /// 当前选中的配方
        /// </summary>
        ProductofLineEntity _selectedModule;
        public ProductofLineEntity SelectedModule
        {
            set { _selectedModule = value; }
            get { return _selectedModule; }
        }

        /// <summary>
        /// 删除单个命令
        /// </summary>
        public RelayCommand RemoveItemCommand { private set; get; }

        /// <summary>
        /// 删除所有命令
        /// </summary>
        public RelayCommand RemoveAllCommand { private set; get; }

        /// <summary>
        /// 删除所有命令
        /// </summary>
        public RelayCommand AddRecipeCommand { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ProductofLineEntity> RecipeDetails { private set; get; }

        /// <summary>
        /// 已经添加的模组列表
        /// </summary>
        public ObservableCollection<ProductofLineEntity> ModuleList { private set; get; }

    }
}
