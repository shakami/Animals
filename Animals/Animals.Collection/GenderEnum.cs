namespace Animals.Logic
{
    public static class Gender
    {
        public enum GenderEnum { male, female };

        public static GenderEnum Male = GenderEnum.male;
        public static GenderEnum Female = GenderEnum.female;

        public static GenderEnum? Parse(string gender)
        {
            switch (gender.ToLower())
            {
                case "m":
                case "male":
                    return Male;
                case "f":
                case "female":
                    return Female;
                default:
                    return null;
            }
        }
    }
}
