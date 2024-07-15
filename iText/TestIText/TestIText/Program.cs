using iText.Barcodes;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace TestIText
{
    class Program
    {
        // 定义PDF文件路径
        const string pdfPath = "example.pdf";
        // 定义字体文件路径
        const string sunFontPath = @"C:\Windows\Fonts\simsun.ttc,0";
        const string heiFontPath = @"C:\Windows\Fonts\simhei.ttf";
        // 定义图片路径
        const string imagePath = @"res\image.png";
        const string cover1Path = @"res\c1.png";
        const string cover2Path = @"res\c2.png";
        const string cover3Path = @"res\c3.png";
        // 定义PDF背景路径
        const string backgroundPath = @"res\background.jpg";
        // 创建字体
        static PdfFont heiFont = PdfFontFactory.CreateFont(heiFontPath, PdfEncodings.IDENTITY_H);
        static PdfFont sunFont = PdfFontFactory.CreateFont(sunFontPath, PdfEncodings.IDENTITY_H);

        static void AddTitle(Document document, string title, string destination)
        {
            // 添加标题
            Paragraph paragraph = new Paragraph(title)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(22)
                .SetBold()
                .SetFont(heiFont)
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

            // 自动添加页码
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler());

            AddParagraph(document);

            AddTable(document);

            AddRotatedText(document);

            AddCodeBlock(document);

            AddAnnotation(pdf, document);

            AddExternalLink(document);

            AddInternalLink(document);

            AddLine(document);

            AddQRCode(pdf, document);

            AddImage(document);

            AddList(document);

            AddBackground(pdf, document);

            AddPageHeader(pdf, document);

            AddPageFooter(pdf, document);

            AddWatermark(pdf, document);

            AddSubfield(pdf, document);

            // 关闭文档
            document.Close();
        }

        private static void AddSubfield(PdfDocument pdf, Document document)
        {
            AddTitle(document, "分栏", "subfield");
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            float offSet = 36;
            PdfPage page = pdf.GetLastPage();
            Rectangle ps = page.GetPageSize();
            float columnWidth = (ps.GetWidth() - offSet * 2 + 10) / 3;
            float columnHeight = ps.GetHeight() - offSet * 2 - 40;
            Rectangle[] columns = new Rectangle[] {
                new Rectangle(offSet - 5, offSet, columnWidth, columnHeight),
                new Rectangle(offSet + columnWidth, offSet, columnWidth, columnHeight),
                new Rectangle(offSet + columnWidth * 2 + 5, offSet, columnWidth, columnHeight)
            };
            document.SetRenderer(new ColumnDocumentRenderer(document, columns));
            Image conver1 = new Image(ImageDataFactory.Create(cover1Path)).SetWidth(columnWidth);
            AddArticle(
                document,
                "硅基流动完成近亿元融资：加速生成式AI技术普惠进程",
                "作者：OneFlow_Official, 2024-07-05",
                conver1,
                "硅基流动（SiliconFlow）近日完成总金额近亿元人民币的天使+轮融资。本轮融资由某知名产业方领投，跟投方包括智谱AI、360 和水木清华校友基金等知名企业及机构，老股东耀途资本继续超额跟进，华兴资本担任独家财务顾问。\r\n\r\n本轮融资不仅是对硅基流动技术实力和市场前景的高度认可，也将为其未来发展提供强劲动力。创始人兼 CEO 袁进辉表示：“非常感谢各位投资方对硅基流动的信任和支持。这次融资将帮助我们进一步加快产品创新，为开发者提供触手可及的 AI 云服务，促进 AI 应用层的繁荣，推动 AGI 技术普惠化。”近两年，生成式 AI 和大模型技术爆发，使得 AI 基础设施（AI Infra）成为市场的关键一环。\r\n\r\n根据 Gartner 的报告，2023 年专用于 AI 工作负载的芯片创造近 534 亿美元的收入，占据了AGI产业中的大部分价值。而未来随着模型提升、架构改进和定制芯片等降本提效措施的实施，AI 应用的盈利能力将逐步提高，AI 应用层的价值将逐步显现，在这一进程中，离开发者最近的 AI Infra 生态位具备显著优势。\r\n \r\n\r\nAI Infra天然提供了应用开发者与硬件、模型之间的桥梁，不仅能提升开发效率和释放创新能力，还有效应对了市场对高性能和低成本 AI 解决方案的强烈需求。\r\n \r\n\r\n历史一再证明，“得开发者得天下”，最接近用户的生态位能够最快响应市场变化和用户需求。硅基流动就瞄准了AI Infra 生态位的机会，通过技术创新，大幅降低了 AI 应用的开发和使用门槛，凭借创新的技术和领先的产品，迅速崛起，成为 AI Infra 领域的重要玩家。");

            Image conver2 = new Image(ImageDataFactory.Create(cover2Path)).SetWidth(columnWidth);
            AddArticle(
                document,
                "国产大模型新标杆！比肩GPT4，DeepSeek V2重磅升级",
                "作者：OneFlow_Official, 2024-07-03",
                conver2,
                "近日，深度求索团队更新了DeepSeek-V2模型，新版本DeepSeek-V2-Chat模型推理能力有了极大提升。尤其在数学解题、逻辑推理、编程、指令跟随、Json格式输出不同维度上，最高有16%的性能提升。\r\n\r\n在Arena-Hard测评中，DeepSeek-V2-Chat与GPT-4-0314的对战胜率从41.6%提升到了68.3%。DeepSeek-V2-Chat模型的角色扮演能力显著增强，可以在对话中按要求扮演不同角色。\r\n\r\n此外，深度求索团队对DeepSeek-V2-Chat的“system”区域指令跟随能力进行了优化，显著增强了沉浸式翻译、RAG 等任务的用户体验。短短半年，深度求索团队的进步堪称神速。\r\n\r\n今年1月，他们开源了国内首个MoE模型，随后在5月初发布了最强开源MoE模型DeepSeek-V2，6月中旬，他们发布了代码生成能力超越GPT4-Turbo的DeepSeek Coder V2。\r\n\r\n这一次，DeepSeek-V2-Chat在各方面或比肩GPT4，至少是国产大模型的新标杆。\r\n\r\n据官方此前介绍，DeepSeek-V2系列模型采用了全新的模型结构。DeepSeek V2没有沿用主流的“类LLaMA的Dense结构”和“类Mistral的Sparse结构”，而是对模型框架进行了全方位的创新，提出了媲美MHA的MLA（Multi-head Latent Attention）架构，大幅减少计算量和推理显存；自研Sparse结构DeepSeekMoE进一步将计算量降低到极致，两者结合最终实现模型性能跨级别的提升。");

            Image conver3 = new Image(ImageDataFactory.Create(cover3Path)).SetWidth(columnWidth);
            AddArticle(
                document,
                "CVPR最佳论文：谷歌基于Spectral Volume从单图生成视频",
                "作者：AI记忆, 2024-06-21",
                conver3,
                "本文提出了一种新颖的方法来模拟场景运动的图像空间先验。通过从真实视频序列中提取的自然振荡动态（如树木、花朵、蜡烛和衣物随风摆动）学习运动轨迹，作者将长期运动建模为傅里叶域中的频谱体积。给定单张图片，训练好的模型使用频率协调的扩散采样过程预测频谱体积，进而转换为整个视频的运动纹理。结合基于图像的渲染模块，预测的运动表示可以用于多种应用，例如将静态图像转换为无缝循环视频，或允许用户与真实图像中的对象进行交互，产生逼真的模拟动态。");
        }

        public static void AddArticle(Document doc, string title, string author, Image img, string text)
        {
            Paragraph p1 = new Paragraph(title)
                .SetFont(heiFont)
                .SetFontSize(14);
            doc.Add(p1);
            doc.Add(img);
            Paragraph p2 = new Paragraph()
                .SetFont(heiFont)
                .SetFontSize(7)
                .SetFontColor(ColorConstants.GRAY)
                .Add(author);
            doc.Add(p2);
            Paragraph p3 = new Paragraph()
                .SetFont(sunFont)
                .SetFontSize(10)
                .Add(text);
            doc.Add(p3);
        }

        private static void AddWatermark(PdfDocument pdf, Document document)
        {
            PdfPage page = pdf.GetLastPage();
            Rectangle pageSize = page.GetPageSize();
            // 添加水印文本
            Canvas canvas = new Canvas(page, pageSize);
            Paragraph watermark = new Paragraph("WineMonk")
                .SetFontSize(50)
                .SetFontColor(ColorConstants.LIGHT_GRAY)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER);

            float xStep = pageSize.GetWidth() / 3;
            float yStep = pageSize.GetHeight() / 5;

            for (float x = xStep / 2; x < pageSize.GetWidth(); x += xStep)
            {
                for (float y = yStep / 2; y < pageSize.GetHeight(); y += yStep)
                {
                    canvas.ShowTextAligned(
                        watermark,
                        x, y,
                        pdf.GetPageNumber(page),
                        TextAlignment.CENTER,
                        VerticalAlignment.MIDDLE,
                        0.8f);
                }
            }
            canvas.Close();
            AddTitle(document, "水印", "watermark");
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddPageFooter(PdfDocument pdf, Document document)
        {
            AddTitle(document, "页脚", "pageFooter");
            PdfPage page = pdf.GetLastPage();
            Rectangle pageSize = page.GetPageSize();
            // 添加页脚文本
            Canvas canvas = new Canvas(page, pageSize);
            Paragraph footer = new Paragraph("这里是页脚~~~")
                .SetFontSize(10)
                .SetFont(heiFont)
                .SetTextAlignment(TextAlignment.CENTER);
            canvas.ShowTextAligned(footer,
                pageSize.GetWidth() / 2, pageSize.GetBottom() + 20, TextAlignment.CENTER);
            canvas.Close();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddPageHeader(PdfDocument pdf, Document document)
        {
            AddTitle(document, "页眉", "pageHeader");
            PdfPage page = pdf.GetLastPage();
            Rectangle pageSize = page.GetPageSize();
            // 添加页眉文本
            Canvas canvas = new Canvas(page, pageSize);
            canvas.ShowTextAligned(new Paragraph("这里是页眉~~~").SetFont(heiFont),
                pageSize.GetWidth() / 2, pageSize.GetTop() - 25, TextAlignment.CENTER);
            canvas.Close();
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddBackground(PdfDocument pdf, Document document)
        {
            // 加载背景图片
            ImageData imageData = ImageDataFactory.Create(backgroundPath);
            Image background = new Image(imageData);
            // 设置背景图片的尺寸和位置
            background.SetFixedPosition(0, 0);
            background.SetWidth(pdf.GetDefaultPageSize().GetWidth());
            background.SetHeight(pdf.GetDefaultPageSize().GetHeight());
            PdfPage page = pdf.GetLastPage();
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.AddImageFittedIntoRectangle(imageData, new Rectangle(0, 0, page.GetPageSize().GetWidth(), page.GetPageSize().GetHeight()), true);
            AddTitle(document, "设置背景", "background");
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddList(Document document)
        {
            AddTitle(document, "列表", "list");
            List list = new List()
                .SetSymbolIndent(12)
                .SetListSymbol("·")
                .SetFont(heiFont);
            for (int i = 1; i < 6; i++)
            {
                ListItem listItem = new ListItem($"条目 {i}");
                Paragraph paragraph = new Paragraph($"这里是条目 {i} 的内容。\n这里是条目 {i} 的内容。")
                    .SetFont(heiFont)
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
            ImageData imageData = ImageDataFactory.Create(imagePath);
            Image image = new Image(imageData);
            document.Add(image);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddQRCode(PdfDocument pdf, Document document)
        {
            AddTitle(document, "二维码", "qrCode");
            Paragraph paragraph = new Paragraph("扫描访问网址：https://www.example.com")
                .SetFont(heiFont)
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
            LineSeparator ls1 = new LineSeparator(new DashedLine());
            LineSeparator ls2 = new LineSeparator(new DottedLine());

            document.Add(ls);
            document.Add(new Paragraph("\n"));
            document.Add(ls1);
            document.Add(new Paragraph("\n"));
            document.Add(ls2);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddInternalLink(Document document)
        {
            AddTitle(document, "内部链接", "internalLink");
            PdfAction action = PdfAction.CreateGoTo("externalLink");
            Text internalLink = new Link("跳转到 -> 外部连接", action).SetFontColor(ColorConstants.BLUE).SetUnderline();
            Paragraph internalLinkParagraph = new Paragraph("这是一个带有内部链接的段落： ").SetFont(heiFont).Add(internalLink);
            document.Add(internalLinkParagraph);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddExternalLink(Document document)
        {
            AddTitle(document, "外部链接", "externalLink");
            Text externalLink = new Link("访问网站", PdfAction.CreateURI("https://www.example.com")).SetFontColor(ColorConstants.BLUE).SetUnderline();
            Paragraph linkParagraph = new Paragraph("这是一个带有外部链接的段落： ").SetFont(heiFont).Add(externalLink);
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
            Paragraph codeBlock = new Paragraph()
                .Add(new Text("public static void main(String[] args)\n{\n"))
                .Add(new Tab())
                .Add(new Text("System.out.println(\"Hello, World!\");"))
                .Add(new Text("\n}"))
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
                .SetFont(heiFont)
                .SetFontSize(12);
            document.Add(rotatedText);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddTable(Document document)
        {
            AddTitle(document, "表格", "table");
            // 创建表格（3 列）
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 }))
                .UseAllAvailableWidth();
            // 添加表头
            table.AddHeaderCell(new Cell().Add(new Paragraph("表头 1").SetFont(heiFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("表头 2").SetFont(heiFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("表头 3").SetFont(heiFont)));
            // 添加五行数据
            for (int i = 1; i <= 5; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph("单元格 " + i + ", 1").SetFont(heiFont)));
                table.AddCell(new Cell().Add(new Paragraph("单元格 " + i + ", 2").SetFont(heiFont)));
                table.AddCell(new Cell().Add(new Paragraph("单元格 " + i + ", 3").SetFont(heiFont)));
            }
            document.Add(table);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private static void AddParagraph(Document document)
        {
            AddTitle(document, "段落", "paragraph");
            Paragraph paragraph = new Paragraph("这是一个段落。")
                .SetFont(heiFont)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.LEFT);
            document.Add(paragraph);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        public class FooterEventHandler : IEventHandler
        {
            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();
                // 添加页脚文本
                Canvas canvas = new Canvas(page, pageSize);
                int pageNumber = pdfDoc.GetPageNumber(page);
                Paragraph footer = new Paragraph($"第 {pageNumber} 页")
                    .SetFont(heiFont)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT);
                canvas.ShowTextAligned(footer,
                    pageSize.GetWidth() - 20, pageSize.GetBottom() + 20, TextAlignment.RIGHT);
                canvas.Close();
            }
        }
    }
}
