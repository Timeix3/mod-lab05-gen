using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScottPlot;

namespace generator
{
    public class BigramsGenerator
    {
        private List<(string bigram, int weight)> data = new();
        private Random random = new Random();
        public int size;
        private int[] np;
        public int summa = 0;
        public BigramsGenerator(string projectDirectory, string fileName = "bigrams.txt")
        {
            if (!File.Exists(Path.Combine(projectDirectory, fileName)))
                throw new FileNotFoundException("File not found");
            using (StreamReader reader = new StreamReader(Path.Combine(projectDirectory, fileName)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                    data.Add((values[1], int.Parse(values[2])));
                }
            }
            size = data.Count;
            for (int i = 0; i < size; i++)
                summa += data[i].weight;
            np = new int[size];
            int s2 = 0;
            for (int i = 0; i < size; i++)
            {
                s2 += data[i].weight;
                np[i] = s2;
            }
        }
        public string GetSym()
        {
            int m = random.Next(0, summa);
            int j;
            for (j = 0; j < size; j++)
            {
                if (m < np[j])
                    break;
            }
            return data[j].bigram;
        }
        public int GetWeight(string bigram) => data.Find(x => x.bigram == bigram).weight;

        public void CreatePlot(List<KeyValuePair<string, int>> statistics, string projectDirectory)
        {
            var myPlot = new Plot();
            int k = 1;
            int barsCount = Math.Min(statistics.Count, 100);
            for (int i = 0; i < barsCount; i++)
            {
                double[] positions = { k + 1, k + 1.8 };
                double[] values = { statistics[i].Value / 1000.0, (double)GetWeight(statistics[i].Key) / summa };
                var bars = myPlot.Add.Bars(positions, values);
                bars.Bars[0].FillColor = Colors.Green;
                bars.Bars[1].FillColor = Colors.Red;
                k += 4;
            }
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            int j = 0;
            for (int x = 1; x <= 4 * barsCount; x += 4)
            {
                tickGen.AddMajor(x + 1.5, statistics[j].Key);
                j++;
            }
            myPlot.Axes.Bottom.TickGenerator = tickGen;
            LegendItem expected = new()
            {
                LineColor = Colors.Red,
                MarkerFillColor = Colors.Red,
                MarkerLineColor = Colors.Red,
                LineWidth = 4,
                LabelText = "Expected frequency"
            };
            LegendItem actual = new()
            {
                LineColor = Colors.Green,
                MarkerFillColor = Colors.Green,
                MarkerLineColor = Colors.Green,
                LineWidth = 4,
                LabelText = "Actual frequency"
            };
            LegendItem[] legend = { expected, actual };
            myPlot.ShowLegend(legend);
            myPlot.Legend.Alignment = Alignment.UpperRight;
            myPlot.Grid.XAxisStyle.IsVisible = false;
            myPlot.Axes.Margins(bottom: 0);
            myPlot.YLabel("Frequency");
            myPlot.SavePng(Path.Combine(Directory.GetParent(projectDirectory).FullName, "Results/gen-1.png"), 2000, 500);
        }
    }

    public class WordsGenerator
    {
        private List<(string word, int weight)> data = new();
        private Random random = new Random();
        public int size;
        private int[] np;
        public int summa = 0;
        public WordsGenerator(string projectDirectory, string fileName = "words.txt")
        {
            if (!File.Exists(Path.Combine(projectDirectory, fileName)))
                throw new FileNotFoundException("File not found");
            using (StreamReader reader = new StreamReader(Path.Combine(projectDirectory, fileName)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                    data.Add((values[1], (int)(double.Parse(values[2]) * 10)));
                }
            }
            size = data.Count;
            for (int i = 0; i < size; i++)
                summa += data[i].weight;
            np = new int[size];
            int s2 = 0;
            for (int i = 0; i < size; i++)
            {
                s2 += data[i].weight;
                np[i] = s2;
            }
        }
        public string GetSym()
        {
            int m = random.Next(0, summa);
            int j;
            for (j = 0; j < size; j++)
            {
                if (m < np[j])
                    break;
            }
            return data[j].word;
        }
        public int GetWeight(string word) => data.Find(x => x.word == word).weight;
        public void CreatePlot(List<KeyValuePair<string, int>> statistics, string projectDirectory)
        {
            var myPlot = new Plot();
            int k = 1;
            int barsCount = Math.Min(statistics.Count, 100);
            for (int i = 0; i < barsCount; i++)
            {
                double[] positions = { k + 1, k + 1.8 };
                double[] values = { statistics[i].Value / 1000.0, (double)GetWeight(statistics[i].Key) / summa };
                var bars = myPlot.Add.Bars(positions, values);
                bars.Bars[0].FillColor = Colors.Green;
                bars.Bars[1].FillColor = Colors.Red;
                k += 4;
            }
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            int j = 0;
            for (int x = 1; x <= 4 * barsCount; x += 4)
            {
                tickGen.AddMajor(x + 1.5, statistics[j].Key);
                j++;
            }
            myPlot.Axes.Bottom.TickGenerator = tickGen;
            LegendItem expected = new()
            {
                LineColor = Colors.Red,
                MarkerFillColor = Colors.Red,
                MarkerLineColor = Colors.Red,
                LineWidth = 4,
                LabelText = "Expected frequency"
            };
            LegendItem actual = new()
            {
                LineColor = Colors.Green,
                MarkerFillColor = Colors.Green,
                MarkerLineColor = Colors.Green,
                LineWidth = 4,
                LabelText = "Actual frequency"
            };
            int largestLabelWidth = statistics.MaxBy(x => x.Key.Length).Key.Length;
            LegendItem[] legend = { expected, actual };
            myPlot.ShowLegend(legend);
            myPlot.Legend.Alignment = Alignment.UpperRight;
            myPlot.Grid.XAxisStyle.IsVisible = false;
            myPlot.Axes.Margins(bottom: 0);
            myPlot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
            myPlot.Axes.Bottom.MinimumSize = largestLabelWidth * 8;
            myPlot.YLabel("Frequency");
            myPlot.SavePng(Path.Combine(Directory.GetParent(projectDirectory).FullName, "Results/gen-2.png"), 2000, 500);
        }
    }

    class Program
    {
        static void Main()
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            GenerateBigramsText(projectDirectory);
            GenerateWordsText(projectDirectory);
        }

        private static void GenerateBigramsText(string projectDirectory)
        {
            BigramsGenerator generator = new(projectDirectory);
            StreamWriter writer = new(Path.Combine(Directory.GetParent(projectDirectory).FullName, "Results/gen-1.txt"));
            SortedDictionary<string, int> statistics = new();
            for (int i = 0; i < 1000; i++)
            {
                string bigram = generator.GetSym();
                if (statistics.ContainsKey(bigram))
                    statistics[bigram]++;
                else
                    statistics.Add(bigram, 1);
                writer.Write(bigram);
            }
            writer.Close();
            var sortedStatistics = statistics.OrderByDescending(entry => entry.Value).ToList();
            generator.CreatePlot(sortedStatistics, projectDirectory);
        }

        private static void GenerateWordsText(string projectDirectory)
        {
            WordsGenerator generator = new(projectDirectory);
            StreamWriter writer = new(Path.Combine(Directory.GetParent(projectDirectory).FullName, "Results/gen-2.txt"));
            SortedDictionary<string, int> statistics = new();
            for (int i = 0; i < 1000; i++)
            {
                string word = generator.GetSym();
                if (statistics.ContainsKey(word))
                    statistics[word]++;
                else
                    statistics.Add(word, 1);
                writer.Write(word + ' ');
            }
            writer.Close();
            var sortedStatistics = statistics.OrderByDescending(entry => entry.Value).ToList();
            generator.CreatePlot(sortedStatistics, projectDirectory);
        }
    }
}

