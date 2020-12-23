using System;
using System.Diagnostics;
using System.Linq;

namespace _2020
{
    internal class Node
    {
        public int Val { get; set; }
        public Node Next { get; set; }
        
        public Node(int v)
        {
            Val = v;
        }
    }

    public class Day23 : IDay
    {
        public Day23() { }

        private int findDestination(int start, Node cut, int highestID)
        {
            int dest = start == 1 ? highestID: start - 1;
            int a = cut.Val;
            int b = cut.Next.Val;
            int c = cut.Next.Next.Val;
  
            while (dest == a || dest == b || dest == c)
            {
                --dest;
                if (dest <= 0)
                    dest = highestID;
            }

            return dest;
        }

        private string listToString(Node node, int startVal)
        {
            while (node.Val != startVal)
                node = node.Next;
            node = node.Next;

            var str = "";            
            do
            {
                str += node.Val.ToString();
                node = node.Next;
            } while (node.Val != startVal);

            return str;
        }

        private void playCrabGame(string initial, int max, int rounds, bool pt2)
        {
            var nums = initial.ToCharArray().Select(n => (int)n - (int)'0').ToList();
            int highestID = nums.Max();

            if (max > 0)
                nums.AddRange(Enumerable.Range(highestID + 1, max - nums.Count));

            Node[] index = new Node[max == 0 ? 10 : max + 1];
            Node start = new Node(nums.First());
            index[nums.First()] = start;
            Node prev = start;            
            foreach (int v in nums.Skip(1))
            {
                Node n = new Node(v);
                index[v] = n;
                prev.Next = n;
                //n.Prev = prev;
                prev = n;

                if (v > highestID)
                    highestID = v;
            }
            prev.Next = start;
            
            Node curr = start;
            for (int j = 0; j < rounds; j++)
            {
                // Remove the three times after curr from the list
                Node cut = curr.Next;
                curr.Next = cut.Next.Next.Next; // 100% fine and not ugly code...
                
                // Find the val where we want to insert the cut nodes
                int destVal = findDestination(curr.Val, cut, highestID);

                Node ip = index[destVal];                
                Node ipn = ip.Next;
                Node tail = cut.Next.Next;
                tail.Next = ipn;
                ip.Next = cut;
                
                curr = curr.Next;
            }

            if (!pt2)
                Console.WriteLine($"P1: {listToString(start, 1)}");
            else
            {
                Node n = index[1];
                ulong res = (ulong)n.Next.Val * (ulong)n.Next.Next.Val;
                Console.WriteLine($"P2: {res}");
            }
        }

        public void Solve()
        {
            playCrabGame("318946572", 0, 100, false);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            playCrabGame("318946572", 1_000_000, 10_000_000, true);
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
        }
    }
}
