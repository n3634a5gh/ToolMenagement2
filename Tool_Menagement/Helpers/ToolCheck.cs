using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tool_Menagement.Models;

namespace Tool_Menagement.Helpers
{
    public class ToolCheck
    {
        public List<int> UpdateToolStatus(ToolsBaseContext _context, int order_Id)
        {
            List<int> pozycjeMagazynowe = new List<int>();
            var disableOrderTools = new Zlecenie_Available_Tools();

            var active_tools = _context.Magazyns
                .Where(magazyn => magazyn.Wycofany == false)
                .Where(magazyn => magazyn.Regeneracja == false)
                .Where(magazyn => magazyn.Uzycie > 0)
                .Where(magazyn => magazyn.Trwalosc > 0)
                .ToArray();

            foreach(var item in active_tools)
            {
                if(item.Uzycie>=item.Trwalosc)
                {
                    int catecory_id=_context.Narzedzies
                        .Where(n=>n.IdNarzedzia==item.IdNarzedzia)
                        .Select(n=>n.IdKategorii)
                        .FirstOrDefault();

                    int tool_policy = _context.Kategoria
                        .Where(n => n.IdKategorii == catecory_id)
                        .Select(n => n.ToolPolicy)
                        .FirstOrDefault();

                    if(tool_policy==1)
                    {
                        item.Wycofany = true;
                    }
                    else
                    {
                        item.Regeneracja = true;
                    }
                    _context.Magazyns.Update(item);
                    _context.SaveChanges();

                    pozycjeMagazynowe.Add(item.PozycjaMagazynowa);

                    var disableOrder = _context.Zlecenies
                        .Where(n => n.IdZlecenia == order_Id)
                        .FirstOrDefault();
                    disableOrder.Aktywne = false;
                    _context.Zlecenies.Update(disableOrder);
                    _context.SaveChanges();

                    disableOrderTools.Close_Zlecenie_ID_Magazyn(_context, order_Id);
                }
            }
            return pozycjeMagazynowe;
        }

        public int CloseSharedToolsOrders(List<int> pozycjeMagazynowe, ToolsBaseContext _context)
        {
            List<int> ordersToClose = new List<int>();

            var open_orders_TT=_context.OrderTTs
                .Where(n=>n.Active==true)
                .ToArray();

            var open_orders = _context.Zlecenies
               .Where(n => n.Aktywne == true)
               .ToArray();

            foreach (var order in open_orders_TT)
            {
                foreach(var item in pozycjeMagazynowe)
                {
                    if(order.ToolId==item)
                    {
                        ordersToClose.Add(order.OrderId);
                    }
                }
            }
            ordersToClose = ordersToClose.Distinct().ToList();

            int closed_orders=ordersToClose.Count();

            foreach (var order in open_orders_TT)
            {
                foreach(var item in ordersToClose)
                {
                    if(order.OrderId==item)
                    {
                        order.Active = false;

                        _context.OrderTTs.Update(order);
                        _context.SaveChanges();
                    }
                }
            }

            foreach (var order in open_orders)
            {
                foreach (var item in ordersToClose)
                {
                    if (order.IdZlecenia == item)
                    {
                        order.Aktywne = false;

                        _context.Zlecenies.Update(order);
                        _context.SaveChanges();
                    }
                }
            }

            return closed_orders;
        }

        public bool CanRegenerate(int pozycjaMagazynowa, ToolsBaseContext _context)
        {
            var toolId=_context.Magazyns.Where(n=>n.PozycjaMagazynowa==pozycjaMagazynowa)
                .FirstOrDefault();
            var catId=_context.Narzedzies
                .Where(n=>n.IdNarzedzia==toolId.IdNarzedzia)
                .FirstOrDefault();
            var toolPolicy=_context.Kategoria
                .Where(n=>n.IdKategorii==catId.IdKategorii)
                .FirstOrDefault();

            if(toolPolicy.ToolPolicy==1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Close_order_on_production(List<NarzedzieUszkodzoneViewModel> 
            uszkodzoneNarzedzia,ToolsBaseContext _context, int order_Id)
        {
            //close active order iz Zlecenie
            var orderToClose = _context.Zlecenies.Where(n => n.IdZlecenia == order_Id).FirstOrDefault();
            if(orderToClose!=null)
            {
                orderToClose.Aktywne = false;
                _context.Zlecenies.Update(orderToClose);
                _context.SaveChanges();
            }

            //close active order in OrderTT

            var positionsToClose = _context.OrderTTs.Where(n => n.OrderId == order_Id);
            _context.SaveChanges();
            if(positionsToClose!=null)
            {
                foreach(var position in positionsToClose)
                {
                    position.Active = false;
                    _context.OrderTTs.Update(position);
                }
                _context.SaveChanges();
            }


            foreach(var tools in uszkodzoneNarzedzia)
            {
                var position=_context.Magazyns.Where(n=>n.PozycjaMagazynowa==tools.ToolId)
                    .FirstOrDefault();
                if(tools.DamageType== "Regenerowalne")
                {
                    if(!Check_policy(tools.ToolId, _context))
                    {
                        position.Wycofany = true;
                        _context.Magazyns.Update(position);
                    }
                    else
                    {
                        position.Regeneracja = true;
                        _context.Magazyns.Update(position);
                    }
                }
                else
                {
                    position.Wycofany = true;
                    _context.Magazyns.Update(position);
                }
                _context.SaveChanges();
            }
        }

        private bool Check_policy(int tool_Id, ToolsBaseContext _context)
        {
            var t_Id = _context.Magazyns.Where(n => n.PozycjaMagazynowa == tool_Id)
                        .FirstOrDefault();
            var cat_Id = _context.Narzedzies.Where(n => n.IdNarzedzia == t_Id.IdNarzedzia)
                .FirstOrDefault();
            var policy = _context.Kategoria.Where(n => n.IdKategorii == cat_Id.IdKategorii)
                .FirstOrDefault();
            if (policy.ToolPolicy == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
