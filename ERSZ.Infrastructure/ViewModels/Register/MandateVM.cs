using ERSZ.Infrastructure.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class MandateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Заседател")]
        public string JurorId { get; set; }

        [Display(Name = "Заседател")]
        public string JurorFullName { get; set; }

        public int? ParentId { get; set; }

        /// <summary>
        /// Съд, от общото събрание на който е избран
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Полето е задължително")]
        [Display(Name = "Съд, в който е избран")]
        [AddToLog]
        public int? CourtId { get; set; }

        /// <summary>
        /// Съд, към който е регистриран
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Полето е задължително")]
        [Display(Name = "Съд, от който е избран")]
        [AddToLog]
        public int? RegisterCourtId { get; set; }

        /// <summary>
        /// Община
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Полето е задължително")]
        [Display(Name = "Община")]
        [AddToLog]
        public int? MunicipalityId { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Тип")]
        public int MandateTypeId { get; set; }
        public string MandateTypeLabel { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Дата от")]
        [AddToLog]
        public DateTime DateStart { get; set; }

        [Display(Name = "Дата до")]
        [AddToLog]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Основание за предсрочно прекратяване")]
        [AddToLog]
        [AutoSanitize]
        public string DateTerminationDescription { get; set; }

        [Display(Name = "Дата на предсрочно прекратяване")]
        [AddToLog]
        public DateTime? DateTermination { get; set; }

        [Display(Name = "Мандат номер")]
        [AddToLog]
        public int MandateNo { get; set; }

    }
    public class FilterMandateVM
    {
        [Display(Name = "Заседател")]
        public string JurorName { get; set; }

        [Display(Name = "Община")]
        public int MunicipalityId { get; set; }

        [Display(Name = "Вид на мандата")]
        public int MandateTypeId { get; set; }

        [Display(Name = "Съд")]
        public int CourtId { get; set; }

        [Display(Name = "От дата")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "До дата")]
        public DateTime? DateTo { get; set; }


    }
}
