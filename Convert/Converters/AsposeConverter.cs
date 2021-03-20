using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Aspose.Pdf;
using Aspose.Pdf.Facades;

namespace PDFManipulate.Converters
{
    public class AsposeConverter : AbstractConvertComponent
    {

        private readonly Document _doc;

        static AsposeConverter()
        {
            LicenseHelper.License.SetLicense2018();
        }

        public AsposeConverter(string file) : base(file)
        {
            _doc = new Document(file);
        }

        public override void AddFile(string fileName)
        {
            var d = new Document(fileName);
            new PdfFileEditor().Concatenate(new[] { d }, _doc);
        }

        public override void Dispose()
        {
            if (_doc != null)
            {
                try
                {
                    _doc.Save(PdfFileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    _doc.Dispose();
                }


            }
        }

        public override void SetTrimBox()
        {
            foreach (Page page in _doc.Pages)
            {
                //page.TrimBox = page.GetAutoTrim();
            }
        }


        public static void SetTrimBox(string file, double width, double height)
        {
            var newFile = Path.GetTempFileName();

            using (var doc = new Document(file))
            {
                foreach (Page page in doc.Pages)
                {
                    //page.SetTrimBox(width, height);
                }
                doc.Save(newFile);
            }

            var res = DialogResult.Retry;
            while (res == DialogResult.Retry)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(newFile, file, true);
                    break;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(e.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        File.Delete(newFile);
                        break;
                    }
                }
            }
        }

        public static void Run(string file)
        {
            // ExStart:ImageInformation
            
            // Load the source PDF file
            Document doc = new Document(file);

            // Define the default resolution for image
            int defaultResolution = 72;
            System.Collections.Stack graphicsState = new System.Collections.Stack();
            // Define array list object which will hold image names
            System.Collections.ArrayList imageNames = new System.Collections.ArrayList(doc.Pages[1].Resources.Images.Names);
            // Insert an object to stack
            graphicsState.Push(new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, 0, 0));

            foreach (Page page in doc.Pages)
            {
                Debug.WriteLine($"Page: {page.Number}");
                // Get all the operators on first page of document
            foreach (Operator op in page.Contents)
            {
                // Use GSave/GRestore operators to revert the transformations back to previously set
                Aspose.Pdf.Operator.GSave opSaveState = op as Aspose.Pdf.Operator.GSave;
                Aspose.Pdf.Operator.GRestore opRestoreState = op as Aspose.Pdf.Operator.GRestore;
                // Instantiate ConcatenateMatrix object as it defines current transformation matrix.
                Aspose.Pdf.Operator.ConcatenateMatrix opCtm = op as Aspose.Pdf.Operator.ConcatenateMatrix;
                // Create Do operator which draws objects from resources. It draws Form objects and Image objects
                Aspose.Pdf.Operator.Do opDo = op as Aspose.Pdf.Operator.Do;

                if (opSaveState != null)
                {
                    // Save previous state and push current state to the top of the stack
                    graphicsState.Push(((System.Drawing.Drawing2D.Matrix)graphicsState.Peek()).Clone());
                }
                else if (opRestoreState != null)
                {
                    // Throw away current state and restore previous one
                    graphicsState.Pop();
                }
                else if (opCtm != null)
                {
                    System.Drawing.Drawing2D.Matrix cm = new System.Drawing.Drawing2D.Matrix(
                       (float)opCtm.Matrix.A,
                       (float)opCtm.Matrix.B,
                       (float)opCtm.Matrix.C,
                       (float)opCtm.Matrix.D,
                       (float)opCtm.Matrix.E,
                       (float)opCtm.Matrix.F);

                    // Multiply current matrix with the state matrix
                    ((System.Drawing.Drawing2D.Matrix)graphicsState.Peek()).Multiply(cm);

                    continue;
                }
                else if (opDo != null)
                {
                    // In case this is an image drawing operator
                    if (imageNames.Contains(opDo.Name))
                    {
                        System.Drawing.Drawing2D.Matrix lastCTM = (System.Drawing.Drawing2D.Matrix)graphicsState.Peek();
                        // Create XImage object to hold images of first pdf page
                        XImage image = doc.Pages[1].Resources.Images[opDo.Name];

                        // Get image dimensions
                        double scaledWidth = Math.Sqrt(Math.Pow(lastCTM.Elements[0], 2) + Math.Pow(lastCTM.Elements[1], 2));
                        double scaledHeight = Math.Sqrt(Math.Pow(lastCTM.Elements[2], 2) + Math.Pow(lastCTM.Elements[3], 2));
                        // Get Height and Width information of image
                        double originalWidth = image.Width;
                        double originalHeight = image.Height;

                        // Compute resolution based on above information
                        double resHorizontal = originalWidth * defaultResolution / scaledWidth;
                        double resVertical = originalHeight * defaultResolution / scaledHeight;

                        // Display Dimension and Resolution information of each image
                        Debug.WriteLine(
                                string.Format(" image {0} ({1:.##}:{2:.##}): res {3:.##} x {4:.##}",
                                             opDo.Name, scaledWidth, scaledHeight, resHorizontal,
                                             resVertical));

                        Debug.WriteLine($"Color type: {image.GetColorType()}");
                    }
                }
            }
            // ExEnd:ImageInformation
            }

            
            
        }
    }
    
}
