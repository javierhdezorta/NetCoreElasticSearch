using Nest;
using System;
using Elasticsearch.Net;

namespace ElasticSearch
{
    class Program
    {
        
        private const string archivo="/home/javier/Documentos/Elastic/PCK_CARGA_DIARIA_ALMONEDA_bulk.json";
        private static int cuenta = 0;

        static void Main(string[] args)
        {
            /*
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            var response = client.ClusterHealth();
            Console.WriteLine(response.Status);
            Console.Read();
            */
            var response = EsClient().ClusterHealth();
            Console.WriteLine(response.Status);
            
            var responseQuery = EsClient().Search<DocumentAttributes>(s => s
                .Index("personas")
                .Type("_doc")
                .From(0)
                .Size(1000)               
                .Query(q => q.MatchAll()));
            
            foreach (var hit in responseQuery.Hits)
            {
                Console.WriteLine( hit.Id.ToString());
                Console.WriteLine( hit.Source.nombre.ToString());
                Console.WriteLine(hit.Source.edad.ToString()); 
                Console.WriteLine( hit.Source.estado.ToString());
            }

            Console.ReadKey();

        }
        public static ElasticClient EsClient()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;
 
            
            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200")
            };
 
            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);
            elasticClient = new ElasticClient(connectionSettings);
 
            return elasticClient;
        }
       
        class DocumentAttributes
        {
            public string id { get; set; }
            public string nombre { get; set; }
            public string edad { get; set; }
            public string estado { get; set; }
        }
    }
}