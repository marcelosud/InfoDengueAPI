using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace InfoDengueAPI.Application.Services
{
    public interface IInfodengueService
    {
        Task<JArray> GetEpidemiologicalData(int codigoIBGE, int ewStart, int ewEnd, int eyStart, int eyEnd, string disease);
    }
}
