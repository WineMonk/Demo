using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Windows;

namespace ModuleA.ViewModels
{
    public class ViewAViewModel : BindableBase, IConfirmNavigationRequest
    {
        public ViewAViewModel()
        {

        }

        /// <summary>
        /// 确定此实例是否接受导航。
        /// </summary>
        /// <param name="navigationContext">导航上下文</param>
        /// <param name="continuationCallback">指示导航何时可以继续的回调函数。</param>
        /// <remarks>
        /// 此方法的实现者在此方法完成之前不需要调用回调函数，但它们必须确保回调函数最终被调用。
        /// </remarks>
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            bool result = true;

            if (MessageBox.Show("Do you to navigate?", "Navigate?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                result = false;

            continuationCallback(result);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
