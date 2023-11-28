using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.ViewModels.Register
{
    public class JurorVM
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        [Display(Name = "Идентификатор")]
        [Required(ErrorMessage = "Полето е задължително")]
        [AddToLog]
        [AutoSanitize]

        public string Uic { get; set; }

        [Display(Name = "Собствено име")]
        [Required(ErrorMessage = "Полето е задължително")]
        [AddToLog]
        [AutoSanitize]

        public string FirstName { get; set; }

        [Display(Name = "Бащино име")]
        [AddToLog]
        [AutoSanitize]

        public string MiddleName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Полето е задължително")]
        [AddToLog]
        [AutoSanitize]

        public string FamilyName { get; set; }

        [Display(Name = "Дата от")]
        [AddToLog]
        public DateTime DateStart { get; set; }

        [Display(Name = "Дата до")]
        [AddToLog]
        public DateTime? DateEnd { get; set; }

        //[Display(Name = "Град")]
        //[Range(1, int.MaxValue, ErrorMessage = "Полето е задължително")]
        ////[Required(ErrorMessage = "Полето е задължително")]
        //[AddToLog]
        public int CityId { get; set; }

        [Display(Name = "Населено място")]
        [Required(ErrorMessage = "Полето е задължително")]
        [AddToLog]
        public string CityCode { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Полето е задължително")]
        [AddToLog]
        [AutoSanitize]

        public string AddressText { get; set; }

        [Display(Name = "Образование")]
        [Range(1, int.MaxValue, ErrorMessage = "Полето е задължително")]
        [AddToLog]
        public int EducationId { get; set; }

        [Display(Name = "Образователно-кв. степен")]
        [AddToLog]
        public int? EducationRangId { get; set; }

        [Display(Name = "Телефон")]
        [AddToLog]
        [RegularExpression(NomenclatureConstants.PhoneRegexPattern, ErrorMessage = "Полето телефон трябва да съдържа само цифри и знакът плюс")]
        public string Phone { get; set; }

        [Display(Name = "Имейл адрес")]
        [AddToLog]
        [RegularExpression(NomenclatureConstants.EmailRegexPattern, ErrorMessage = "Невалидна електронна поща")]
        public string EMail { get; set; }

        [Display(Name = "Регистров номер")]
        //[Required(ErrorMessage = "Полето е задължително")]
        //[AddToLog]
        public string RegNumber { get; set; }

        [Display(Name = "Специалности")]
        public IList<CheckListVM> Specialities { get; set; }

        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the mandate model.
        /// </summary>
        /// <value>The mandate model.</value>
        public MandateVM MandateModel { get; set; }

        public JurorVM()
        {
            Specialities = new List<CheckListVM>();
        }

    }

    public class FilterJurorVM
    {
        [Display(Name = "Идентификатор на лице")]
        public string Uic { get; set; }

        [Display(Name = "Имена")]
        public string FullName { get; set; }

        [Display(Name = "Вид съд")]
        public int CourtTypeId { get; set; }

        [Display(Name = "Област")]
        public int DistrictId { get; set; }

        [Display(Name = "Съд")]
        public int CourtId { get; set; }

        [Display(Name = "Мандат от дата")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Мандат до дата")]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// 1-Само активни, <0 всички
        /// </summary>
        [Display(Name = "Активни")]
        public int ActiveOnly { get; set; }

        public bool IsEdit { get; set; }

    }
}
