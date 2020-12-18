using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    class CPU
    {
        public int PC { get; set; }
        public int Acc { get; set; }
        public bool LoopDetected { get; private set; }
        public bool Terminated { get; private set; }
        public (string, int)[] Instructions { get; set; }
        private List<string> history { get; set; }
        public HashSet<int> Seen { get; set; }

        public CPU()
        {
            history = new List<string>();
            Seen = new HashSet<int>();
            LoopDetected = false;
            Terminated = false;
        }

        public void Step()
        {
            var s = $"{PC.ToString().PadLeft(4)}: {Instructions[PC].Item1} {Instructions[PC].Item2}";
            history.Add(s);
            Seen.Add(PC);

            switch (Instructions[PC].Item1)
            {
                case "acc":
                    Acc += Instructions[PC].Item2;
                    ++PC;
                    break;
                case "nop":
                    ++PC;
                    break;
                case "jmp":
                    PC += Instructions[PC].Item2;
                    break;
            }

            if (PC >= Instructions.Length)
                Terminated = true;
            if (Seen.Contains(PC))
                LoopDetected = true;
        }

        public void DumpHistory()
        {
            foreach (var s in history)
                Console.WriteLine(s);
            Console.WriteLine($"Seen: {Seen.Count}");
        }
    }

    public class Day8 : IDay
    {
        public Day8() { }

        private (string, int) parse(string line)
        {
            var pieces = line.Split(' ');

            return (pieces[0], int.Parse(pieces[1]));
        }

        private CPU makeCPU()
        {
            using TextReader tr = new StreamReader("inputs/day8.txt");
            var lines = tr.ReadToEnd().Split('\n');

            return new CPU()
            {
                Acc = 0,
                PC = 0,
                Instructions = lines.Select(line => parse(line)).ToArray()
            };
        }

        public void Solve()
        {
            part1();
            part2();
        }

        private void part1()
        {
            var cpu = makeCPU();    

            while (cpu.PC <= cpu.Instructions.Length)
            {
                cpu.Step();                
                if (cpu.LoopDetected)
                {
                    Console.WriteLine($"P1: {cpu.Acc}");
                    break;
                }
            }
        }

        private void part2()
        {
            var cpu = makeCPU();
            while (!cpu.LoopDetected)
                cpu.Step();                
                
            // Okay, we've found the loop, now find all the instructions that may possibly
            // be causing the problem. Then check each one until we find the culprit. Brute
            // force but my only other idea was to check it by hand...
            var suspects = cpu.Seen.Where(i => cpu.Instructions[i].Item1 != "acc").ToList();

            foreach (int i in suspects)
            {
                var cpu2 = makeCPU();
                var instr = cpu2.Instructions[i];
                Console.WriteLine($"Flipping instruction {i} ({instr.Item1}, {instr.Item2})");
                instr.Item1 = instr.Item1 == "nop" ? "jmp" : "nop";
                cpu2.Instructions[i] = instr;

                while (!cpu2.LoopDetected && !cpu2.Terminated)
                    cpu2.Step();

                if (cpu2.Terminated)
                {
                    Console.WriteLine($"P2 {cpu2.Acc}");
                    break;
                }
            }
        }
    }
}
