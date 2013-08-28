namespace Orc.FluentTypeFilter.Tests
{
    using System;

    class ManyProperties
    {
        public int Int1 { get; set; }
        private int Int2 { get; set; }
        private int Int3 { get; set; }
        
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }

        public DateTime DateTime1 { get; set; }
        public static DateTime DateTime2 { get; set; }
        private static DateTime DateTime3 { get; set; }

        public TimeSpan TimeSpan1 { get; set; }
        public TimeSpan TimeSpan2 { get; set; }
        public TimeSpan TimeSpan3 { get; set; }
    }
}