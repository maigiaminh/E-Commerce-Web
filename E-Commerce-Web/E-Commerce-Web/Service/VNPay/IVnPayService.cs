using E_Commerce_Web.Service.VNPay.Response;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace E_Commerce_Web.Service.VNPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpRequestBase context);

        PaymentResponseModel PaymentExecute(NameValueCollection collections);

    }
}
