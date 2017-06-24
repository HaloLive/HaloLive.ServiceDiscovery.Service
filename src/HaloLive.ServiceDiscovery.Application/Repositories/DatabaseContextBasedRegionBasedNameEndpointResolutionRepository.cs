using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloLive.Models.NameResolution;
using HaloLive.Network.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaloLive.ServiceDiscovery
{
	public sealed class DatabaseContextBasedRegionBasedNameEndpointResolutionRepository : IRegionbasedNameEndpointResolutionRepository
	{
		/// <summary>
		/// DBContext containing the endpoints.
		/// </summary>
		public NamedEndpointDbContext EndpointsContext { get; }

		public DatabaseContextBasedRegionBasedNameEndpointResolutionRepository([FromServices] NamedEndpointDbContext endpointsContext)
		{
			if (endpointsContext == null) throw new ArgumentNullException(nameof(endpointsContext));

			EndpointsContext = endpointsContext;
		}

		/// <inheritdoc />
		public async Task<ResolvedEndpoint> RetrieveAsync(ClientRegionLocale locale, NetworkServiceType serviceType)
		{
			if (!Enum.IsDefined(typeof(ClientRegionLocale), locale)) throw new ArgumentOutOfRangeException(nameof(locale), "Value should be defined in the ClientRegionLocale enum.");
			if (!Enum.IsDefined(typeof(NetworkServiceType), serviceType)) throw new ArgumentOutOfRangeException(nameof(serviceType), "Value should be defined in the NetworkServiceType enum.");

			NamedResolvedEndpointEntryModel model = await EndpointsContext.Endpoints.FirstOrDefaultAsync(e => e.Region == locale && e.Service == serviceType);

			if(model == null)
				throw new KeyNotFoundException($"Provided keypair {locale} and {serviceType} not found.");

			return new ResolvedEndpoint(model.EndpointAddress, model.EndpointPort);
		}

		/// <inheritdoc />
		public async Task<bool> HasDataForRegionAsync(ClientRegionLocale locale)
		{
			if (!Enum.IsDefined(typeof(ClientRegionLocale), locale)) throw new ArgumentOutOfRangeException(nameof(locale), "Value should be defined in the ClientRegionLocale enum.");

			return await EndpointsContext.Endpoints.AnyAsync(e => e.Region == locale);
		}

		/// <inheritdoc />
		public async Task<bool> HasEntryAsync(ClientRegionLocale locale, NetworkServiceType serviceType)
		{
			if (!Enum.IsDefined(typeof(ClientRegionLocale), locale)) throw new ArgumentOutOfRangeException(nameof(locale), "Value should be defined in the ClientRegionLocale enum.");
			if (!Enum.IsDefined(typeof(NetworkServiceType), serviceType)) throw new ArgumentOutOfRangeException(nameof(serviceType), "Value should be defined in the NetworkServiceType enum.");

			return await EndpointsContext.Endpoints.AnyAsync(e => e.Region == locale && e.Service == serviceType);
		}
	}
}
