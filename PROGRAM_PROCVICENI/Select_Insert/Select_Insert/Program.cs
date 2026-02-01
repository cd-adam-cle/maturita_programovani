using System.Security.Cryptography;

namespace Select_Insert;


class Program
{
    static void Main(string[] args)
    {
        BubbleSort cernoch = new BubbleSort();
        Console.WriteLine(String.Join('_',cernoch.Bubble_serazeni()));
        Console.WriteLine("");
        MergeSort negr = new MergeSort();
        negr.Merge( 0, negr.pole.Length - 1,negr.pole);
        Console.WriteLine(string.Join('<',negr.pole));
        
    }

    class Algorytmy
    {
        Random R_C = new Random();
        public int[] pole { get; private set; }

        protected int delkapole { get;  set; }

        public Algorytmy()
        {
            delkapole = R_C.Next(8,18);
            pole = new int[delkapole];
            for (int i = 0; i < delkapole; i++)
            {
                pole[i] = R_C.Next(67, 71);
            }
            Console.WriteLine(string.Join(',',pole));
        }
    }

    class InsertSort : Algorytmy
    {
        public InsertSort() : base() { }

        public int[] Insert_serazeni()
        {
            for (int i = 1; i < delkapole; i++)
            {
                int j = i - 1;
                int negr = pole[i];

                while (j >= 0 && pole[j] > negr)
                {
                    pole[j + 1] = pole[j];
                    j--;
                }
                pole[j+1] = negr;
            }
            return pole;
        }
    }

    class SelectSort : Algorytmy
    {
        public SelectSort() : base() { }

        public int[] VyberSerad()
        {
            for (int i = 0; i < delkapole - 1; i++)
            { 
                int dosavad_min = i;
                
                for (int j = i + 1; j < delkapole; j++)
                {
                    if (pole[j] < pole[dosavad_min])
                    {
                        dosavad_min = j;
                    }
                }

                int dih = pole[i];
                pole[i] = pole[dosavad_min];
                pole[dosavad_min] = dih;
            }

            return pole;
        }
    }

    class BubbleSort : Algorytmy
    {
        public BubbleSort() : base(){}

        public int[] Bubble_serazeni()
        {
            bool jeprehozeno = true;
            while (jeprehozeno == true)
            {
                jeprehozeno = false;
                for (int i = 1; i < delkapole; i++)
                {
                    if (pole[i - 1] > pole[i])
                    {
                        jeprehozeno = true;
                        int dih = pole[i - 1];
                        pole[i - 1] = pole[i];
                        pole[i] = dih;
                    }
                }
            }
            return pole;
        }
    }

    class MergeSort : Algorytmy
    {
        public MergeSort( ):base(){}

        public void Merge(int levy,int pravy , int[] pole )
        {
            if (levy >= pravy){return;}

            int prostredek = (levy + pravy) / 2;
            Merge(levy, prostredek, pole);
            Merge(prostredek + 1, pravy, pole);
            Sort(pole, prostredek ,pravy, levy);
        }

        static void Sort(int[] pole, int prostredek,int p, int l)
        {
            int delka_1 = prostredek - l ;
            int delka_2 = p - prostredek;

            int[] levypole = new int[delka_1];
            int[] pravepoel = new int[delka_2];
            for (int i = 0; i < delka_1; i++)
            {
                levypole[i] = pole[l+i];
            }

            for (int i = 0; i < delka_2; i++)
            {
                pravepoel[i] = pole[i + prostredek + 1];
            }
            int f = 0;
            int j = 0;
            int k = l;  
            
            while (f < delka_1 && j < delka_2)
            {
                if (levypole[f] <= pravepoel[j])
                {
                    pole[k] = levypole[f];
                    f++;
                }
                else
                {
                    pole[k] = pravepoel[j];
                    j++;
                }
                k++;
            }

            while (f < delka_1)
            {
                pole[k] = levypole[f];
                f++;
                k++;
            }

            while (j < delka_2)
            {
                pole[k] = pravepoel[j];
                j++;
                k++;
            }

        }
    }

    class Quicksort : Algorytmy
    {
        public Quicksort() : base() { }
        
        public int[] Quicktrida(int[] pole)
        {
            QuickSortRecursive(pole, 0, pole.Length - 1);
            return pole;
        }

        static void QuickSortRecursive(int[] pole, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = PartitionLomuto(pole, low, high);
                QuickSortRecursive(pole, low, pivotIndex - 1);
                QuickSortRecursive(pole, pivotIndex + 1, high);
            }
        }

        static int PartitionLomuto(int[] pole, int low, int high)
        {
            int pivot = pole[high];
            int i = low - 1;
    
            for (int j = low; j < high; j++)
            {
                if (pole[j] < pivot)
                {
                    i++;
                    Swap(pole, i, j);
                }
            }
    
            Swap(pole, i + 1, high);
            return i + 1;
        }

        static void Swap(int[] pole, int a, int b)
        {
            (pole[a], pole[b]) = (pole[b], pole[a]);  // C# tuple swap
        }

}