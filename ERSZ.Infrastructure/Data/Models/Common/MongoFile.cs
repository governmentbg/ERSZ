using ERSZ.Infrastructure.Contracts;
using ERSZ.Infrastructure.Data.Models.Base;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.Data.Models.Common
{
    /// <summary>
    /// Файлово съдържание
    /// </summary>
    public class MongoFile : UserWrtModel, IExpiredInfo
    {
        [Key]
        public int Id { get; set; }
        public string FileId { get; set; }
        public int SourceType { get; set; }
        public string SourceId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public int FileSize { get; set; }

        public int? FileTypeId { get; set; }

        [ForeignKey(nameof(FileTypeId))]
        public virtual NomFileType FileType { get; set; }
        
        public DateTime? DateExpired { get; set; }

        public string ExpiredUserId { get; set; }

        public string ExpiredDescription { get; set; }
    }
}
