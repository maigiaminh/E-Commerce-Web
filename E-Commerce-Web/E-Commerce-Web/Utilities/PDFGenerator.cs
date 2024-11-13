using E_Commerce_Web.Models;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Utilities
{
    public class PDFGenerator
    {
        public static byte[] GenerateOrderPdf(Order order)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);


                document.SetMargins(20, 20, 20, 20);

                Paragraph header = new Paragraph($"Order Summary\nOrder ID: {order.OrderID}")
                    .SetFontSize(20)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20);
                document.Add(header);

                Paragraph customerInfo = new Paragraph($"Name: {order.RecipientName}\nPhone: {order.RecipientPhone}\nAddress: {order.RecipientAddress}\nOrder Date: {order.OrderDate.ToShortDateString()}")
                    .SetFontSize(12)
                    .SetMarginBottom(20);
                document.Add(customerInfo);

                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4, 1, 1, 1 })).UseAllAvailableWidth();
                table.SetMarginBottom(20);

                table.AddHeaderCell(new Cell().Add(new Paragraph("No").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Product Name").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Size").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantity").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Price").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                int itemCount = 1;
                foreach (var item in order.OrderDetails)
                {
                    table.AddCell(new Cell().Add(new Paragraph(itemCount.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(item.Product.ProductName)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Size.Name)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph($"${item.Product.Price:F2}")));

                    itemCount++;
                }

                document.Add(table);

                Paragraph total = new Paragraph($"Subtotal: ${order.Subtotal:F2}\nShipping: ${order.ShippingFee:F2}\nDiscount: ${order.Discount:F2}\nTotal Amount: ${order.TotalAmount:F2}")
                    .SetFontSize(12)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginTop(10);
                document.Add(total);

                Paragraph footer = new Paragraph("Thank you for shopping with us!")
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(20);
                document.Add(footer);
                document.Close();

                return ms.ToArray();
            }
        }
    }
}