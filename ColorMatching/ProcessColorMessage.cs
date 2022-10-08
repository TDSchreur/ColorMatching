using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ColorMatching
{
    public class ProcessColorMessage
    {
        private readonly MandM Rood = new MandM()
        {
            Red = new MandM.Range { Min = 160, Max = 170 },
            Green = new MandM.Range { Min = 45, Max = 55 },
            Blue = new MandM.Range { Min = 25, Max = 35 },
            Kleur = "Rood"
        };

        private readonly MandM Geel = new MandM()
        {
            Red = new MandM.Range { Min = 120, Max = 130 },
            Green = new MandM.Range { Min = 80, Max = 95 },
            Blue = new MandM.Range { Min = 20, Max = 30 },
            Kleur = "Geel"
        };

        private readonly MandM Groen = new MandM()
        {
            Red = new MandM.Range { Min = 75, Max = 85 },
            Green = new MandM.Range { Min = 110, Max = 120 },
            Blue = new MandM.Range { Min = 40, Max = 50 },
            Kleur = "Groen"
        };

        private readonly MandM Oranje = new MandM()
        {
            Red = new MandM.Range { Min = 130, Max = 140 },
            Green = new MandM.Range { Min = 65, Max = 75 },
            Blue = new MandM.Range { Min = 25, Max = 40 },
            Kleur = "Oranje"
        };

        private readonly MandM Blauw = new MandM()
        {
            Red = new MandM.Range { Min = 50, Max = 65 },
            Green = new MandM.Range { Min = 90, Max = 100 },
            Blue = new MandM.Range { Min = 85, Max = 100 },
            Kleur = "Blauw"
        };

        private readonly MandM Bruin = new MandM()
        {
            Red = new MandM.Range { Min = 110, Max = 120 },
            Green = new MandM.Range { Min = 80, Max = 90 },
            Blue = new MandM.Range { Min = 40, Max = 50 },
            Kleur = "Bruin"
        };

        [FunctionName("ProcessColorMessage")]
        public void Run([ServiceBusTrigger("color-sensor", Connection = "sb-connection")] Color color, ILogger log)
        {

            log.LogInformation($"Color: Red:{color.Red} Green:{color.Green} Blue:{color.Blue}");
            log.LogInformation($"Kleur volgens ESP32: {color.Kleur}");
            log.LogInformation($"Rood: {Rood.IsMatch(color)}");
            log.LogInformation($"Geel: {Geel.IsMatch(color)}");
            log.LogInformation($"Groen: {Groen.IsMatch(color)}");
            log.LogInformation($"Oranje: {Oranje.IsMatch(color)}");
            log.LogInformation($"Blauw: {Blauw.IsMatch(color)}");
            log.LogInformation($"Bruin: {Bruin.IsMatch(color)}");
        }
    }

    public class Color
    {
        public double Red { get; init; }
        public double Green { get; init; }
        public double Blue { get; init; }

        /// <summary>
        /// kleur volgens de ESP32
        /// </summary>
        public string Kleur { get; init; }
    }

    public class MandM
    {
        public Range Red { get; init; }
        public Range Green { get; init; }
        public Range Blue { get; init; }
        public string Kleur { get; init; }

        public bool IsMatch(Color color)
        {
            return Red.InRange(color.Red) && Green.InRange(color.Green) && Blue.InRange(color.Blue);
        }

        public class Range
        {
            public int Min { get; init; }
            public int Max { get; init; }

            public bool InRange(double value)
            {
                return value > Min && value < Max;
            }
        }
    }
}
