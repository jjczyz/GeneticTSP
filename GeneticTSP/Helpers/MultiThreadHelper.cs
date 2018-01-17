using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeneticTSP.Helpers
{
    public static class MultiThreadHelper
    {
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
        }

        public static void ClearOnUI<T>(this ICollection<T> collection)
        {
            Action clearMethod = collection.Clear;
            Application.Current.Dispatcher.BeginInvoke(clearMethod);
        }
    }
}
