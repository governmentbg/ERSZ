using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.Constants
{
    public static class JurorConstants
    {
        public class Message
        {
            public const string UicExist = "Съществува заседател с този идентификатор";
            public const string MandateExists = "Вече съществува активен мандат м/у избраните дати";
            public const string MandateMissionExists = "Вече съществува командироване в този съд м/у избраните дати";
        }
        public class Mandate
        {
            /// <summary>
            /// Мандат
            /// </summary>
            public const int MandateType = 1;
            /// <summary>
            /// Командироване
            /// </summary>
            public const int MandateMissionType = 2;
        }

        public class MandateLable
        {
            /// <summary>
            /// Мандат
            /// </summary>
            public const string MandateType = "Мандат";
            /// <summary>
            /// Командироване
            /// </summary>
            public const string MandateMissionType = "Командироване";
        }
    }
}
