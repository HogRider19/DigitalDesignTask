using DigitalDesignTask;

Console.Write("Input file Path [../../../Input.txt]: ");
var inputPath = Console.ReadLine();

Console.Write("Output file Path [../../../Output.txt]: ");
var outputPath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(inputPath))
    inputPath = "../../../Input.txt";

if (string.IsNullOrWhiteSpace(outputPath))
    outputPath = "../../../Output.txt";


using (var wordCounter = new WordCounter(inputPath, outputPath))
{
    await wordCounter.WriteResultAsync();
}