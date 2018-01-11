using GeneticTSP.Extensions;
using GeneticTSP.Helpers;
using GeneticTSP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP.Services
{
    public class TSPSolver
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        BackgroundWorker _solver;
        public bool PendingNewWorker = false;
        public bool IsFinished = false;
        public bool IsInitialized = false;

        public ObservableCollection<KeyValuePair<int, int>> Results = new ObservableCollection<KeyValuePair<int, int>>();
        private List<Route> _population;
        private Graph _graph;

        public TSPSolver()
        {
            // BackgroundWorker do wykonywania operacji asynchronicznie
            _solver = new BackgroundWorker();
            _solver.WorkerSupportsCancellation = true;
            _solver.WorkerReportsProgress = true;
            _solver.DoWork += new DoWorkEventHandler(SolverDoWork);
            _solver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SolverCompleted);
        }

        public void Initialize(int size, bool symmetrical)
        {
            Results.Clear();
            _graph = new Graph(size, symmetrical);
            //pozostałe parametry ewolucyjne
            InitializePopulation();
            IsInitialized = true;
        }

        private void InitializePopulation() // pewnie parametr wielkości populacji, póki co dam ze 100
        {
            var basicPermutation = new int[_graph.Size - 1]; // każdy chromosom ma n+1 elementów, ale pierwszy i ostatni element to zawsze 0
            for (int i = 1; i < _graph.Size; i++)
            {
                basicPermutation[i - 1] = i;
            }
            _population = new List<Route>();
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int[] routeValues = new int[basicPermutation.Length];
                Array.Copy(basicPermutation, routeValues, basicPermutation.Length);
                var r = random.Next();
                random.Shuffle(routeValues);
                var route = new Route();
                route.order = routeValues;
                route.graph = _graph;
                _population.Add(route);
            }
        }

        public void Run()
        {
            if (_solver.IsBusy)
            {
                _solver.CancelAsync();
                PendingNewWorker = true;
            }

            else _solver.RunWorkerAsync();
        }

        public void Stop()
        {
            if (_solver.WorkerSupportsCancellation == true && _solver.IsBusy)
            {
                _solver.CancelAsync();
            }
        }

        private void SolverDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //TODO

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void SolverCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
            {
                log.Error("BackgroundWorker zakończył działanie przez błąd:   " + e.Error.Message);
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                log.Warn("BackgroundWorker zatrzymany");
                if (PendingNewWorker)
                {
                    PendingNewWorker = false;
                    _solver.RunWorkerAsync();
                }
            }
            else
            {
                log.Info("BackgroundWorker zakończył działanie");
            }
            IsFinished = true;
        }

        public void ProgressPopulation(int num)
        {
            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                var crossoverSubset = GeneticHelper.GetCrossoverSubset(_population, 0.50f);             
                while(crossoverSubset.Count > 1)
                {
                    var secondParentIdx = random.Next(0, crossoverSubset.Count());
                    var child = GeneticHelper.Crossover(crossoverSubset[0], crossoverSubset[secondParentIdx]);
                    if (random.Next(1, 100) < 10)
                        child = GeneticHelper.Mutation(child);
                    _population.Add(child);
                    crossoverSubset.RemoveAt(secondParentIdx);
                    crossoverSubset.RemoveAt(0);
                }
                _population = GeneticHelper.TrimPopulation(_population, 100);
                Results.Add(new KeyValuePair<int, int>(Results.Count + 1, GetBestResult()));
            }
        }

        public int GetBestResult()
        {
            var bestResult = int.MaxValue;
            foreach (Route route in _population)
            {
                bestResult = Math.Min(bestResult, route.fitness);
            }
            return bestResult;
        }

        
    }
}
