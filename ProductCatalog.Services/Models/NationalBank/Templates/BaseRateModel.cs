namespace ProductCatalog.Services.Models.NationalBank.Templates
{
    public class BaseRateModel : BaseNationalBankModel
    {
        public DateTime Date { get; set; }
        public decimal Cur_OfficialRate { get; set; }
    }
}
