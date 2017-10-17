namespace LogSearcher
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    class Program
    {
        #region Methods

        private static List<string> GetDeviceLogsKeywordsToFind()
        {
            string dateTime1 = "2017/04/19 8:29";
            List<string> result = new List<string>()
            {
                dateTime1
            };
            return result;
        }

        private static List<string> GetTekSpeechAgentsKeywordsToFind()
        {
            string agentName = "PepAssignmentHeaderAgent";
            string pickerName = "kg";
            string branch = "6132";
            string lineNumber = "9093";
            //string sku = "895527";
            string packingSlip = "866160146";
            //string location = "B229";
            //string quantity = "11";
            ////string date = "2017-03-27";
            //string date = "2013/09/23";
            //string time = " 5:";
            //string pickStatus = "R)";
            //string deviceId = "5916457527";
            //string text = "649473165,NATS";

            List<string> result = new List<string>()
                {
                    //agentName.ToLower(),
                    //pickerName.ToLower(),
                    //branch.ToLower(),
                    //lineNumber.ToString(),
                    packingSlip
                };
            return result;
        }

        static void Main(string[] args)
        {
            try
            {
                string targetDirectory = @"C:\Docs\Git\Pep-Speech\logs\2017-10-17";
                List<string> keywordsToFind = GetTekSpeechAgentsKeywordsToFind();

                if (!Directory.Exists(targetDirectory))
                {
                    throw new DirectoryNotFoundException(string.Format("Could not find {0}.", targetDirectory));
                }
                string resultsFilePath = Path.Combine(Information.GetExecutingDirectory(), "LogSearcherResults.txt");
                if (File.Exists(resultsFilePath))
                {
                    File.Delete(resultsFilePath);
                }
                List<string> logFilePaths = Directory.GetFiles(targetDirectory, "*", SearchOption.TopDirectoryOnly).ToList();
                using (StreamWriter resultsWriter = new StreamWriter(resultsFilePath, false))
                {
                    int logNumber = 0;
                    foreach (string filePath in logFilePaths)
                    {
                        resultsWriter.WriteLine(string.Format("{0} : searching through {1} ...", logNumber, filePath));
                        Dictionary<int, string> foundLines = FindLinesInFile(filePath, keywordsToFind, resultsWriter);
                        foreach (KeyValuePair<int, string> line in foundLines)
                        {
                            StringBuilder message = new StringBuilder();
                            message.AppendLine(string.Format("Found Line {0}: {1}", line.Key, filePath));
                            message.AppendLine(line.Value);
                            resultsWriter.WriteLine(message);
                            resultsWriter.WriteLine();
                            Console.WriteLine(message);
                        }
                        logNumber++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        private static Dictionary<int, string> FindLinesInFile(string filePath, List<string> keywordsToFind, StreamWriter resultsWriter)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("Could not find {0}.", filePath));
            }
            string fileName = Path.GetFileName(filePath);
            Dictionary<int, string> result = new Dictionary<int, string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                int lineNumber = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string lineLower = line.ToLower();
                    Console.WriteLine(string.Format("Searching Line {0} of {1}", lineNumber, fileName));
                    bool includeLine = true;
                    foreach(string keyword in keywordsToFind)
                    {
                        if (lineLower.Contains(keyword))
                        {
                            continue;
                        }
                        includeLine = false;
                        break;
                    }
                    if (includeLine)
                    {
                        result.Add(lineNumber, line);
                    }
                    lineNumber++;
                }
            }
            return result;
        }

        private static void ZipBomb()
        {
            string path = Path.Combine(Information.GetExecutingDirectory(), "000.txt");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            long megCounter = 0;
            long bytesCounter = 0;
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (long i = 0; i < 357564416; i++) //357564416 = 1GB
                {
                    writer.WriteLine("0");
                    bytesCounter++;
                    if (bytesCounter >= 349184) //349184 = 1MB
                    {
                        megCounter++;
                        bytesCounter = 0;
                        Console.WriteLine("MB Written: {0}", megCounter);
                    }
                }
                writer.Flush();
                writer.Close();
            }
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }

        #endregion //Methods
    }
}
