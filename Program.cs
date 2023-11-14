using System;
using System.Text.Json;

namespace HuntingWords {
  class Program {
    static void Main(string[] args) {
      string jsonString = File.ReadAllText(@"./data.json");
      string[] wordBase = JsonSerializer.Deserialize<string[]>(jsonString);
      string letters = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzáéêãíóç";
      Random rnd = new Random();

      char[,] table = new char[10, 10];
      List<int[]> usedPositions = new List<int[]>();

      for (int x = 0; x < table.GetLength(0); x++) {
        for (int y = 0; y <table.GetLength(1); y++) {
          table[x, y] = letters[rnd.Next(0, letters.Length)];
        }
      }

      List<string> words = new List<string>();
      int numOfHiddenWords = 4;

      //Cria a grade
      string selectedWord;
      for (int i = 0; i < numOfHiddenWords; i++) {
        selectedWord = wordBase[rnd.Next(0, wordBase.Length)].ToLower();
        words.Add(selectedWord);
      }

      foreach (string word in words) {
        int xWordPosition;
        int yWordPosition;
        bool wordIsUnverified = true;
        switch (rnd.Next(0, 2)) {
          case 1:
            //horizontal
            while(wordIsUnverified) {
              xWordPosition = rnd.Next(0, table.GetLength(0) - word.Length - 1);
              yWordPosition = rnd.Next(0, table.GetLength(1) - 1);

              if (TestGradePosition(usedPositions, xWordPosition, yWordPosition, word, false)) {
                foreach(char c in word) {
                  table[xWordPosition, yWordPosition] = c;
                  usedPositions.Add(new int[] {xWordPosition, yWordPosition});
                  xWordPosition++;
                }
                Console.WriteLine("HOR");
                wordIsUnverified = false;
              }
            }
          break;
          default:
            //vertical
            while(wordIsUnverified) {
              xWordPosition = rnd.Next(0, table.GetLength(0) - 1);
              yWordPosition = rnd.Next(0, table.GetLength(1) - word.Length - 1);

              if (TestGradePosition(usedPositions, xWordPosition, yWordPosition, word, true)) {
                foreach(char c in word) {
                  table[xWordPosition, yWordPosition] = c;
                  usedPositions.Add(new int[] {xWordPosition, yWordPosition});
                  yWordPosition++;
                }
                Console.WriteLine("VER");
                wordIsUnverified = false;
              }
            }
          break;
        }
      }

      while (numOfHiddenWords != 0) {
        Console.Clear();
        foreach (string w in words) {
          Console.WriteLine(w);
        }
        Console.Clear();
        PrintGrade(table);
        Console.WriteLine($"Faltam {numOfHiddenWords} palavras.");
        Console.Write("Palavra: ");
        string word = Console.ReadLine();
        if (words.Contains(word.ToLower())) {
          numOfHiddenWords--;
          words.Remove(word.ToLower());
        }
      }
    }

    static bool TestGradePosition(List<int[]> _usedPositions, int _x, int _y, string _word, bool _isVertical) {
      if (_isVertical) {
        foreach(char c in _word) {
          if (_usedPositions.Any(p => p.SequenceEqual(new int[] {_x, _y}))) {
            return false;
          }
          _y++;
        }
        return true;
      } else {
          foreach(char c in _word) {
            if (_usedPositions.Any(p => p.SequenceEqual(new int[] {_x, _y}))) {
              return false;
            }
            _x++;
          }
          return true;
      }
    }

    static void PrintGrade(char[,] table) {
      for (int x = 0; x < table.GetLength(0); x++) {
        for (int y = 0; y < table.GetLength(1); y++) {
          Console.Write($"{table[x, y]}  ");
        }
        Console.WriteLine();
      }
    }
  }
}
