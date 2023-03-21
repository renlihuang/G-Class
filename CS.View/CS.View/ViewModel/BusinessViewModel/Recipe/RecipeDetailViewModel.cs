using CS.Base.HttpHelper;
using CS.IBLL.Basic;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
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
    class RecipeDetailViewModel : ViewModelBase
    {
        public RecipeDetailViewModel(IRecipeService recipeService, IProductService productService, string recipeList)
        {
            _recipeService = recipeService;
            _productService = productService;
            //模组明细列表
            RecipeDetails = new ObservableCollection<ProductEntity>();
            //模组列表
            ModuleList = new ObservableCollection<ProductEntity>();
            //关联命令
            RemoveItemCommand = new RelayCommand(RemoveItem);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            AddRecipeCommand = new RelayCommand(AddRecipe);

            LoadRecipeDetails(recipeList);
        }

        /// <summary>
        /// 加载配方明细
        /// </summary>
        private  async void LoadRecipeDetails(string recipeList)
        {
            //循环添加
            var recipeDetails =  await _recipeService.GetRecipeListAsync();

            recipeDetails.ForEach(x => 
            {
                ModuleList.Add(x);
            });

            var ids = LoadIdList(recipeList, recipeDetails);

            if (ids != null&& ids.Count>0)
            {
               var result =  await _recipeService.GetRecipeListByIDAsync(ids);

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
        private List<long> LoadIdList(string recipeList, List<ProductEntity> recipeDetails)
        {
            List<long> ids = new List<long>();
            List<ProductEntiry> productEntiries = null;
            if (!string.IsNullOrEmpty(recipeList))
            {
                try
                {
                    productEntiries = JsonConvert.DeserializeObject<List<ProductEntiry>>(recipeList);
                    foreach (ProductEntiry entiry in productEntiries)
                    {
                        var productEntity = recipeDetails.Find(x => x.ProductName == entiry.ProductName);
                        ids.Add((long)productEntity.Id);
                    }
                }
                catch (Exception ex) 
                {
                    throw ex;
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
                List<ProductEntity> productEntities =new List<ProductEntity>();
                productEntities= RecipeDetails.ToList();
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
                    List<ProductEntiry> ids = new List<ProductEntiry>();
                    foreach (var item in RecipeDetails)
                    {
                        ids.Add(new ProductEntiry { ProductCode = item.ProductCode, ProductName = item.ProductName });
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

        readonly IProductService _productService;

        /// <summary>
        /// 当前选中的配方
        /// </summary>
        ProductEntity _selectedRecipe;
        public ProductEntity SelectedRecipe
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
        ProductEntity _selectedModule;
        public ProductEntity SelectedModule
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
        public  ObservableCollection<ProductEntity> RecipeDetails { private set; get; }

        /// <summary>
        /// 已经添加的模组列表
        /// </summary>
        public ObservableCollection<ProductEntity> ModuleList { private set; get; }

    }
}
