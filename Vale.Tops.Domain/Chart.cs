using System.Collections.Generic;

namespace Vale.Tops.Domain
{
    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
    public class Datasets
    {
        public bool fill { get; set; }
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data { get; set; }
        public string[] pointBackgroundColor { get; set; }
        public string[] pointBorderColor { get; set; }
    }
}