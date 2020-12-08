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
        public (string, int)[] Instructions { get; set; }

        public CPU() { }

        public void Step()
        {
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
        }
    }

    public class Day8
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

        public void SolveP1()
        {
            var cpu = makeCPU();    

            HashSet<int> ran = new HashSet<int>();
            while (cpu.PC <= cpu.Instructions.Length)
            {
                if (ran.Contains(cpu.PC))
                {
                    Console.WriteLine($"P1: {cpu.Acc}");
                    break;
                }

                ran.Add(cpu.PC);

                cpu.Step();
            }
        }
    }
}
