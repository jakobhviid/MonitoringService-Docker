using System.Threading.Tasks;
using Confluent.Kafka;

namespace MonitoringService.Infrastructure.Gateways
{
    public interface IKafkaProducerGateway
    {
        public Task SendMessageAsync(string topic, object messageToSerialize, IProducer<Null, string> p);
        public Task SendMessageAsync(string topic, object messageToSerialize);
    }
}