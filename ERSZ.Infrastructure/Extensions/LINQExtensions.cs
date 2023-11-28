using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERSZ.Infrastructure.Extensions
{
    public static class LINQExtensions
    {
        /// <summary>
        /// Маркира определен елемент, като избран
        /// </summary>
        /// <typeparam name="TSource">Тип на колекцията</typeparam>
        /// <param name="source">Изходна колекция</param>
        /// <param name="selected">Елемент, който да бъде избран</param>
        /// <returns></returns>
        public static SelectList SetSelected<TSource>(
        this IEnumerable<TSource> source, object selected)
        {
            if (source == null)
            {
                return new SelectList(new List<SelectListItem>());
            }
            return new SelectList(source, "Value", "Text", selected);
        }

        public static int[] ToIntArray(this string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                return (new List<int>()).ToArray();
            }
            try
            {
                return model.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            }
            catch
            {
                return (new List<int>()).ToArray();
            }
        }

        public static string EmptyToNull(this string model, string nullVal = "")
        {
            if (model == null || model?.Trim() == nullVal)
            {
                return null;
            }
            return model;
        }

        public static int? EmptyToNull(this int? model, int nullVal = -1)
        {
            if (model == null || model == nullVal)
            {
                return null;
            }
            return model;
        }

        public static DateTime? MakeEndDate(this DateTime? model)
        {
            if (model.HasValue && model.Value.Hour == 0 && model.Value.Minute == 0)
            {
                return model.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            return model;
        }

        public static DateTime MakeEndDate(this DateTime model)
        {
            if (model.Hour == 0 && model.Minute == 0)
            {
                return model.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            return model;
        }
        //public static string ToPaternSearch(this string model)
        //{
        //    if (string.IsNullOrWhiteSpace(model))
        //    {
        //        return "%";
        //    }
        //    return $"%{model.Replace(" ", "%")}%";
        //}
    }
}
