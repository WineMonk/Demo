using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace ModuleA.ViewModels
{
    public class ViewAViewModel : BindableBase, INavigationAware
    {
        private string _title = "ViewA";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _pageViews;
        public int PageViews
        {
            get { return _pageViews; }
            set { SetProperty(ref _pageViews, value); }
        }

        public ViewAViewModel()
        {

        }

        /// <summary>
        /// 当实现者被导航到时调用。
        /// </summary>
        /// <param name="navigationContext">导航上下文</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            PageViews++;
        }

        /// <summary>
        /// 调用以确定此实例是否可以处理导航请求。
        /// </summary>
        /// <param name="navigationContext">导航上下文</param>
        /// <returns>如果这个实例接受导航请求，返回True;否则,False</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 当实现者被导航离开时调用。
        /// </summary>
        /// <param name="navigationContext">导航上下文</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
