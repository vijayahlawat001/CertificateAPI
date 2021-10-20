using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SuperManagerCertificateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateCertificateController : Controller
    {
        [HttpPost]
        public string Post(string Name, string Model, string Email)
        {
            try
            {
                //var header = Request.Headers;
                //Microsoft.Extensions.Primitives.StringValues Keyvalue;
                //header.TryGetValue("Authorization", out Keyvalue);
                //if (!header.ContainsKey("Authorization"))
                //{
                //    return "Invalid Request";
                //}
                if (Name == null)
                {
                    throw new Exception("Please provide user name");
                }
                if (Model == null)
                {
                    throw new Exception("Please provide Model");
                }
                if (Email == null)
                {
                    throw new Exception("Please provide user Email");
                }
                string FileName = Name + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssffffff") + ".pdf";
                var fs = PDF(FileName, Name, Model);
                SendMail.SendEmail(Email, "", "", "Your SUPERMANAGER.APP certificate has arrived!", "Congratulations on completing the course, we hope that you've enjoyed the course and have broadened your understanding of the principles and methods of becoming a future SUPERMANAGER.<b><br /><br />Kind regards,<br /><br />The management of SUPERMANAGER.</b>", fs);
                fs.Delete();
                return "Email has been sent successfully.";
            }
            catch (Exception)
            {
                throw;
            }
        }

        static FileInfo PDF(string FileName, string Name, string Model)
        {
            try
            {
                PdfDocument doc = new PdfDocument();
                PdfPageSettings settings = new PdfPageSettings();
                doc.PageSettings.Size = new SizeF(500, 500);
                doc.PageSettings.SetMargins(10);
                PdfSection section = doc.Sections.Add();
                PdfPageBase page = section.Pages.Add();
                PdfImage image = PdfImage.FromFile(@"Untitled.png");
                float width = image.Width * 0.75f;
                float height = page.Size.Height * 0.75f;
                float x = (page.Canvas.ClientSize.Width - width) / 2;
                string[] NameSplit = Name.Split(' ');
                page.Canvas.DrawImage(image, 10, 20, width, height);
                page.Canvas.DrawString("CERTIFICATE OF EXCELLENCE", new PdfFont(PdfFontFamily.Helvetica, 16f, PdfFontStyle.Bold), new PdfSolidBrush(Color.SteelBlue), 170, 60);
                page.Canvas.DrawString("This is presented to", new PdfFont(PdfFontFamily.Helvetica, 12f), new PdfSolidBrush(Color.Gray), 170, 90);
                page.Canvas.DrawString(NameSplit[0], new PdfFont(PdfFontFamily.Helvetica, 16f, PdfFontStyle.Bold), new PdfSolidBrush(Color.SteelBlue), 170, 110);
                if (NameSplit.Length > 1)
                {
                    page.Canvas.DrawString(NameSplit[1], new PdfFont(PdfFontFamily.Helvetica, 16f, PdfFontStyle.Bold), new PdfSolidBrush(Color.SteelBlue), 170, 140);
                }
                string text = "for successfully completing " + Model;
                PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);
                PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Left);
                format.LineSpacing = 20f;
                format.RightToLeft = true;
                PdfBrush brush = PdfBrushes.Gray;
                PdfTextWidget textWidget = new PdfTextWidget(text, font, brush);
                float y = 0;
                PdfTextLayout textLayout = new PdfTextLayout();
                textLayout.Break = PdfLayoutBreakType.FitPage;
                textLayout.Layout = PdfLayoutType.Paginate;
                SizeF f = new SizeF(260, 200);
                RectangleF bounds = new RectangleF(new PointF(170, 170), f);
                textWidget.StringFormat = format;
                textWidget.Draw(page, bounds, textLayout);
                page.Canvas.DrawString(DateTime.Now.ToString("dd-MMM-yyyy"), new PdfFont(PdfFontFamily.Helvetica, 12f), new PdfSolidBrush(Color.Gray), 170, 240);

                doc.SaveToFile(FileName, FileFormat.PDF);
                FileInfo fs = new FileInfo(FileName);
                return fs;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}

