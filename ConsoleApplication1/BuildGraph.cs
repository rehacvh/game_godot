using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallWorld
{
    class BuildGraph
    {
        static Dictionary<string, List<ActorsEdges>> AdjList;           //O(1)
        static Dictionary<string, KeyValuePair<int, int>> VertexInfo;   //O(1)
        static Dictionary<string, KeyValuePair<string, string>> InfoMatrix; //O(1)
        static Dictionary<KeyValuePair<string, string>, bool> visited;        //O(1)
        static Dictionary<string, int> SHAREDMOVIES;                        //O(1)
        public BuildGraph(Dictionary<string, List<ActorsEdges>> adj)   //O(1)
        {
            AdjList = adj;
            VertexInfo = new Dictionary<string, KeyValuePair<int, int>>();
            InfoMatrix = new Dictionary<string, KeyValuePair<string, string>>();
            visited = new Dictionary<KeyValuePair<string, string>, bool>();
        }
        public void CalculateDeg(string actor1, string actor2, int opt, Dictionary<string, int> sharedMovies)   //O(AdjList^2)
        {
            Console.Write(actor1 + "/" + actor2);   //O(1)
            SHAREDMOVIES = sharedMovies;            //O(1)
            KeyValuePair<int, int> res = Dijkstra(actor1, actor2, opt);    //O(AdjList^2)
            Console.Write("\t  " + res.Key + " \t \t ");     //O(1)
            Console.Write(res.Value + "   \t");              //O(1)
            BuildChain(actor1, actor2);  //O(AdjList)
        }

        public KeyValuePair<int, int> Dijkstra(string actor1, string actor2, int opt)    //O(AdjList^2)
        {
            
            //try { VertexInfo.Add(actor1, new KeyValuePair<int, int>(0, 0)); }catch{}
            VertexInfo.Add(actor1, new KeyValuePair<int, int>(0, 0));    //O(1)
            prioqueue pq = new prioqueue();                     //O(1)
            pq.Enqueue(new ActorsEdges("", actor1, ""), 0);        //O(1)
        
            while(!pq.IsEmpty())      //O(AdjList)
            {
                ActorsEdges edge = (ActorsEdges)pq.Peek();      //O(1)

                pq.Dequeue();    //O(1)
                string to = edge.to;   //O(1)
                string from = edge.from;    //O(1)
                KeyValuePair<string, string> k1 = new KeyValuePair<string, string>(to, from);   //O(1)
                KeyValuePair<string, string> k2 = new KeyValuePair<string, string>(from, to);   //O(1)

                if (visited.ContainsKey(k1) && visited.ContainsKey(k2))    //O(1)
                {
                    continue;
                }
                else    //O(1)
                {
                    visited.Add(new KeyValuePair<string, string>(to, from), true);
                    visited.Add(new KeyValuePair<string, string>(from, to), true);
                }
                /*try
                {                  
                    visited.Add(new KeyValuePair<string, string>(to,from), true);
                    visited.Add(new KeyValuePair<string, string>(from, to), true);
                }
                catch
                {
                    continue;
                }*/

                for (int i = 0; i < AdjList[edge.to].Count; i++)   
                {

                    ActorsEdges neighbour = AdjList[edge.to][i];    
                    
                    if(!VertexInfo.ContainsKey(neighbour.to))
                    {
                        VertexInfo.Add(neighbour.to, new KeyValuePair<int, int>(int.MaxValue, -int.MaxValue));
                    }
                    

                    /*try
                    {
                        KeyValuePair<int,int> info = VertexInfo[neighbour.to];                         
                    }catch
                    {
                        VertexInfo.Add(neighbour.to, new KeyValuePair<int, int>(int.MaxValue , -int.MaxValue));                        
                    }*/


                    if (VertexInfo[edge.to].Key + neighbour.Edgecost < VertexInfo[neighbour.to].Key)     //O(1)
                    {                    

                        int moviesCount = 0;
                        string s = edge.to + neighbour.to;
                        moviesCount = SHAREDMOVIES[s] / 2;

                        /*try
                        {
                            string s = edge.to + neighbour.to;
                            moviesCount = SHAREDMOVIES[s] / 2;
                        }
                        catch
                        {
                            string s = neighbour.to + edge.to;
                            moviesCount = SHAREDMOVIES[s] / 2;
                        }*/


                        VertexInfo[neighbour.to] = new KeyValuePair<int, int>(VertexInfo[edge.to].Key + neighbour.Edgecost , VertexInfo[edge.to].Value + moviesCount );
                         
                        if(InfoMatrix.ContainsKey(neighbour.to))
                        {
                            InfoMatrix[neighbour.to] = new KeyValuePair<string, string>(neighbour.from, neighbour.movie);
                        }else
                        {
                            InfoMatrix.Add(neighbour.to, new KeyValuePair<string, string>(neighbour.from, neighbour.movie));
                        }
                       /* try
                        {
                            InfoMatrix.Add(neighbour.to,new KeyValuePair<string, string>(neighbour.from,neighbour.movie));
                        }catch
                        {
                            InfoMatrix[neighbour.to] = new KeyValuePair<string, string>(neighbour.from, neighbour.movie);
                        }*/

                    }else if(VertexInfo[edge.to].Key + neighbour.Edgecost == VertexInfo[neighbour.to].Key)
                    {
                        int moviesCount = 0;
                        string s = edge.to + neighbour.to;
                        moviesCount = SHAREDMOVIES[s] / 2;

                        /*try
                        {
                            string s = edge.to + neighbour.to;
                            moviesCount = SHAREDMOVIES[s] / 2;
                        }
                        catch
                        {
                            string s = neighbour.to + edge.to;
                            moviesCount = SHAREDMOVIES[s] / 2;
                        }*/

                        if (VertexInfo[edge.to].Value + moviesCount > VertexInfo[neighbour.to].Value)
                        {                        
                           
                            VertexInfo[neighbour.to] = new KeyValuePair<int, int>(VertexInfo[neighbour.to].Key, VertexInfo[edge.to].Value + moviesCount);
                            if (InfoMatrix.ContainsKey(neighbour.to))
                            {
                                InfoMatrix[neighbour.to] = new KeyValuePair<string, string>(neighbour.from, neighbour.movie);
                            }
                            else
                            {
                                InfoMatrix.Add(neighbour.to, new KeyValuePair<string, string>(neighbour.from, neighbour.movie));
                            }
                            /* try
                             {                                
                                 InfoMatrix.Add(neighbour.to, new KeyValuePair<string, string>(neighbour.from, neighbour.movie));
                             }
                             catch
                             {
                                 InfoMatrix[neighbour.to] = new KeyValuePair<string, string>(neighbour.from, neighbour.movie);
                             }*/
                        }
                    }
                    pq.Enqueue(neighbour, VertexInfo[neighbour.to].Key);   //O(1)
                }
                if (edge.to == actor2 && opt == 2)        //O(1)
                {
                    return VertexInfo[actor2];
                }
            }
            if (opt == 3) { return VertexInfo[actor1]; }    //O(1)
            return VertexInfo[actor2];      //O(1)
        }
        public void BuildChain(string actor1, string actor2)     //O(AdjList)
        {
            Stack<string> movieChain = new Stack<string>();    //O(1)
            string test = actor2;                              //O(1)

            while (test != actor1)        //O(AdjList)  
            {
                movieChain.Push(InfoMatrix[test].Value);
                test = InfoMatrix[test].Key;
            }

            int i = 0;

            foreach (var element in movieChain)     //O(AdjList)
            {
                i++;
                if (i == movieChain.Count)
                {
                    Console.Write(element);
                }
                else
                    Console.Write(element + " -> ");
            }
            Console.WriteLine();
        }

        public void Bonuse()      //O(AdjList^2)
        {
            string src, dest = "";   //O(1)
            int maxrs = -1;          //O(1)
            int[] frequancy = new int[13];   //O(1)
            frequancy[0] = 1;    //O(1)
            Console.WriteLine("Enter Actor name: ");    //O(1)
            src = Console.ReadLine();      //O(1)

            this.Dijkstra(src, "", 3);     //O(AdjList^2)

            for (int index = 0; index < VertexInfo.Count; index++)   //O(VertexInfo.Count)
            {
                var item = VertexInfo.ElementAt(index); //<string , <int , int >>
                var actor = item.Key;   //string
                var deg = item.Value.Key;   //int deg
                var rs = item.Value.Value;  //int rs

                int dos = deg;
                if (dos < 12) frequancy[dos]++;
                else frequancy[12]++;

                if (rs > maxrs)
                {
                    maxrs = rs;
                    dest = actor;
                }
            }

            Console.WriteLine("Deg. of Separ.  \t Frequency.");
            Console.WriteLine("--------------------------------------");

            for (int i = 0; i < 13; i++)      //O(1)
            { 
                //print distribution of the degree of separation
                if (i == 12) Console.WriteLine(">"+ (i - 1) +" \t\t\t "+ (frequancy[i]));
                else Console.WriteLine(i + "\t\t\t " + frequancy[i]);
            }


            //print The strongest path (based on the relation strength)
            BuildChain(src, dest);          //O(AdjList)   
            Console.WriteLine("The strongest path (based on the relation strength): " + maxrs);
           //Console.ReadLine();
        }
    }
}
