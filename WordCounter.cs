using System.Text;
using System.Text.RegularExpressions;

namespace DigitalDesignTask;

public class WordCounter : IDisposable
{
    public int SpaceCountInOutput { get; set; } = 20;
    public int ReadBufferSize { get; set; } = 1024;
    public string InputFilePath { get; set; }
    public string OutputFilePath { get; set; }

    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    private readonly Queue<string> _words = new();
    private readonly Dictionary<string, int> _distribution = new();
        

    public WordCounter(string inputFilePath, string outputFilePath)
    {
        if (!File.Exists(inputFilePath))
            throw new ArithmeticException($"File {inputFilePath} does not exist");

        FileStream? outputStream = null;
        if (!File.Exists(outputFilePath))
            outputStream = File.Create(outputFilePath);
        
        InputFilePath = inputFilePath;
        OutputFilePath = outputFilePath;
        
        _reader = new StreamReader(InputFilePath);
        _writer = outputStream == null ? new StreamWriter(OutputFilePath)
            : new StreamWriter(outputStream); 
    }

    public async Task WriteResultAsync()
    {
        while (true)
        {
            var word = await GetNextWord();
            if (word == null)
                break;

            _distribution.TryAdd(word, 0);
            _distribution[word]++;
        }
        
        foreach (var pair in _distribution.OrderByDescending(p => p.Value))
        {
            var wordLength = pair.Key.Length;
            var spacesCount = wordLength < SpaceCountInOutput 
                ? SpaceCountInOutput - wordLength : 1;
            var spaces = new string(' ', spacesCount);
            await _writer.WriteLineAsync($"{pair.Key}{spaces}{pair.Value}");
        }
        
    }

    private async Task<string?> GetNextWord()
    {
        if (!_words.Any())
        {
            var block = await GetNextTextBlockAsync();
            if (block == null)
                return null;
            
            foreach (var word in block.Split(" "))
            {
                if (!string.IsNullOrWhiteSpace(word))
                    _words.Enqueue(word);   
            }
        }
        
        return _words.Dequeue();
    }

    private async Task<string?> GetNextTextBlockAsync()
    {
        StringBuilder stringBuilder = new();
        while (stringBuilder.Length < ReadBufferSize || _reader.EndOfStream)
        {
            var rawLine = await _reader.ReadLineAsync();
            if (rawLine == null)
                break;
            
            var line = Regex.Replace(rawLine, @"[^\sa-яА-Яa-zA-Z]", "");
            line = Regex.Replace(line, @"\s+", " ");
            stringBuilder.Append(line.ToLower());
        }
        
        var block = stringBuilder.ToString();
        return !string.IsNullOrWhiteSpace(block) ? block : null;
    }

    public void Dispose()
    {
        _reader.Dispose();
        _writer.Dispose();
    }
}