namespace ERSZ.Infrastructure.Constants
{
    public static class NomenclatureConstants
    {
        public const string CountryBG = "BG";
        public const string AssemblyQualifiedName = "ERSZ.Infrastructure.Data.Models.Nomenclatures.{0}, ERSZ.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        public const string PhoneRegexPattern = @"^[0-9 /\+\-\(\)\.]{5,32}$";
        public const string EmailRegexPattern = @"^(?:[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[A-Za-z0-9-]*[A-Za-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])$";
        public class UicTypes
        {
            public const int EGN = 1;
            public const int LNCh = 2;
            public const int EIK = 3;
            public const int BirthDate = 4;
            public const int Bulstat = 5;
        }

        /// <summary>
        /// Стойности за видове операции в журнал на промените
        /// </summary>
        public class AuditOperations
        {
            public const string Add = "Добавяне";
            public const string Edit = "Редакция";
            public const string Patch = "Актуализация";
            public const string Delete = "Изтриване";
            public const string View = "Преглед";
            public const string Correct = "Корекция";
            public const string List = "Списък";
            public const string Login = "Вход в системата";
            public const string Import = "Импорт";
        }

        public class SourceType
        {
            public const int Juror = 10;
            public const int JurorMandate = 20;
        }

        public class Roles
        {
            public const string GlobalAdmin = "GLOBAL_ADMIN";
            public const string Admin = "ADMIN";
            public const string Report = "REPORT";
            public const string Register = "REGISTER";
        }

        public class IsFinishedText
        {
            public const string All = "Избери";
            public const string DoneCase = "Свършило дело";
            public const string UnfinishedCase = "Несвършило дело";
        }

        public class IsFinishedValue
        {
            public const string All = "A";
            public const string DoneCase = "Y";
            public const string UnfinishedCase = "N";
        }
    }
}
