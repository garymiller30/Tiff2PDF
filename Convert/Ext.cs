using System;
using System.Drawing;
using PDFManipulate.Shema;
//using Page = Aspose.Pdf.Page;
//using Rectangle = Aspose.Pdf.Rectangle;

namespace PDFManipulate
{
    public static class Ext
    {
        public const double Mn = 2.83465;// (1/2.54)*7.2;// 2.83465;
        /*
                public static Rectangle GetAutoTrim(this Page page)
                {

                    const double delta = 1;


                    foreach (var format in Converter.AutoTrimBoxes)
                    {
                        if (page.MediaBox.Width >= (format.Width + format.BleedsFrom * 2 - delta) * Mn &&
                            page.MediaBox.Width <=(format.Width + format.BleedsTo * 2 + delta) * Mn &&
                            page.MediaBox.Height >= (format.Height + format.BleedsFrom * 2 - delta) * Mn &&
                            page.MediaBox.Height <= (format.Height + format.BleedsTo * 2 + delta) * Mn)
                        {

                            double x = (page.MediaBox.Width - format.Width * Mn) / 2;
                            double y = (page.MediaBox.Height - format.Height * Mn) / 2;

                            return new Rectangle(
                                x,
                                y, 
                                format.Width * Mn + x, 
                                format.Height*Mn + y);
                        }
                        if (page.MediaBox.Width >= (format.Height + format.BleedsFrom * 2 - delta) * Mn &&
                            page.MediaBox.Width <= (format.Height + format.BleedsTo * 2 + delta) * Mn &&
                            page.MediaBox.Height >= (format.Width + format.BleedsFrom * 2 - delta) * Mn &&
                            page.MediaBox.Height <= (format.Width + format.BleedsTo * 2 + delta) * Mn)

                        {

                            double x = (page.MediaBox.Width - format.Height * Mn) / 2;
                            double y = (page.MediaBox.Height - format.Width * Mn) / 2;
                            return new Rectangle(
                                x,
                                y, 
                                format.Height * Mn + x, 
                                format.Width * Mn + y);
                        }
                    }

                    return new Rectangle(page.TrimBox.LLX, page.TrimBox.LLY, page.TrimBox.Width, page.TrimBox.Height);
                }
        */


        public static Box GetAutoTrim(double width, double height)
        {
            const double delta = 1;


            foreach (var format in Converter.AutoTrimBoxes)
            {
                if (width >= (format.Width + format.BleedsFrom * 2 - delta) * Mn &&
                    width <= (format.Width + format.BleedsTo * 2 + delta) * Mn &&
                    height >= (format.Height + format.BleedsFrom * 2 - delta) * Mn &&
                    height <= (format.Height + format.BleedsTo * 2 + delta) * Mn)
                {

                    double x = (width - format.Width * Mn) / 2;
                    double y = (height - format.Height * Mn) / 2;

                    return new Box()
                    {
                        X = x,
                        Y = y,
                        Width = format.Width * Mn + x,
                        Height = format.Height * Mn + y
                    };
                }

                if (width >= (format.Height + format.BleedsFrom * 2 - delta) * Mn &&
                    width <= (format.Height + format.BleedsTo * 2 + delta) * Mn &&
                    height >= (format.Width + format.BleedsFrom * 2 - delta) * Mn &&
                    height <= (format.Width + format.BleedsTo * 2 + delta) * Mn)

                {

                    double x = (width - format.Height * Mn) / 2;
                    double y = (height - format.Width * Mn) / 2;
                    return new Box()
                    {
                        X = x,
                        Y = y,
                        Width = format.Height * Mn + x,
                        Height = format.Width * Mn + y
                    };
                }
            }

            return new Box() { X = 0, Y = 0, Width = width, Height = height };
        }

        public static Box GetTrimBox(double mediaW, double mediaH, double trimW, double trimH)
        {

            double x = (mediaW - trimW * Mn) / 2;
            double y = (mediaH - trimH * Mn) / 2;

            return new Box
            {
                X = x,
                Y = y,
                Width = trimW * Mn + x,
                Height = trimH * Mn + y
            };
        }

        public static Box GetTrimBox(double mediaW, double mediaH, double bleed)
        {
            var trimW = mediaW - bleed * Mn * 2;
            var trimH = mediaH - bleed * Mn * 2;

            double x = (mediaW - trimW) / 2;
            double y = (mediaH - trimH) / 2;

            return new Box
            {
                X = x,
                Y = y,
                Width = trimW + x,
                Height = trimH + y
            };
        }

        public static Box GetTrimBox(int pageNo, double mediaW, double mediaH, double outside, double inside,
            double top, double bottom)
        {
            var trimW = mediaW - (outside + inside) * Mn;
            var trimH = mediaH - (top + bottom) * Mn;

            var x = pageNo % 2 != 0 ? inside * Mn : outside * Mn;
            var y = bottom * Mn;

            return new Box
            {
                X = x,
                Y = y,
                Width = trimW + x,
                Height = trimH + y
            };
        }

        public static void CreateCustomBox(this Box box, double width, double height, double bleeds)
        {
            box.X = bleeds * Mn;
            box.Y = bleeds * Mn;
            box.Width = width * Mn;
            box.Height = height * Mn;

        }
        public static void RotateClockWise90deg(this Box box)
        {
            var tmp = box.Width;
            box.Width = box.Height;
            box.Height = tmp;

            tmp = box.X;
            box.X = box.Y;
            box.Y = tmp;

        }

        public static void RotateCounerClockWise90deg(this Box box, Box media)
        {
            box.X = media.Width - box.X - box.Width;
            box.Y = media.Height - box.Y - box.Height;

            var tmp = box.Width;
            box.Width = box.Height;
            box.Height = tmp;

            tmp = box.X;
            box.X = box.Y;
            box.Y = tmp;


        }

        public static (double Width, double Height) GetMediaBox(this Box box)
        {
            return (Width: box.Width + box.X * 2, Height: box.Height + box.Y * 2);

        }
        //public static void SetTrimBox(this Page page, double width, double height)
        //{
        //    double x = (page.MediaBox.Width - width * Mn) / 2;
        //    double y = (page.MediaBox.Height - height * Mn) / 2;

        //    page.TrimBox =new Rectangle(
        //        x,
        //        y, 
        //        width * Mn + x, 
        //        height * Mn + y);
        //}

    }
}
