using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Windows.Controls;
using TestLiveCharts.Views.ChartViews;

namespace TestLiveCharts.ViewModels.ChartViews
{
    public class BaseSeriesViewModel : BindableBase
    {
        private string _title = "线系列";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private ObservableCollection<ISeries> _series = new ObservableCollection<ISeries>();
        public ObservableCollection<ISeries> Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        public ICommand ChangeSeriesCommand { get; set; }

        public BaseSeriesViewModel()
        {
            ChangeSeriesCommand = new DelegateCommand<string>(ChangeSeries);
        }

        private void ChangeSeries(string seriesType)
        {
            ISeries? series = null;
            if (seriesType == null)
            {
                return;
            }
            else if (seriesType == "Line")
            {
                Title = "线系列";
                series = Series.FirstOrDefault(s => s is LineSeries<int>);
                series ??= series = new LineSeries<int> { Values = GetValues() };
            }
            else if (seriesType == "Column")
            {
                Title = "列系列";
                series = Series.FirstOrDefault(s => s is ColumnSeries<int>);
                series ??= series = new ColumnSeries<int> { Values = GetValues() };
            }
            else if (seriesType == "Scatter")
            {
                Title = "散点系列";
                series = Series.FirstOrDefault(s => s is ScatterSeries<int>);
                series ??= series = new ScatterSeries<int> { Values = GetValues() };
            }
            else if (seriesType == "StepLine")
            {
                Title = "阶梯系列";
                series = Series.FirstOrDefault(s => s is StepLineSeries<int>);
                series ??= series = new StepLineSeries<int> { Values = GetValues() };
            }
            else if (seriesType == "Heat")
            {
                Title = "热力系列";
                series = Series.FirstOrDefault(s => s is HeatSeries<int>);
                series ??= series = new HeatSeries<int> { Values = GetValues() };
            }
            else if (seriesType == "Row")
            {
                Title = "行系列";
                series = Series.FirstOrDefault(s => s is RowSeries<int>);
                series ??= series = new RowSeries<int> 
                {
                    Values = new int[] { 5,2,1,1,3,1,4 } 
                };
            }
            else if (seriesType == "RowTest")
            {
                Title = "行系列";
                List<ISeries> seriesList = Series.Where(s => s is RowSeries<int>).ToList();
                if (seriesList.Count() > 0)
                {
                    foreach (var item in seriesList)
                    {
                        Series.Remove(item);
                    }
                }
                else
                {
                    Random random = new Random();
                    for (int i = 0; i < 7; i++)
                    {
                        Series.Add(new RowSeries<int>
                        {
                            
                            Values = new int[] { random.Next(100,200) }
                        });
                    }
                }
            }
            if (series == null)
            {
                return;
            }
            if (Series.Contains(series))
            {
                Series.Remove(series);
            }
            else
            {
                Series.Add(series);
            }
            
        }

        private int[] GetValues()
        {
            Random random = new Random();
            int[] ints = new int[7];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = random.Next(-5, 5);
            }
            return ints;
        }
    }
}
