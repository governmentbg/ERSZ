using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.Extensions;
using ERSZ.Infrastructure.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERSZ.Infrastructure.Data.Models.Base
{
    public class NamesBase
    {
        [Display(Name = "Идентификатор")]
        public string Uic { get; set; }
    
        [Display(Name = "Вид идентификатор")]
        public int UicTypeId { get; set; }
 
        [Display(Name = "Собствено име")]
        public string FirstName { get; set; }

        [Display(Name = "Бащино име")]
        public string MiddleName { get; set; }
  
        [Display(Name = "Фамилия 1")]
        public string FamilyName { get; set; }
 
        [Display(Name = "Фамилия 2")]
        public string Family2Name { get; set; }

        [Display(Name = "Наименование")]
        public string FullName { get; set; }
 
        [Display(Name = "Отдел/структура")]
        public string DepartmentName { get; set; }
 
        public string LatinName { get; set; }

        [Display(Name = "Починало лице")]
        public bool? IsDeceased { get; set; }

        [Display(Name = "Дата на смъртта")]
        public DateTime? DateDeceased { get; set; }

        public int? Person_SourceType { get; set; }
        
        public long? Person_SourceId { get; set; }

        public string Person_SourceCode { get; set; }

        [ForeignKey(nameof(UicTypeId))]
        public virtual NomUicType UicType { get; set; }



        public string UicTypeLabel
        {
            get
            {
                switch (this.UicTypeId)
                {
                    case NomenclatureConstants.UicTypes.EGN:
                        return "ЕГН";
                    case NomenclatureConstants.UicTypes.LNCh:
                        return "ЛНЧ";
                    default:
                        return "";
                }
            }
        }

        public bool IsPerson
        {
            get
            {
                switch (this.UicTypeId)
                {
                    case NomenclatureConstants.UicTypes.EGN:
                    case NomenclatureConstants.UicTypes.LNCh:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public string FullName_Initials
        {
            get
            {
                var firstName = (!string.IsNullOrEmpty(FirstName) ? FirstName.ToShortNameCyrlillic() + "." + (!string.IsNullOrEmpty(MiddleName + FamilyName + Family2Name) ? " " : string.Empty) : "");
                var middleName = (!string.IsNullOrEmpty(MiddleName) ? MiddleName.ToShortNameCyrlillic() + "." + (!string.IsNullOrEmpty(FamilyName + Family2Name) ? " " : string.Empty) : "");
                var familyName = (!string.IsNullOrEmpty(FamilyName) ? FamilyName.ToShortNameCyrlillic() + "." + (!string.IsNullOrEmpty(Family2Name) ? " " : string.Empty) : "");
                var family2Name = (!string.IsNullOrEmpty(Family2Name) ? Family2Name.ToShortNameCyrlillic() + "." : "");
                return firstName + middleName + familyName + family2Name;
            }
        }

        public string FullName_MiddleNameInitials
        {
            get
            {
                var firstName = (!string.IsNullOrEmpty(FirstName) ? FirstName + (!string.IsNullOrEmpty(MiddleName + FamilyName + Family2Name) ? " " : string.Empty) : "");
                var middleName = (!string.IsNullOrEmpty(MiddleName) ? MiddleName.ToShortNameCyrlillic() + "." + (!string.IsNullOrEmpty(FamilyName + Family2Name) ? " " : string.Empty) : "");
                var familyName = (!string.IsNullOrEmpty(FamilyName) ? FamilyName + (!string.IsNullOrEmpty(Family2Name) ? " " : string.Empty) : "");
                var family2Name = (!string.IsNullOrEmpty(Family2Name) ? Family2Name : "");
                return firstName + middleName + familyName + family2Name;
            }
        }

        public string FirstNameInitial_Family
        {
            get
            {
                var firstName = (!string.IsNullOrEmpty(FirstName) ? FirstName.ToShortNameCyrlillic() + "." + (!string.IsNullOrEmpty(FamilyName) ? " " : string.Empty) : "");
                var familyName = (!string.IsNullOrEmpty(FamilyName) ? FamilyName : "");
                return firstName + familyName;
            }
        }

        public string FirstNameFamilyInitial
        {
            get
            {
                var firstName = !string.IsNullOrEmpty(FirstName) ? FirstName.ToShortNameCyrlillic() + "." + (!string.IsNullOrEmpty(FamilyName) ? " " : string.Empty) : "";
                var familyName = !string.IsNullOrEmpty(FamilyName) ? FamilyName.ToShortNameCyrlillic() + "." : "";
                return firstName + familyName;
            }
        }

        public void SplitPersonNames(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            //Махат се двойните интервали в името и тирета за втора фамилия
            value = value.Replace("-", "").Replace("  ", "").Replace("  ", "");
            var names = value.Split(' ');
            if (names.Length > 0)
            {
                this.FirstName = names[0];

                if (names.Length > 1)
                {
                    this.MiddleName = names[1];

                    if (names.Length > 2)
                    {
                        this.FamilyName = names[2];
                        if (names.Length > 3)
                        {
                            this.Family2Name = names[3];
                        }
                    }
                    else
                    {
                        //ако са само две имена стават собствено име и фамилия.
                        this.FamilyName = this.MiddleName;
                        this.MiddleName = string.Empty;
                    }
                }
            }
        }
    }
}
