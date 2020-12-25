using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    struct Query
    {
        public string Top { get; init; }
        public string Bottom { get; init; }
        public string Left { get; init; }
        public string Right { get; set; }

        public Query (string t, string b, string l, string r)
        {
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }
    }

    class Tile
    {
        public string Num { get; set; }
        public string Text { get; set; }
        public List<(string, string)> Edges { get; set; }

        public Tile() { }
    }

    class Piece
    {
        public const short TOP = 1;
        public const short BOTTOM = 2;
        public const short LEFT = 3;
        public const short RIGHT = 4;
        public static HashSet<short> EdgeNums = new HashSet<short>() { TOP, BOTTOM, LEFT, RIGHT };

        public string ID { get; set; }
        public string Pixels { get; set; }
        public Dictionary<short, string> Edges { get; set; }
        
        // txt is the image out of the file with line breaks
        public Piece(string id, string txt)
        {
            this.ID = id;
            Pixels = txt.Replace("\n", "");
            Edges = new Dictionary<short, string>();
            Edges.Add(TOP, Pixels.Substring(0, 10));
            Edges.Add(BOTTOM, Pixels.Substring(Pixels.Length - 10, 10));
            string left = "";
            string right = "";
            for (int j = 0; j < Pixels.Length; j += 10)
            {
                left += Pixels[j];
                right += Pixels[j + 9];
            }
            Edges.Add(LEFT, left);
            Edges.Add(RIGHT, right);
        }

        // Magic numbers 'cause by spec all our tiles are 10x10
        public Piece Rotate()
        {
            char[] pixels = new char[100];
            int[] indices = new int[100];

            for (int i = 0; i < 100; i++)
            {
                if (i < 10)
                    indices[i] = i * 10 + 9;
                else
                    indices[i] = indices[i - 10] - 1;
            }

            foreach (var i in indices)
                pixels[indices[i]] = Pixels[i];
                
            Piece rotated = new Piece(this.ID, string.Concat(pixels));
            return rotated;
        }

        public Piece Flip()
        {
            char[] pixels = new char[100];
            for (int j = 0; j < Pixels.Length; j+= 10)
            {
                for (int k = 0; k < 5; k++)
                {
                    pixels[j + k] = Pixels[j + 9 - k];
                    pixels[j + 9 - k] = Pixels[j + k];
                }
            }

            Piece flipped = new Piece(this.ID, string.Concat(pixels));
            return flipped;
        }

        public void Dump()
        {
            for (int j = 0; j < Pixels.Length; j += 10)
            {
                Console.WriteLine(Pixels.Substring(j, 10));
            }
        }
    }

    class Catalogue
    {
        private int openIndex;
        private Dictionary<string, int> idMap = new Dictionary<string, int>();

        public Piece[] Pieces;

        public Catalogue(int totalPieces)
        {
            Pieces = new Piece[totalPieces];
            openIndex = 0;
            idMap = new Dictionary<string, int>();
        }

        public void Add(string pieceID, List<Piece> pieces)
        {
            idMap.Add(pieceID, openIndex);
            for (int j = 0; j < pieces.Count; j++, openIndex++)
                Pieces[openIndex] = pieces[j];
        }

        public List<int> AllTransformIDsOf(string pieceID)
        {
            int index = idMap[pieceID];
            List<int> pieceIDs = new List<int>();
            for (int j = 0; j < 8; j++)
                pieceIDs.Add(index + j);

            return pieceIDs;
        }

        public IEnumerable<Piece> AllPiecesExcept(HashSet<string> excluded)
        {
            // This should be cached or pre-generated, I think
            HashSet<int> excludedIDs = new HashSet<int>();
            foreach (string pieceID in excluded)
            {
                for (int j = idMap[pieceID]; j < idMap[pieceID] + 8; j++)
                    excludedIDs.Add(j);
            }
            
            for (int j = 0; j < Pieces.Length; j++)
            {
                if (excludedIDs.Contains(j))
                    continue;
                yield return Pieces[j];
            }
        }
    }

    public class Day20 : IDay
    {
        private Dictionary<string, int> _edgesSeen = new Dictionary<string, int>();
        private List<Tile> _tiles = new List<Tile>();
        private int _imgWidth;

        // The sea monster is:
        //                   # 
        // #    ##    ##    ###
        //  #  #  #  #  #  #   
        //
        // So I want to make a hashset that is those offsets in order to search through the grid
        private HashSet<(int, int)> _seaMonster = new HashSet<(int, int)>() {
            (0, 18), (1, 0), (1, 5), (1, 6), (1, 11), (1, 12), (1, 17), (1, 18), (1, 19),
            (2, 1), (2, 4), (2, 7), (2, 10), (2, 13), (2, 16) };

        public Day20() { }

        private string reverse(string s)
        {
            var arr = s.ToCharArray();
            Array.Reverse(arr);
            return string.Concat(arr);
        }

        private void parseInput()
        {
            TextReader tr = new StreamReader("inputs/day20.txt");
            var tiles = tr.ReadToEnd().Split("\n\n");

            foreach (var tile in tiles)
            {
                var rows = tile.Split('\n');
                Tile t = new Tile()
                {
                    Num = rows[0].Replace("Tile ", "").Substring(0, 4),
                    Text = tile,
                    Edges = new List<(string, string)>()
                };

                t.Edges.Add((rows[1], reverse(rows[1])));
                t.Edges.Add((rows[10], reverse(rows[10])));
                string left = "", right = "";
                for (int j = 1; j < 11; j++)
                {
                    left += rows[j][0];
                    right += rows[j][9];
                }
                t.Edges.Add((left, reverse(left)));
                t.Edges.Add((right, reverse(right)));

                foreach (var edge in t.Edges)
                {
                    if (!_edgesSeen.ContainsKey(edge.Item1))
                        _edgesSeen.Add(edge.Item1, 1);
                    else
                        _edgesSeen[edge.Item1] += 1;

                    if (!_edgesSeen.ContainsKey(edge.Item2))
                        _edgesSeen.Add(edge.Item2, 1);
                    else
                        _edgesSeen[edge.Item2] += 1;
                }

                _tiles.Add(t);
            }
        }

        // A corner should be a tile which has exactly 2 unique edges
        private bool isCorner(Tile tile, string[] uniqueEdges)
        {
            int c = 0;
            foreach (var edge in tile.Edges)
            {
                if (uniqueEdges.Contains(edge.Item1) || uniqueEdges.Contains(edge.Item2))
                    ++c;
            }

            return c == 2;
        }

        private Piece findMatch(Catalogue catalogue, Query query, HashSet<string> alreadyUsed)
        {
            foreach (Piece candidate in catalogue.AllPiecesExcept(alreadyUsed))
            {
                // Need to look for a candidate whose edges match the query's (if not null)
                // Ie., given:
                //
                // +----+----+----+
                // |    |    |    |
                // |    |    |    |
                // |    |    |    |
                // +----+----+----+
                // |    |    |    |
                // |  * | C  |    |
                // |    |    |    |
                // +----+----+----+
                // |    |    |    |
                // |  A | B  |    |
                // |    |    |    |
                // +----+----+----+
                //
                // query will have Right == C's left edge and Bottom == A's top edge
                // (query's Left and Top values will be null)
                // So, we want to loop over possible edges and find the one which matches
                // those two values.                
                if (query.Top is string && query.Top != candidate.Edges[Piece.TOP])
                {
                    continue;
                }
                if (query.Bottom is string && query.Bottom != candidate.Edges[Piece.BOTTOM])
                {
                    continue;
                }
                if (query.Left is string && query.Left != candidate.Edges[Piece.LEFT])
                {
                    continue;
                }
                if (query.Right is string && query.Right != candidate.Edges[Piece.RIGHT])
                {
                    continue;
                }

                return candidate;
            }

            return null;
        }

        private bool inBounds(int row, int col)
        {
            if (row < 0 || col < 0)
                return false;
            if (row >= _imgWidth || col >= _imgWidth)
                return false;
            return true;
        }

        private Query queryForLoc(int row, int col, Catalogue catalogue, Piece[,] image)
        {
            string left = null, right = null, top = null, bottom = null;

            // Look for the top edge we want to match to
            var adjRow = row - 1;
            var adjCol = col;
            if (inBounds(adjRow, adjCol) && image[adjRow, adjCol] is Piece)
            {                
                top = image[adjRow, adjCol].Edges[Piece.BOTTOM];
            }

            // Look for the bottom edge we want to match to
            adjRow = row + 1;
            adjCol = col;
            if (inBounds(adjRow, adjCol) && image[adjRow, adjCol] is Piece)
            {
                bottom = image[adjRow, adjCol].Edges[Piece.TOP];
            }

            // Look for the left edge we want to match to
            adjRow = row;
            adjCol = col - 1;
            if (inBounds(adjRow, adjCol) && image[adjRow, adjCol] is Piece)
            {
                left = image[adjRow, adjCol].Edges[Piece.RIGHT];
            }

            // Look for the right edge we want to match to
            adjRow = row;
            adjCol = col + 1;
            if (inBounds(adjRow, adjCol) && image[adjRow, adjCol] is Piece)
            {
                right = image[adjRow, adjCol].Edges[Piece.LEFT];
            }

            return new Query(top, bottom, left, right);
        }

        private char[,] stitchImageTogether(Catalogue catalogue, Piece topLeft)
        {
            _imgWidth = (int)Math.Floor(Math.Sqrt(_tiles.Count));
            Piece[,] image = new Piece[_imgWidth, _imgWidth];
            image[0, 0] = topLeft;
            
            HashSet<string> alreadyUsed = new HashSet<string>();
            alreadyUsed.Add(image[0, 0].ID);

            for (int row = 0; row < _imgWidth; row++)
            {
                for (int col = 0; col < _imgWidth; col++)
                {
                    if (row == 0 && col == 0)
                        continue;
                    var q = queryForLoc(row, col, catalogue, image);
                    var match = findMatch(catalogue, q, alreadyUsed);
                    image[row, col] = match;
                    alreadyUsed.Add(match.ID);
                }
            }

            return removeBorders(image);
        }

        private char[,] removeBorders(Piece[,] image)
        {
            char[,] pixels = new char[_imgWidth * 8, _imgWidth * 8];

            for (int r = 0; r < _imgWidth; r++)
            {
                for (int c = 0; c < _imgWidth; c++)
                {
                    List<char> txt = new List<char>();
                    for (int i = 10; i < 90; i += 10)
                    {
                        for (int j = 1; j <= 8; j++)
                        {
                            txt.Add(image[r, c].Pixels[i + j]);
                        }                        
                    }

                    // Okay, I have the pixels to draw in a 1D array, now write them to
                    // the grid of pxiels
                    for (int i = 0; i < 64; i++)
                    {
                        int pr = i / 8;
                        int pc = i % 8;

                        // But I have to transpose them to the larger matri%x
                        pixels[r * 8 + pr, c * 8 + pc] = txt[i];
                    }
                }
            }

            return pixels;
        }

        private void dumpImage(char[,] pixels)
        {
            for (int r = 0; r < _imgWidth * 8; r++)
            {
                List<char> line = new List<char>();
                for (int c = 0; c < _imgWidth * 8; c++)
                    line.Add(pixels[r, c]);
                Console.WriteLine(string.Concat(line));
            }
        }

        private int countSeaMonsters(char[,] pixels)
        {
            HashSet<(int, int)> monsterSqs = new HashSet<(int, int)>();
            int monsterCount = 0;
            for (int r = 0; r < _imgWidth * 8 - 3; r ++)
            {
                for (int c = 0; c < _imgWidth * 8 - 19; c++)
                {
                    bool found = true;
                    foreach (var offset in _seaMonster)
                    {                        
                        if (pixels[r + offset.Item1, c + offset.Item2] != '#')
                        {
                            found = false;
                            break;
                        }                        
                    }

                    if (found)
                    {
                        ++monsterCount;
                        foreach (var offset in _seaMonster)
                            monsterSqs.Add((r + offset.Item1, c + offset.Item2));                        
                    }
                }
            }

            if (monsterCount > 0)
            {
                foreach ((int, int) loc in monsterSqs)
                    pixels[loc.Item1, loc.Item2] = '0';
            }

            return monsterCount;
        }

        public void Solve()
        {
            parseInput();

            /* Part 1 is so small and easy when I ignore actually assembling the map... */
            var uniqueEdges = _edgesSeen.Keys.Where(e => _edgesSeen[e] == 1).ToArray();
            var corners = _tiles.Where(t => isCorner(t, uniqueEdges)).ToArray();
            Console.WriteLine($"P1: {corners.Select(t => long.Parse(t.Num)).ToArray().Aggregate((total, next) => total * next)}");

            Catalogue catalogue = new Catalogue(_tiles.Count * 8);
            // Build the catalogue of all the pieces in their 8 configurations
            foreach (var tile in _tiles)
            {
                List<Piece> pieces = new List<Piece>();
                pieces.Add(new Piece(tile.Num, tile.Text.Replace("\n", "").Substring(10)));

                // add all the rotations
                Piece piece = pieces[0];
                for (int j = 0; j < 3;j ++)
                {
                    piece = piece.Rotate();
                    pieces.Add(piece);
                }

                // add all the rotations of the flipped OG tile
                piece = pieces[0].Flip();
                pieces.Add(piece);
                for (int j = 0; j < 3; j++)
                {
                    piece = piece.Rotate();
                    pieces.Add(piece);
                }

                catalogue.Add(tile.Num, pieces);
            }

            // Lazy coder's way to check all orientations of the map: generate each possible map
            // with each possible orientation of a top-left corner because I already have that working.
            List<int> allCornerIDs = new List<int>();
            foreach (var corner in corners)
            {
                allCornerIDs.AddRange(catalogue.AllTransformIDsOf(corner.Num));
            }

            // Now, find an orientation which contains sea monsters!
            foreach (int cornerID in allCornerIDs)
            {
                // Find one of the possible tiles to use as the top-left corner
                HashSet<string> alreadyUsed = new HashSet<string>();
                alreadyUsed.Add(catalogue.Pieces[cornerID].ID);
                foreach (int id in catalogue.AllTransformIDsOf(catalogue.Pieces[cornerID].ID))
                {
                    Piece corner = catalogue.Pieces[id];
                    Piece match1 = findMatch(catalogue, new Query(corner.Edges[Piece.BOTTOM], null, null, null), alreadyUsed);
                    Piece match2 = findMatch(catalogue, new Query(null, null, corner.Edges[Piece.RIGHT], null), alreadyUsed);

                    if (match1 is Piece && match2 is Piece)
                    {
                        char[,] pixels = stitchImageTogether(catalogue, corner);
                        int monsters = countSeaMonsters(pixels);
                        if (monsters > 0)
                        {
                            dumpImage(pixels);
                            Console.WriteLine($"Found {monsters} sea monsters");
                            Console.WriteLine($"P2: {pixels.Cast<char>().Where(c => c == '#').Count()}");
                            return;
                        }                        
                    }
                }
            }            
        }
    }
}