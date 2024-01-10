using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateServices
{
    public interface ITranslateService
    {
        Task<string> TranslateAsync(string input, LanguageEnum from = LanguageEnum.Auto, LanguageEnum to = LanguageEnum.English);
    }
    public enum LanguageEnum
    {
        Auto = 0,
        Vietnamese = 1,
        English = 2,
        Chinese = 3,
    }
}
