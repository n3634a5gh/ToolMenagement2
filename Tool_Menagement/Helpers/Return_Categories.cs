
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;
using Tool_Menagement.Models;

namespace Tool_Menagement.Helpers
{
    public class Return_Categories
    {
        public void fillTable(ToolsBaseContext _context)
        {
            string[] materials = new string[] { "HSS", "HSS-Co", "HM", "PM", "DIA", "VHM" };
            //string[] opisy = new string[] { "Frez kątowy", "Frez modułowy", "Frez trzpieniowy składany", "Frez walcowo-czołowy", "Frez walcowo-czołowy nasadzany", "Frez walcowy z łbem kulistym", "Głowica Frezarska", "Gwintownik", "Wiertło" };
            string[] przeznacz = new string[] { "Drewno", "Nieżelazne", "Stal", "Tworzywa sztuczne" };
            var category = new Kategorium();
            var categorydetails=new KategoriaDetail();
            /*for (int i = 0; i < materials.Length; i++)
            {
                for (int j = 0; j < opisy.Length; j++)
                {
                    for (int k = 0; k < przeznacz.Length; k++)
                    {
                        category = new Kategorium()
                        {
                            Opis = opisy[j], 
                            MaterialWykonania = materials[i],
                            Przeznaczenie = przeznacz[k]
                        };
                        _context.Kategoria.Add(category);
                        // _context.SaveChanges(); 
                    }
                }
            }*/

            for(int i=0;i<materials.Length;i++)
            {
                for (int j = 0; j < przeznacz.Length; j++)
                {
                    for(int k = 11;k <= 11;k++)
                    {
                        categorydetails = new KategoriaDetail()
                        {
                            IdKategorii =k,
                            MaterialWykonania =materials[i],
                            Przeznaczenie =przeznacz[j]
                        };
                        _context.KategoriaDetails.Add(categorydetails);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
