using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP.Model
{
    public class Route
    {
        public int[] order { get; set; }
        public int fitness
        {
            get { return CalculateFitness(); }
        }
        public Graph graph { get; set; }

        private int CalculateFitness()
        {
            var fitness = graph.Distances[0, order[1]] // odleglosc z miasta 0 do pierwszego elementu
                        + graph.Distances[order[order.Length - 2], 0]; //odleglosc z ostatniego elementu do miasta 0
            for (int i = 1; i < order.Length; i++)
            {
                fitness += graph.Distances[order[i - 1], order[i]];
            }
            return fitness;
        }
    }
}
