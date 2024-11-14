using E_Commerce_Web.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Utilities
{
    public static class QueryStringExtensions
    {
        public static string ToQueryString(this PaymentReturnDtos dto)
        {
            var properties = dto.GetType().GetProperties();
            var queryString = string.Empty;

            foreach (var prop in properties)
            {
                var value = prop.GetValue(dto, null);
                if (value != null)
                {
                    queryString += $"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(value.ToString())}&";
                }
            }

            return queryString.TrimEnd('&');
        }
    }
}