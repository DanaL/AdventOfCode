using System;
using System.Collections.Generic;
using System.Linq;

using AoC;

namespace _2021
{
    enum PacketType 
    {
        Literal = 4,
        Operator = 0
    }

    class PacketInfo
    {
        public int Version { get; set; }
        public PacketType Type { get; set; }
        public int Literal { get; set; }
        public int LengthTypeID { get; set; }
        public int SubpacketLength { get; set; }
        public int NumOfSubpackets { get; set; }        
    }

    internal class Day16 : IDay
    {
        int _pos;
        string _binaryString;
        List<PacketInfo> _packets;

        public Day16()
        {
            _packets = new List<PacketInfo>();
        }

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

        int ParseLiteral()
        {
            var firstPiece = _binaryString.Substring(_pos + 1, 4);
            var secondPiece = _binaryString.Substring(_pos + 6, 4);
            var thirdPiece = _binaryString.Substring(_pos + 11, 4);
            _pos += 18; // 18 because the literal has three trailing bits we don't care about

            var bs = $"{firstPiece}{secondPiece}{thirdPiece}";
            return Convert.ToInt32(bs, 2);
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
                //packet.SubpacketLength = subpacketLength;

                // ofc we'll have to do something with that string but for part 1 we can
                // just skip them for name
                _pos += subpacketLength;
            }
            else
            {
                var numOfSubpackets = Convert.ToInt32(_binaryString.Substring(_pos, 11), 2);
                //packet.NumOfSubpackets = numOfSubpackets;
                _pos += 11;

                while (numOfSubpackets-- > 0)
                {
                    // It seems like from the problem description the sub packets are always
                    // literals of 11 bits?
                    _pos += 11;
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
        void ParsePacket(PacketType ptype)
        {                       
            switch (ptype)
            {
                case PacketType.Literal:
                    ParseLiteral();
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
            _pos = 0;
            _binaryString = binaryString;

            int versionsTotal = 0;
            while (_pos < binaryString.Length)
            {
                (int ver, PacketType ptype) = ParseHeader();
                versionsTotal += ver;
                ParsePacket(ptype);
            }
        }

        public void Solve()
        {
            var hexString = "8A004A801A8002F478";
            var binString = string.Join("", hexString.Select(HexToBinary));
            Console.WriteLine(binString);
            //ParsePacketString(binString);

            //foreach (var packet in _packets)  
            //    Console.WriteLine($"version: {packet.Version}, type: {packet.Type}, value: {packet.Literal}");
        }
    }
}
