﻿using System;
using System.Text;
using Waher.Networking.CoAP;
using Waher.Networking.CoAP.CoRE;
using Waher.Security.DTLS;

namespace Waher.Networking.LWM2M
{
	/// <summary>
	/// Contains a reference to an LWM2M Server.
	/// </summary>
	public class Lwm2mServerReference
	{
		private readonly string remoteEndpoint;
		private readonly string uri;
		private string locationPath = null;
		private readonly IDtlsCredentials credentials;
		private LinkDocument linkDoc = null;
		private bool registered = false;

		/// <summary>
		/// Contains a reference to an LWM2M Server.
		/// </summary>
		/// <param name="RemoteEndpoint">Host name or IP address of Remote endpoint.</param>
		public Lwm2mServerReference(string RemoteEndpoint)
			: this(RemoteEndpoint, CoapEndpoint.DefaultCoapPort, null)
		{
		}

		/// <summary>
		/// Contains a reference to an LWM2M Server.
		/// </summary>
		/// <param name="RemoteEndpoint">Host name or IP address of Remote endpoint.</param>
		/// <param name="Credentials">Optional credentials to use to establish DTLS session, if required.</param>
		public Lwm2mServerReference(string RemoteEndpoint, IDtlsCredentials Credentials)
			: this(RemoteEndpoint, Credentials is null ? CoapEndpoint.DefaultCoapPort : CoapEndpoint.DefaultCoapsPort, Credentials)
		{
		}

		/// <summary>
		/// Contains a reference to an LWM2M Server.
		/// </summary>
		/// <param name="RemoteEndpoint">Host name or IP address of Remote endpoint.</param>
		/// <param name="Port">Port number.</param>
		public Lwm2mServerReference(string RemoteEndpoint, int Port)
			: this(RemoteEndpoint, Port, null)
		{
		}

		/// <summary>
		/// Contains a reference to an LWM2M Server.
		/// </summary>
		/// <param name="RemoteEndpoint">Host name or IP address of Remote endpoint.</param>
		/// <param name="Port">Port number.</param>
		/// <param name="Credentials">Optional credentials to use to establish DTLS session, if required.</param>
		public Lwm2mServerReference(string RemoteEndpoint, int Port, IDtlsCredentials Credentials)
		{
			this.remoteEndpoint = RemoteEndpoint;
			this.credentials = Credentials;

			StringBuilder sb = new StringBuilder();

			sb.Append("coap");
			if (this.credentials != null)
				sb.Append('s');
			sb.Append("://");
			sb.Append(this.remoteEndpoint);

			if (this.credentials is null)
			{
				if (Port != CoapEndpoint.DefaultCoapPort)
				{
					sb.Append(':');
					sb.Append(Port.ToString());
				}
			}
			else
			{
				if (Port != CoapEndpoint.DefaultCoapsPort)
				{
					sb.Append(':');
					sb.Append(Port.ToString());
				}
			}

			sb.Append('/');

			this.uri = sb.ToString();
		}

		/// <summary>
		/// Host name of IP address of Remote endpoint.
		/// </summary>
		public string RemoteEndpoint => this.remoteEndpoint;

		/// <summary>
		/// Optional DTLS Credentials.
		/// </summary>
		public IDtlsCredentials Credentials => this.credentials;

		/// <summary>
		/// If encryption is expected, i.e. if credentials is not null.
		/// </summary>
		public bool Encrypted
		{
			get { return this.credentials != null; }
		}

		/// <summary>
		/// URI to server.
		/// </summary>
		public string Uri => this.uri;

		/// <summary>
		/// CoRE Link Document, if available.
		/// </summary>
		public LinkDocument LinkDocument
		{
			get => this.linkDoc;
			internal set => this.linkDoc = value;
		}

		/// <summary>
		/// If the client is registered with the server.
		/// </summary>
		public bool Registered
		{
			get => this.registered;
			internal set => this.registered = value;
		}

		/// <summary>
		/// Location path of registration.
		/// </summary>
		public string LocationPath
		{
			get => this.locationPath;
			internal set => this.locationPath = value;
		}
	}
}
