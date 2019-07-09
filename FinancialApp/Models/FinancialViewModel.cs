using FinancialApp.BLL.Properties;
using FinancialApp.DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialApp.Models
{
    public class FinancialViewModel
    {
        [Required(ErrorMessageResourceName = "MennyisegErrorMessage", ErrorMessageResourceType = typeof(ErrorMessageResource), AllowEmptyStrings = false)]
        public string Amount { get; set; }

        public DateTime Date { get; set; }

        public decimal SelectedPartnerID { get; set; }

        public List<SelectListItem> PartnerList { get; set; }

        public int ID { get; set; }
    }
}