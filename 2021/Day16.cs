using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    enum PacketType 
    {
        Literal = 4,
        Operator = 0
    }

    struct PacketInfo
    {
        public PacketType PType { get; init; }
        public int Version { get; init; }
    }

    internal class Day16 : IDay
    {
        int _pos;
        string _binaryString;
        List<PacketInfo> _packets;

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

        void ParseOperator()
        {
            int lengthTypeID = _binaryString[_pos++] - '0';
            //packet.LengthTypeID = lengthTypeID;

            if (lengthTypeID == 0)
            {
                // 15 bits represent the length in bits of sub-packets in this packet
                var subpacketLength = Convert.ToInt32(_binaryString.Substring(_pos, 15), 2);
                _pos += 15;

                ParsePacket();
            }
            else
            {
                var numOfSubpackets = Convert.ToInt32(_binaryString.Substring(_pos, 11), 2);
                _pos += 11;

                while (numOfSubpackets-- > 0)
                {
                    ParsePacket();
                }
            }
        }

        (int, PacketType) ParseHeader()
        {
            int version = Convert.ToInt32(_binaryString.Substring(_pos, 3), 2);
            PacketType type = (PacketType) Convert.ToInt32(_binaryString.Substring(_pos + 3, 3), 2);
            _pos += 6;

            return (version, type);
        }

        // Not quite working right currently.
        // Subpackets won't have headers (with version + type) so I have to separate out
        // that
        void ParsePacket()
        {
            (int ver, PacketType ptype) = ParseHeader();
            PacketInfo packet = new PacketInfo()
            {
                PType = ptype,
                Version = ver
            };
            _packets.Add(packet);

            switch (ptype)
            {
                case PacketType.Literal:
                    var num = ParseLiteral();
                    break;
                default:
                    // # doesn't distinguish between operator types but I suspect that'll
                    // change in part 2 so at least for now I'll leave this switch stmt in
                    ParseOperator();
                    break;
            }
        }

        void ParsePacketString(string binaryString)
        {
            _packets = new List<PacketInfo>();
            _pos = 0;
            _binaryString = binaryString;
           
            while (binaryString.Length - _pos > 11) // 11 is the minimal packet size
                ParsePacket();            
        }

        string Input()
        {
            return File.ReadAllText("inputs/day16.txt").Trim();
        }

        public void Solve()
        {
            //var hexString = "D2FE28";
            var hexString = Input();
            var binString = string.Join("", hexString.Select(HexToBinary));
            ParsePacketString(binString);

            Console.WriteLine($"P1: {_packets.Select(p => p.Version).Sum()}");
        }
    }
}
