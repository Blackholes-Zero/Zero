using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public class ProductInfoToClass
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public int ProductID { get; set; }
        public int BusinessID { get; set; }
    }
}