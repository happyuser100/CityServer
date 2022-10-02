using CityWebApi.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        [HttpGet]
        [Route("GetCitiesAll")]
        public async Task<ActionResult> GetCitiesAll()
        {
            var fileName = @"world-cities_csv.csv";
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };

            var cityItems = new List<CityItem>();
            
            using (var fs = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var textReader = new StreamReader(fs, Encoding.UTF8))
                using (var csv = new CsvReader(textReader, configuration))
                {
                    await csv.ReadAsync();

                    csv.ReadHeader();

                    while (await csv.ReadAsync())
                    {
                        var record = csv.GetRecord<CityItem>();

                        CityItem cityItem = new CityItem();
                        cityItem.name = record.name;
                        cityItem.country = record.country;
                        cityItem.subcountry = record.subcountry;
                        cityItem.geonameid = record.geonameid;

                        cityItems.Add(cityItem);
                    }

                    return Ok(JsonConvert.SerializeObject(cityItems));
                }
            }
        }

        public void DBWrite(string column, object value)
        {
            Console.WriteLine($"Storing {column}: {value}");
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<ActionResult> GetCities()
        {
            var fileName = @"world-cities_csv.csv";
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding
                Delimiter = "," // The delimiter is a comma
            };

            using (var fs = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var textReader = new StreamReader(fs, Encoding.UTF8))
                using (var csv = new CsvReader(textReader, configuration))
                {
                    //var data = csv.GetRecordsAsync<CityItem>();
                    var data = csv.GetRecords<CityItem[]>();

                    //await foreach (var person in data) // Iterate through the collection asynchronously
                    //{
                    //    // Do something with values in each row
                    //}


                    await Task.Run(() => { });

                    return Ok(JsonConvert.SerializeObject(data));
                }
            }
        }


    }
}
