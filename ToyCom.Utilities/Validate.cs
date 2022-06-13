using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToyCom.Utilities
{
    /// <summary>
    /// Class for code validation
    /// </summary>
    public static class Validate
    {
        private static Regex regex1 = new Regex(@"\d{4}");
        private static Regex regex2 = new Regex(@"\d{4} *;{1}.*");

        /// <summary>
        /// Validates the code
        /// </summary>
        /// <param name="line">Line on which the error occured</param>
        /// <param name="code">Code string</param>
        /// <returns>Is code valid</returns>
        public static bool ValidateCode(ref int line, ref Exception e, string code)
        {
            string[] lines = TrimCode(code).ToArray();

            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[i][0] == ';' || String.IsNullOrEmpty(lines[i]))
                {
                    // Skip loop iteration if the line is a comment or empty
                    continue;
                }
                else if(regex1.IsMatch(lines[i]) || regex2.IsMatch(lines[i]))
                {
                    // Validate opcode
                    if(Int32.Parse(lines[i].Substring(0, 2)) > 11)
                    {
                        // Set number of the line on which the error occured
                        line = i;
                        // Set exception
                        e = Exception.WrongOpCode;
                        // Return result
                        return false;
                    }
                }
                else
                {
                    // Set number of the line on which the error occured
                    line = i;
                    // Set exception
                    e = Exception.WrongCommand;
                    // Return result
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Trim every line of code
        /// </summary>
        /// <param name="code">Code string</param>
        /// <returns>Collection of trimed lines</returns>
        public static IEnumerable<string> TrimCode(string code)
        {
            string[] lines = code.Split('\n');

            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }

            return lines;
        }
    }
}