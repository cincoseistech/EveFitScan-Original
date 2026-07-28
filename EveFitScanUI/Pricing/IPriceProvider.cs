using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EveFitScanUI.Pricing;

public interface IPriceProvider
{
	Task<IReadOnlyDictionary<string, double>> GetPricesAsync(IEnumerable<string> itemNames, CancellationToken cancellationToken);
}
