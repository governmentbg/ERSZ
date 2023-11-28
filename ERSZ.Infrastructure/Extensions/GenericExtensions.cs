using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.Extensions
{
    public static class GenericExtensions
    {

        /// <summary>
        /// Съкращава имена до първа гласна буква
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appendDot"></param>
        /// <returns></returns>
        public static string ToShortNameCyrlillic(this string name, bool appendDot = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            char[] vowelLetters = { 'А', 'Ъ', 'О', 'У', 'Е', 'И', 'Я', 'а', 'ъ', 'о', 'у', 'е', 'и', 'я' };
            char letterInVowel;
            string shortName = "";
            for (int i = 0; i < name.Length; i++)
            {
                letterInVowel = vowelLetters
                .FirstOrDefault(l => l == name[i]);

                if (letterInVowel != '\0' && i != 0)
                {
                    if (appendDot)
                    {
                        return $"{shortName}.";
                    }
                    else
                    {
                        return $"{shortName}";
                    }

                }

                shortName += name[i];
            }
            return name;
        }

        /// <summary>
        /// Check value is EGN
        /// </summary>
        /// <param name="EGN">string</param>
        /// <returns>string</returns>
        public static bool IsEGN(this string EGN, bool InitiallyValidation = false)
        {
            if (EGN == null) return false;
            if (EGN.Length != 10) return false;
            if (EGN == "0000000000") return false;

            // само първична валидация
            if (InitiallyValidation)
            {
                decimal egn = 0;
                if (!decimal.TryParse(EGN, out egn)) return false;
                return true;
            }

            // пълна валидация
            int a = 0;
            int valEgn = 0;
            for (int i = 0; i < 10; i++)
            {
                if (!int.TryParse(EGN.Substring(i, 1), out a)) return false;
                switch (i)
                {
                    case 0:
                        valEgn += 2 * a;
                        continue;
                    case 1:
                        valEgn += 4 * a;
                        continue;
                    case 2:
                        valEgn += 8 * a;
                        continue;
                    case 3:
                        valEgn += 5 * a;
                        continue;
                    case 4:
                        valEgn += 10 * a;
                        continue;
                    case 5:
                        valEgn += 9 * a;
                        continue;
                    case 6:
                        valEgn += 7 * a;
                        continue;
                    case 7:
                        valEgn += 3 * a;
                        continue;
                    case 8:
                        valEgn += 6 * a;
                        continue;
                }
            }
            long chkSum = valEgn % 11;
            if (chkSum == 10)
                chkSum = 0;
            if (chkSum != Convert.ToInt64(EGN.Substring(9, 1))) return false;
            if ((int.Parse(EGN.Substring(8, 1)) / 2) == 0)
            {
                // girl person
                return true;
            }
            // guy person
            return true;
        }
    }
}
