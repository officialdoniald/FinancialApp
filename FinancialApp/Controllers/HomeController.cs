using FinancialApp.BLL;
using FinancialApp.Models;
using System.Web.Mvc;

namespace FinancialApp.Controllers
{
    public class HomeController : Controller
    {
        #region Properties

        private readonly BaseFunctions _baseFunctions = new BaseFunctions();

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult GetFinancials()
        {
            HomeViewModel model = new HomeViewModel();

            var list = _baseFunctions.GetFinancialItems();

            model.FinancialItems = list;

            return View(model);
        }

        [HttpGet]
        public ActionResult CalculateCommission(HomeViewModel model)
        {
            model.PartnersCommissions = _baseFunctions.GetCommissions();

            return View(model);
        }
    }
}