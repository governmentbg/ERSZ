using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Case;
using ERSZ.Infrastructure.ViewModels.Common;
using System;

namespace ERSZ.Infrastructure.Contracts.Data
{
    public class ErszBaseModel
    {
        public ErszContextModel Context { get; set; }
    }

    public class ErszCaseModel : ErszBaseModel
    {
        public string CaseTypeCode { get; set; }

        public string RegNumber { get; set; }

        public int RegYear { get; set; }
        public bool IsFinished { get; set; }
    }

    public class ErszCaseSelectionProtokolModel : ErszBaseModel
    {
        public string CaseGid { get; set; }

        public DateTime SelectionDate { get; set; }

        public string FileContentType { get; set; }
        /// <summary>
        /// Base64
        /// </summary>
        public string FileContent { get; set; }
    }

    public class ErszCaseDismissalModel : ErszBaseModel
    {
        public string CaseGid { get; set; }

        public DateTime DismissalDate { get; set; }

        public string DismissalKind { get; set; }
        public string Reason { get; set; }
    }

    public class ErszCaseSessionModel : ErszBaseModel
    {

        public string CaseGid { get; set; }

        public string SessionKind { get; set; }
        public string State { get; set; }
        public string Result { get; set; }
        public string ResultBase { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }

    public class ErszCaseSessionActModel : ErszBaseModel
    {

        public string CaseSessionGid { get; set; }

        public string ActKind { get; set; }
        public string RegNumber { get; set; }
        public DateTime RegDate { get; set; }
    }

    public class ErszCaseSessionAmountModel : ErszBaseModel
    {
        public string CaseSessionGid { get; set; }

        public decimal Fee { get; set; }
        public decimal Expences { get; set; }
        public decimal Fine { get; set; }
        public bool FineIsPaid { get; set; }
    }

    public class ErszContextModel
    {
        public string Gid { get; set; }
        public string CourtCode { get; set; }
        public string JurorUic { get; set; }
    }


    public class ErszResponseModel
    {
        public string ResultCode { get; set; }

        public string Message { get; set; }
    }

    public class ErszBaseActivitySaveResultWrapper
    {
        public BaseActivity BaseActivity { get; set; }
        public SaveResultCaseVM SaveResultCaseVM { get; set; }
    }

    public class ErszConstants
    {

        public const int RegNumberLength = 14;
        public class ResultCodes
        {
            public const string OK = "0";

            /// <summary>
            /// Ненамерен по ЕГН
            /// </summary>
            public const string InvalidJuror = "101";

            /// <summary>
            /// Ненамерен мандат за заседател в подадения съд
            /// </summary>
            public const string InvalidJurorMandate = "102";

            public const string InvalidCaseGid = "103";
            public const string InvalidCaseSesionGid = "104";

            /// <summary>
            /// Ненамерена стойност по код - маппинг
            /// </summary>
            public const string InvalidCode = "105";

            /// <summary>
            /// Подаден модел няма стойност
            /// </summary>
            public const string NullValue = "106";
            /// <summary>
            /// Невалидна стойност - при валидация (дати, отрицателни суми)
            /// </summary>
            public const string InvalidValue = "199";

            public const string GeneralException = "500";

        }

        public class Messages
        {
            public const string JurorNotFound = "Juror was not found";
            public const string JurorMandatesNotFound = "Juror mandates not found";
            public const string CaseTypeNotFound = "Case type was not found";
            public const string CaseNotFound = "Case was not found";
            public const string CaseSessionNotFound = "CaseSession was not found";
            public const string CaseSessionAmountNotFound = "CaseSessionAmount was not found";
            public const string CourtNotFound = "Court was not found";
            public const string GidExists = "Gid already exists";
            public const string GidNullOrEmpty = "Gid data is null or empty";
            public const string DateStartNullOrEmpty = "DateStart is null or empty";
            public const string DataMapFailed = "Model data is null";
            public const string RegNumberLength = "RegNumber must be 14 symbols";
            public const string RegNumberInvalid = "RegNumber is in invalid format";
            public const string GeneralException = "Unexpected error";
        }
    }
}
