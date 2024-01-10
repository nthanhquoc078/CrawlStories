using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateServices
{
    public static partial class ServiceExtensions
    {
        public static string GetLanguageShortName(this LanguageEnum languageEnum, bool isSource = false)
        {
            return languageEnum
                switch
            {
                LanguageEnum.English => "en",
                LanguageEnum.Vietnamese => "vi",
                LanguageEnum.Chinese => "zh-CN",
                _ => isSource ? "auto" : "en"
            };
        }
        public static string GetLanguageFullName(this LanguageEnum languageEnum, bool isSource = false)
        {
            return languageEnum
                switch
            {
                LanguageEnum.English => "English",
                LanguageEnum.Vietnamese => "Vietnamese",
                LanguageEnum.Chinese => "zh-CN",
                _ => isSource ? "Auto" : "English"
            };
        }
    }
}
