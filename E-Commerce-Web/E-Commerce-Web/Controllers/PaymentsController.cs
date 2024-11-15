using E_Commerce_Web.Models.Payment;
using E_Commerce_Web.Service.Momo.Command;
using E_Commerce_Web.Service.Momo.Request;
using MediatR;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using E_Commerce_Web.Utilities;
using E_Commerce_Web.Service.VNPay;
using System.Web;
using System;
using E_Commerce_Web.Models;
using System.Linq;
using System.Diagnostics;

namespace E_Commerce_Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IMediator mediator;
        private readonly VnPayService _vnPayService;
        private readonly EcommerceContext _context;

        public PaymentsController()
        {
            _vnPayService = new VnPayService();
            _context = new EcommerceContext();
        }


        [HttpGet]
        public async Task<ActionResult> MomoReturn(MomoOneTimePaymentResultRequest response)
        {
            string returnUrl = string.Empty;
            var returnModel = new PaymentReturnDtos();
            var processRequest = new ProcessMomoPaymentReturn
            {
                partnerCode = response.partnerCode,
                orderId = response.orderId,
                requestId = response.requestId,
                amount = response.amount,
                orderInfo = response.orderInfo,
                orderType = response.orderType,
                transId = response.transId,
                message = response.message,
                resultCode = response.resultCode,
                payType = response.payType,
                responseTime = response.responseTime,
                extraData = response.extraData,
                signature = response.signature
            };

            var processResult = await mediator.Send(processRequest);

            if (processResult.Success)
            {
                returnModel = processResult.Data.Item1 as PaymentReturnDtos;
                returnUrl = processResult.Data.Item2 as string;
            }

            if (returnUrl.EndsWith("/"))
                returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
            string queryString = returnModel.ToQueryString();

            return Redirect($"{returnUrl}?{queryString}");
        }

        [HttpGet]
        public ActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.QueryString);

            if (response.VnPayResponseCode == "00")
            {
                Debug.WriteLine("DESCRIPTION: " + response.OrderDescription);
                string[] parts = response.OrderDescription.Split(new[] { "ID:" }, StringSplitOptions.None);
                string id = parts[1].Trim();
                int orderID = Convert.ToInt32(id);
                var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderID);

                if (order != null)
                {
                    order.Status = "Delivering";
                    _context.SaveChanges();

                    ViewBag.OrderID = orderID;
                    ViewBag.OrderDescription = response.OrderDescription;
                    ViewBag.Amount = order.TotalAmount;
                    ViewBag.TransactionID = response.TransactionId;
                    ViewBag.PaymentID = response.PaymentId;
                }
                else
                {
                    return RedirectToAction("Index", "Cart");
                }

            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }

    }
}