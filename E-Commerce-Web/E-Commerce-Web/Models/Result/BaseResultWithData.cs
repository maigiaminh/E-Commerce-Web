using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Models
{
    public class BaseResultWithData<T> : BaseResult
    {
        public T Data { get; set; }

        public void Set(bool success, string message, T data)
        {
            this.Data = data;
            this.Success = success;
            this.Message = message;
        }
    }
}