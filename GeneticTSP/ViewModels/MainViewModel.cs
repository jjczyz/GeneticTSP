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

        public ICommand ExitButtonCommand { get; set; }

        //wyjściowa tablica wszystkich pokoleń i najlepszego wyniku w każdym pokoleniu
        private ObservableCollection<KeyValuePair<int, int>> _results = new ObservableCollection<KeyValuePair<int, int>>(); 
        public ObservableCollection<KeyValuePair<int, int>> Results 
        {
            get { return _results; }
        }

        public MainViewModel()
        {
            ExitButtonCommand = new RelayCommand(ExitButtonClick);
            Results.Add(new KeyValuePair<int, int>(0, 100));
            Results.Add(new KeyValuePair<int, int>(1, 60));
            Results.Add(new KeyValuePair<int, int>(2, 30));
            Results.Add(new KeyValuePair<int, int>(3, 15));
            Results.Add(new KeyValuePair<int, int>(4, 13));
            Results.Add(new KeyValuePair<int, int>(5, 13));
        }

        private void ExitButtonClick(object v)
        {
            Environment.Exit(0);
        }

        public void NotifyPropertyChanged(string s)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }
}
