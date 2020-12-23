using System;
using System.Collections.Generic;
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
            HashSet<int> valsInCut = new HashSet<int>();
            
            Node n = cut;
            for (int j = 0; j < 3; j++)
            {
                valsInCut.Add(n.Val);
                n = n.Next;                    
            }

            while (valsInCut.Contains(dest))
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
            var nums = initial.ToCharArray().Select(n => (int)n - (int)'0');
            Node start = new Node(nums.First());
            Node prev = start;
            int highestID = -1;
            foreach (int v in nums.Skip(1))
            {
                Node n = new Node(v);
                prev.Next = n;
                prev = n;

                if (v > highestID)
                    highestID = v;
            }
            prev.Next = start;

            Node curr = start;
            for (int j = 0; j < rounds; j++)
            {
                // 1) Remove the three times after curr from the list
                Node cut = curr.Next;
                curr.Next = cut.Next.Next.Next; // 100% fine and not ugly code...

                // Find the val where we want to insert the cut nodes
                int destVal = findDestination(curr.Val, cut, highestID);

                Node ip = curr.Next;
                while (ip.Val != destVal)
                    ip = ip.Next;

                cut.Next.Next.Next = ip.Next;
                ip.Next = cut;

                curr = curr.Next;
            }

            if (!pt2)
                Console.WriteLine($"P1: {listToString(start, 1)}");
        }

        public void Solve()
        {
            playCrabGame("318946572", 0, 100, false);

        }
    }
}
