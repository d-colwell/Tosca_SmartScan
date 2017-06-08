using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tricentis.CrowdIQ.Server.Data.Mock;

namespace Tricentis.CrowdIQ.Server.Data
{
    public class MockDataProvider
    {
        private static volatile MockDataProvider instance;
        private static object syncRoot = new Object();
        private RecomendationProvider _recomendation;

        private MockDataProvider() { }

        public static MockDataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MockDataProvider();
                    }
                }

                return instance;
            }
        }

        public Mock.RecomendationProvider Recomendation
        {
            get
            {
                if (_recomendation == null)
                    _recomendation = new Mock.RecomendationProvider();
                return _recomendation;
            }
        }

    }
}
