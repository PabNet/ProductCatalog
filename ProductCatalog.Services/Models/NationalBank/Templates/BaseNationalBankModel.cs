namespace ProductCatalog.Services.Models.NationalBank.Templates
{
    public class BaseNationalBankModel
    {
        public int? Cur_ID { get; set; }
        public string? Cur_Name { get; set; }
        public int? Cur_Scale { get; set; }
        public int? Cur_Periodicity { get; set; }
        public DateTime? Cur_DateStart { get; set; }
        public DateTime? Cur_DateEnd { get; set; }
    }
}
