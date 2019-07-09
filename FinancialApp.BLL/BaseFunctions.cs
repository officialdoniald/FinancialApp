using FinancialApp.BLL.Models;
using FinancialApp.DAL;
using FinancialApp.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialApp.BLL
{
    public class BaseFunctions
    {
        #region Properties

        private readonly DBContext _dBContext = new DBContext();

        private List<Partner> _team = new List<Partner>();

        #endregion

        #region HelperFunctions

        /// <summary>
        /// Alkalmazás indulásakor lefut, törli az adatokat és generál Partnereket.
        /// </summary>
        public void InitTables()
        {
            DeleteDBContent();

            RandomizePartners();
        }

        /// <summary>
        /// Kigenerál Partnereket, 10db-ot.
        /// </summary>
        public void RandomizePartners()
        {
            Random rnd = new Random();

            List<Partner> partners = new List<Partner>();

            for (int i = 0; i < 10; i++)
            {
                int fee = rnd.Next(1, 21);

                var partner = new Partner()
                {
                    PARTNER_ID = i + 1,
                    PARTNER_NAME = string.Format("Felhasználó {0}", i + 1),
                    FEE_PERCENT = fee
                };

                partners.Add(partner);
            }

            var mockPartners = new List<Partner>();

            foreach (var item in partners)
            {
                mockPartners.Add(new Partner()
                {
                    FEE_PERCENT = item.FEE_PERCENT,
                    PARTNER_ID = item.PARTNER_ID,
                    PARENT_PARTNER_ID = item.PARENT_PARTNER_ID,
                    PARTNER_NAME = item.PARTNER_NAME
                });
            }

            //1. Kiválasztok n db parentet.
            int howmanyparent = rnd.Next(1, mockPartners.Count + 1);

            int[] parentIDs = new int[howmanyparent];

            //2. Randomizálok szülőket, és a PARENT_PARTNER_ID-t 0-ra állítom.
            for (int i = 0; i < howmanyparent; i++)
            {
                int parentID = rnd.Next(1, 11);

                while (parentID == i + 1)
                {
                    parentID = rnd.Next(1, 11);
                }

                parentIDs[i] = parentID;

                var partner = partners.Where(p => p.PARTNER_ID == parentID).FirstOrDefault();

                var deletedMock = mockPartners.Where(m => m.PARTNER_ID == partner.PARTNER_ID).FirstOrDefault();

                mockPartners.Remove(deletedMock);

                partner.PARENT_PARTNER_ID = 0;
            }

            var firstChildren = true;

            //3. Generálok gyerekeket.
            for (int i = 0; i < parentIDs.Length; i++)
            {
                firstChildren = true;

                var stopChildrenGen = 1;

                var parent = partners.Where(p => p.PARTNER_ID == parentIDs[i]).FirstOrDefault();

                while (stopChildrenGen == 1)
                {
                    int howmanychildren = rnd.Next(1, mockPartners.Count + 1);

                    List<decimal> availableIDs = new List<decimal>();

                    foreach (var item in mockPartners)
                    {
                        availableIDs.Add(item.PARTNER_ID);
                    }

                    for (int k = 0; k < howmanychildren; k++)
                    {
                        int childrenid = rnd.Next(1, mockPartners.Count);

                        var partner = partners.Where(p => p.PARTNER_ID == childrenid).FirstOrDefault();

                        mockPartners.Remove(partner);

                        if (!firstChildren)
                        {
                            partner.PARENT_PARTNER_ID = availableIDs[childrenid];
                        }
                        else
                        {
                            partner.PARENT_PARTNER_ID = parent.PARTNER_ID;
                        }
                    }

                    firstChildren = false;

                    stopChildrenGen = rnd.Next(1, 3);
                }
            }

            foreach (var item in partners)
            {
                InsertPartner(item);
            }
        }

        #endregion

        #region CalculateFunctions

        /// <summary>
        /// Kigyűjti a csapatot az egyes Partnereknek.
        /// </summary>
        /// <param name="id"></param>
        public void GetTeam(decimal id, List<Partner> partners)
        {
            var parents = partners.Where(p => p.PARENT_PARTNER_ID == id).ToList();

            foreach (var item in parents)
            {
                _team.Add(item);

                GetTeam(item.PARTNER_ID, partners);
            }
        }

        /// <summary>
        /// Kiszámolja a csapat vásárlását.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public decimal CalculateTeamAmountSum(List<Partner> partner)
        {
            decimal sum = 0;

            foreach (var item in partner)
            {
                foreach (var financial in GetFinancialItemByPARTNERID(item.PARTNER_ID))
                {
                    sum += financial.AMOUNT;
                }
            }

            return sum;
        }

        /// <summary>
        /// Kiszámolja a közvetlen gyerek összértékének és a saját FEE-jének a jutalékát.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public decimal CalculateParnterAmountSum(Partner partner)
        {
            decimal sum = 0;

            foreach (var financial in GetFinancialItemByPARTNERID(partner.PARTNER_ID))
            {
                sum += financial.AMOUNT;
            }

            return sum;
        }

        /// <summary>
        /// Kiszámolja a saját összértéket.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public decimal CalculateOwnPurchase(Partner partner)
        {
            decimal sum = 0;

            var financials = GetFinancialItemByPARTNERID(partner.PARTNER_ID);

            foreach (var item in financials)
            {
                sum += item.AMOUNT;
            }

            return sum * (1 + (partner.FEE_PERCENT / 100));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="partner"></param>
        /// <returns></returns>
        public decimal CalculateChildrenPurchase(List<PartnersCommission> list, PartnersCommission partner)
        {
            decimal sum = 0;

            var nearChildren = partner.Team.Where(p => p.PARENT_PARTNER_ID == partner.PARTNER_ID);

            decimal fee = 0;

            foreach (var item in nearChildren)
            {
                if (item.FEE_PERCENT < partner.PARENT.FEE_PERCENT)
                {
                    fee = partner.PARENT.FEE_PERCENT - item.FEE_PERCENT;

                    sum = list.Where(f => f.PARTNER_ID == item.PARTNER_ID).FirstOrDefault().ALL_PURCHASE;
                }
            }

            return sum * (1 + (fee / 100));
        }

        /// <summary>
        /// Kiszámolja a Jutalékot és a vásárlásokat.
        /// </summary>
        /// <returns></returns>
        public List<PartnersCommission> GetCommissions()
        {
            var partnersCommissionList = new List<PartnersCommission>();

            var partners = GetPartners();
            var financials = GetFinancialItems();

            foreach (var item in partners)
            {
                _team = new List<Partner>();

                GetTeam(item.PARTNER_ID, partners);

                var partner = new PartnersCommission()
                {
                    Team = _team,
                    PARTNER_ID = item.PARTNER_ID,
                    TEAM_PURCHASE = CalculateTeamAmountSum(_team)
                };

                partner.PARENT = item;
                partner.ALL_PURCHASE = partner.TEAM_PURCHASE + CalculateParnterAmountSum(item);
                partner.OWN_PURCHASE = CalculateOwnPurchase(item);

                partnersCommissionList.Add(partner);
            }

            foreach (var item in partnersCommissionList)
            {
                item.CHILDREN_PURCHASE = CalculateChildrenPurchase(partnersCommissionList, item);
                item.COMMISSION = item.CHILDREN_PURCHASE + item.OWN_PURCHASE;
            }

            return partnersCommissionList;
        }

        #endregion

        #region DatabaseFunctions

        /// <summary>
        /// Beszúr egy Partnert.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public int InsertPartner(Partner partner)
        {
            return _dBContext.InsertPartner(partner);
        }

        /// <summary>
        /// Kilistázza a Partnereket.
        /// </summary>
        /// <returns></returns>
        public List<Partner> GetPartners()
        {
            return _dBContext.GetPartners();
        }

        /// <summary>
        /// Beszúr egy FinancialItem-et.
        /// </summary>
        /// <param name="financialItem"></param>
        /// <returns></returns>
        public int InsertFinancialItem(FinancialItem financialItem)
        {
            return _dBContext.InsertFinancialItem(financialItem);
        }

        /// <summary>
        /// Kilistázza a FinancialItem-eket.
        /// </summary>
        /// <returns></returns>
        public List<FinancialItem> GetFinancialItems()
        {
            return _dBContext.GetFinancialItems();
        }

        /// <summary>
        /// Kivesz egy FinancialItem-et.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FinancialItem GetFinancialItemByID(int id)
        {
            return _dBContext.GetFinancialItemByID(id);
        }

        /// <summary>
        /// Kivesz n db FinancialItem-et.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FinancialItem> GetFinancialItemByPARTNERID(decimal id)
        {
            return _dBContext.GetFinancialItemByPARTNERID(id);
        }

        /// <summary>
        /// Frissít egy FinancialItem-et.
        /// </summary>
        /// <param name="financialItem"></param>
        /// <returns></returns>
        public int UpdateFinancialItem(FinancialItem financialItem)
        {
            return _dBContext.UpdateFinancialItem(financialItem);
        }

        /// <summary>
        /// Töröl egy FinancialItem-et.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteFinancialItem(int id)
        {
            return _dBContext.DeleteFinancialItem(id);
        }

        /// <summary>
        /// Törli a FinancialItem-eket.
        /// </summary>
        /// <returns></returns>
        public int DeleteFinancialItems()
        {
            return _dBContext.DeleteFinancialItems();
        }

        /// <summary>
        /// Törli az összes adatot.
        /// </summary>
        /// <returns></returns>
        public int DeleteDBContent()
        {
            return _dBContext.DeleteDBContent();
        }

        #endregion
    }
}