using ERSZ.Infrastructure.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Nomenclatures
{
    public class BaseCommonNomenclature : ICommonNomenclature
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Номер по ред")]
        public int OrderNumber { get; set; }

        [Display(Name = "Код")]
        public string Code { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        public string Label { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }


        [Display(Name = "Активен запис")]
        public bool IsActive { get; set; }

        [Display(Name = "Начална дата")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Крайна дата")]
        public DateTime? DateEnd { get; set; }
    }
}
