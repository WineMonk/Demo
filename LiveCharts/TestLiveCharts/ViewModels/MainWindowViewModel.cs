using LiveChartsCore;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TestLiveCharts.ViewModels.ChartViews;
using TestLiveCharts.Views.ChartViews;

namespace TestLiveCharts.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IRegionManager _regionManager;
        IContainerProvider _containerProvider;
        public MainWindowViewModel(IRegionManager regionManager, IContainerProvider containerProvider) 
        {
            _regionManager = regionManager;
            _containerProvider = containerProvider;
            BaseSeriesCommand = new DelegateCommand(BaseSeries);
        }

        public ICommand BaseSeriesCommand {  get; set; }

        private void BaseSeries()
        {
            IRegion region = _regionManager.Regions["chartRegion"];
            object? rv = region.Views.FirstOrDefault(v => v is BaseSeriesView);
            if (rv == null)
            {
                rv = _containerProvider.Resolve<BaseSeriesView>();
                region.Add(rv);
            }
            region.Activate(rv);
        }
    }
}
