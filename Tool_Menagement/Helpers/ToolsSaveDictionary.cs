using Tool_Menagement.Models;

namespace Tool_Menagement.Helpers
{
    public class ToolsSaveDictionary
    {
        public void SaveToolsWithTime(ToolsBaseContext _context, int[] magazynPositionsId)
        {
            int[] tools_id;

            int[][] tools_table = [new int[magazynPositionsId.Length], new int[magazynPositionsId.Length]];
            for(int i=0;i<magazynPositionsId.Length;i++)
            {
                tools_table[0][i] = magazynPositionsId[i];

                tools_table[1][i] = _context.Magazyns
                    .Where(x => x.PozycjaMagazynowa== magazynPositionsId[i])
                    .Select(x=>x.IdNarzedzia)
                    .FirstOrDefault();
            }
        }
    }
}
