using System;
using System.Net;
using Newtonsoft.Json;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Net.Http;
using System.Xml;

namespace RestConverter
{
    class Program
    {
        private static void Main(string[] args)
        {
            // A continuacion se realiza el proceso de extraccion del Json del Rest service para luego
            // ser transformado en una datatable.
            string URL = "https://jsonplaceholder.typicode.com/posts";
            var Json = new WebClient().DownloadString(URL);
            dynamic ArchivoJson = JsonConvert.DeserializeObject(Json);
            var JsonContenido = JsonConvert.SerializeObject(ArchivoJson);
            Console.WriteLine(Json);
        }
        // Una vez extraido el Json del Rest Service se procede a convertirlo en una Datatable
        public static DataTable jsonStringToTable(string JsonContenido)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonContenido);
            return dt;
        }
        // A continuacion se Procede a pasar la informacion extraida en una datatable a un archivo. CSV
        public static string jsonToCSV(string JsonContenido, string delimiter)
        {
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                csv.Configuration.Delimiter = delimiter;

                using (var dt = jsonStringToTable(JsonContenido))
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        // Se repite el proceso por la cantidad de records almacenados en la Datatable
                        csv.NextRecord();
                    }
                }
            }
            // Devuelve la cadena de Json en un archivo .CSV
            return csvString.ToString();
        }

    }
}


