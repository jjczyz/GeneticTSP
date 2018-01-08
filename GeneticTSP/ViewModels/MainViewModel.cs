using GeneticTSP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeneticTSP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand StopButtonCommand { get; set; }
        public ICommand RunButtonCommand { get; set; }
        public ICommand ProgressPopulationBy10ButtonCommand { get; set; }
        public ICommand ProgressPopulationBy100ButtonCommand { get; set; }

        TSPSolver Solver;
        //wyjściowa tablica wszystkich pokoleń i najlepszego wyniku w każdym pokoleniu
        
        public ObservableCollection<KeyValuePair<int, int>> Results
        {
            get { return Solver.Results; }
        }

        private int _graphSize { get; set; }
        public int GraphSize
        {
            get { return _graphSize; }
            set
            {
                if (_graphSize != value)
                {
                    _graphSize = value;
                    NotifyPropertyChanged("Option1");
                }
            }
        }

        private bool _graphSymmetrical { get; set; }
        public bool GraphSymmetrical
        {
            get { return _graphSymmetrical; }
            set
            {
                if (_graphSymmetrical != value)
                {
                    _graphSymmetrical = value;
                    NotifyPropertyChanged("GraphSymmetrical");
                }
            }
        }



        private int _option1 { get; set; }
        public int Option1Max
        {
            get { return 100; }
        }
        public int Option1Min
        {
            get { return 0; }
        }
        public int Option1
        {
            get { return _option1; }
            set
            {
                if (_option1 != value)
                {
                    if (value > Option1Max)
                        _option1 = Option1Max;
                    else if (value < Option1Min)
                        _option1 = Option1Max;
                    else
                        _option1 = value;
                    NotifyPropertyChanged("Option1");
                }
            }
        }

        public MainViewModel()
        {
            Solver = new TSPSolver();
            RunButtonCommand = new RelayCommand(Run);
            StopButtonCommand = new RelayCommand(param => Solver.Stop());
            ProgressPopulationBy10ButtonCommand = new RelayCommand(param => ProgressPopulation(10));
            ProgressPopulationBy100ButtonCommand = new RelayCommand(param => ProgressPopulation(100));
            GraphSymmetrical = true;
            GraphSize = 10;
        }

        private void ProgressPopulation(int num)
        {
            Solver.Stop();
            if(!Solver.IsInitialized)
                Solver.Initialize(GraphSize, GraphSymmetrical);
            Solver.ProgressPopulation(num);
        }

        private void Run(object v)
        {
            Solver.Stop();
            Solver.Initialize(GraphSize, GraphSymmetrical);
            Solver.Run();
        }

        public void NotifyPropertyChanged(string s)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }
}
