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

        private int findDestination(int start, Node cut)
        {
            int dest = start == 1 ? 9: start - 1;
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
                    dest = 9;
            }

            return dest;
        }

        private void printList(Node node, int startVal)
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

            Console.WriteLine(str);
        }

        public void Solve()
        {
            // Build the initial circular linked list
            var nums = "318946572".ToCharArray().Select(n => (int)n - (int)'0');
            Node start = new Node(nums.First());
            Node prev = start;
            foreach (int v in nums.Skip(1))
            {
                Node n = new Node(v);
                prev.Next = n;
                prev = n;
            }
            prev.Next = start;

            Node curr = start;
            for (int j = 0; j < 100; j++)
            {
                // 1) Remove the three times after curr from the list
                Node cut = curr.Next;
                curr.Next = cut.Next.Next.Next; // 100% fine and not ugly code...

                // Find the val where we want to insert the cut nodes
                int destVal = findDestination(curr.Val, cut);

                Node ip = curr.Next;
                while (ip.Val != destVal)
                    ip = ip.Next;

                cut.Next.Next.Next = ip.Next;
                ip.Next = cut;

                curr = curr.Next;
            }

            printList(start, 1);
        }
    }
}
