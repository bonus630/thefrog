namespace br.com.bonus630.thefrog.Shared
{
    public static class Extensions
    {
        public static int FlipIfNegative(this int value,int modifier)
        {
            return modifier < 0 ? -value : value;
        }
        public static float FlipIfNegative(this float value,int modifier)
        {
            return modifier < 0 ? -value : value;
        }
        public static float FlipIfNegative(this float value,float modifier)
        {
            return modifier < 0 ? -value : value;
        }
        public static int FlipIfNegative(this int value,float modifier)
        {
            return modifier < 0 ? -value : value;
        }
        public static int FlipIfNegative(this int value,bool invert)
        {
            return invert ? -value : value;
        }
        public static float FlipIfNegative(this float value,bool invert)
        {
            return invert ? -value : value;
        }
        
    }
}
