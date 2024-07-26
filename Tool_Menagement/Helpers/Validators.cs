namespace Tool_Menagement.Helpers
{
    public class Validators
    {
        public string Validate_Name(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            char firstChar = name[0];
            if (char.IsLower(firstChar))
            {
                name = char.ToUpper(firstChar) + name.Substring(1);
            }
            if (name.Length >= 5)
            {
                return name;
            }

            return string.Empty;
        }
        public bool Validate_Double(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            bool isdouble = double.TryParse(input, out double result);
            return isdouble;
        }
    }
}
