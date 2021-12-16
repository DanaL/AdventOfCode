using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    internal class Day16 : IDay
    {
        int _pos;
        string _binaryString;

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

        void ParseLiteral(PacketInfo packet)
        {
            var firstPiece = _binaryString.Substring(_pos + 1, 4);
            var secondPiece = _binaryString.Substring(_pos + 6, 4);
            var thirdPiece = _binaryString.Substring(_pos + 11, 4);

            var bs = $"{firstPiece}{secondPiece}{thirdPiece}";
            packet.Literal = Convert.ToInt32(bs, 2);

            _pos += 18; // 18 because the literal has three trailing bits we don't care about
        }

        void ParseOperator(PacketInfo packet)
        {
            int lengthTypeID = _binaryString[_pos++] - '0';
            packet.LengthTypeID = lengthTypeID;

            if (lengthTypeID == 0)
            {
                // 15 bits represent the length in bits of sub-packets in this packet
                var subpacketLength = Convert.ToInt32(_binaryString.Substring(_pos, 15), 2);
                _pos += 15;
                packet.SubpacketLength = subpacketLength;

                // ofc we'll have to do something with that string but for part 1 we can
                // just skip them for name
                _pos += subpacketLength;
            }
        }

        PacketInfo ParsePacketString()
        {
            PacketInfo packet = new PacketInfo()
            {
                Version = Convert.ToInt32(_binaryString.Substring(_pos, 3), 2)
            };

            int typeID = Convert.ToInt32(_binaryString.Substring(_pos + 3, 3), 2);
            packet.Type = typeID switch
            {
                4 => PacketType.Literal,
                _ => PacketType.Operator
            };
            _pos += 6;

            switch (packet.Type)
            {
                case PacketType.Literal:
                    ParseLiteral(packet);
                    break;
                default:
                    // # doesn't distinguish between operator types but I suspect that'll
                    // change in part 2 so at least for now I'll leave this switch stmt in
                    ParseOperator(packet);
                    break;
            }

            return packet;
        }

        List<PacketInfo> ParsePacketString(string binaryString)
        {
            List<PacketInfo> packets = new List<PacketInfo>();
            _pos = 0;
            _binaryString = binaryString;

            while (_pos < binaryString.Length) 
            {
                packets.Add(ParsePacketString());
            }
            
            return packets;
        }

        public void Solve()
        {                        
            var packets = ParsePacketString("0011100000000000011011110100010100101001000100100");

            foreach (var packet in packets)  
                Console.WriteLine($"version: {packet.Version}, type: {packet.Type}, value: {packet.Literal}");
        }
    }
}
