﻿using System;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace ModuleA.ViewModels
{
    public class ViewAViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        public ViewAViewModel()
        {

        }

        public bool KeepAlive
        {
            get
            {
                return true;
            }
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
