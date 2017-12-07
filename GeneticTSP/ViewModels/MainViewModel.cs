using System;
using System.Collections.Generic;
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

        public MainViewModel()
        {
            ExitButtonCommand = new RelayCommand(ExitButtonClick);
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
