using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PatternExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Wrong usage, it should be: \nPatternExtractor.exe <input> <output>\n");
                Console.WriteLine("Example: \nPatternExtractor.exe test.mbox output.txt\n\n");
                return;
            }

            var input = args[0];
            var output = args[1];
            
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ExtractEmails(input, output);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        public static void ExtractEmails(string inFilePath, string outFilePath)
        {
            string data = File.ReadAllText(inFilePath); //read File 
                                                        //instantiate with this pattern 
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                RegexOptions.IgnoreCase);
            //find items that matches with our pattern
            MatchCollection emailMatches = emailRegex.Matches(data);

            // get distinct and sort
            IEnumerable<string> distinctEmails = emailMatches.Cast<Match>().Select(m => m.Groups[0].Value).Distinct().OrderBy(m => m).Where(m => !(m.EndsWith("leboncoin.fr") || m.EndsWith("leboncoin.lan")));

            // filter anything which ends by leboncoin.fr


            Console.WriteLine($"{distinctEmails.Count()} emails found");

            StringBuilder sb = new StringBuilder();

            foreach (var email in distinctEmails)
            {
                sb.AppendLine(email);
            }
            //store to file
            File.WriteAllText(outFilePath, sb.ToString());
        }

        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsValid2(string input)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(input);
            return match.Success;
        }
    }
        
}
