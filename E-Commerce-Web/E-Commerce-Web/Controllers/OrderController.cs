using E_Commerce_Web.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iText.Layout;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using E_Commerce_Web.Utilities;
using E_Commerce_Web.Service.Momo.Request;
using E_Commerce_Web.Utilities.Momo.Config;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration;
using E_Commerce_Web.Models.Result;
using E_Commerce_Web.Service.VNPay;

namespace E_Commerce_Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly VnPayService _vnPayService;

        public OrderController()
        {
            _context = new EcommerceContext();
            _vnPayService = new VnPayService();

        }

        public ActionResult OrderDetails(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == id);

            if (order == null || order.UserID != Convert.ToInt32(Session["UserID"]))
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult OrderCreate(string paymentMethod, string RecipientName, string RecipientPhone, string Address, string Note, decimal Discount, decimal Shipping, decimal Total, decimal Subtotal)
        {
            Debug.WriteLine("OK");
            if (string.IsNullOrEmpty(paymentMethod))
            {
                ModelState.AddModelError("", "Please select a payment method.");
                return RedirectToAction("Index", "Cart");
            }

            List<CartItem> cartItems = Session["Cart"] as List<CartItem>;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var newOrder = new Order
                    {
                        UserID = Convert.ToInt32(Session["UserID"]),
                        OrderDate = DateTime.Now,
                        Discount = Discount,
                        ShippingFee = Shipping,
                        TotalAmount = Total,
                        Subtotal = Subtotal,
                        PaymentMethod = paymentMethod,
                        Status = paymentMethod == "Cash On Delivery (COD)" ? "Delivering" : "Confirmed",
                        RecipientName = RecipientName,
                        RecipientPhone = RecipientPhone,
                        RecipientAddress = Address,
                        Note = Note
                    };

                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        var product = _context.Products.Find(item.ProductId);
                        var productSize = _context.ProductSizes.FirstOrDefault(ps => ps.ProductID == item.ProductId && ps.SizeID == item.SizeID);

                        if (product != null && product.Stock >= item.Quantity)
                        {
                            product.Stock -= item.Quantity;
                        }
                        else
                        {
                            throw new Exception("Insufficient stock for product: " + item.ProductName);
                        }

                        if (productSize != null && productSize.Stock >= item.Quantity)
                        {
                            productSize.Stock -= item.Quantity;
                        }
                        else
                        {
                            throw new Exception("Insufficient stock for product size: " + item.ProductName + " - " + item.Size);
                        }

                        var orderDetail = new OrderDetail
                        {
                            OrderID = newOrder.OrderID,
                            ProductID = item.ProductId,
                            SizeID = item.SizeID,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price
                        };

                        _context.OrderDetails.Add(orderDetail);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    if(paymentMethod == "Momo E-wallet")
                    {
                        var result = new BaseResultWithData<PaymentLinkDtos>();
                        var paymentContent = "THANH TOAN DON HANG " + newOrder.OrderID;
                        var paymentUrl = "";
                        var requestId = newOrder.OrderID.ToString();
                        var orderID = "MOMO" + newOrder.OrderID.ToString();
                        var amount = (long)Math.Round(newOrder.TotalAmount * 25345);

                        Debug.WriteLine("AMOUNT :" + amount + "\nrequestId: " + requestId + "\norderID" + orderID);
                        var momoOneTimePayRequest = new MomoOneTimePaymentRequest(
                                ConfigurationManager.AppSettings["Momo:PartnerCode"],
                                requestId,
                                amount,
                                orderID,
                                paymentContent,
                                ConfigurationManager.AppSettings["Momo:ReturnUrl"],
                                ConfigurationManager.AppSettings["Momo:IpnUrl"],
                                "captureWallet",
                                string.Empty
                            );

                        momoOneTimePayRequest.MakeSignature(
                            ConfigurationManager.AppSettings["Momo:AccessKey"],
                            ConfigurationManager.AppSettings["Momo:SecretKey"]);

                        Debug.WriteLine("Order ID: " + newOrder.OrderID);
                        (bool createMomoLinkResult, string createMessage) = momoOneTimePayRequest.GetLink(ConfigurationManager.AppSettings["Momo:PaymentUrl"]);
                        if (createMomoLinkResult)
                        {
                            paymentUrl = createMessage;
                            TempData["PaymentUrl"] = paymentUrl;

                            Debug.WriteLine("create sucessfully" + createMessage);

                        }
                        else
                        {
                            result.Message = createMessage;
                            Debug.WriteLine("error" + createMessage);

                            return RedirectToAction("Index", "Cart");
                        }

                        result.Set(true, "OK", new PaymentLinkDtos()
                        {
                            PaymentId = "01",
                            PaymentUrl = paymentUrl,
                        });
        
                    }

                    else if (paymentMethod == "VNPay E-wallet")
                    {
                        Debug.WriteLine("VN PAY NE");
                        double amount = (double)Math.Round(newOrder.TotalAmount * 25345);

                        Debug.WriteLine("AMOUNT " + amount);
                        PaymentInformationModel model = new PaymentInformationModel()
                        {
                            OrderType = "other",
                            Amount = amount,
                            OrderDescription = newOrder.OrderID.ToString(),
                            Name = newOrder.User.FullName
                        };

                        Debug.WriteLine("service vnpay" + _vnPayService);
                        Debug.WriteLine("model vnpay" + model);

                        var url = _vnPayService.CreatePaymentUrl(model, HttpContext.Request);

                        Debug.WriteLine("url " + url);
                        return Redirect(url);
                    }

                    Session.Remove("Discount");
                    Session.Remove("CouponCode");
                    Session.Remove("Cart");

                    return RedirectToAction("OrderDetails", new { id = newOrder.OrderID });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Error processing your order: " + ex.Message);
                    Debug.WriteLine("error" + ex.Message);

                    return RedirectToAction("Index", "Cart");
                }
            }
        }

        public ActionResult PrintSelectedOrderPdf(int orderId)
        {

            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null || order.UserID != Convert.ToInt32(Session["UserID"]))
            {
                return HttpNotFound();
            }

            var pdfBytes = PDFGenerator.GenerateOrderPdf(order);
            var filename = "OrderDetail" + orderId + ".pdf";
            return File(pdfBytes, "application/pdf", filename);
        }
    }
}