using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HaloLive.Models.NameResolution;
using HaloLive.Network.Common;

namespace HaloLive.ServiceDiscovery
{
	/// <summary>
	/// Loads Endpoint{Region}.json files from the Endpoints directory.
	/// This directory should be relative to the working directory of the launched application.
	/// </summary>
	public sealed class FilestoreBasedRegionNamedEndpointStoreRepository : IRegionNamedEndpointStoreRepository
	{
		/// <inheritdoc />
		public async Task<NameEndpointResolutionStorageModel> Retrieve(ClientRegionLocale region)
		{
			using (StreamReader reader = File.OpenText(BuildRegionEndpointFileLocation(region)))
			{
				return Newtonsoft.Json.JsonConvert.DeserializeObject<NameEndpointResolutionStorageModel>(await reader.ReadToEndAsync());
			}
		}

		/// <inheritdoc />
		public Task<bool> HasRegionStore(ClientRegionLocale region)
		{
			return Task.FromResult(File.Exists(BuildRegionEndpointFileLocation(region)));
		}

		private string BuildRegionEndpointFileLocation(ClientRegionLocale region)
		{
			return $@"Endpoints/Endpoints{region.ToString()}.json";
		}
	}
}
