using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.ViewModels.Cdn
{
    public class CdnUploadRequest : CdnFileSelect
    {
        /// <summary>
        /// Friendly name of content
        /// </summary>
        [Display(Name = "Описание на документа")]
        public string Title { get; set; }
        /// <summary>
        /// Actual file name, including extension
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Mime type
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Binary content of the file
        /// </summary>
        public byte[] FileContent { get; set; }

        public string FileTypeName { get; set; }
        [Display(Name = "Тип документ")]
        public int FileTypeId { get; set; }

        public string FileContentBase64 { get; set; }
        /// <summary>
        /// UserId of user uploaded
        /// </summary>
        public string UserUploaded { get; set; }
        /// <summary>
        /// Element id of div tag file container
        /// </summary>
        public string FileContainer { get; set; }       

        public int MaxFileSize { get; set; }
    }
}
