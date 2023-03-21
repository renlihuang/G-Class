using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CS.View.Common
{
    class ImageHelp
    {
        //创建位图
        public static BitmapImage CreateBitmapImage(byte[] imageBytes)
        {
            BitmapImage bitmapImage = null;

            if (imageBytes != null)
            {
                //创建文件流
                MemoryStream memoryStream = new MemoryStream(imageBytes);
                //显示图片
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
     
            return bitmapImage;
        }
    }
}
