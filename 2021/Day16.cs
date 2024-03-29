﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    enum PacketType 
    {
        Literal = 4,
        Sum = 0,
        Product = 1,
        Min = 2,
        Max = 3,
        GT = 5,
        LT = 6,
        Equal = 7
    }

    internal class Day16 : IDay
    {
        int _pos;
        string _binaryString;
        int _verTotals;

        // Pretty sure C# has something built in for this but can't
        // google everything!
        string HexToBinary(char hexDigit)
        {
            return hexDigit switch
            {
                '0' => "0000",
                '1' => "0001",
                '2' => "0010",
                '3' => "0011",
                '4' => "0100",
                '5' => "0101",
                '6' => "0110",
                '7' => "0111",
                '8' => "1000",
                '9' => "1001",
                'A' => "1010",
                'B' => "1011",
                'C' => "1100",
                'D' => "1101",
                'E' => "1110",               
                 _  => "1111"
            };
        }

        ulong ParseLiteral()
        {
            int bitsRead = 6;
            string literalBits = "";
            while (true)
            {
                string section = _binaryString.Substring(_pos, 5);
                _pos += 5;
                literalBits += section.Substring(1);
                bitsRead += 5;
                if (section[0] == '0')
                    break;
            }
            
            return Convert.ToUInt64(literalBits, 2);
        }

        List<ulong> FetchSubpacketValues()
        {
            var values = new List<ulong>();
            int lengthTypeID = _binaryString[_pos++] - '0';

            if (lengthTypeID == 0)
            {
                var endOfSubpackets = 15 + _pos + Convert.ToInt32(_binaryString.Substring(_pos, 15), 2);
                _pos += 15;

                while (_pos < endOfSubpackets)
                    values.Add(ParsePacket());
            }
            else
            {
                var numOfSubpackets = Convert.ToInt32(_binaryString.Substring(_pos, 11), 2);
                _pos += 11;

                while (numOfSubpackets-- > 0)               
                    values.Add(ParsePacket());                
            }

            return values;
        }

         ulong ParsePacket()
        {
            int ver = Convert.ToInt32(_binaryString.Substring(_pos, 3), 2);
            PacketType ptype = (PacketType)Convert.ToInt32(_binaryString.Substring(_pos + 3, 3), 2);
            _pos += 6;
            _verTotals += ver;

            List<ulong> values;
            ulong acc;
            switch (ptype)
            {
                case PacketType.Literal:
                    return ParseLiteral();
                case PacketType.Sum:
                    acc = 0;
                    foreach (var val in FetchSubpacketValues())
                        acc += val;
                    return acc;
                case PacketType.Product:
                    acc = 1;
                    foreach (var val in FetchSubpacketValues())
                        acc *= val;
                    return acc;
                case PacketType.Min:
                    return FetchSubpacketValues().Min();
                case PacketType.Max:
                    return FetchSubpacketValues().Max();
                case PacketType.GT:
                    values = FetchSubpacketValues();
                    return (ulong)(values[0] > values[1] ? 1 : 0);
                case PacketType.LT:
                    values = FetchSubpacketValues();
                    return (ulong)(values[0] < values[1] ? 1 : 0);
                case PacketType.Equal:
                    values = FetchSubpacketValues();
                    return (ulong)(values[0] == values[1] ? 1 : 0);
            }

            return 0;
        }

        public void Solve()
        {
            _pos = 0;
            _verTotals = 0;
            var hexString = File.ReadAllText("inputs/day16.txt").Trim();
            _binaryString = string.Join("", hexString.Select(HexToBinary));
            var value = ParsePacket();

            Console.WriteLine($"P1: {_verTotals}");
            Console.WriteLine($"P2: {value}");
        }
    }
}