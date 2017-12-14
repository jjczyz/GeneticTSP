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
        private ObservableCollection<KeyValuePair<int, int>> _results = new ObservableCollection<KeyValuePair<int, int>>();
        public ObservableCollection<KeyValuePair<int, int>> Results
        {
            get { return _results; }
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
            RunButtonCommand = new RelayCommand(param => Solver.Run());
            StopButtonCommand = new RelayCommand(param => Solver.Stop());
            ProgressPopulationBy10ButtonCommand = new RelayCommand(param => ProgressPopulation(10));
            ProgressPopulationBy100ButtonCommand = new RelayCommand(param => ProgressPopulation(100));
            GraphSymmetrical = true;
        }

        private void Run(object v)
        {
            Solver.Stop();
            Solver = new TSPSolver();
        }

        private void ProgressPopulation(int num) //to będzie potem w Solverze
        {
            for (int i = 0; i < num; i++)
            {
                if (Results.Count() == 0)
                    Results.Add(new KeyValuePair<int, int>(1, 1000));
                else
                    Results.Add(new KeyValuePair<int, int>(Results.Count, 1000 / Results.Count()));
            }
        }

        public void NotifyPropertyChanged(string s)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }
}
