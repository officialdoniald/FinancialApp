using FinancialApp.BLL.Models;
using FinancialApp.BLL.Properties;
using FinancialApp.DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialApp.Models
{
    public class HomeViewModel
    {
        public List<FinancialItem> FinancialItems { get; set; }

        public List<PartnersCommission> PartnersCommissions { get; set; }
    }
}