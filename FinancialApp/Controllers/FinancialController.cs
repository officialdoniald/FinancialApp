using FinancialApp.BLL;
using FinancialApp.DAL.Model;
using FinancialApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialApp.Controllers
{
    public class FinancialController : Controller
    {
        private readonly BaseFunctions _baseFunctions = new BaseFunctions();

        [HttpGet]
        public ActionResult UploadFinancial()
        {
            var list = _baseFunctions.GetPartners();

            FinancialViewModel model = new FinancialViewModel
            {
                PartnerList = new List<SelectListItem>(list.Select(x => new SelectListItem {
                    Text = x.PARTNER_NAME, Value = x.PARTNER_ID.ToString()
                })),
                Date = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveFinancialItem(FinancialViewModel model)
        {
            if (ModelState.IsValid)
            {
                FinancialItem financialItem = new FinancialItem()
                {
                    PARTNER_ID = model.SelectedPartnerID,
                    AMOUNT = Convert.ToDecimal(model.Amount),
                    DATE = model.Date
                };

                int success = _baseFunctions.InsertFinancialItem(financialItem);

                if (success == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("UploadFinancial", model);
        }

        [HttpPost]
        public ActionResult DeleteFinancial(int id)
        {
            var response = _baseFunctions.DeleteFinancialItem(id);

            return Json(new { success = response == 1 });
        }

        [HttpPost]
        public ActionResult DeleteAllFinancial()
        {
            var response = _baseFunctions.DeleteFinancialItems();

            return Json(new { success = response != 0 });
        }

        [HttpGet]
        public ActionResult UpdateFinancial(int id)
        {
            var financialItem = _baseFunctions.GetFinancialItemByID(id);

            FinancialViewModel model = new FinancialViewModel()
            {
                ID = financialItem.ID,
                Amount = financialItem.AMOUNT.ToString(),
                Date = financialItem.DATE,
                PartnerList = _baseFunctions.GetPartners().Select(p => new SelectListItem
                {
                    Text = p.PARTNER_NAME,
                    Value = p.PARTNER_ID.ToString()
                }).ToList(),
                SelectedPartnerID = financialItem.PARTNER_ID
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateFinancialItem(FinancialViewModel model)
        {
            if (ModelState.IsValid)
            {
                FinancialItem financialItem = new FinancialItem()
                {
                    ID = model.ID,
                    PARTNER_ID = model.SelectedPartnerID,
                    AMOUNT = Convert.ToDecimal(model.Amount),
                    DATE = model.Date
                };

                int success = _baseFunctions.UpdateFinancialItem(financialItem);

                if (success == 1)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }

            return View();
        }
    }
}