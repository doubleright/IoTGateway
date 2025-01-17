﻿using System.Threading.Tasks;

namespace Waher.Networking.HTTP
{
	/// <summary>
	/// Ranged GET Interface for HTTP resources.
	/// </summary>
	public interface IHttpGetRangesMethod
	{
		/// <summary>
		/// Executes the ranged GET method on the resource.
		/// </summary>
		/// <param name="Request">HTTP Request</param>
		/// <param name="Response">HTTP Response</param>
		/// <param name="FirstInterval">First byte range interval.</param>
		/// <exception cref="HttpException">If an error occurred when processing the method.</exception>
		Task GET(HttpRequest Request, HttpResponse Response, ByteRangeInterval FirstInterval);

		/// <summary>
		/// If the GET method is allowed.
		/// </summary>
		bool AllowsGET
		{
			get;
		}
	}
}
