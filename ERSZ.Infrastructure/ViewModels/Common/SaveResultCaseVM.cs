namespace ERSZ.Infrastructure.ViewModels.Common
{
    public class SaveResultCaseVM : SaveResultVM
    {
        public int CourtId { get; set; }
        public int JurorMandateId { get; set; }
        public string JurorId { get; set; }

        public SaveResultCaseVM()
        {

        }

        public SaveResultCaseVM(bool isSuccessfull, string errorMessage = null):base(isSuccessfull, errorMessage)
        {
            IsSuccessfull = isSuccessfull;
            ErrorMessage = errorMessage;
        }
    }
}
