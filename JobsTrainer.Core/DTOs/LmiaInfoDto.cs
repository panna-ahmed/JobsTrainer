namespace JobsTrainer.Core.DTOs
{
    public class LmiaInfoDto
    {
        public int Id { get; set; }
        public string ProvinceTerritory { get; set; }
        public string ProgramStream { get; set; }
        public string Employer { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public int Approved { get; set; }
        public string CompanyLink { get; set; }

        public int? NextLmiaId { get; set; }
        public int? PrevLmiaId { get; set; }
    }
}
