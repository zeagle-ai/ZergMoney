using IronOcr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ZergMoney.Models;

namespace ZergMoney.Helper
{
    public class TR
    {
        public OcrResult ReadText(Image img)
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

            var result = Ocr.Read(img);

            return (result);
        }

        public Transaction Scan(string textGroup)
        {
            string date = @"(0?[1-9]|1[0-2])[\/](0?[1-9]|[12]\d|3[01])[\/](19|20)\d{2}";
            string date1 = @"(0?[1-9]|1[0-2])[\-](0?[1-9]|[12]\d|3[01])[\-](19|20)\d{2}";
            string total = @"\$[0-9]*\.[0-9]{2,}";
            Match FoundDate = Regex.Match(textGroup, date);
            Match FoundDate1 = Regex.Match(textGroup, date1);
            Match FoundTotal = Regex.Match(textGroup, total);

            Transaction trans = new Transaction();
            if(FoundDate != null)
            {
                trans.Date = DateTime.Parse(FoundDate.ToString());  
            }
            if(FoundDate1 != null && FoundDate == null)
            {
                trans.Date = DateTime.Parse(FoundDate1.ToString());
            }
            if(FoundTotal != null)
            {
                var newTotal = FoundTotal.ToString().Remove(0,1);
                trans.Amount = decimal.Parse(newTotal);
            }
            return trans;
        }
    }
}