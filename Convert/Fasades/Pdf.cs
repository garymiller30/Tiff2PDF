using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PDFManipulate.Converters;
using PDFManipulate.Forms;
using PDFManipulate.Shema;

namespace PDFManipulate.Fasades
{
    public static class Pdf
    {
        public static void SetTrimBox(string file, double width, double height)
        {
            if (File.Exists(file))
            {
                var extension = Path.GetExtension(file);
                if (extension != null && extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    PdfLibConverter.SetTrimBox(file, width, height);
                    //AsposeConverter.SetTrimBox(file,width,height);
                }
            }
        }

        public static void SetTrimBoxByBleed(string file, double bleed)
        {
            if (File.Exists(file))
            {
                var extension = Path.GetExtension(file);
                if (extension != null && extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    PdfLibConverter.SetTrimBoxByBleed(file, bleed);
                }
            }
        }

        public static void SetTrimBoxBySpread(string file, double inside, double outside, double top, double bottom)
        {
            if (File.Exists(file))
            {
                var extension = Path.GetExtension(file);
                if (extension != null && extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    PdfLibConverter.SetTrimBoxBySpread(file, inside, outside, top, bottom);
                }
            }
        }

        public static PageInfo[] GetPagesInfo(string file)
        {
            if (File.Exists(file))
            {
                var extension = Path.GetExtension(file);
                if (extension != null && extension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    return PdfLibConverter.GetPagesInfo(file);
                }
            }

            throw new Exception("File not exist");
        }
        /// <summary>
        /// convert files to pdf
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static Converter ConvertToPDF(IEnumerable<string> files)
        {

            var enumerable = files as string[] ?? files.ToArray();

            if (enumerable.Length > 1)
            {
                //показати меню вибора режиму

                using (var smd = new FormSelectMode())
                {
                    if (smd.ShowDialog() == DialogResult.OK)
                    {
                        if (enumerable.Length > 2 && smd.ConvertMode == ConvertModeEnum.MultipleFiles)
                        {

                            using (var f = new FormList(enumerable))
                            {
                                f.ShowDialog();

                                if (f.ConvertFiles != null)
                                {

                                    return new Converter(
                                        new ConverterSettings
                                        {
                                            Files = f.ConvertFiles,
                                            Mode = smd.ConvertMode
                                        });

                                    //new Converter().Start(f.ConvertFiles, ConvertModeEnum.SingleFile);
                                }
                            }
                        }
                        else
                        {
                            return new Converter(
                                new ConverterSettings
                                {
                                    Files = enumerable.ToArray(),
                                    Mode = smd.ConvertMode
                                });
                        }

                    }
                }

            }
            else
            {
                //конвертувати за замовчуванням

                var file = enumerable.First();

                if (Directory.Exists(file))
                {
                    return new Converter(
                        new ConverterSettings
                        {
                            Files = Directory.GetFileSystemEntries(file),
                            Mode = ConvertModeEnum.SingleFile
                        });
                    //    .Start(Directory.GetFileSystemEntries(file),ConvertModeEnum.SingleFile);

                }


                if (File.Exists(file))
                {
                    return new Converter(
                        new ConverterSettings
                        {
                            Files = new[] { file },
                            Mode = ConvertModeEnum.SingleFile
                        }); //.Start(new[] {file},ConvertModeEnum.SingleFile);
                }
            }


            return null;

        }

        public static Converter ConvertToPDF(IEnumerable<string> files, ConvertModeEnum mode)
        {

            var settings = new ConverterSettings()
            {
                Mode = mode
            };


            if (files.Count() > 1)
            {
                if (files.Count() > 2 && mode == ConvertModeEnum.MultipleFiles)
                {
                    using (var f = new FormList(files.ToArray()))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            if (f.ConvertFiles != null)
                            {
                                settings.Files = f.ConvertFiles;
                            }
                            else
                            {
                                settings.Mode = ConvertModeEnum.NotAssigned;
                            }
                        }
                        else
                        {
                            settings.Mode = ConvertModeEnum.NotAssigned;
                        }

                    }
                }
                else
                {
                    settings.Files = files.ToArray();
                }


            }
            else
            {
                var file = files.First();
                if (Directory.Exists(file))
                {
                    settings.Files = Directory.GetFileSystemEntries(file);

                }
                else if (File.Exists(file))
                {
                    settings.Files = new[] { file };
                }
                else
                {
                    throw new Exception("File or Directory not exists");
                }


            }

            var converter = new Converter(settings);

            return converter;
        }


        public static Converter SplitPDF(IEnumerable<string> files)
        {
            if (files.Any())
            {

                using (var f = new FormSelectCountPages())
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {

                        var settings = f.SplitSettings;
                        settings.Files = files.ToArray();

                        return new Converter
                        {
                            SplitPdfSettings = settings
                        };


                    }
                }

            }

            return null;
        }

        public static Converter ExtractPages(IEnumerable<string> files)
        {
            if (files.Any())
            {
                using (var f = new FormSelectCountPages())
                {

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        var settings = f.SplitSettings;
                        settings.Files = files.ToArray();
                        return new Converter
                        {
                            SplitPdfSettings = settings
                        };
                    }
                }
            }

            return null;
        }


        public static void RepeatPages(IEnumerable<string> files)
        {
            if (files.Any())
            {
                using (var f = new FormSelectCountPages())
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var file in files)
                        {
                            PdfLibConverter.RepeatPages(file, f.SplitSettings.FixedCountPages);
                        }
                    }
                }
            }
        }

        public static void MergeFrontsAndBack(IEnumerable<string> files)
        {
            if (files.Count() > 1)
            {
                using (var f = new FormSelectFile(files))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        var fronts = files.Except(new[] { f.Back });

                        foreach (var front in fronts)
                        {
                            PdfLibConverter.MergeFrontsAndBack(front, f.Back);
                        }
                    }
                }
            }
        }

        public static void ReversePages(IEnumerable<string> files)
        {
            if (files.Any())
            {
                foreach (var file in files)
                {
                    PdfLibConverter.ReversePages(file);
                }
            }
        }

        public static void RepeatDocument(IEnumerable<string> files)
        {
            if (files.Any())
            {
                using (var f = new FormSelectCountPages())
                {
                    f.Text = "Кількість копій";
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var file in files)
                        {
                            PdfLibConverter.RepeatDocument(file, f.SplitSettings.FixedCountPages);
                        }
                    }
                }
            }
        }

        public static void CreateElipse(IEnumerable<string> files)
        {
            if (files.Any())
            {
                foreach (var file in files)
                {
                    PdfLibConverter.CreateElipse(file);
                }
            }
        }

        public static void CreateRectangle(IEnumerable<string> files)
        {
            if (files.Any())
            {
                foreach (var file in files)
                {
                    PdfLibConverter.CreateRectangle(file);
                }
            }
        }

        public static void SplitCoverAndBlock(IEnumerable<string> files)
        {
            if (!files.Any()) return;

            foreach (var file in files)
            {
                PdfLibConverter.SplitCoverAndBlock(file);
            }

        }

        public static void SplitOddAndEven(IEnumerable<string> files)
        {
            if (!files.Any()) return;

            foreach (var file in files)
            {
                PdfLibConverter.SplitOddAndEven(file);
            }
        }

        public static void RotateMirrorFrontAndBack(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                PdfLibConverter.RotateMirrorFrontAndBack(file);
            }
        }

        public static void CreateEmptyPdfTemplateWithCount(string pathTo, List<EmptyTemplate> templates)
        {
            if (!templates.Any()) return;
            if (!Directory.Exists(pathTo)) return;

            foreach (var template in templates)
            {
                PdfLibConverter.CreateEmptyPdfTemplateWithCount(pathTo, template);
            }


        }

        public static void MergeOddAndEven(IEnumerable<string> files)
        {
            if (files.Count() == 2)
            {
                using (var f = new FormSelectFile(files))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        var fronts = files.Except(new[] { f.Back }).ToList();
                        PdfLibConverter.MergeOddAndEven(fronts[0], f.Back);
                    }
                }
            }
        }
    }
}
