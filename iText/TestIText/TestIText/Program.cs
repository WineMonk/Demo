using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Barcodes;
using iText.Forms.Fields;
using iText.Forms;
using iText.IO.Font.Constants;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Pdf.Xobject;
using iText.Forms.Form.Element;
using iText.Forms.Form;
using System.Text;

namespace TestIText
{
    class Program
    {
        // 定义PDF文件路径
        const string pdfPath = "example.pdf";
        // 定义字体文件路径
        const string fontPath = @"C:\Windows\Fonts\simsun.ttc,0";
        // 创建字体
        static PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

        static void AddTitle(Document document, string title, string destination)
        {
            // 添加标题
            Paragraph paragraph = new Paragraph(title)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(22)
                .SetBold()
                .SetFont(font)
                .SetMarginBottom(20)
                .SetDestination(destination);
            document.Add(paragraph);
            // 添加标签
            PdfOutline rootOutline = document.GetPdfDocument().GetOutlines(false);
            PdfOutline firstSection = rootOutline.AddOutline(title);
            firstSection.AddDestination(PdfDestination.MakeDestination(new PdfString(destination)));
        }

        static void Main()
        {
            // 创建一个PDF writer
            PdfWriter writer = new PdfWriter(pdfPath);
            // 创建一个PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // 创建一个Document
            Document document = new Document(pdf, PageSize.A4);

            AddParagraph(document);

            AddTable(document);

            AddForm(document);

            AddRotatedText(document);

            AddCodeBlock(document);

            AddAnnotation(pdf, document);

            AddExternalLink(document);

            AddInternalLink(document);

            AddLine(document);

            AddQRCode(pdf, document);

            AddImage(document);

            AddList(document);

            //AddTitle(document, "嵌入音频", "audio");
            //PdfDocument pdfDocument = document.GetPdfDocument();
            //string embeddedFileName = "audio.mp3";
            //string embeddedFileDescription = "audio_test";
            //byte[] embeddedFileContentBytes = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "res/audio.mp3");
            //PdfFileSpec spec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDocument, embeddedFileContentBytes,
            //    embeddedFileDescription, embeddedFileName, new PdfName("audio/mpeg"), null, null); ;
            //pdfDocument.AddFileAttachment("embedded_file", spec);

            //PdfFileSpec audioSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdf, AppDomain.CurrentDomain.BaseDirectory + "res/audio.mp3", "audio", new PdfName("pdfName"));
            //pdf.AddFileAttachment("audio", audioSpec);
            //document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            //AddTitle(document, "附件", "file");
            //PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdf, "res/attachment.pdf", "attachment", new PdfName("pdfName"));
            //pdf.AddFileAttachment("file", fileSpec);
            //document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            // 关闭文档
            document.Close();
        }

        private static void AddList(Document document)
        {
            AddTitle(document, "列表", "list");
            List list = new List()
                .SetSymbolIndent(12)
                .SetListSymbol("\u2022")
                .SetFont(font);
            for (int i = 1; i < 6; i++)
            {
                ListItem listItem = new ListItem($"条目 {i}");
                Paragraph paragraph = new Paragraph($"这里是条目 {i} 的内容。\n这里是条目 {i} 的内容。")
                    .SetFont(font)
                    .SetFontSize(12)
                    .SetBackgroundColor(new DeviceRgb(0xEE, 0xF0, 0xF4))
                    .SetPadding(10)
                    .SetBorderRadius(new BorderRadius(5));
                listItem.Add(paragraph);
                list.Add(listItem);
            }
            document.Add(list);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddImage(Document document)
        {
            AddTitle(document, "嵌入图像", "image");
            ImageData imageData = ImageDataFactory.Create("res/image.png");
            Image image = new Image(imageData);
            document.Add(image);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddQRCode(PdfDocument pdf, Document document)
        {
            AddTitle(document, "条形码", "qrCode");
            Paragraph paragraph = new Paragraph("扫描访问网址：https://www.example.com")
                .SetFont(font)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.LEFT);
            document.Add(paragraph);
            BarcodeQRCode qrCode = new BarcodeQRCode("https://www.example.com");
            PdfFormXObject barcodeObject = qrCode.CreateFormXObject(ColorConstants.BLACK, pdf);
            Image barcodeImage = new Image(barcodeObject);
            barcodeImage.SetWidth(100);
            barcodeImage.SetHeight(100);
            document.Add(barcodeImage);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddLine(Document document)
        {
            AddTitle(document, "线条", "line");
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddInternalLink(Document document)
        {
            AddTitle(document, "内部链接", "internalLink");
            PdfAction action = PdfAction.CreateGoTo("externalLink");
            Link internalLink = new Link("跳转到 -> 外部连接", action);
            Paragraph internalLinkParagraph = new Paragraph("这是一个带有内部链接的段落： ").SetFont(font).Add(internalLink);
            document.Add(internalLinkParagraph);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddExternalLink(Document document)
        {
            AddTitle(document, "外部链接", "externalLink");
            Link externalLink = new Link("访问网站", PdfAction.CreateURI("https://www.example.com"));
            Paragraph linkParagraph = new Paragraph("这是一个带有外部链接的段落： ").SetFont(font).Add(externalLink);
            document.Add(linkParagraph);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddAnnotation(PdfDocument pdf, Document document)
        {
            AddTitle(document, "注释", "annotation");
            PdfAnnotation annotation = new PdfTextAnnotation(new Rectangle(100, 600, 0, 0))
                .SetOpen(true)
                .SetTitle(new PdfString("iText"))
                .SetContents(new PdfString("annotation content..."));
            pdf.GetLastPage().AddAnnotation(annotation);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddCodeBlock(Document document)
        {
            AddTitle(document, "代码块", "codeBlock");
            Paragraph codeBlock = new Paragraph("public static void main(String[] args) \n{\n\tSystem.out.println(\"Hello, World!\");\n}")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.COURIER))
                .SetFontSize(12)
                .SetBackgroundColor(new DeviceRgb(0xEE, 0xF0, 0xF4))
                .SetPadding(10)
                .SetBorderRadius(new BorderRadius(5));
            document.Add(codeBlock);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddRotatedText(Document document)
        {
            AddTitle(document, "旋转文本", "rotatedText");
            Paragraph rotatedText = new Paragraph("旋转的文本")
                .SetRotationAngle(Math.PI / 4)
                .SetFont(font)
                .SetFontSize(12);
            document.Add(rotatedText);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddForm(Document document)
        {
            AddTitle(document, "表单", "form");
            PdfDocument pdfDoc = document.GetPdfDocument();
            PdfFormField personal = new NonTerminalFormFieldBuilder(pdfDoc, "personal")
                .CreateNonTerminalFormField();
            PdfTextFormField name = new TextFormFieldBuilder(pdfDoc, "name")
                .SetWidgetRectangle(new Rectangle(36, 760, 108, 30)).CreateText();
            name.SetValue("");
            personal.AddKid(name);
            PdfTextFormField password = new TextFormFieldBuilder(pdfDoc, "password")
                .SetWidgetRectangle(new Rectangle(150, 760, 300, 30)).CreateText();
            password.SetValue("");
            personal.AddKid(password);
            PdfFormCreator.GetAcroForm(pdfDoc, true).AddField(personal, pdfDoc.GetLastPage());


            //PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);
            //ChoiceFormFieldBuilder choiceFormFieldBuilder = new ChoiceFormFieldBuilder(document.GetPdfDocument(), "listBox");
            //choiceFormFieldBuilder.SetOptions(new string[] { "item1", "item2", "item3" });
            //PdfChoiceFormField pdfChoiceFormField = choiceFormFieldBuilder.CreateComboBox();
            //form.AddField(pdfChoiceFormField);

            //TextArea flattenTextArea = new TextArea("表单");
            //flattenTextArea.SetInteractive(true);
            //flattenTextArea.SetProperty(FormProperty.FORM_FIELD_VALUE, "Hello iText!\nHello iText!\nHello iText!");
            //flattenTextArea.SetProperty(Property.HEIGHT, new UnitValue(UnitValue.POINT, 100));
            //flattenTextArea.SetProperty(Property.BORDER, new SolidBorder(2f));
            //document.Add(flattenTextArea);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddTable(Document document)
        {
            AddTitle(document, "表格", "table");
            // 创建表格（3 列）
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 }))
                .UseAllAvailableWidth();
            // 添加表头
            table.AddHeaderCell(new Cell().Add(new Paragraph("表头 1").SetFont(font)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("表头 2").SetFont(font)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("表头 3").SetFont(font)));
            // 添加五行数据
            for (int i = 1; i <= 5; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph("单元格 " + i + ", 1").SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph("单元格 " + i + ", 2").SetFont(font)));
                table.AddCell(new Cell().Add(new Paragraph("单元格 " + i + ", 3").SetFont(font)));
            }
            document.Add(table);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddParagraph(Document document)
        {
            AddTitle(document, "段落", "paragraph");
            Paragraph paragraph = new Paragraph("这是一个段落。")
                .SetFont(font)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.LEFT);
            document.Add(paragraph);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }
    }
}
