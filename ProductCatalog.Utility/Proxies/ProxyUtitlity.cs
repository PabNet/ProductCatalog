namespace ProductCatalog.Utility.Proxies
{
    public static class ProxyUtitlity<TUtility>
    {
        private static TUtility? _utility;

        public static void Initialize(TUtility? utility)
        {
            _utility = utility;
        }

        public static TUtility GetUtility()
        {
            if (_utility == null)
            {
                throw new InvalidOperationException($"Instance of {typeof(TUtility).Name} has not been initialized.");
            }

            return _utility;
        }
    }
}
