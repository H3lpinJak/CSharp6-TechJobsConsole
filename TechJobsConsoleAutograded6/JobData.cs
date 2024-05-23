using System;
using System.Text;

namespace TechJobsConsoleAutograded6
{
    public class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }

            return values;
        }

        // Search all columns for the given term
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            LoadData();

            List<Dictionary<string, string>> foundJobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                bool jobFound = false;

                foreach (KeyValuePair<string, string> pair in job)
                {
                    if (pair.Value.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        jobFound = true;
                        break;
                    }
                }

                if (jobFound && !foundJobs.Any(j => j.SequenceEqual(job)))
                {
                    foundJobs.Add(job);
                }
            }

            return foundJobs;
        }

        // Returns results of search the jobs data by key/value, using inclusion of the search term.
        // For example, searching for employer "Enterprise" will include results with "Enterprise Holdings, Inc".
        public static List<Dictionary<string, string>> FindByColumnAndValue(
            string column,
            string value
        )
        {
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column];

                // Make search case-insensitive
                if (aValue.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        // Load and parse data from job_data.csv
        private static void LoadData()
        {
            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArray = CSVRowToStringArray(line);
                    if (rowArray.Length > 0)
                    {
                        rows.Add(rowArray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        // Parse a single line of a CSV file into a string array
        private static string[] CSVRowToStringArray(
            string row,
            char fieldSeparator = ',',
            char stringSeparator = '\"'
        )
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }
    }
}
