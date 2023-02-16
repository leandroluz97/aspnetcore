using ServiceContracts;
namespace Services
{
    public class CitiesService: ICitiesService, IDisposable
    {
        private List<string> _cities;

        private Guid _serviceInstanceId;
        public  Guid ServiceInstanceId
        {
            get { return _serviceInstanceId; }
        }


        public CitiesService()
        {
            _serviceInstanceId = Guid.NewGuid();
            _cities = new List<string>()
            {
                "Praia",
                "Lisbon",
                "Mindel",
                "Tokyo",
                "Porto"
            };
            // TO DO : Add logic to open db connection
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public void Dispose()
        {
            //TO DO: Add logic to olose db connection
        }
    }
}