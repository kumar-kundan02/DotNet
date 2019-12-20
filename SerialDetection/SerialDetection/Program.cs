using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"E:\Tech\Angular\serialTest.txt";
            var gdLinesFromText = ReadFile(filePath);

            SerialIdentifier serialIdentifier = new SerialIdentifier();
            var serialOutput = serialIdentifier.DetectSerial(gdLinesFromText);

            //SerialRelation serialRelation = new SerialRelation();
            //var serialRelaitonOutput = serialRelation.IdentifyRelation(serialOutput);


            foreach (var item in serialOutput)
            {
                Console.WriteLine($"Line Number: {item.Key}, Serial Type: {item.Value.Type}, Serial Value: {item.Value.Value}");
            }

            Console.ReadLine();
        }

        private static List<string> ReadFile(string filePath)
        {
            List<string> lines = new List<string>();

            if (File.Exists(filePath))
            {
                lines = File.ReadAllLines(filePath).ToList();
            }
            return lines;
        }
    }
}
