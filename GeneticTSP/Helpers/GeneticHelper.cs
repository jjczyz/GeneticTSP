using GeneticTSP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP.Helpers
{
    public static class GeneticHelper
    {
        public static Route Crossover(Route parentOne, Route parentTwo)
        {
            Route child = new Route();
            child.order = new int[parentOne.order.Length];
            child.graph = parentOne.graph;
            Random rand = new Random();
            int passedGenesNum = rand.Next(1, child.order.Length - 1);
            int[] passedGenes = new int[passedGenesNum];

            for (int i = 0; i < passedGenesNum; i++)
            {
                int passedIdx = rand.Next(0, child.order.Length - 1);
                child.order[passedIdx] = parentOne.order[passedIdx];
                passedGenes[i] = child.order[passedIdx];
            }
            int j = 0; //indeks w rodzicu
            for (int i = 0; i < child.order.Length; i++)
            {
                while (passedGenes.Contains(parentTwo.order[j]))
                    j++;
                if (child.order[i] == 0)
                {
                    child.order[i] = parentTwo.order[j];
                    j++;
                }
            }
            return child;
        }

        public static List<Route> GetCrossoverSubset(List<Route> population, float crossoverRatio)
        {
            population = population.OrderBy(x => x.fitness).ToList();
            var parentsAmount = (int)Math.Round(crossoverRatio * population.Count());
            return population.GetRange(0, parentsAmount);
        }

        public static List<Route> TrimPopulation (List<Route> population, int maxAmount)
        {
            population = population.OrderByDescending(x => x.fitness).ToList();
            while(population.Count > maxAmount)
            {
                population.RemoveAt(0);
            }
            return population;
        }
    }
}
