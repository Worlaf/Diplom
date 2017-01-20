using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HoloImmitation
{
    public class LoadImageProperties
    {
        public enum SizeOptions
        {
            [Description("Без масштабирования")]            
            None,
            [Description("Растянуть до необходимых размеров")]
            Stretch,
            [Description("Растянуть с соблюдением пропорций")]
            Proportions
        }

        [DisplayName("Масштабирование")]
        [Description("Масштабирование загружаемого изображения")]
        public SizeOptions PasteSizeOptions { get; set; }


        [DisplayName("Центрирование")]
        [Description("Размещать загружаемое изображение по центру. Только когда не используется масштабирование")]
        public bool PlaseAtCenter { get; set; }


        public LoadImageProperties()
        {
            PlaseAtCenter = true;
        }
    }

    class ImageLoad
    {
    }
}
