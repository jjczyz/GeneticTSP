using GeneticTSP.Extensions;
using GeneticTSP.Helpers;
using GeneticTSP.Model;
using GeneticTSP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GeneticTSP.Services
{
    public class TSPSolver
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        BackgroundWorker _solver;
        public bool PendingNewWorker = false;
        public bool IsFinished = true;
        public bool IsInitialized = false;

        public int BestResult
        {
            get { return _vm.BestResult; }
            set { _vm.BestResult = value; }
        }

        private List<Route> _population;
        private Graph _graph;
        MainViewModel _vm;

        public TSPSolver(MainViewModel vm)
        {
            // BackgroundWorker do wykonywania operacji asynchronicznie
            _solver = new BackgroundWorker();
            _solver.WorkerSupportsCancellation = true;
            _solver.WorkerReportsProgress = true;
            _solver.DoWork += new DoWorkEventHandler(SolverDoWork);
            _solver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SolverCompleted);

            _vm = vm;
        }

        public void Initialize()
        {
            _vm.BestResults.Clear();
            _graph = new Graph(_vm.GraphSize, _vm.GraphSymmetrical);
            InitializePopulation(_vm.PopulationSize);
            IsInitialized = true;
        }

        private void InitializePopulation(int size) // pewnie parametr wielkości populacji, póki co dam ze 100
        {
            var basicPermutation = new int[_graph.Size - 1]; // każdy chromosom ma n+1 elementów, ale pierwszy i ostatni element to zawsze 0
            for (int i = 1; i < _graph.Size; i++)
            {
                basicPermutation[i - 1] = i;
            }
            _population = new List<Route>();
            Random random = new Random();
            for (int i = 0; i < size; i++)
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
            IsFinished = false;
            while(_vm.BestResults.Count() > 0)
                MultiThreadHelper.ClearOnUI(_vm.BestResults);
            bool hasPlateaued = false; // czy wyniki już się nie polepszają
            int stopTerm = 1000; //po ilu populacjach bez zmiany zatrzymać algorytm
            BestResult = GetBestResult();
            while(!hasPlateaued)
            {
                ProgressPopulation(1);
                if (GetBestResult() < BestResult) //wynik sie poprawił
                {
                    stopTerm = 1000;
                    BestResult = GetBestResult();
                }
                else
                    stopTerm--;
                if (stopTerm == 0)
                    hasPlateaued = true;

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
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
                var crossoverSubset = GeneticHelper.GetCrossoverSubset(_population, _vm.CrossoverRatio);             
                while(crossoverSubset.Count > 1)
                {
                    var secondParentIdx = random.Next(0, crossoverSubset.Count());
                    var child = GeneticHelper.Crossover(crossoverSubset[0], crossoverSubset[secondParentIdx]);
                    if (random.Next(1, 100) < _vm.MutationRatio)
                        child = GeneticHelper.Mutation(child);
                    _population.Add(child);
                    crossoverSubset.RemoveAt(secondParentIdx);
                    crossoverSubset.RemoveAt(0);
                }
                _population = GeneticHelper.TrimPopulation(_population, _vm.PopulationSize);
                System.Threading.Thread.Sleep(10);
                if (!_solver.CancellationPending && _solver.IsBusy)
                    MultiThreadHelper.AddOnUI(_vm.BestResults, new KeyValuePair<int, int>(_vm.BestResults.Count + 1, GetBestResult()));
                else if (_solver.IsBusy)
                    return;
                else
                    _vm.BestResults.Add(new KeyValuePair<int, int>(_vm.BestResults.Count + 1, GetBestResult()));
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
