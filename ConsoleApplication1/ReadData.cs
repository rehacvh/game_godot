using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SmallWorld;



namespace SmallWorld
{
    class ReadData
    {
        public static Dictionary<string, List<ActorsEdges>> adj = new Dictionary<string, List<ActorsEdges>>();  //O(1)
        public static Dictionary<string, int> sharedMovies = new Dictionary<string, int>();                     //O(1)
        public List<string> actors = new List<string>();                                                        //O(1)
        public void ReadSample(int option)   //O(movies*(actors^2))
        {
            string filename = @"C:\Users\green\Desktop\SmallWorldPhenomenon\Algorithms-Project-main\small\Case1\Movies193.txt"; //O(1)
            var lines = File.ReadLines(filename);    //O(1)
            string movie = "";                       //O(1)
            foreach (var line in lines)            //O(Movies*(actors^2)) //Lines
            {
                string fileLine = (string)line;       //O(1)
                string[] subs = fileLine.Split('/'); 
                movie = subs[0];                     //O(1)   
                for (int i = 1; i < subs.Length; i++)   //O(subs.Length)
                {
                    actors.Add(subs[i]);    //O(1)
                }

                for (int i = 0; i < actors.Count; i++) //O( actors^2 )
                {
                    if (!adj.ContainsKey(actors[i]))   //O(1)
                    {
                        adj.Add(actors[i], new List<ActorsEdges>()); 
                    }

                    /*try
                    {adj.Add(actors[i], new List<ActorsEdges>());}
                    catch{}*/


                    for (int j = 0; j < actors.Count; j++)  //O(actors)
                    {
                        if (i != j)       //O(1)
                        {
                            ActorsEdges AE = new ActorsEdges(actors[i], actors[j], movie);
                            adj[actors[i]].Add(AE);
                            string stest = actors[i] + actors[j];
                            string stest2 = actors[j] + actors[i];
                            if(sharedMovies.ContainsKey(stest) && sharedMovies.ContainsKey(stest2))
                            {
                                sharedMovies[stest]++;
                                sharedMovies[stest2]++;
                            }else 
                            {
                                sharedMovies.Add(stest, 1);
                                sharedMovies.Add(stest2, 1);
                            }
                           
                        }
                    }
                }
                actors = new List<string>();     //O(1)
            }
            Console.WriteLine("Done Reading Movie File!");    //O(1)

            if (option == 3)   
            {
                BuildGraph BG = new BuildGraph(adj);  //O(1)
                BG.Bonuse();
            }
        }
        public void ReadQueries(int opt) //O(queries*(AdjList^2))
        {
            string filename = @"C:\Users\green\Desktop\SmallWorldPhenomenon\Algorithms-Project-main\small\Case1\queries110.txt";   //O(1)
            var lines = File.ReadLines(filename);   //O(1)
            string actor1, actor2;   //O(1)


            Console.WriteLine("Query \t Degree \t RS \t Chain");   //O(1)
            foreach (var line in lines)   //O(queries*(AdjList^2))  //lines
            {
                string fileLine = (string)line;     //O(1)
                string[] subs = fileLine.Split('/');    //O(1)
                Console.WriteLine();                    //O(1)
                actor1 = subs[0];                       //O(1)
                actor2 = subs[1];                       //O(1)
                try
                {
                    List<ActorsEdges> t1 = adj[actor1];  //O(1)
                    List<ActorsEdges> t2 = adj[actor2];  //O(1)
                    BuildGraph BG = new BuildGraph(adj); //O(1)
                    BG.CalculateDeg(actor1, actor2,opt, sharedMovies);     //O(AdjList^2)          
                }catch
                {
                    Console.WriteLine("The Entered Actor1 neither Actor2 Doesn't Exist!! ");  //O(1)
                }
            }
            Console.WriteLine("done reading queries");   //O(1)
        }
    }
}
