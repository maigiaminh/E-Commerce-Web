using E_Commerce_Web.Models.Payment;
using E_Commerce_Web.Service.Momo.Command;
using E_Commerce_Web.Service.Momo.Request;
using MediatR;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using E_Commerce_Web.Utilities;

namespace E_Commerce_Web.Controllers
{
    [Route("api/payment")]
    public class PaymentsController : Controller
    {
        private readonly IMediator mediator;

        public PaymentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        [Route("momo-return")]
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
    }
}