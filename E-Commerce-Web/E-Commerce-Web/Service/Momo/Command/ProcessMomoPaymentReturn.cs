using E_Commerce_Web.Models.Result;
using E_Commerce_Web.Models;
using E_Commerce_Web.Service.Momo.Request;
using E_Commerce_Web.Utilities.Momo.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Management;
using MediatR;
using E_Commerce_Web.Models.Payment;
using iText.Layout.Borders;

namespace E_Commerce_Web.Service.Momo.Command
{
    public class ProcessMomoPaymentReturn : MomoOneTimePaymentResultRequest,
        IRequest<BaseResultWithData<(PaymentReturnDtos, string)>>
    {
    }
    
    public class ProcessMomoPaymentReturnHandler
        : IRequestHandler<ProcessMomoPaymentReturn, BaseResultWithData<(PaymentReturnDtos, string)>>
    {

        private readonly EcommerceContext _context;
        private readonly MomoConfig momoConfig;

        public ProcessMomoPaymentReturnHandler(
            IOptions<MomoConfig> momoConfigOptions)
        {
            this._context = new EcommerceContext();
            this.momoConfig = momoConfigOptions.Value;
        }

        public Task<BaseResultWithData<(PaymentReturnDtos, string)>> Handle(ProcessMomoPaymentReturn request, CancellationToken cancellationToken)
        {
            string returnUrl = string.Empty;
            var result = new BaseResultWithData<(PaymentReturnDtos, string)>();

            try
            {
                var resultData = new PaymentReturnDtos();
                var isValidSignature = request.IsValidSignature(momoConfig.AccessKey, momoConfig.SecretKey);

                if (isValidSignature)
                {
                    int orderId = Convert.ToInt32(request.orderId);
                    var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);

                    if (order != null)
                    {
                        if (request.resultCode == 0)
                        {
                            resultData.PaymentStatus = "00";
                            resultData.PaymentId = orderId.ToString();
                            resultData.Signature = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            resultData.PaymentStatus = "10";
                            resultData.PaymentMessage = "Payment process failed";
                        }

                        result.Success = true;
                        result.Message = "OK";
                        result.Data = (resultData, returnUrl);
                    }
                    else
                    {
                        resultData.PaymentStatus = "11";
                        resultData.PaymentMessage = "Can't find payment at payment service";
                    }
                }
                else
                {
                    resultData.PaymentStatus = "99";
                    resultData.PaymentMessage = "Invalid signature in response";
                }
            }
            catch (Exception ex)
            {
                result.Set(false, "Error");
                result.Errors.Add(new BaseError()
                {
                    Code = "Exception",
                    Message = ex.Message,
                });
            }

            return Task.FromResult(result);
        }
    }
    
}