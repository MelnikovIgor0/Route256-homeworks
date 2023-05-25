using System.Runtime.Serialization;

namespace Route256.PriceCalculator;

[Serializable]
internal class ConfigData
{
    public string InputFile { get; set; }

    public string OutputFile { get; set; }

    public int Parallelism { get; set; }
    
    public double VolumeToPriceRatio { get; set; }
    
    public double WeightToPriceRatio { get; set; }
}