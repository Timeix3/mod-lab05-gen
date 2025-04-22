using generator;

namespace ProjCharGenerator.Tests
{
    public class Tests
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        [Fact]
        public void Test1()
        {
            var exception = Assert.Throws<FileNotFoundException>(() => new BigramsGenerator(projectDirectory,"invalid.txt"));
            Assert.Equal("File not found", exception.Message);
        }

        [Fact]
        public void Test2()
        {
            BigramsGenerator generator = new(projectDirectory);
            Assert.Equal(10, generator.size);
            Assert.Equal(7598794, generator.summa);
        }

        [Fact]
        public void Test3()
        {
            BigramsGenerator generator = new(projectDirectory);
            Assert.Equal(917165, generator.GetWeight("af"));
        }

        [Fact]
        public void Test4()
        {
            BigramsGenerator generator = new(projectDirectory);
            var bigrams = new[] { "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj" };
            string result = generator.GetSym();
            Assert.Contains(result, bigrams);
        }

        [Fact]
        public void Test5()
        {
            BigramsGenerator generator = new(projectDirectory);
            SortedDictionary<string, int> statistics = new();
            for (int i = 0; i < 10000; i++)
            {
                string bigram = generator.GetSym();
                if (statistics.ContainsKey(bigram))
                    statistics[bigram]++;
                else
                    statistics.Add(bigram, 1);
            }
            Assert.Equal((double)generator.GetWeight("aa") / generator.summa, statistics["aa"] / 10000.0, 1);
        }

        [Fact]
        public void Test6()
        {
            var exception = Assert.Throws<FileNotFoundException>(() => new WordsGenerator(projectDirectory, "invalid.txt"));
            Assert.Equal("File not found", exception.Message);
        }

        [Fact]
        public void Test7()
        {
            WordsGenerator generator = new(projectDirectory);
            Assert.Equal(10, generator.size);
            Assert.Equal(16557140, generator.summa);
        }

        [Fact]
        public void Test8()
        {
            WordsGenerator generator = new(projectDirectory);
            Assert.Equal(1268440, generator.GetWeight("im"));
        }

        [Fact]
        public void Test9()
        {
            WordsGenerator generator = new(projectDirectory);
            var words = new[] { "and", "in", "not", "on", "im", "be", "him", "with", "what", "a" };
            string result = generator.GetSym();
            Assert.Contains(result, words);
        }

        [Fact]
        public void Test10()
        {
            WordsGenerator generator = new(projectDirectory);
            SortedDictionary<string, int> statistics = new();
            for (int i = 0; i < 10000; i++)
            {
                string word = generator.GetSym();
                if (statistics.ContainsKey(word))
                    statistics[word]++;
                else
                    statistics.Add(word, 1);
            }
            Assert.Equal((double)generator.GetWeight("and") / generator.summa, statistics["and"] / 10000.0, 1);
        }
    }
}