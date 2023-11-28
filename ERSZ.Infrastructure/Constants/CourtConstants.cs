using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.Constants
{
    public static class CourtConstants
    {
        public class CourtType
        {
            /// <summary>
            /// Конституционния съд
            /// </summary>
            public const int KS = 1;

            /// <summary>
            /// Върховен касационен съд
            /// </summary>
            public const int VKS = 2;

            /// <summary>
            /// Върховен административен съд
            /// </summary>
            public const int VAS = 3;

            /// <summary>
            /// Апелативен специализиран наказателен съд
            /// </summary>
            public const int ASNS = 4;

            /// <summary>
            /// Специализиран наказателен съд
            /// </summary>
            public const int SNS = 5;

            /// <summary>
            /// Военно-апелативен съд
            /// </summary>
            public const int VoApS = 6;

            /// <summary>
            /// Военен съд
            /// </summary>
            public const int VS = 7;

            /// <summary>
            /// Апелативен съд
            /// </summary>
            public const int AS = 8;

            /// <summary>
            /// Административен съд
            /// </summary>
            public const int AdmS = 9;

            /// <summary>
            /// Окръжен съд
            /// </summary>
            public const int OS = 10;

            /// <summary>
            /// Районен съд
            /// </summary>
            public const int RS = 11;

            /// <summary>
            /// Висш съдебен съвет
            /// </summary>
            public const int VSS = 12;

            public static int[] CourtFromSelected = { OS, AS, VoApS, ASNS };
            public static int[] CourtInSelected = { OS, RS, SNS, VS };
            public static int[] CourtАppointment = { AS, OS, VoApS, ASNS, SNS, VoApS, VS, RS };
        }
    }
}
