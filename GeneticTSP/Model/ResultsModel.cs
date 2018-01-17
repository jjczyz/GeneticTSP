using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP.Model
{
    public class ResultsModel
    {
        private ObservableCollection<KeyValuePair<int, int>> _bestResults = new ObservableCollection<KeyValuePair<int, int>>();
        public ObservableCollection<KeyValuePair<int, int>> BestResults { get { return _bestResults; } }
    }
}
