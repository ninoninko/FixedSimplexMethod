using System;
using System.Linq;
using System.Collections.Generic;
using ConsoleTables;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CourseWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The following 2 lines are used to calculate the execution time of the program
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            // Create function variables
            // Let column be the total number of elements - a total of 8 
            // O(column) time complexity
            decimal[] functionParameters = new decimal[]
            {
                75, 120, 110, 100, 0, 0, 0, 0
            };

            // Create final function variables
            // Let column be the total number of elements - a total of 8 
            // O(column) time complexity
            decimal[] functionParametersFinal = new decimal[]
            {
                75, 120, 110, 100, 0, 0, 0, 0
            };

            // Create function limits
            // Let column be the total number of elements - a total of 8 
            // Let row be the total number of rows - a total of 4
            // O(row * column) time complexity
            decimal[,] systemParameters = new decimal[,] 
            {
                {1, 1, 1, 1, 1, 0, 0, 0},
                {85, 105, 95, 90, 0, 1, 0, 0},
                {0, 0, 1, 0, 0, 0, 1, 0},
                {0, 0, 0, 1, 0, 0, 0, 1}
            };

            // Create basis changes
            // Let row be the total number of rows - a total of 4
            // O(row) time complexity 
            List<decimal> values = new List<decimal>()
            {
                10500, 762500, 2000, 1000
            };

            // Create base variable string names
            // Let row be the total number of rows - a total of 4
            // O(row) time complexity
            string[] basis = new string[] { "x5", "x6", "x7", "x8" };

            // Create tables until all of the deltas are bigger or equal than 0
            // O(tables) where tables is the total number of tables initialized
            while (true)
            {
                // Declare some variables which will come in handy in a minute
                // Their descriptions can be found underneat in the method descriptions 
                // A total of O(5) operations
                List<decimal> deltas = new List<decimal>();
                string resultString0 = Regex.Match(basis[0], @"\d+").Value;
                string resultString1 = Regex.Match(basis[1], @"\d+").Value;
                string resultString2 = Regex.Match(basis[2], @"\d+").Value;
                string resultString3 = Regex.Match(basis[3], @"\d+").Value;

                // The following method deals with the drawing of the tables
                // The following method has a time complexity of O(column * 21 + 114)
                createTableMethod(basis, values, systemParameters, deltas, resultString0, resultString1, resultString2, resultString3, functionParameters);

                // The following method creates a new base plan for our program
                // The following method has a time complexity of O(columns * 29 + 2)
                createNewBasePlan(resultString0, resultString1, resultString2, resultString3, values, functionParametersFinal);

                /**
                 * We can see that some of the deltas are < 0, which means
                 * that this is not the highest possible revenue
                 */
                int deltaLocation = 0;                      // 1 operation
                decimal smallestDelta = int.MaxValue;       // 1 operation
                for (int i = 0; i < deltas.Count; i++)      // O(columns * 11 + 2)
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
                } // 3 operations

                Console.WriteLine($"This is our lowest delta in {deltaLocation + 1} column");   // 1 operation
                Console.WriteLine("Since it is < 0, we will now look for the key number:");     // 1 operation

                // Now we will seek out our key number
                List<decimal> possibleKeyNumbers = new List<decimal>();   // 1 operation

                for (int i = 0; i < 4; i++)  // O(rows * 11 + 2)
                {
                    if (systemParameters[i, deltaLocation] != 0)
                    {
                        // if we are not dividing by 0
                        possibleKeyNumbers.Add(values[i] / systemParameters[i, deltaLocation]);  // 6 operations
                    } else
                    {
                        // add an enourmous number to take account for the row
                        possibleKeyNumbers.Add(decimal.MaxValue);  // 3 operations
                    }
                }

                int rowLocation = possibleKeyNumbers.IndexOf(possibleKeyNumbers.Min());      // 6 operations
                decimal keyNumber = systemParameters[rowLocation, deltaLocation];            // 3 operations
                Console.WriteLine($"Our key is at values {rowLocation}, {deltaLocation}");   // 3 operations

                // Change the basis in the table
                basis[rowLocation] = "x" + (deltaLocation + 1);                              // 5 operations

                // The following method will recalculate our variables list
                // O(rows * 23 + 7)
                changeVariablesList(rowLocation, deltaLocation, values, keyNumber, systemParameters);

                // The following method will recalculate our system parameters
                // O(row * (columns * 11 + 10) + 2 + columns * 7 + 3)
                changeSystemParametersArray(systemParameters, rowLocation, deltaLocation, keyNumber);
                
            }

            Console.WriteLine("\n\n\nAlthough this may seem as our solution we are missing a very important thing: " +
                "\nWe did not start with the values in the task but rather did some tricks." +
                "\nMr. Popov, refer to the blank pages." +
                "\nHence, to the first four instances of the functionParametersFinal array we have to add 2500, 2000, 3000 and 2000 respectively." +
                $"\nThis means that we need to sow a total of 2500 + {functionParametersFinal[0].ToString("N2")} decares of rapeseed" + 
                $"\nAnd a total of 2000 + {functionParametersFinal[1].ToString("N2")} decares of sunflower" + 
            $"\nAnd a total of 3000 + {functionParametersFinal[2].ToString("N2")} decares of wheat" +
            $"\nAnd a total of 2000 + {functionParametersFinal[3].ToString("N2")} decares of corn\n\n");   // O(9) operations

            Console.WriteLine("This means that our total profit from the land will be: " +
                $"\n{2500 + functionParametersFinal[0]} times 75 for the rapeseed" +
                $"\n{(2000 + functionParametersFinal[1]).ToString("N2")} times 120 for the sunflower" +
                $"\n{3000 + functionParametersFinal[2]} times 110 for the wheat" +
                $"\n{2000 + functionParametersFinal[3]} times 100 for the corn" +
                $"\nfor a final total of {Math.Round(((2500 + functionParametersFinal[0]) * 75 + (2000 + functionParametersFinal[1]) * 120 + (3000 + functionParametersFinal[2]) * 110 + (2000 + functionParametersFinal[3]) * 100), 2)} leva."); // O(34) operations


            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
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
            var table = new ConsoleTable("Basis", "value", "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8");   // 1 operation
            table.AddRow(basis[0], values[0].ToString("N2"), systemParameters[0, 0].ToString("N2"), systemParameters[0, 1].ToString("N2"), systemParameters[0, 2].ToString("N2"), systemParameters[0, 3].ToString("N2"), systemParameters[0, 4].ToString("N2"), systemParameters[0, 5].ToString("N2"), systemParameters[0, 6].ToString("N2"), systemParameters[0, 7].ToString("N2"));   // 20 operations
            table.AddRow(basis[1], values[1].ToString("N2"), systemParameters[1, 0].ToString("N2"), systemParameters[1, 1].ToString("N2"), systemParameters[1, 2].ToString("N2"), systemParameters[1, 3].ToString("N2"), systemParameters[1, 4].ToString("N2"), systemParameters[1, 5].ToString("N2"), systemParameters[1, 6].ToString("N2"), systemParameters[1, 7].ToString("N2"));   // 20 operations
            table.AddRow(basis[2], values[2].ToString("N2"), systemParameters[2, 0].ToString("N2"), systemParameters[2, 1].ToString("N2"), systemParameters[2, 2].ToString("N2"), systemParameters[2, 3].ToString("N2"), systemParameters[2, 4].ToString("N2"), systemParameters[2, 5].ToString("N2"), systemParameters[2, 6].ToString("N2"), systemParameters[2, 7].ToString("N2"));   // 20 operations
            table.AddRow(basis[3], values[3].ToString("N2"), systemParameters[3, 0].ToString("N2"), systemParameters[3, 1].ToString("N2"), systemParameters[3, 2].ToString("N2"), systemParameters[3, 3].ToString("N2"), systemParameters[3, 4].ToString("N2"), systemParameters[3, 5].ToString("N2"), systemParameters[3, 6].ToString("N2"), systemParameters[3, 7].ToString("N2"));   // 20 operations


            // Now we will calculate the first deltas
            // Total O(column * 21) operations
            for (int i = 0; i < 8; i++)
            {
                decimal delta = Math.Round(systemParameters[0, i] * functionParameters[int.Parse(resultString0) - 1] + systemParameters[1, i] * functionParameters[int.Parse(resultString1) - 1] + systemParameters[2, i] * functionParameters[int.Parse(resultString2) - 1] + systemParameters[3, i] * functionParameters[int.Parse(resultString3) - 1] - functionParameters[i], 3);   // 20 operations

                deltas.Add(delta); // 1 operation
            }

            // Calculate the sum and draw the table
            decimal currentMaxSum = Math.Round(values[0] * functionParameters[int.Parse(resultString0) - 1]
                                 + values[1] * functionParameters[int.Parse(resultString1) - 1]
                                 + values[2] * functionParameters[int.Parse(resultString2) - 1]
                                 + values[3] * functionParameters[int.Parse(resultString3) - 1], 3);   // 11 operations

            table.AddRow("    ", "Sum = " + currentMaxSum.ToString("N2"), deltas[0].ToString("N2"), deltas[1].ToString("N2"), deltas[2].ToString("N2"), deltas[3].ToString("N2"), deltas[4].ToString("N2"), deltas[5].ToString("N2"), deltas[6].ToString("N2"), deltas[7].ToString("N2"));   // 20 operations
            table.Write();   // 1 operation
        } // the following method has a time complexity of O(column * 21 + 113)

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
            // O(columns * 25)
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
        } // O(columns * 29 + 1)

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
            for (int i = 0; i < 4; i++)  // 0(rows * 23 + 2)
            {
                decimal value;
                if (i == rowLocation)
                {
                    value = Math.Round(values[i] / keyNumber, 3);
                }
                else
                {
                    value = Math.Round((values[i] * keyNumber - values[rowLocation] * systemParameters[i, deltaLocation]) / keyNumber, 3);
                }
                values.Add(value);
            }

            values.RemoveAt(0);
            values.RemoveAt(0);
            values.RemoveAt(0);
            values.RemoveAt(0);
        } // O(rows * 23 + 6)

        /// <summary>
        /// The following method is supposed to recalculate the values of our system parameters
        /// </summary>
        /// <param name="systemParameters"> are all of the parameters in our system </param>
        /// <param name="rowLocation"> is the row location of our key number </param>
        /// <param name="deltaLocation"> is the column location of our key number </param>
        /// <param name="keyNumber"> is the key number itself </param>
        public static void changeSystemParametersArray(decimal[,] systemParameters, int rowLocation, int deltaLocation, decimal keyNumber)
        {
            for (int i = 0; i < 8; i++)   // O(columns * 7 + 2)
            {
                systemParameters[rowLocation, i] = Math.Round(systemParameters[rowLocation, i] / keyNumber, 3);
            }

            for (int row = 0; row < 4; row++) // O(row * (columns * 11 + 10) + 2)
            {
                if (row == rowLocation)
                {
                    continue;
                }
                else
                {
                    decimal multiplyTimesDeltaRow = systemParameters[row, deltaLocation];
                    for (int col = 0; col < 8; col++)  // O(columns * 11 + 2)
                    {
                        systemParameters[row, col] = Math.Round(systemParameters[row, col] - multiplyTimesDeltaRow * systemParameters[rowLocation, col], 3);
                    }
                }
            }
        }
    }



    // По груби сметки, времевата комплексност на дадената задачата е приблизително 
    // O(2*columns + rows*columns + 2*rows + 43 + tables*(68*colums + 11*rows*colums + 44*rows + 162)), където
    // columns е броят на елементите на всеки ред, rows е броят на редовете в матрицата с ограничения,
    // а tables е броят на таблиците, които се строят, за да бъде направено решението.
    // Тъй като в конкретната задача се строят 3 таблици, може да заместим tables с 3 и да изчислим, че комклексността е
    // O(206*columns + 34*rows*colums + 134*rows + 439), но тъй като говорим за алгоритмична комплексност, можем да премахнем
    // свободния член и коефициентите пред променливите и така ще останем със следната комплексност:
    // O(columns + rows*colums + rows), но винаги се интересуваме само от водещите степени (произведението в случая),
    // което ще означава, че нашата финална времева комплексност е O(rows * columns)
    //
    // В програмата е използван класът Stopwatch, който е вграден в C# и изчислява времето на изпълнение между 2 
    // определени момента в даден код. Задавайки го да започне от самото начало и да приключи засичането накрая
    // на програмата, той ще принтира накрая за колко милисекунди е бил изпълнен симплекс методът.
    //
    // При анализиране на кода с Analyze -> Calculate Code Metrics се получават следните резултати: 
    // Maintainability index - 51/100 като според Google над 20 значи е нормално
    // Cyclomatic Complexity - 24, като според Google приемливото е до 8 за парче код, а тук говорим за цялата програма от 310 реда
    // Depth of Inheritance - 1, защото никъде не е използвано унаследяване
    // Class Coupling - 11, което според Google не е оптимално, но става дума за класове, които са критични за изпълнението на програмата като Stopwatch, List, ConsoleTable
    //
    // Личното ми заключение е, че макар програмата да не е идеална, тя успешно изпълнява зададената задача. 
    // Не е идеална, защото абстракцията й стига до това да работи с всяка задача от този тип, която не променя 
    // броя на променливите или броя на ограниченията и дефактно не промеяна реда на изпълнение на командите.
    // Друга причина е високата стойност на Class Coupling и Cyclomatic Complexity, които могат да бъдат оправдани.
    // В допълнение програмата използва следния NuGet пакет - ConsoleTable, което ще означава, че за да има правилна
    // компилация на друг компютър ще е необходимо да се изпърви първо следната команда:
    // Install-Package ConsoleTables -Version 2.4.2
    //
    // Обяснено накратко, програмата извършва следните операции по време на жизнения си цикъл:
    // Инициализират се масиви със стойностите на променливите според условието на задачата и се влиза в цикъл.
    // В цикъла се влиза в следните методи - createTableMethod, който създава и принтира таблицата,
    // createNewBasePlan, който метод създава нов базов план. След това се изчисляват стойностите на базисите
    // и ако всички от тях са по - големи или равни на 0 се приключва изпълнението на цикъла.
    // В случай на отрицателен такъв се определя ключово число, намира се нов базис и чрез методите - 
    // changeVariablesList и changeSystemParametersArray се манипулират съответно векторът със стойности на
    // базисните променливи и цялата матрица със стойности на задачата. След края изпълнението на цикъла се
    // принтират финалните резултати на потребителя и времето, което е било необходимо за изпълнението на задачата.

}
