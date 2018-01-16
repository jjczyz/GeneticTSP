using GeneticTSP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
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
        
        public ObservableCollection<KeyValuePair<int, int>> Results { get; set; }
        public static readonly object _resultsLock = new object();

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
                    if (value < 2)
                        _populationSize = 2;
                    else
                        _populationSize = value;
                    NotifyPropertyChanged("PopulationSize");
                }
            }
        }

        private float _crossoverRatio { get; set; }
        public float CrossoverRatio
        {
            get { return _crossoverRatio; }
            set
            {
                if (_crossoverRatio != value)
                {
                    if (value < 0.1f)
                        _crossoverRatio = 0.1f;
                    else
                        _crossoverRatio = value;
                    NotifyPropertyChanged("CrossoverRatio");
                }
            }
        }

        private float _mutationRatio { get; set; }
        public float MutationRatio
        {
            get { return _mutationRatio; }
            set
            {
                if (_mutationRatio != value)
                {
                    if (value < 0.1f)
                        _mutationRatio = 0.1f;
                    else if (value > 100.0f)
                        _mutationRatio = 100.0f;
                    else
                        _mutationRatio = value;
                    NotifyPropertyChanged("MutationRatio");
                }
            }
        }

        public MainViewModel()
        {
            Results = new ObservableCollection<KeyValuePair<int, int>>();
            BindingOperations.EnableCollectionSynchronization(Results, _resultsLock);
            Solver = new TSPSolver(this);          
            RunButtonCommand = new RelayCommand(Run);
            StopButtonCommand = new RelayCommand(param => Solver.Stop());
            ProgressPopulationBy10ButtonCommand = new RelayCommand(param => ProgressPopulation(10));
            ProgressPopulationBy100ButtonCommand = new RelayCommand(param => ProgressPopulation(100));
            GraphSymmetrical = true;

            //defaults
            GraphSize = 50;
            PopulationSize = 100;
            CrossoverRatio = 50;
            MutationRatio = 10;
        }

        private void ProgressPopulation(int num)
        {
            Solver.Stop();
            if(!Solver.IsInitialized)
                Solver.Initialize();
            Solver.ProgressPopulation(num);
        }

        private void Run(object v)
        {
            Solver.Stop();
            Solver.Initialize();
            Solver.Run();
        }

        public void NotifyPropertyChanged(string s)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }
}
