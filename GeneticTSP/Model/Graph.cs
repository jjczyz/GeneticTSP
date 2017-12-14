using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP.Model
{
    public class Graph
    {

        public int Size { get; set; }

        //symetryczny tzn. że odległości między dwoma punktami są takie same w samą strone, więc Dij = Dji
        public bool Symmetrical { get; set; }

        //odległości między wszystkimi punktami, więc wektor rozmiar wektora [Size][Size]
        public int[,] Distances { get; set; }

        public Graph(int size, bool symmetrical)
        {
            Size = size;
            Symmetrical = symmetrical;
        }

        private void SetDistances()
        {
            Distances = new int[Size, Size]; //wszystkie możliwe odległości, łącznie z odległościami punktu do samego siebie

            Random rand = new Random();
            for (int i = 0; i < Distances.GetLength(0); i++)
                for (int j = 0; j <= Distances.GetLength(1); i++)
                {
                    if (i == j) //odległość z danego punktu do samego siebie, więc zostaje 0
                        continue;
                    if (Distances[i, j] != 0) //jeśli już coś było ustawione to znaczy, że robimy graf symetryczny i ta wartość zostaje
                        continue;
                    else
                    {
                        Distances[i, j] = rand.Next(1, 10); // odległości losowe od 1 do 10
                        if (Symmetrical) Distances[j, i] = Distances[i, j]; //jeśli symetryczny to ustawiamy też odległość w drugą strone (jak było 1 do 2 to 2 do 1)
                    }
                }
        }




    }
}
