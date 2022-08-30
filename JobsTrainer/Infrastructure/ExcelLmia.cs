using CsvHelper.Configuration.Attributes;

namespace JobsTrainer.Infrastructure
{
    public class ExcelLmia
    {
        [Index(0)]
        public string ProvinceTerritory { get; set; }
        [Index(1)]
        public string ProgramStream { get; set; }
        [Index(2)]
        public string Employer { get; set; }
        [Index(3)]
        public string Address { get; set; }
        [Index(4)]
        public string Occupation { get; set; }
        [Index(6)]
        public int Approved { get; set; }
    }
}
