namespace Tool_Menagement.Helpers
{
    public class Create_name
    {
        public static string Tool_Name(string Opis, double Srednica, string Material, string Przeznaczenie)
        {
            string name = "";
            double tmp = (Srednica / 2);
            name = $"{Opis}, D {Srednica}, {Material}, ";
            if (Przeznaczenie == "Stal")
            {
                name += "Z-S";
            }
            else if (Przeznaczenie == "Nieżelazne")
            {
                name += "Z-NZ";
            }
            else if (Przeznaczenie == "Drewno")
            {
                name += "Z-D";
            }
            else if (Przeznaczenie == "Tworzywa sztuczne")
            {
                name += "Z-TW";
            }

            if (Opis == "Frez walcowy z łbem kulistym")
            {
                name += $", R={tmp.ToString()}";
            }
            return name;
        }
    }
}
