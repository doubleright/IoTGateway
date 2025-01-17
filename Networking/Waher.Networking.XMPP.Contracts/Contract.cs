﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Waher.Content;
using Waher.Content.Xml;
using Waher.Content.Xsl;
using Waher.Networking.XMPP.Contracts.HumanReadable;
using Waher.Script;

namespace Waher.Networking.XMPP.Contracts
{
	/// <summary>
	/// Contains the definition of a contract
	/// </summary>
	public class Contract
	{
		private string contractId = null;
		private string templateId = string.Empty;
		private string provider = null;
		private string forMachinesLocalName = null;
		private string forMachinesNamespace = null;
		private byte[] contentSchemaDigest = null;
		private XmlElement forMachines = null;
		private Role[] roles = null;
		private Part[] parts = null;
		private Parameter[] parameters = null;
		private HumanReadableText[] forHumans = null;
		private ClientSignature[] clientSignatures = null;
		private Attachment[] attachments = null;
		private ServerSignature serverSignature = null;
		private Waher.Security.HashFunction contentSchemaHashFunction = Waher.Security.HashFunction.SHA256;
		private ContractState state = ContractState.Proposed;
		private ContractVisibility visibility = ContractVisibility.CreatorAndParts;
		private ContractParts partsMode = ContractParts.ExplicitlyDefined;
		private DateTime created = DateTime.MinValue;
		private DateTime updated = DateTime.MinValue;
		private DateTime from = DateTime.MinValue;
		private DateTime to = DateTime.MaxValue;
		private DateTime? signAfter = null;
		private DateTime? signBefore = null;
		private Duration? duration = null;
		private Duration? archiveReq = null;
		private Duration? archiveOpt = null;
		private bool canActAsTemplate = false;

		/// <summary>
		/// Contains the definition of a contract
		/// </summary>
		public Contract()
		{
		}

		/// <summary>
		/// Contract identity
		/// </summary>
		public string ContractId
		{
			get => this.contractId;
			set => this.contractId = value;
		}

		/// <summary>
		/// Contract identity, as an URI.
		/// </summary>
		public Uri ContractIdUri => ContractsClient.ContractIdUri(this.contractId);

		/// <summary>
		/// Contract identity, as an URI string.
		/// </summary>
		public string ContractIdUriString => ContractsClient.ContractIdUriString(this.contractId);

		/// <summary>
		/// JID of the Trust Provider hosting the contract
		/// </summary>
		public string Provider
		{
			get => this.provider;
			set => this.provider = value;
		}

		/// <summary>
		/// Contract identity of template, if one was used to create the contract.
		/// </summary>
		public string TemplateId
		{
			get => this.templateId;
			set => this.templateId = value;
		}

		/// <summary>
		/// Contract identity of template, if one was used to create the contract, as an URI.
		/// </summary>
		public Uri TemplateIdUri => string.IsNullOrEmpty(this.templateId) ? null : ContractsClient.ContractIdUri(this.templateId);

		/// <summary>
		/// Contract identity of template, if one was used to create the contract, as an URI string.
		/// </summary>
		public string TemplateIdUriString => string.IsNullOrEmpty(this.templateId) ? null : ContractsClient.ContractIdUriString(this.templateId);

		/// <summary>
		/// Contract state
		/// </summary>
		public ContractState State
		{
			get => this.state;
			set => this.state = value;
		}

		/// <summary>
		/// When the contract was created
		/// </summary>
		public DateTime Created
		{
			get => this.created;
			set => this.created = value;
		}

		/// <summary>
		/// When the contract was last updated
		/// </summary>
		public DateTime Updated
		{
			get => this.updated;
			set => this.updated = value;
		}

		/// <summary>
		/// From when the contract is valid (if signed)
		/// </summary>
		public DateTime From
		{
			get => this.from;
			set => this.from = value;
		}

		/// <summary>
		/// Until when the contract is valid (if signed)
		/// </summary>
		public DateTime To
		{
			get => this.to;
			set => this.to = value;
		}

		/// <summary>
		/// Signatures will only be accepted after this point in time.
		/// </summary>
		public DateTime? SignAfter
		{
			get => this.signAfter;
			set => this.signAfter = value;
		}

		/// <summary>
		/// Signatures will only be accepted until this point in time.
		/// </summary>
		public DateTime? SignBefore
		{
			get => this.signBefore;
			set => this.signBefore = value;
		}

		/// <summary>
		/// Contrat Visibility
		/// </summary>
		public ContractVisibility Visibility
		{
			get => this.visibility;
			set => this.visibility = value;
		}

		/// <summary>
		/// Duration of the contract. Is counted from the time it is signed by the required parties.
		/// </summary>
		public Duration? Duration
		{
			get => this.duration;
			set => this.duration = value;
		}

		/// <summary>
		/// Requied time to archive a signed smart contract, after it becomes obsolete.
		/// </summary>
		public Duration? ArchiveRequired
		{
			get => this.archiveReq;
			set => this.archiveReq = value;
		}

		/// <summary>
		/// Optional time to archive a signed smart contract, after it becomes obsolete, and after its required archivation period.
		/// </summary>
		public Duration? ArchiveOptional
		{
			get => this.archiveOpt;
			set => this.archiveOpt = value;
		}

		/// <summary>
		/// The hash digest of the schema used to validate the machine-readable contents (<see cref="ForMachines"/>) of the smart contract,
		/// if such a schema was used.
		/// </summary>
		public byte[] ContentSchemaDigest
		{
			get => this.contentSchemaDigest;
			set => this.contentSchemaDigest = value;
		}

		/// <summary>
		/// Hash function of the schema used to validate the machine-readable contents (<see cref="ForMachines"/>) of the smart contract,
		/// if such a schema was used.
		/// </summary>
		public Security.HashFunction ContentSchemaHashFunction
		{
			get => this.contentSchemaHashFunction;
			set => this.contentSchemaHashFunction = value;
		}

		/// <summary>
		/// Roles defined in the smart contract.
		/// </summary>
		public Role[] Roles
		{
			get => this.roles;
			set => this.roles = value;
		}

		/// <summary>
		/// How parts are defined in the smart contract.
		/// </summary>
		public ContractParts PartsMode
		{
			get => this.partsMode;
			set => this.partsMode = value;
		}

		/// <summary>
		/// Defined parts for the smart contract.
		/// </summary>
		public Part[] Parts
		{
			get => this.parts;
			set => this.parts = value;
		}

		/// <summary>
		/// Defined parameters for the smart contract.
		/// </summary>
		public Parameter[] Parameters
		{
			get => this.parameters;
			set => this.parameters = value;
		}

		/// <summary>
		/// Machine-readable contents of the contract.
		/// </summary>
		public XmlElement ForMachines
		{
			get => this.forMachines;
			set
			{
				this.forMachines = value;
				this.forMachinesLocalName = value?.LocalName;
				this.forMachinesNamespace = value?.NamespaceURI;
			}
		}

		/// <summary>
		/// Namespace used by the root node of the machine-readable contents of the contract (<see cref="ForMachines"/>).
		/// </summary>
		public string ForMachinesNamespace => this.forMachinesNamespace;

		/// <summary>
		/// Local name used by the root node of the machine-readable contents of the contract (<see cref="ForMachines"/>).
		/// </summary>
		public string ForMachinesLocalName => this.forMachinesLocalName;

		/// <summary>
		/// Human-readable contents of the contract.
		/// </summary>
		public HumanReadableText[] ForHumans
		{
			get => this.forHumans;
			set => this.forHumans = value;
		}

		/// <summary>
		/// Client signatures of the contract.
		/// </summary>
		public ClientSignature[] ClientSignatures
		{
			get => this.clientSignatures;
			set => this.clientSignatures = value;
		}

		/// <summary>
		/// Attachments assigned to the legal identity.
		/// </summary>
		public Attachment[] Attachments
		{
			get => this.attachments;
			set => this.attachments = value;
		}

		/// <summary>
		/// Server signature attesting to the validity of the contents of the contract.
		/// </summary>
		public ServerSignature ServerSignature
		{
			get => this.serverSignature;
			set => this.serverSignature = value;
		}

		/// <summary>
		/// If the contract can act as a template for other contracts.
		/// </summary>
		public bool CanActAsTemplate
		{
			get => this.canActAsTemplate;
			set => this.canActAsTemplate = value;
		}

		/// <summary>
		/// Validates a contract XML Document, and returns the contract definition in it.
		/// </summary>
		/// <param name="Xml">XML representation</param>
		/// <returns>Parsed contract, or null if it contains errors.</returns>
		/// <exception cref="Exception">If XML is invalid.</exception>
		public static Task<ParsedContract> Parse(XmlDocument Xml)
		{
			XSL.Validate(string.Empty, Xml, "contract", ContractsClient.NamespaceSmartContracts, 
				contractSchema, identitiesSchema, e2eSchema, p2pSchema, xmlSchema);

			return Parse(Xml.DocumentElement);
		}

		private static readonly XmlSchema identitiesSchema = XSL.LoadSchema(typeof(Contract).Namespace + ".Schema.LegalIdentities.xsd");
		private static readonly XmlSchema contractSchema = XSL.LoadSchema(typeof(Contract).Namespace + ".Schema.SmartContracts.xsd");
		private static readonly XmlSchema e2eSchema = XSL.LoadSchema(typeof(Contract).Namespace + ".Schema.E2E.xsd");
		private static readonly XmlSchema p2pSchema = XSL.LoadSchema(typeof(Contract).Namespace + ".Schema.P2P.xsd");
		private static readonly XmlSchema xmlSchema = XSL.LoadSchema(typeof(Contract).Namespace + ".Schema.Xml.xsd");

		/// <summary>
		/// Parses a contract from is XML representation.
		/// </summary>
		/// <param name="Xml">XML representation</param>
		/// <returns>Parsed contract, or null if it contains errors.</returns>
		public static async Task<ParsedContract> Parse(XmlElement Xml)
		{
			bool HasVisibility = false;
			Contract Result = new Contract();
			ParsedContract ParsedContract = new ParsedContract()
			{
				Contract = Result,
				HasStatus = false,
				ParametersValid = true
			};

			foreach (XmlAttribute Attr in Xml.Attributes)
			{
				switch (Attr.Name)
				{
					case "id":
						Result.contractId = Attr.Value;
						break;

					case "visibility":
						if (Enum.TryParse<ContractVisibility>(Attr.Value, out ContractVisibility Visibility))
						{
							Result.visibility = Visibility;
							HasVisibility = true;
						}
						else
							return null;
						break;

					case "duration":
						if (Waher.Content.Duration.TryParse(Attr.Value, out Duration D))
							Result.duration = D;
						else
							return null;
						break;

					case "archiveReq":
						if (Waher.Content.Duration.TryParse(Attr.Value, out D))
							Result.archiveReq = D;
						else
							return null;
						break;

					case "archiveOpt":
						if (Waher.Content.Duration.TryParse(Attr.Value, out D))
							Result.archiveOpt = D;
						else
							return null;
						break;

					case "signAfter":
						if (XML.TryParse(Attr.Value, out DateTime TP))
							Result.signAfter = TP;
						else
							return null;
						break;

					case "signBefore":
						if (XML.TryParse(Attr.Value, out TP))
							Result.signBefore = TP;
						else
							return null;
						break;

					case "canActAsTemplate":
						if (CommonTypes.TryParse(Attr.Value, out bool b))
							Result.canActAsTemplate = b;
						else
							return null;
						break;

					case "xmlns":
						break;

					default:
						if (Attr.Prefix == "xmlns")
							break;
						else
							return null;
				}
			}

			if (!HasVisibility ||
				Result.duration is null ||
				Result.archiveReq is null ||
				Result.archiveOpt is null ||
				Result.signBefore <= Result.signAfter)
			{
				return null;
			}

			List<HumanReadableText> ForHumans = new List<HumanReadableText>();
			List<Role> Roles = new List<Role>();
			List<Parameter> Parameters = new List<Parameter>();
			List<ClientSignature> Signatures = new List<ClientSignature>();
			List<Attachment> Attachments = new List<Attachment>();
			XmlElement Content = null;
			HumanReadableText Text;
			bool First = true;
			bool PartsDefined = false;

			foreach (XmlNode N in Xml.ChildNodes)
			{
				if (!(N is XmlElement E))
					continue;

				if (First)
				{
					Content = E;
					First = false;
					continue;
				}

				switch (E.LocalName)
				{
					case "role":
						List<HumanReadableText> Descriptions = new List<HumanReadableText>();
						Role Role = new Role()
						{
							MinCount = -1,
							MaxCount = -1,
							CanRevoke = false
						};

						foreach (XmlAttribute Attr in E.Attributes)
						{
							switch (Attr.Name)
							{
								case "name":
									Role.Name = Attr.Value;
									if (string.IsNullOrEmpty(Role.Name))
										return null;
									break;

								case "minCount":
									if (int.TryParse(Attr.Value, out int i) && i >= 0)
										Role.MinCount = i;
									else
										return null;
									break;

								case "maxCount":
									if (int.TryParse(Attr.Value, out i) && i >= 0)
										Role.MaxCount = i;
									else
										return null;
									break;

								case "canRevoke":
									if (CommonTypes.TryParse(Attr.Value, out bool b))
										Role.CanRevoke = b;
									else
										return null;
									break;

								case "xmlns":
									break;

								default:
									if (Attr.Prefix == "xmlns")
										break;
									else
										return null;
							}
						}

						if (string.IsNullOrEmpty(Role.Name) ||
							Role.MinCount < 0 ||
							Role.MaxCount < 0)
						{
							return null;
						}

						foreach (XmlNode N2 in E.ChildNodes)
						{
							if (N2 is XmlElement E2)
							{
								if (E2.LocalName == "description")
								{
									Text = HumanReadableText.Parse(E2);
									if (Text is null || !Text.IsWellDefined)
										return null;

									Descriptions.Add(Text);
								}
								else
									return null;
							}
						}

						if (Descriptions.Count == 0)
							return null;

						Role.Descriptions = Descriptions.ToArray();

						Roles.Add(Role);
						break;

					case "parts":
						List<Part> Parts = null;
						ContractParts? Mode = null;

						foreach (XmlNode N2 in E.ChildNodes)
						{
							if (N2 is XmlElement E2)
							{
								switch (E2.LocalName)
								{
									case "open":
										if (Mode.HasValue)
											return null;

										Mode = ContractParts.Open;
										break;

									case "templateOnly":
										if (Mode.HasValue)
											return null;

										Mode = ContractParts.TemplateOnly;
										break;

									case "part":
										if (Mode.HasValue)
										{
											if (Mode.Value != ContractParts.ExplicitlyDefined)
												return null;
										}
										else
											Mode = ContractParts.ExplicitlyDefined;

										string LegalId = null;
										string RoleRef = null;

										foreach (XmlAttribute Attr in E2.Attributes)
										{
											switch (Attr.Name)
											{
												case "legalId":
													LegalId = Attr.Value;
													break;

												case "role":
													RoleRef = Attr.Value;
													break;

												case "xmlns":
													break;

												default:
													if (Attr.Prefix == "xmlns")
														break;
													else
														return null;
											}
										}

										if (LegalId is null || RoleRef is null || string.IsNullOrEmpty(LegalId) || string.IsNullOrEmpty(RoleRef))
											return null;

										bool RoleFound = false;

										foreach (Role Role2 in Roles)
										{
											if (Role2.Name == RoleRef)
											{
												RoleFound = true;
												break;
											}
										}

										if (!RoleFound)
											return null;

										if (Parts is null)
											Parts = new List<Part>();

										Parts.Add(new Part()
										{
											LegalId = LegalId,
											Role = RoleRef
										});

										break;

									default:
										return null;
								}
							}
						}

						if (!Mode.HasValue)
							return null;

						Result.partsMode = Mode.Value;
						Result.parts = Parts?.ToArray();

						PartsDefined = true;
						break;

					case "parameters":
						foreach (XmlNode N2 in E.ChildNodes)
						{
							if (N2 is XmlElement E2)
							{
								string Name = XML.Attribute(E2, "name");
								if (string.IsNullOrEmpty(Name))
									return null;

								Descriptions = new List<HumanReadableText>();

								foreach (XmlNode N3 in E2.ChildNodes)
								{
									if (N3 is XmlElement E3)
									{
										if (E3.LocalName == "description")
										{
											Text = HumanReadableText.Parse(E3);
											if (Text is null || !Text.IsWellDefined)
												return null;

											Descriptions.Add(Text);
										}
										else
											return null;
									}
								}

								switch (E2.LocalName)
								{
									case "stringParameter":
										Parameters.Add(new StringParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value") : null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											RegEx = XML.Attribute(E2, "regEx"),
											Min = E2.HasAttribute("min") ? XML.Attribute(E2, "min") : null,
											MinIncluded = XML.Attribute(E2, "minIncluded", true),
											Max = E2.HasAttribute("max") ? XML.Attribute(E2, "max") : null,
											MaxIncluded = XML.Attribute(E2, "maxIncluded", true),
											MinLength = E2.HasAttribute("minLength") ? XML.Attribute(E2, "minLength", 0) : (int?)null,
											MaxLength = E2.HasAttribute("maxLength") ? XML.Attribute(E2, "maxLength", 0) : (int?)null,
											Descriptions = Descriptions.ToArray()
										});
										break;

									case "numericalParameter":
										Parameters.Add(new NumericalParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value", 0.0m) : (decimal?)null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Min = E2.HasAttribute("min") ? XML.Attribute(E2, "min", 0.0m) : (decimal?)null,
											MinIncluded = XML.Attribute(E2, "minIncluded", true),
											Max = E2.HasAttribute("max") ? XML.Attribute(E2, "max", 0.0m) : (decimal?)null,
											MaxIncluded = XML.Attribute(E2, "maxIncluded", true),
											Descriptions = Descriptions.ToArray()
										});
										break;

									case "booleanParameter":
										Parameters.Add(new BooleanParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value", false) : (bool?)null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Descriptions = Descriptions.ToArray()
										});
										break;

									case "dateParameter":
										Parameters.Add(new DateParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value", DateTime.MinValue).Date : (DateTime?)null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Min = E2.HasAttribute("min") ? XML.Attribute(E2, "min", DateTime.MinValue).Date : (DateTime?)null,
											MinIncluded = XML.Attribute(E2, "minIncluded", true),
											Max = E2.HasAttribute("max") ? XML.Attribute(E2, "max", DateTime.MinValue).Date : (DateTime?)null,
											MaxIncluded = XML.Attribute(E2, "maxIncluded", true),
											Descriptions = Descriptions.ToArray()
										});
										break;

									case "dateTimeParameter":
										Parameters.Add(new DateTimeParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value", DateTime.MinValue) : (DateTime?)null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Min = E2.HasAttribute("min") ? XML.Attribute(E2, "min", DateTime.MinValue) : (DateTime?)null,
											MinIncluded = XML.Attribute(E2, "minIncluded", true),
											Max = E2.HasAttribute("max") ? XML.Attribute(E2, "max", DateTime.MinValue) : (DateTime?)null,
											MaxIncluded = XML.Attribute(E2, "maxIncluded", true),
											Descriptions = Descriptions.ToArray()
										});
										break;

									case "timeParameter":
										Parameters.Add(new TimeParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value", TimeSpan.Zero) : (TimeSpan?)null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Min = E2.HasAttribute("min") ? XML.Attribute(E2, "min", TimeSpan.Zero) : (TimeSpan?)null,
											MinIncluded = XML.Attribute(E2, "minIncluded", true),
											Max = E2.HasAttribute("max") ? XML.Attribute(E2, "max", TimeSpan.Zero) : (TimeSpan?)null,
											MaxIncluded = XML.Attribute(E2, "maxIncluded", true),
											Descriptions = Descriptions.ToArray()
										});
										break;

									case "durationParameter":
										Parameters.Add(new DurationParameter()
										{
											Name = Name,
											Value = E2.HasAttribute("value") ? XML.Attribute(E2, "value", Waher.Content.Duration.Zero) : (Duration?)null,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Min = E2.HasAttribute("min") ? XML.Attribute(E2, "min", Waher.Content.Duration.Zero) : (Duration?)null,
											MinIncluded = XML.Attribute(E2, "minIncluded", true),
											Max = E2.HasAttribute("max") ? XML.Attribute(E2, "max", Waher.Content.Duration.Zero) : (Duration?)null,
											MaxIncluded = XML.Attribute(E2, "maxIncluded", true),
											Descriptions = Descriptions.ToArray()
										});
										break;


									case "calcParameter":
										Parameters.Add(new CalcParameter()
										{
											Name = Name,
											Guide = XML.Attribute(E2, "guide"),
											Expression = XML.Attribute(E2, "exp"),
											Descriptions = Descriptions.ToArray()
										});
										break;

									default:
										return null;
								}
							}
						}

						if (Parameters.Count == 0)
							return null;
						break;

					case "humanReadableText":
						Text = HumanReadableText.Parse(E);
						if (Text is null || !Text.IsWellDefined)
							return null;

						ForHumans.Add(Text);
						break;

					case "signature":
						ClientSignature Signature = new ClientSignature();

						foreach (XmlAttribute Attr in E.Attributes)
						{
							switch (Attr.Name)
							{
								case "legalId":
									Signature.LegalId = Attr.Value;
									break;

								case "bareJid":
									Signature.BareJid = Attr.Value;
									break;

								case "role":
									Signature.Role = Attr.Value;
									break;

								case "timestamp":
									if (XML.TryParse(Attr.Value, out DateTime TP))
										Signature.Timestamp = TP;
									else
										return null;
									break;

								case "transferable":
									if (CommonTypes.TryParse(Attr.Value, out bool b))
										Signature.Transferable = b;
									else
										return null;
									break;

								case "xmlns":
									break;

								default:
									if (Attr.Prefix == "xmlns")
										break;
									else
										return null;
							}
						}

						Signature.DigitalSignature = Convert.FromBase64String(E.InnerText);

						if (string.IsNullOrEmpty(Signature.LegalId) ||
							string.IsNullOrEmpty(Signature.BareJid) ||
							string.IsNullOrEmpty(Signature.Role) ||
							Signature.Timestamp == DateTime.MinValue)
						{
							return null;
						}

						Signatures.Add(Signature);
						break;

					case "attachment":
						Attachments.Add(new Attachment()
						{
							Id = XML.Attribute(E, "id"),
							LegalId = XML.Attribute(E, "legalId"),
							ContentType = XML.Attribute(E, "contentType"),
							FileName = XML.Attribute(E, "fileName"),
							Signature = Convert.FromBase64String(XML.Attribute(E, "s")),
							Timestamp = XML.Attribute(E, "timestamp", DateTime.MinValue)
						});
						break;

					case "status":
						ParsedContract.HasStatus = true;
						foreach (XmlAttribute Attr in E.Attributes)
						{
							switch (Attr.Name)
							{
								case "provider":
									Result.provider = Attr.Value;
									break;

								case "state":
									if (Enum.TryParse<ContractState>(Attr.Value, out ContractState ContractState))
										Result.state = ContractState;
									break;

								case "created":
									if (XML.TryParse(Attr.Value, out DateTime TP))
										Result.created = TP;
									break;

								case "updated":
									if (XML.TryParse(Attr.Value, out TP))
										Result.updated = TP;
									break;

								case "from":
									if (XML.TryParse(Attr.Value, out TP))
										Result.from = TP;
									break;

								case "to":
									if (XML.TryParse(Attr.Value, out TP))
										Result.to = TP;
									break;

								case "templateId":
									Result.templateId = Attr.Value;
									break;

								case "schemaDigest":
									Result.contentSchemaDigest = Convert.FromBase64String(Attr.Value);
									break;

								case "schemaHashFunction":
									if (Enum.TryParse<Waher.Security.HashFunction>(Attr.Value, out Waher.Security.HashFunction H))
										Result.contentSchemaHashFunction = H;
									else
										return null;
									break;

								case "xmlns":
									break;

								default:
									if (Attr.Prefix == "xmlns")
										break;
									else
										return null;
							}
						}
						break;

					case "serverSignature":
						ServerSignature ServerSignature = new ServerSignature();

						foreach (XmlAttribute Attr in E.Attributes)
						{
							switch (Attr.Name)
							{
								case "timestamp":
									if (XML.TryParse(Attr.Value, out DateTime TP))
										ServerSignature.Timestamp = TP;
									else
										return null;
									break;

								case "xmlns":
									break;

								default:
									if (Attr.Prefix == "xmlns")
										break;
									else
										return null;
							}
						}

						ServerSignature.DigitalSignature = Convert.FromBase64String(E.InnerText);

						if (ServerSignature.Timestamp == DateTime.MinValue)
							return null;

						Result.serverSignature = ServerSignature;
						break;

					case "attachmentRef":
						string AttachmentId = XML.Attribute(E, "attachmentId");
						string Url = XML.Attribute(E, "url");

						foreach (Attachment Attachment in Attachments)
						{
							if (Attachment.Id == AttachmentId)
							{
								Attachment.Url = Url;
								break;
							}
						}
						break;

					default:
						return null;
				}
			}

			if (Content is null || ForHumans.Count == 0 || !PartsDefined)
				return null;

			Variables Variables = new Variables
			{
				["Duration"] = Result.duration
			};

			foreach (Parameter Parameter in Parameters)
				Parameter.Populate(Variables);

			foreach (Parameter Parameter in Parameters)
			{
				if (!await Parameter.IsParameterValid(Variables))
				{
					ParsedContract.ParametersValid = false;
					break;
				}
			}

			Result.roles = Roles.ToArray();
			Result.parameters = Parameters.ToArray();
			Result.forMachines = Content;
			Result.forMachinesLocalName = Content.LocalName;
			Result.forMachinesNamespace = Content.NamespaceURI;
			Result.forHumans = ForHumans.ToArray();
			Result.clientSignatures = Signatures.ToArray();
			Result.attachments = Attachments.ToArray();

			return ParsedContract;
		}

		/// <summary>
		/// Normalizes an XML element.
		/// </summary>
		/// <param name="Xml">XML element to normalize</param>
		/// <param name="Output">Normalized XML will be output here</param>
		/// <param name="CurrentNamespace">Namespace at the encapsulating entity.</param>
		public static void NormalizeXml(XmlElement Xml, StringBuilder Output, string CurrentNamespace)
		{
			Output.Append('<');

			SortedDictionary<string, string> Attributes = null;
			string TagName = Xml.LocalName.Normalize(NormalizationForm.FormC);

			if (!string.IsNullOrEmpty(Xml.Prefix))
				TagName = Xml.Prefix.Normalize(NormalizationForm.FormC) + ":" + TagName;

			Output.Append(TagName);

			foreach (XmlAttribute Attr in Xml.Attributes)
			{
				if (Attributes is null)
					Attributes = new SortedDictionary<string, string>();

				Attributes[Attr.Name.Normalize(NormalizationForm.FormC)] = Attr.Value.Normalize(NormalizationForm.FormC);
			}

			if (Xml.NamespaceURI != CurrentNamespace && string.IsNullOrEmpty(Xml.Prefix))
			{
				if (Attributes is null)
					Attributes = new SortedDictionary<string, string>();

				Attributes["xmlns"] = Xml.NamespaceURI;
				CurrentNamespace = Xml.NamespaceURI;
			}
			else
				Attributes?.Remove("xmlns");

			if (!(Attributes is null))
			{
				foreach (KeyValuePair<string, string> Attr in Attributes)
				{
					Output.Append(' ');
					Output.Append(Attr.Key);
					Output.Append("=\"");
					Output.Append(XML.Encode(Attr.Value));
					Output.Append('"');
				}
			}

			bool HasContent = false;

			foreach (XmlNode N in Xml.ChildNodes)
			{
				if (N is XmlElement E)
				{
					if (!HasContent)
					{
						HasContent = true;
						Output.Append('>');
					}

					NormalizeXml(E, Output, CurrentNamespace);
				}
				else if (N is XmlText || N is XmlCDataSection || N is XmlSignificantWhitespace)
				{
					if (!HasContent)
					{
						HasContent = true;
						Output.Append('>');
					}

					Output.Append(XML.Encode(N.InnerText.Normalize(NormalizationForm.FormC)));
				}
			}

			if (HasContent)
			{
				Output.Append("</");
				Output.Append(TagName);
				Output.Append('>');
			}
			else
				Output.Append("/>");
		}

		/// <summary>
		/// Checks if a contract is legally binding.
		/// </summary>
		/// <param name="CheckCurrentTime">If the current time should be checked as well.</param>
		/// <returns>If the contract is legally binding.</returns>
		public bool IsLegallyBinding(bool CheckCurrentTime)
		{
			if (this.clientSignatures is null || this.serverSignature is null)
				return false;

			switch (this.state)
			{
				case ContractState.Proposed:
				case ContractState.Obsoleted:
				case ContractState.Deleted:
				case ContractState.Rejected:
				case ContractState.Failed:
					return false;
			}

			switch (this.partsMode)
			{
				case ContractParts.TemplateOnly:
					return false;

				case ContractParts.ExplicitlyDefined:
					if (this.parts is null)
						return false;

					foreach (Part Part in this.parts)
					{
						bool Found = false;

						foreach (ClientSignature Signature in this.clientSignatures)
						{
							if (string.Compare(Signature.LegalId, Part.LegalId, true) == 0 &&
								string.Compare(Signature.Role, Part.Role, true) == 0)
							{
								Found = true;
								break;
							}
						}

						if (!Found)
							return false;
					}
					break;
			}

			if (!(this.roles is null))
			{
				foreach (Role Role in this.roles)
				{
					int Count = 0;

					foreach (ClientSignature Signature in this.clientSignatures)
					{
						if (string.Compare(Signature.Role, Role.Name, true) == 0)
							Count++;
					}

					if (Count < Role.MinCount || Count > Role.MaxCount)
						return false;
				}
			}

			if (CheckCurrentTime)
			{
				DateTime Now = DateTime.Now;

				if (Now < this.from || Now > this.to)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Serializes the Contract, in normalized form.
		/// </summary>
		/// <param name="Xml">XML Output</param>
		/// <param name="IncludeNamespace">If namespace attribute should be included.</param>
		/// <param name="IncludeIdAttribute">If id attribute should be included.</param>
		/// <param name="IncludeClientSignatures">If client signatures should be included.</param>
		/// <param name="IncludeAttachments">If attachments are to be included.</param>
		/// <param name="IncludeStatus">If the status element should be included.</param>
		/// <param name="IncludeServerSignature">If the server signature should be included.</param>
		/// <param name="IncludeAttachmentReferences">If attachment references (URLs) are to be included.</param>
		public void Serialize(StringBuilder Xml, bool IncludeNamespace, bool IncludeIdAttribute, bool IncludeClientSignatures,
			bool IncludeAttachments, bool IncludeStatus, bool IncludeServerSignature, bool IncludeAttachmentReferences)
		{
			Xml.Append("<contract archiveOpt=\"");
			Xml.Append(this.archiveOpt.ToString());
			Xml.Append("\" archiveReq=\"");
			Xml.Append(this.archiveReq.ToString());
			Xml.Append("\" canActAsTemplate=\"");
			Xml.Append(CommonTypes.Encode(this.canActAsTemplate));
			Xml.Append("\" duration=\"");
			Xml.Append(this.duration.ToString());
			Xml.Append('"');

			if (IncludeIdAttribute)
			{
				Xml.Append(" id=\"");
				Xml.Append(XML.Encode(this.contractId));
				Xml.Append('"');
			}

			if (this.signAfter.HasValue)
			{
				Xml.Append(" signAfter=\"");
				Xml.Append(XML.Encode(this.signAfter.Value));
				Xml.Append('"');
			}

			if (this.signBefore.HasValue)
			{
				Xml.Append(" signBefore=\"");
				Xml.Append(XML.Encode(this.signBefore.Value));
				Xml.Append('"');
			}

			Xml.Append(" visibility=\"");
			Xml.Append(this.visibility.ToString());
			Xml.Append('"');

			if (IncludeNamespace)
			{
				Xml.Append(" xmlns=\"");
				Xml.Append(ContractsClient.NamespaceSmartContracts);
				Xml.Append('"');
			}

			Xml.Append('>');

			if (this.forMachines is null)
				throw new InvalidOperationException("No Machine-readable XML provided.");

			NormalizeXml(this.forMachines, Xml, ContractsClient.NamespaceSmartContracts);

			if (!(this.roles is null))
			{
				foreach (Role Role in this.roles)
				{
					Xml.Append("<role maxCount=\"");
					Xml.Append(Role.MaxCount.ToString());
					Xml.Append("\" minCount=\"");
					Xml.Append(Role.MinCount.ToString());
					Xml.Append("\" name=\"");
					Xml.Append(XML.Encode(Role.Name));

					if (Role.CanRevoke)
						Xml.Append("\" canRevoke=\"true");

					Xml.Append("\">");

					foreach (HumanReadableText Description in Role.Descriptions)
						Description.Serialize(Xml, "description", false);

					Xml.Append("</role>");
				}
			}

			Xml.Append("<parts>");

			switch (this.partsMode)
			{
				case ContractParts.Open:
					Xml.Append("<open/>");
					break;

				case ContractParts.TemplateOnly:
					Xml.Append("<templateOnly/>");
					break;

				case ContractParts.ExplicitlyDefined:
					if (!(this.parts is null))
					{
						foreach (Part Part in this.parts)
						{
							Xml.Append("<part legalId=\"");
							Xml.Append(XML.Encode(Part.LegalId));
							Xml.Append("\" role=\"");
							Xml.Append(XML.Encode(Part.Role));
							Xml.Append("\"/>");
						}
					}
					break;
			}

			Xml.Append("</parts>");

			if (!(this.parameters is null) && this.parameters.Length > 0)
			{
				Xml.Append("<parameters>");

				foreach (Parameter Parameter in this.parameters)
					Parameter.Serialize(Xml, false);

				Xml.Append("</parameters>");
			}

			if (!(this.forHumans is null) && this.forHumans.Length > 0)
			{
				foreach (HumanReadableText Text in this.forHumans)
					Text.Serialize(Xml);
			}

			if (IncludeClientSignatures && !(this.clientSignatures is null))
			{
				foreach (Signature Signature in this.clientSignatures)
					Signature.Serialize(Xml);
			}

			if (IncludeAttachments && !(this.attachments is null))
			{
				foreach (Attachment A in this.attachments)
				{
					Xml.Append("<attachment contentType=\"");
					Xml.Append(A.ContentType.Normalize(NormalizationForm.FormC));
					Xml.Append("\" fileName=\"");
					Xml.Append(A.FileName.Normalize(NormalizationForm.FormC));
					Xml.Append("\" id=\"");
					Xml.Append(A.Id.Normalize(NormalizationForm.FormC));
					Xml.Append("\" legalId=\"");
					Xml.Append(A.LegalId.Normalize(NormalizationForm.FormC));
					Xml.Append("\" s=\"");
					Xml.Append(Convert.ToBase64String(A.Signature));
					Xml.Append("\" timestamp=\"");
					Xml.Append(XML.Encode(A.Timestamp));
					Xml.Append("\"/>");
				}
			}

			if (IncludeStatus)
			{
				Xml.Append("<status created=\"");
				Xml.Append(XML.Encode(this.created));

				if (this.from != DateTime.MinValue)
				{
					Xml.Append("\" from=\"");
					Xml.Append(XML.Encode(this.from));
				}

				Xml.Append("\" provider=\"");
				Xml.Append(XML.Encode(this.provider));

				if (!(this.contentSchemaDigest is null) && this.contentSchemaDigest.Length > 0)
				{
					Xml.Append("\" schemaDigest=\"");
					Xml.Append(Convert.ToBase64String(this.contentSchemaDigest));

					Xml.Append("\" schemaHashFunction=\"");
					Xml.Append(this.contentSchemaHashFunction.ToString());
				}

				Xml.Append("\" state=\"");
				Xml.Append(this.state.ToString());

				if (!string.IsNullOrEmpty(this.templateId))
				{
					Xml.Append("\" templateId=\"");
					Xml.Append(XML.Encode(this.templateId));
				}

				if (this.to != DateTime.MaxValue)
				{
					Xml.Append("\" to=\"");
					Xml.Append(XML.Encode(this.to));
				}

				if (this.updated != DateTime.MinValue)
				{
					Xml.Append("\" updated=\"");
					Xml.Append(XML.Encode(this.updated));
				}

				Xml.Append("\"/>");
			}

			if (IncludeServerSignature)
				this.serverSignature?.Serialize(Xml);

			if (IncludeAttachmentReferences && !(this.attachments is null))
			{
				foreach (Attachment A in this.attachments)
				{
					Xml.Append("<attachmentRef attachmentId=\"");
					Xml.Append(XML.Encode(A.Id));
					Xml.Append("\" url=\"");
					Xml.Append(XML.Encode(A.Url));
					Xml.Append("\"/>");
				}
			}

			Xml.Append("</contract>");
		}

		/// <summary>
		/// Gets event tags describing the contract.
		/// </summary>
		/// <returns>Tags</returns>
		public KeyValuePair<string, object>[] GetTags()
		{
			List<KeyValuePair<string, object>> Response = new List<KeyValuePair<string, object>>()
			{
				new KeyValuePair<string, object>("ID", this.contractId),
				new KeyValuePair<string, object>("Provider", this.provider),
				new KeyValuePair<string, object>("State", this.state),
				new KeyValuePair<string, object>("Visibility", this.visibility),
				new KeyValuePair<string, object>("Contract Local Name", this.forMachinesLocalName),
				new KeyValuePair<string, object>("Contract Namespace", this.forMachinesNamespace),
				new KeyValuePair<string, object>("Created", this.created)
			};

			if (this.updated != DateTime.MinValue)
				Response.Add(new KeyValuePair<string, object>("Updated", this.updated));

			if (this.from != DateTime.MinValue)
				Response.Add(new KeyValuePair<string, object>("From", this.from));

			if (this.to != DateTime.MaxValue)
				Response.Add(new KeyValuePair<string, object>("To", this.to));

			if (!string.IsNullOrEmpty(this.templateId))
				Response.Add(new KeyValuePair<string, object>("Template ID", this.templateId));

			if (!(this.parameters is null))
			{
				foreach (Parameter P in this.parameters)
					Response.Add(new KeyValuePair<string, object>(P.Name, P.ObjectValue ?? string.Empty));
			}

			return Response.ToArray();
		}

		/// <summary>
		/// Access to contract parameters.
		/// </summary>
		/// <param name="Key"></param>
		/// <returns>Parameter value.</returns>
		public object this[string Key]
		{
			get
			{
				if (!(this.parameters is null))
				{
					foreach (Parameter P in this.parameters)
					{
						if (P.Name == Key)
							return P.ObjectValue;
					}
				}

				return null;
			}

			set
			{
				if (!(this.parameters is null))
				{
					foreach (Parameter P in this.parameters)
					{
						if (P.Name == Key)
						{
							P.SetValue(value);
							return;
						}
					}
				}

				throw new IndexOutOfRangeException("A parameter named " + Key + " not found.");
			}
		}

		/// <summary>
		/// Default language for contract.
		/// </summary>
		public string DefaultLanguage
		{
			get
			{
				if (!(this.forHumans is null))
				{
					foreach (HumanReadableText Text in this.forHumans)
						return Text.Language;
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Gets available languages encoded in contract.
		/// </summary>
		/// <returns>Array of languages.</returns>
		public string[] GetLanguages()
		{
			SortedDictionary<string, bool> Languages = new SortedDictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);

			this.Add(Languages, this.forHumans);

			if (!(this.roles is null))
			{
				foreach (Role Role in this.roles)
					this.Add(Languages, Role.Descriptions);
			}

			if (!(this.parameters is null))
			{
				foreach (Parameter Parameter in this.parameters)
					this.Add(Languages, Parameter.Descriptions);
			}

			string[] Result = new string[Languages.Count];
			Languages.Keys.CopyTo(Result, 0);

			return Result;
		}

		private void Add(SortedDictionary<string, bool> Languages, HumanReadableText[] Texts)
		{
			if (Texts is null)
				return;

			foreach (HumanReadableText Text in Texts)
			{
				if (!string.IsNullOrEmpty(Text.Language))
					Languages[Text.Language] = true;
			}
		}

		/// <summary>
		/// Creates a human-readable Markdown document for the contract.
		/// </summary>
		/// <param name="Language">Desired language</param>
		/// <returns>Markdown</returns>
		public string ToMarkdown(string Language)
		{
			return this.ToMarkdown(Language, MarkdownType.ForRendering);
		}

		/// <summary>
		/// Creates a human-readable Markdown document for the contract.
		/// </summary>
		/// <param name="Language">Desired language</param>
		/// <param name="Type">Type of Markdown being generated.</param>
		/// <returns>Markdown</returns>
		public string ToMarkdown(string Language, MarkdownType Type)
		{
			return this.ToMarkdown(this.forHumans, Language, Type);
		}

		/// <summary>
		/// Creates a human-readable HTML document for the contract.
		/// </summary>
		/// <param name="Language">Desired language</param>
		/// <returns>Markdown</returns>
		public Task<string> ToHTML(string Language)
		{
			return this.ToHTML(this.forHumans, Language);
		}

		/// <summary>
		/// Creates a human-readable Plain Trext document for the contract.
		/// </summary>
		/// <param name="Language">Desired language</param>
		/// <returns>Markdown</returns>
		public Task<string> ToPlainText(string Language)
		{
			return this.ToPlainText(this.forHumans, Language);
		}

		/// <summary>
		/// Creates a human-readable WPF XAML document for the contract.
		/// </summary>
		/// <param name="Language">Desired language</param>
		/// <returns>Markdown</returns>
		public Task<string> ToXAML(string Language)
		{
			return this.ToXAML(this.forHumans, Language);
		}

		/// <summary>
		/// Creates a human-readable Xamarin.Forms XAML document for the contract.
		/// </summary>
		/// <param name="Language">Desired language</param>
		/// <returns>Markdown</returns>
		public Task<string> ToXamarinForms(string Language)
		{
			return this.ToXamarinForms(this.forHumans, Language);
		}

		internal string ToMarkdown(HumanReadableText[] Text, string Language, MarkdownType Type)
		{
			return Select(Text, Language)?.GenerateMarkdown(this, Type);
		}

		internal Task<string> ToPlainText(HumanReadableText[] Text, string Language)
		{
			return Select(Text, Language)?.GeneratePlainText(this) ?? Task.FromResult<string>(null);
		}

		internal Task<string> ToHTML(HumanReadableText[] Text, string Language)
		{
			return Select(Text, Language)?.GenerateHTML(this) ?? Task.FromResult<string>(null);
		}

		internal Task<string> ToXAML(HumanReadableText[] Text, string Language)
		{
			return Select(Text, Language)?.GenerateXAML(this) ?? Task.FromResult<string>(null);
		}

		internal Task<string> ToXamarinForms(HumanReadableText[] Text, string Language)
		{
			return Select(Text, Language)?.GenerateXamarinForms(this) ?? Task.FromResult<string>(null);
		}

		internal HumanReadableText Select(HumanReadableText[] Text, string Language)
		{
			if (Text is null)
				return null;

			HumanReadableText First = null;

			foreach (HumanReadableText T in Text)
			{
				if (string.Compare(T.Language, Language, StringComparison.CurrentCultureIgnoreCase) == 0)
					return T;

				if (First is null)
					First = T;
			}

			return First;
		}

	}
}
