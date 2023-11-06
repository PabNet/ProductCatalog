using ProductCatalog.Services.Models.NationalBank.Templates;

namespace ProductCatalog.Services.Models.NationalBank
{
    public class Currency : BaseNationalBankModel
    {
        public int? Cur_ParentID { get; set; }
        public int? Cur_Code { get; set; }
        public string? Cur_Abbreviation { get; set; }
        public string? Cur_Name_Bel { get; set; }
        public string? Cur_Name_Eng { get; set; }
        public string? Cur_QuotName { get; set; }
        public string? Cur_QuotName_Bel { get; set; }
        public string? Cur_QuotName_Eng { get; set; }
        public string? Cur_NameMulti { get; set; }
        public string? Cur_Name_BelMulti { get; set; }
        public string? Cur_Name_EngMulti { get; set; }
    }
}
