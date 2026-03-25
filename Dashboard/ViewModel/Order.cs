namespace Dashboard.ViewModel
{
    public class Order
    {
        public string Country { get; set; } = string.Empty;
        public decimal Percentage { get; set; }

        public decimal GrossValue { get; set; }

        public string Label { get; set; }

        public string Color { get; set; }
    }

    public class Duration
    {
        public string Month { get; set; }
        public decimal Value { get; set; }
    }

    public class MetricBarChartData
    {
        public string X { get; set; }
        public decimal Y { get; set; }
    }
}
