using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Constants;
using ERSZ.Infrastructure.Data.Models.Common;
using ERSZ.Infrastructure.Data.Models.Nomenclatures;
using ERSZ.Infrastructure.ViewModels.Cdn;
using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERSZ.Controllers
{
    public class FilesController : BaseController
    {
        private readonly ICdnService cdnService;
        private readonly INomenclatureService nomService;
        public FilesController(
            ICdnService _cdnService,
            INomenclatureService _nomService)
        {
            cdnService = _cdnService;
            nomService = _nomService;
        }
        /// <summary>
        /// Изтегляне списък на файлове към даден обект (отвод)
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="sourceID"></param>
        /// <returns></returns>
        public IActionResult GetFileList(int sourceType, string sourceID)
        {
            List<CdnItemVM> model = cdnService.Select(sourceType, sourceID)
                                    .Where(x => x.DateExpired == null).ToList();
            return Json(model);
        }

        /// <summary>
        /// Добавяне на нов файл
        /// </summary>
        /// <param name="sourceType">Тип на обекта</param>
        /// <param name="sourceId">Идентификатор на обекта</param>
        /// <param name="container">Контайнер за прикачване</param>
        /// <param name="defaultTitle">Заглавие на контрола</param>
        /// <returns></returns>
        public PartialViewResult UploadFile(int sourceType, string sourceId, string container, string defaultTitle)
        {
            CdnUploadRequest model = new CdnUploadRequest()
            {
                SourceType = sourceType,
                SourceId = sourceId,
                FileContainer = container,
                Title = defaultTitle,
                MaxFileSize = 30
            };
            ViewBag.FileTypeId_ddl = nomService.GetDropDownListExpr<NomFileType>(x => x.SourceType == sourceType, false);
            return PartialView(model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(ICollection<IFormFile> files, CdnUploadRequest model)
        {
            if (files != null && files.Count() > 0)
            {
                string result = "failed";
                if (model.MaxFileSize > 0)
                {
                    long maxSize = (long)model.MaxFileSize * 1024 * 1024;
                    if (files.Any(x => x.Length > maxSize))
                    {
                        return Content("max_size");
                    }
                }
                foreach (var file in files)
                {
                    var fileExt = Path.GetExtension(file.FileName).Replace(".", "").ToLower();
                    string[] acceptFileExts = { "doc", "docx", "pdf", "png", "jpeg", "jpg" };
                    if (!acceptFileExts.Contains(fileExt))
                    {
                        return Content("file_ext");
                    }
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        model.FileContentBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                    model.ContentType = file.ContentType;
                    model.FileName = Path.GetFileName(file.FileName);
                    var res = await cdnService.MongoCdn_UploadFile(model);
                    if (res.Succeded)
                    {
                        result = "ok";
                        switch (model.SourceType)
                        {
                            case NomenclatureConstants.SourceType.Juror:
                                SaveLogOperation("juror", "edit", $"Добавяне на файл {model.FileName}", model.SourceId);
                                break;
                            case NomenclatureConstants.SourceType.JurorMandate:
                                SaveLogOperation("mandate", "edit", $"Добавяне на файл {model.FileName}", model.SourceId);
                                break;
                        }

                    }
                    else
                    {
                        result = "failed";
                        break;
                    }
                }
                return Content(result);
            }
            else
            {
                return Content("no_file");
            }
        }

        /// <summary>
        /// Изтегляне на документ по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileResult> Download(string id)
        {
            var model = await cdnService.MongoCdn_Download(id);
            return File(Convert.FromBase64String(model.FileContentBase64), model.ContentType, model.FileName);
        }

        public IActionResult DeleteFile(string cdnFileId)
        {
            if (!string.IsNullOrEmpty(cdnFileId))
            {
                var fileInfo = cdnService.Select(0, null, cdnFileId).FirstOrDefault();
                if (cdnService.DeleteMongoFileData(cdnFileId))
                {
                    switch (fileInfo.SourceType)
                    {
                        case NomenclatureConstants.SourceType.Juror:
                            SaveLogOperation("juror", "edit", $"Премахване на файл {fileInfo.FileTypeName} \\ {fileInfo.Title}", fileInfo.SourceId);
                            break;
                        case NomenclatureConstants.SourceType.JurorMandate:
                            SaveLogOperation("mandate", "edit", $"Премахване на файл {fileInfo.FileTypeName} \\ {fileInfo.Title}", fileInfo.SourceId);
                            break;
                    }
                    return Content("ok");

                }
                return Content("failed");
            }
            else
            {
                return Content("failed");
            }
        }

        public IActionResult ExpiredFile(string cdnFileId, string sourceType, string sourceId)
        {
            ExpiredInfoVM model = new ExpiredInfoVM()
            {
                StringId = cdnFileId,
                ExpireSubmitUrl = Url.Action("ExpiredFile_Save"),
                DialogTitle = "Премахване на файл",
                SourceId = sourceId,
                SourceType = sourceType
            };

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult ExpiredFile_Save(ExpiredInfoVM model)
        {
            var fileInfo = cdnService.Select(0, null, model.StringId).FirstOrDefault();
            var res = cdnService.ExpiredFile(model);

            switch (int.Parse(model.SourceType))
            {
                case NomenclatureConstants.SourceType.Juror:
                    {
                        SaveLogOperation("juror", "edit", $"Премахване на файл {fileInfo.FileTypeName} \\ {fileInfo.Title}", model.StringId);
                        SetSuccessMessage(res ? "Файлът е премахнат успешно." : "Проблем при изтриване на файл");
                        return RedirectToAction("Edit", "Juror", new { id = model.SourceId });
                    }
                case NomenclatureConstants.SourceType.JurorMandate:
                    {
                        SaveLogOperation("mandate", "edit", $"Премахване на файл {fileInfo.FileTypeName} \\ {fileInfo.Title}", model.StringId);
                        SetSuccessMessage(res ? "Файлът е премахнат успешно." : "Проблем при изтриване на файл");
                        return RedirectToAction("Edit", "Mandate", new { id = int.Parse(model.SourceId) });
                    }
                default: return Content("failed");
            }
        }
    }
}
