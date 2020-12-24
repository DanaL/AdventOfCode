using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Priority_Queue;

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

    struct Possibility
    {
        public string PieceID { get; init; }
        public int CatalogueID { get; init; }
        
        public Possibility(string pieceID, int catalogueID)
        {
            PieceID = pieceID;
            CatalogueID = catalogueID;
        }
    }

    class Image : IComparable
    {
        public Possibility[,] Pieces { get; set;  }
        public HashSet<string> AlreadyUsed { get; set; }
        private int width { get; init; }
        public int EmptySqs { get; set; }

        public Image(int size)
        {
            width = size;
            Pieces = new Possibility[size, size];
            AlreadyUsed = new HashSet<string>();
            EmptySqs = width * width;
        }

        public void Add(Possibility opt, int row, int col)
        {
            Pieces[row, col] = opt;
            --EmptySqs;
        }

        public int CompareTo(object obj)
        {
            var other = (Image)obj;

            return this.EmptySqs.CompareTo(other.EmptySqs);
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
        private readonly List<(int, int)> _deltas = new List<(int, int)>() { (-1, 0), (1, 0), (0, 1), (0, -1) };
        private Dictionary<string, int> _edgesSeen = new Dictionary<string, int>();
        private List<Tile> _tiles = new List<Tile>();
        private Dictionary<string, List<Piece>> _catalogue = new Dictionary<string, List<Piece>>();
        private int _imgWidth;

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

        private short compareEdges(string edge, Piece p2)
        {            
            foreach (var e2 in p2.Edges.Keys)
            {
                if (edge == p2.Edges[e2])
                    return e2;
            }

            return -1;
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

        private void matchPieces(Catalogue catalogue, Image start)
        {
            SimplePriorityQueue<Image> q = new SimplePriorityQueue<Image>();
            q.Enqueue(start, start.EmptySqs);

            while (q.Count > 0)
            {
                Image img = q.Dequeue();
                Piece piece = catalogue.Pieces[img.Pieces[img.Next.Row, img.Next.Col].CatalogueID];

                // Find which neighbouring cells are in bounds and empty
                foreach (var e in Piece.EdgeNums.Where(a => !piece.UsedEdges.Contains(a)))
                {
                    int adjRow = img.Next.Row, adjCol = img.Next.Col;
                    switch (e)
                    {
                        case Piece.TOP:
                            adjRow -= 1;
                            break;
                        case Piece.BOTTOM:
                            adjRow += 1;
                            break;
                        case Piece.LEFT:
                            adjCol -= 1;
                            break;
                        case Piece.RIGHT:
                            adjCol += 1;
                            break;
                    }
                    if (!inBounds(adjRow, adjCol))
                        continue;
                    if (img.Pieces[adjRow, adjCol].PieceID != null)
                        continue;
                    //Neighbour n = findMatch(catalogue, piece.Edges[e], img.AlreadyUsed);
                    //image[adjRow, adjCol] = n.Other;
                }
            }
        }

        //private void dumpImage(Piece[,] image)
        //{
        //    Console.WriteLine("The image so far:");
        //    char[,] pixels = new char[_imgWidth * 10, _imgWidth * 10];
        //    for (int r = 0; r < _imgWidth; r++)
        //    {
        //        for (int c = 0; c < _imgWidth; c++)
        //        {
        //            List<char> txt;
        //            if (image[r, c] is null)
        //            {
        //                txt = new List<char>();
        //                for (int j = 0; j < 100; j++)
        //                    txt.Add(' ');
        //            }
        //            else
        //                txt = image[r, c].Pixels.ToCharArray().ToList();

        //            // Okay, I have the pixels to draw in a 1D array, now write them to
        //            // the grid of pxiels
        //            for (int i = 0; i < 100; i ++)
        //            {
        //                int pr = i / 10;
        //                int pc = i % 10;

        //                // But I have to transpose them to the larger matrix
        //                pixels[r * 10 + pr, c * 10 + pc] = txt[i];
        //            }                   
        //        }
        //    }

        //    for (int r = 0; r < _imgWidth * 10; r++)
        //    {
        //        char[] row = new char[_imgWidth * 10];
        //        for (int c = 0; c < _imgWidth * 10; c++)
        //            row[c] = pixels[r, c];
        //        Console.WriteLine(string.Concat(row));
        //    }
        //}

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

            // We need a tile to start with, so take one of the corner pieces and find two other edges which match it.
            // Conceptually, this will be the top left corner of my image.
            HashSet<string> alreadyUsed = new HashSet<string>();
            alreadyUsed.Add(corners[0].Num);
            Piece startingPiece = null;
            Possibility possibility = new Possibility(null, -1);

            foreach (int id in catalogue.AllTransformIDsOf(corners[0].Num))
            {
                Piece corner = catalogue.Pieces[id];
                Query q = new Query(corner.Edges[Piece.BOTTOM], null, null, null);
                Piece neighbour1 = findMatch(catalogue, q, alreadyUsed);
                
                q = new Query(null, null, corner.Edges[Piece.RIGHT], null);
                Piece neighbour2 = findMatch(catalogue, q, alreadyUsed);

                if (neighbour1 is Piece && neighbour2 is Piece)
                {
                    startingPiece = corner;
                    possibility = new Possibility(corner.ID, id);
                    break;
                }
            }

                //if (neighbour1 != null && neighbour2 != null)
                //{
                //    possibility = new Possibility(corner.ID, id);
                //    startingPiece = corner;
                //    //startingPiece.UsedEdges.Add(neighbour1.ParentEdge);
                //    //startingPiece.UsedEdges.Add(neighbour2.ParentEdge);
                //    break;
                //}
           

            // Alright! We have our top-left corner! Let's create our matrix of the pieces and add the corner to it.
            _imgWidth = (int)Math.Floor(Math.Sqrt(_tiles.Count));
            Image image = new Image(_imgWidth);
            image.AlreadyUsed.Add(startingPiece.ID);
            image.Add(possibility, 0, 0); // We found the top-left corner (or *a* top-left corner, really)            
            matchPieces(catalogue, image);
        }
    }
}
