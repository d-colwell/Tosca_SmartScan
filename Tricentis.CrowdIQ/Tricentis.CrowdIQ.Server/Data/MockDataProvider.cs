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
        private RecommendationProvider _recommendation;

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

        public Mock.RecommendationProvider Recommendation
        {
            get
            {
                if (_recommendation == null)
                    _recommendation = new Mock.RecommendationProvider();
                return _recommendation;
            }
        }

    }
}
