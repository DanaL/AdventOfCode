using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{    
    public class Day21 : IDay
    {
        public Day21() { }

        private List<(string[], string[])> parseInput()
        {
            var foods = new List<(string[], string[])>();

            // Not gonna bother with regexes for this one!
            foreach (string line in File.ReadLines("inputs/day21.txt"))
            {
                var ingredients = line.Substring(0, line.IndexOf('(') - 1).Split(' ');
                var allergens = line.Substring(line.IndexOf('(') + 10, line.IndexOf(')') - line.IndexOf('(') - 10).Split(", ");
                foods.Add((ingredients, allergens));                
            }

            return foods;
        }

        private void eliminateIngedient(string ingredient, Dictionary<string, HashSet<string>> allergens)
        {
            foreach (string allergen in allergens.Keys)
                allergens[allergen].Remove(ingredient);            
        }

        public void Solve()
        {
            var foods = parseInput();

            var occurrences = new Dictionary<string, int>();
            var allergens = new Dictionary<string, HashSet<string>>();
            foreach (var food in foods)
            {
                foreach (var allergen in food.Item2)
                {
                    if (!allergens.ContainsKey(allergen))
                        allergens.Add(allergen, new HashSet<string>(food.Item1));
                    else
                        allergens[allergen] = new HashSet<string>(allergens[allergen].Intersect(new HashSet<string>(food.Item1)));
                }

                foreach (var ingredient in food.Item1)
                {
                    if (!occurrences.ContainsKey(ingredient))
                        occurrences.Add(ingredient, 1);
                    else
                        occurrences[ingredient] += 1;
                }
            }

            HashSet<string> identifiedAllergens = new HashSet<string>();
            HashSet<string> ingredientsWithAllergens = new HashSet<string>();
            HashSet<(string, string)> ingredientAllergenPair = new HashSet<(string, string)>();
            bool ambiguous;
            while (true)
            {
                ambiguous = false;
                foreach (string allergen in allergens.Keys.Where(k => !identifiedAllergens.Contains(k)))
                {
                    if (allergens[allergen].Count == 1)
                    {
                        var ingredient = allergens[allergen].First();
                        ingredientsWithAllergens.Add(ingredient);
                        eliminateIngedient(ingredient, allergens);
                        identifiedAllergens.Add(allergen);
                        ingredientAllergenPair.Add((ingredient, allergen));
                    }
                    else
                        ambiguous = true;
                }
            
                if (!ambiguous)
                    break;
            }

            var p1 = occurrences.Keys.Where(k => !ingredientsWithAllergens.Contains(k))
                            .Select(k => occurrences[k]).Sum();
            Console.WriteLine($"P1: {p1}");

            var p2 = string.Join(',', ingredientAllergenPair.OrderBy(i => i.Item2).Select(i => i.Item1));
            Console.WriteLine($"P2 {p2}");
        }
    }
}
