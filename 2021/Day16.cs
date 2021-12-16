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
        Literal = 4
    }

    class PacketInfo
    {
        public int Version { get; set; }
        public PacketType Type { get; set; }
        public int Literal { get; set; }
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

        PacketInfo ParsePacketString()
        {
            PacketInfo packet = new PacketInfo()
            {
                Version = Convert.ToInt32(_binaryString.Substring(_pos, 3), 2)
            };

            PacketType typeID = (PacketType) Convert.ToInt32(_binaryString.Substring(_pos + 3, 3), 2);
            packet.Type = typeID;
            _pos += 6;

            switch (typeID)
            {
                case PacketType.Literal:
                    ParseLiteral(packet);
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
            var packets = ParsePacketString("110100101111111000101000100100100111111001000111001100100001100100011010");

            foreach (var packet in packets)  
                Console.WriteLine($"version: {packet.Version}, type: {packet.Type}, value: {packet.Literal}");
        }
    }
}
