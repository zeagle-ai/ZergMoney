using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ZergMoney.Helper
{
    public class TR
    {
        public OcrResult ReadText(string imgPath)
        {
            var Ocr = new AdvancedOcr()
            {
                CleanBackgroundNoise = false,
                EnhanceContrast = false,
                EnhanceResolution = false,
                Language = IronOcr.Languages.English.OcrLanguagePack,
                Strategy = IronOcr.AdvancedOcr.OcrStrategy.Fast,
                ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                DetectWhiteTextOnDarkBackgrounds = false,
                InputImageType = AdvancedOcr.InputTypes.AutoDetect,
                RotateAndStraighten = true,
                ReadBarCodes = false,
                ColorDepth = 4
            };

            var result = Ocr.Read(HttpRuntime.AppDomainAppPath + imgPath);

            return (result);
        }

        public string[] Scan(string textGroup)
        {
            string date = @"(0?[1-9]|1[0-2])[\/](0?[1-9]|[12]\d|3[01])[\/](19|20)\d{2}";
            string date1 = @"(0?[1-9]|1[0-2])[\-](0?[1-9]|[12]\d|3[01])[\-](19|20)\d{2}";
            string total = @"\$[0-9]*\.[0-9]{2,}";
            Match FoundDate = Regex.Match(textGroup, date);
            Match FoundDate1 = Regex.Match(textGroup, date1);
            Match FoundTotal = Regex.Match(textGroup, total);

            bool IsDeposit = textGroup.IndexOf("deposit", StringComparison.OrdinalIgnoreCase) >= 0;
            var listOfDTT = new List<string>();
            if (IsDeposit)
            {
                listOfDTT.Add("true");
            }
            else
            {
                listOfDTT.Add("false");
            }
            if(FoundDate != null)
            {
                var addedDate = FoundDate.ToString();
                listOfDTT.Add(addedDate);
            }
            if(FoundDate1 != null && FoundDate == null)
            {
                var addedDate = FoundDate1.ToString();
                listOfDTT.Add(addedDate);
            }
            if(FoundTotal != null)
            {
                var addedTotal = FoundTotal.ToString();
                listOfDTT.Add(addedTotal);
            }
            return listOfDTT.ToArray();
        }
    }
}