using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySettings
{
    public static class SliderImage
    {
        private static int _maxImageCount { get; set; } = 5;

        public static int MaxImageCount()
        {
            return _maxImageCount;
        }
    }
}
