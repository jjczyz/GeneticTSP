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

        private int _populationSize { get; set; }
        public int PopulationSize
        {
            get { return _populationSize; }
            set
            {
                if (_populationSize != value)
                {
                    if (value > 500)
                        _crossoverRatio = 500;
                    else if (value < 3)
                        _crossoverRatio = 3;
                    else
                        _crossoverRatio = value;
                    NotifyPropertyChanged("CrossoverRatio");
                }
            }
        }

        private int _crossoverRatio { get; set; }
        public int CrossoverRatio //w %
        {
            get { return _crossoverRatio; }
            set
            {
                if (_crossoverRatio != value)
                {
                    if (value > 100)
                        _crossoverRatio = 0;
                    else if (value < 100)
                        _crossoverRatio = 0;
                    else
                        _crossoverRatio = value;
                    NotifyPropertyChanged("CrossoverRatio");
                }
            }
        }

        private int _mutationRatio { get; set; }
        public int MutationRatio // w %
        {
            get { return _mutationRatio; }
            set
            {
                if (_mutationRatio != value)
                {
                    if (value > 100)
                        _mutationRatio = 0;
                    else if (value < 100)
                        _mutationRatio = 0;
                    else
                        _mutationRatio = value;
                    NotifyPropertyChanged("MutationRatio");
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
            GraphSize = 30;
            PopulationSize = 100;
            CrossoverRatio = 50;
            MutationRatio = 30;
        }

        private void ProgressPopulation(int num)
        {
            Solver.Stop();
            if(!Solver.IsInitialized)
                Solver.Initialize(GraphSize, GraphSymmetrical, PopulationSize, CrossoverRatio, MutationRatio);
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
