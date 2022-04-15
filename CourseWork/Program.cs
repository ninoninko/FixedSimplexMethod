using System;
using System.Linq;
using System.Collections.Generic;
using ConsoleTables;
using System.Text.RegularExpressions;

namespace CourseWork
{
    internal class Program
    {
        static void Main(string[] args)
        {      
            // Create function variables
            decimal[] functionParameters = new decimal[]
            {
                75, 120, 110, 100, 0, 0, 0, 0
            };

            // Create final function variables
            decimal[] functionParametersFinal = new decimal[]
            {
                75, 120, 110, 100, 0, 0, 0, 0
            };

            // Create function limits
            decimal[,] systemParameters = new decimal[,] 
            {
                {1, 1, 1, 1, 1, 0, 0, 0},
                {85, 105, 95, 90, 0, 1, 0, 0},
                {0, 0, 1, 0, 0, 0, 1, 0},
                {0, 0, 0, 1, 0, 0, 0, 1}
            };

            // Create basis changes
            List<decimal> values = new List<decimal>()
            {
                10500, 762500, 2000, 1000
            };

            string[] basis = new string[] { "x5", "x6", "x7", "x8" };

            while (true)
            {
                // Declare some variables which will come in handy in a minute
                // Their descriptions can be found underneat in the method descriptions 
                List<decimal> deltas = new List<decimal>();
                string resultString0 = Regex.Match(basis[0], @"\d+").Value;
                string resultString1 = Regex.Match(basis[1], @"\d+").Value;
                string resultString2 = Regex.Match(basis[2], @"\d+").Value;
                string resultString3 = Regex.Match(basis[3], @"\d+").Value;

                // The following method deals with the drawing of the tables
                createTableMethod(basis, values, systemParameters, deltas, resultString0, resultString1, resultString2, resultString3, functionParameters);

                // The following method creates a new base plan for our program
                createNewBasePlan(resultString0, resultString1, resultString2, resultString3, values, functionParametersFinal);

                /**
                 * We can see that some of the deltas are < 0, which means
                 * that this is not the highest possible revenue
                 */
                int deltaLocation = 0;
                decimal smallestDelta = int.MaxValue;
                for (int i = 0; i < deltas.Count; i++)
                {
                    if (smallestDelta > deltas[i])
                    {
                        deltaLocation = i;
                        smallestDelta = deltas[i];
                    }
                }

                /**
                 * Check the value of the smallest delta.
                 * If the value is < 0, then we have to continue with the tables.s
                 */
                if (smallestDelta >= 0)
                {
                    break;
                }

                Console.WriteLine($"This is our lowest delta in {deltaLocation + 1} column");
                Console.WriteLine("Since it is < 0, we will now look for the key number:");

                // Now we will seek out our key number
                List<decimal> possibleKeyNumbers = new List<decimal>();

                for (int i = 0; i < 4; i++)
                {
                    if (systemParameters[i, deltaLocation] != 0)
                    {
                        possibleKeyNumbers.Add(values[i] / systemParameters[i, deltaLocation]);
                    }
                }

                int rowLocation = possibleKeyNumbers.IndexOf(possibleKeyNumbers.Min());
                decimal keyNumber = systemParameters[rowLocation, deltaLocation];
                Console.WriteLine($"Our key is at values {rowLocation}, {deltaLocation}");

                // Change the basis in the table
                basis[rowLocation] = "x" + (deltaLocation + 1);

                // The following method will recalculate our variables list
                changeVariablesList(rowLocation, deltaLocation, values, keyNumber, systemParameters);

                // The following method will recalculate our system parameters
                changeSystemParametersArray(systemParameters, rowLocation, deltaLocation, keyNumber);
                
            }

            Console.WriteLine("\n\n\nAlthough this may seem as our solution we are missing a very important thing: " +
                "\nWe did not start with the values in the task but rather did some tricks." +
                "\nMr. Popov, refer to the blank pages." +
                "\nHence, to the first four instances of the functionParametersFinal array we have to add 2500, 2000, 3000 and 2000 respectively." +
                $"\nThis means that we need to sow a total of 2500 + {functionParametersFinal[0].ToString("N2")} decares of rapeseed" + 
                $"\nAnd a total of 2000 + {functionParametersFinal[1].ToString("N2")} decares of sunflower" + 
            $"\nAnd a total of 3000 + {functionParametersFinal[2].ToString("N2")} decares of wheat" +
            $"\nAnd a total of 2000 + {functionParametersFinal[3].ToString("N2")} decares of corn\n\n");

            Console.WriteLine("This means that our total profit from the land will be: " +
                $"\n{2500 + functionParametersFinal[0]} times 75 for the rapeseed" +
                $"\n{(2000 + functionParametersFinal[1]).ToString("N2")} times 120 for the sunflower" +
                $"\n{3000 + functionParametersFinal[2]} times 110 for the wheat" +
                $"\n{2000 + functionParametersFinal[3]} times 100 for the corn" +
                $"\nfor a final total of {Math.Round(((2500 + functionParametersFinal[0]) * 75 + (2000 + functionParametersFinal[1]) * 120 + (3000 + functionParametersFinal[2]) * 110 + (2000 + functionParametersFinal[3]) * 100), 2)} leva.");

        }

        /// <summary>
        /// The following method's task is to draw a table with all of values for our simplex method task
        /// </summary>
        /// <param name="basis"> are the base variables </param>
        /// <param name="values"> are the respected values of those base variables </param>
        /// <param name="systemParameters"> are all of the parameters in our system </param>
        /// <param name="deltas"> are the deltas of our system </param>
        /// <param name="resultString0"> is the number of the 1st base variable </param>
        /// <param name="resultString1"> is the number of the 2nd base variable </param>
        /// <param name="resultString2"> is the number of the 3rd base variable</param>
        /// <param name="resultString3"> is the number of the 4th base variable </param>
        /// <param name="functionParameters"> are the parameters of our function </param>
        public static void createTableMethod(string[] basis, List<decimal> values, decimal[,] systemParameters, List<decimal> deltas, string resultString0, string resultString1, string resultString2, string resultString3, decimal[] functionParameters)
        {
            // Declare a table in which we will add the values of our simplex method
            var table = new ConsoleTable("Basis", "value", "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8");
            table.AddRow(basis[0], values[0].ToString("N2"), systemParameters[0, 0].ToString("N2"), systemParameters[0, 1].ToString("N2"), systemParameters[0, 2].ToString("N2"), systemParameters[0, 3].ToString("N2"), systemParameters[0, 4].ToString("N2"), systemParameters[0, 5].ToString("N2"), systemParameters[0, 6].ToString("N2"), systemParameters[0, 7].ToString("N2"));
            table.AddRow(basis[1], values[1].ToString("N2"), systemParameters[1, 0].ToString("N2"), systemParameters[1, 1].ToString("N2"), systemParameters[1, 2].ToString("N2"), systemParameters[1, 3].ToString("N2"), systemParameters[1, 4].ToString("N2"), systemParameters[1, 5].ToString("N2"), systemParameters[1, 6].ToString("N2"), systemParameters[1, 7].ToString("N2"));
            table.AddRow(basis[2], values[2].ToString("N2"), systemParameters[2, 0].ToString("N2"), systemParameters[2, 1].ToString("N2"), systemParameters[2, 2].ToString("N2"), systemParameters[2, 3].ToString("N2"), systemParameters[2, 4].ToString("N2"), systemParameters[2, 5].ToString("N2"), systemParameters[2, 6].ToString("N2"), systemParameters[2, 7].ToString("N2"));
            table.AddRow(basis[3], values[3].ToString("N2"), systemParameters[3, 0].ToString("N2"), systemParameters[3, 1].ToString("N2"), systemParameters[3, 2].ToString("N2"), systemParameters[3, 3].ToString("N2"), systemParameters[3, 4].ToString("N2"), systemParameters[3, 5].ToString("N2"), systemParameters[3, 6].ToString("N2"), systemParameters[3, 7].ToString("N2"));


            // Now we will calculate the first deltas              
            for (int i = 0; i < 8; i++)
            {
                decimal delta = systemParameters[0, i] * functionParameters[int.Parse(resultString0) - 1]
                              + systemParameters[1, i] * functionParameters[int.Parse(resultString1) - 1]
                              + systemParameters[2, i] * functionParameters[int.Parse(resultString2) - 1]
                              + systemParameters[3, i] * functionParameters[int.Parse(resultString3) - 1]
                              - functionParameters[i];

                deltas.Add(delta);
            }

            // Calculate the sum and draw the table
            decimal currentMaxSum = values[0] * functionParameters[int.Parse(resultString0) - 1]
                                 + values[1] * functionParameters[int.Parse(resultString1) - 1]
                                 + values[2] * functionParameters[int.Parse(resultString2) - 1]
                                 + values[3] * functionParameters[int.Parse(resultString3) - 1];

            table.AddRow("    ", "Sum = " + currentMaxSum.ToString("N2"), deltas[0].ToString("N2"), deltas[1].ToString("N2"), deltas[2].ToString("N2"), deltas[3].ToString("N2"), deltas[4].ToString("N2"), deltas[5].ToString("N2"), deltas[6].ToString("N2"), deltas[7].ToString("N2"));
            table.Write();
        }

        /// <summary>
        /// The following method's goal is to recalculate our base vector
        /// </summary>
        /// <param name="resultString0"> is the number of the 1st base variable </param>
        /// <param name="resultString1"> is the number of the 2nd base variable </param>
        /// <param name="resultString2"> is the number of the 3rd base variable</param>
        /// <param name="resultString3"> is the number of the 4th base variable </param>
        /// <param name="values"> are the respected values of those base variables </param>
        /// <param name="functionParametersFinal"> are the final parameters of our function </param>
        public static void createNewBasePlan(string resultString0, string resultString1, string resultString2, string resultString3, List<decimal> values, decimal[] functionParametersFinal)
        {
            // Calculate the values of the base vector
            for (int i = 0; i < functionParametersFinal.Length; i++)
            {
                if (int.Parse(resultString0) - 1 == i)
                {
                    functionParametersFinal[i] = values[0];
                }
                else if (int.Parse(resultString1) - 1 == i)
                {
                    functionParametersFinal[i] = values[1];
                }
                else if (int.Parse(resultString2) - 1 == i)
                {
                    functionParametersFinal[i] = values[2];
                }
                else if (int.Parse(resultString3) - 1 == i)
                {
                    functionParametersFinal[i] = values[3];
                }
                else
                {
                    functionParametersFinal[i] = 0;
                }
            }

            // print the base vector
            Console.WriteLine("The current state of our base plan is: ");
            for (int i = 0; i < functionParametersFinal.Length; i++)
            {
                Console.WriteLine("X" + (i + 1) + " equals  " + functionParametersFinal[i].ToString("N2"));
            }
        }

        /// <summary>
        /// The following method is supposed to recalculate the values of our variables
        /// </summary>
        /// <param name="rowLocation"> is the row location of our key number </param>
        /// <param name="deltaLocation"> is the column location of our key number </param>
        /// <param name="values"> are the respected values of those base variables </param>
        /// <param name="keyNumber"> is the key number itself </param>
        /// <param name="systemParameters"> are all of the parameters in our system </param>
        public static void changeVariablesList(int rowLocation, int deltaLocation, List<decimal> values, decimal keyNumber, decimal[,] systemParameters)
        {
            for (int i = 0; i < 4; i++)
            {
                decimal value;
                if (i == rowLocation)
                {
                    value = values[i] / keyNumber;
                }
                else
                {
                    value = (values[i] * keyNumber - values[rowLocation] * systemParameters[i, deltaLocation]) / keyNumber;
                }
                values.Add(value);
            }

            values.RemoveAt(0);
            values.RemoveAt(0);
            values.RemoveAt(0);
            values.RemoveAt(0);
        }

        /// <summary>
        /// The following method is supposed to recalculate the values of our system parameters
        /// </summary>
        /// <param name="systemParameters"> are all of the parameters in our system </param>
        /// <param name="rowLocation"> is the row location of our key number </param>
        /// <param name="deltaLocation"> is the column location of our key number </param>
        /// <param name="keyNumber"> is the key number itself </param>
        public static void changeSystemParametersArray(decimal[,] systemParameters, int rowLocation, int deltaLocation, decimal keyNumber)
        {
            for (int i = 0; i < 8; i++)
            {
                systemParameters[rowLocation, i] = systemParameters[rowLocation, i] / keyNumber;
            }

            for (int row = 0; row < 4; row++)
            {
                if (row == rowLocation)
                {
                    continue;
                }
                else
                {
                    decimal multiplyTimesDeltaRow = systemParameters[row, deltaLocation];
                    for (int col = 0; col < 8; col++)
                    {
                        systemParameters[row, col] = systemParameters[row, col] - multiplyTimesDeltaRow * systemParameters[rowLocation, col];
                    }
                }
            }
        }
    }
}
