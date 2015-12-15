using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cvm.Backend.Business.Config;
using Cvm.Backend.Business.Resources;

namespace Cvm.Web.Code
{
    public class ImageUtility
    {
        public static void ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;
            }

            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            NewImage.Save(NewFile);
        }

        public static string EstimateUrl(Resource resource)
        {
            //Example http://www.adfaerdsprofil.dk/CVNav/PAEI_spind_CvNav.asp?P1=614142b5-ef37-4b59-bbe8-ff6a213a32e7

            return String.Format("{0}?P1={1}", WebConfigMgr.EstimateUrlResultImage, resource.ResourceIdEncoded);
        }
    }
}