using GeneticTSP.Model;
using System;
using System.Collections.Generic;
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

        private List<int[]> _population;
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

        public void Initialize()
        {
            //TODO tutaj wygenerować graf i ustawić te parametry ewolucyjne
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
    }
}
