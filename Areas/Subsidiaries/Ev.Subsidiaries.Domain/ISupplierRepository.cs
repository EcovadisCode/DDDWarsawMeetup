using System.Threading.Tasks;

namespace Ev.Subsidiaries.Domain
{
    public interface ISupplierRepository 
    {
        Task Save(Supplier supplier);
        Task<Supplier> Get(SupplierId supplierId);
    }

}
