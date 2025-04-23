using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JoygameProject.Application.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string name)
        {
            string slug = name.ToLower()
                .Replace(" ", "-")
                .Replace("ı", "i")
                .Replace("ş", "s")
                .Replace("ç", "c")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ö", "o")
                .Replace("--", "-");

            return Regex.Replace(slug, "[^a-z0-9\\-]", "");
        }
    }
}
