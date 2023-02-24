using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace NameToElementsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return new JsonResult(NameToElementsAPI(id));
        }

        private string[] NameToElementsAPI(string Name)
        {
            string[] Output = new string[Name.Length];
            string[] RawElements = System.IO.File.ReadAllLines("PeriodicElements.txt");
            string[] ElementSymbols = new string[RawElements.Length];
            int i = 0;
            int LastElement = 118;
            int OutCurr = 0;
            foreach (string s in RawElements)
            {
                ElementSymbols[i] = s.Substring(0, 2).Trim();
                i++;
            }
            Letter[] NameLetters = new Letter[Name.Length];
            i = 0;
            foreach (char c in Name)
            {
                NameLetters[i] = new Letter(c, ' ');
                if (i != 0)
                {
                    NameLetters[i - 1].NextChar = c;
                }
                i++;
            }
            i = 0;
            foreach (Letter l in NameLetters)
            {
                if (l.ThisChar != ' ')
                {
                    bool found = false;
                    string CombChar = l.ThisChar.ToString() + l.NextChar.ToString();
                    int ii = 0;
                    foreach (string s in ElementSymbols)
                    {
                        if (s.ToLower() == CombChar && ElementSymbols[LastElement] != s)
                        {
                            NameLetters[i + 1].ThisChar = ' ';
                            LastElement = ii;
                            found = true;
                            Output[OutCurr] =  RawElements[ii];
                            OutCurr++;
                        }
                        ii++;

                    }
                    if (!found)
                    {
                        ii = 0;

                        foreach (string s in ElementSymbols)
                        {
                            if (s.ToLower() == l.ThisChar.ToString().ToLower() && ElementSymbols[LastElement] != s)
                            {
                                found = true;
                                Output[OutCurr] = RawElements[ii];
                                OutCurr++;
                                LastElement = ii;

                            }
                            ii++;

                        }
                        if (!found)
                        {
                            Output[OutCurr] = l.ThisChar.ToString();
                            OutCurr++; 
                        }

                    }

                }
                i++;
            }
            string[] NewOut = new string[OutCurr];
            for (int j = 0; j < OutCurr; j++)
            {
                NewOut[j] = Output[j];
            }
            return NewOut;
        }
        public class Letter
        {
            public char ThisChar;
            public char NextChar;

            public Letter(char thischar, char nextchar)
            {
                ThisChar = thischar;
                NextChar = nextchar;
            }
        }
    }
}