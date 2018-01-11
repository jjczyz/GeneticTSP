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
        public static Random rand = new Random();
        public static Route Crossover(Route parentOne, Route parentTwo)
        {
            Route child = new Route();
            child.order = new int[parentOne.order.Length];
            child.graph = parentOne.graph;
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

        public static Route Mutation(Route route)
        {
            var swappedIdxOne = rand.Next(0, route.order.Length - 1);
            var swappedIdxTwo = rand.Next(0, route.order.Length - 1);
            while(route.order.Length > 1 && swappedIdxOne == swappedIdxTwo)
                swappedIdxTwo = rand.Next(0, route.order.Length - 1);
            var temp = route.order[swappedIdxOne];
            route.order[swappedIdxOne] = route.order[swappedIdxTwo];
            route.order[swappedIdxTwo] = temp;
            return route;
        }

        public static List<Route> GetCrossoverSubset(List<Route> population, float crossoverRatio)
        {
            population = population.OrderBy(x => x.fitness).ToList();
            var parentsAmount = (int)Math.Round(crossoverRatio * population.Count());
            List<Route> crossoverSubset = new List<Route>();
            float fitnessSum = population.Sum(x => x.fitness);
            float reverseFitnessSum = population.Sum(x => (fitnessSum - x.fitness));
            //koło fortuny
            for (int i = 0; i < parentsAmount; i++)
            {
                var spinResult = rand.Next(1, 10000)/10000.0f;
                var popIdx = -1;
                while(spinResult > 0)
                {
                    popIdx++;
                    spinResult -= (fitnessSum - population[popIdx].fitness) / reverseFitnessSum;
                }
                if (crossoverSubset.Contains(population[popIdx]))
                {
                    i--;
                    continue;
                }
                crossoverSubset.Add(population[popIdx]);
            }
            return population.GetRange(0, parentsAmount);
        }

        public static List<Route> TrimPopulation (List<Route> population, int maxAmount)
        {
            population = population.OrderByDescending(x => x.fitness).ToList();
            List<Route> removedPop = new List<Route>();
            float fitnessSum = population.Sum(x => x.fitness);
            //koło fortuny
            for (int i = 0; i < population.Count() - maxAmount; i++)
            {
                var spinResult = rand.Next(1, 10000) / 10000.0f;
                var popIdx = -1;
                while (spinResult > 0)
                {
                    popIdx++;
                    spinResult -= population[popIdx].fitness / fitnessSum;
                }
                if (removedPop.Contains(population[popIdx]))
                {
                    i--;
                    continue;
                }
                removedPop.Add(population[popIdx]);
            }
            
            foreach(Route route in removedPop)
            {
                population.Remove(route);
            }

            return population;
        }
    }
}
