﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Waher.Content;
using Waher.Networking.XMPP.DataForms;
using Waher.Networking.XMPP.DataForms.FieldTypes;
using Waher.Networking.XMPP.DataForms.Layout;
using Waher.Runtime.Inventory;

namespace Waher.Client.WPF.Dialogs
{
	/// <summary>
	/// Interaction logic for ParameterDialog.xaml
	/// </summary>
	public partial class ParameterDialog : Window
	{
		private readonly DataForm form;
		private FrameworkElement makeVisible = null;

		private ParameterDialog(DataForm Form)
		{
			this.form = Form;
			InitializeComponent();
		}

		/// <summary>
		/// Interaction logic for ParameterDialog.xaml
		/// </summary>
		public static async Task<ParameterDialog> CreateAsync(DataForm Form)
		{
			ParameterDialog Result = new ParameterDialog(Form)
			{
				Title = Form.Title
			};

			Panel Container = Result.DialogPanel;
			TabControl TabControl = null;
			TabItem TabItem;
			StackPanel StackPanel;
			ScrollViewer ScrollViewer;
			Control First = null;
			Control Control;

			if (Form.HasPages)
			{
				TabControl = new TabControl();
				Result.DialogPanel.Children.Add(TabControl);
				DockPanel.SetDock(TabControl, Dock.Top);
			}
			else
			{
				ScrollViewer = new ScrollViewer()
				{
					VerticalScrollBarVisibility = ScrollBarVisibility.Auto
				};

				Result.DialogPanel.Children.Add(ScrollViewer);
				DockPanel.SetDock(ScrollViewer, Dock.Top);

				StackPanel = new StackPanel()
				{
					Margin = new Thickness(10, 10, 10, 10),
				};

				ScrollViewer.Content = StackPanel;
				Container = StackPanel;
			}

			foreach (Networking.XMPP.DataForms.Layout.Page Page in Form.Pages)
			{
				if (TabControl != null)
				{
					TabItem = new TabItem()
					{
						Header = Page.Label
					};

					TabControl.Items.Add(TabItem);

					ScrollViewer = new ScrollViewer()
					{
						VerticalScrollBarVisibility = ScrollBarVisibility.Auto
					};

					TabItem.Content = ScrollViewer;

					StackPanel = new StackPanel()
					{
						Margin = new Thickness(10, 10, 10, 10)
					};

					ScrollViewer.Content = StackPanel;
					Container = StackPanel;
				}
				else
					TabItem = null;

				if (Form.Instructions != null && Form.Instructions.Length > 0)
				{
					foreach (string Row in Form.Instructions)
					{
						TextBlock TextBlock = new TextBlock()
						{
							TextWrapping = TextWrapping.Wrap,
							Margin = new Thickness(0, 5, 0, 5),
							Text = Row
						};

						Container.Children.Add(TextBlock);
					}
				}

				foreach (LayoutElement Element in Page.Elements)
				{
					Control = await Result.Layout(Container, Element, Form);
					if (First is null)
						First = Control;
				}

				if (TabControl != null && TabControl.Items.Count == 1)
					TabItem.Focus();
			}

			if (First != null)
				First.Focus();

			Result.CheckOkButtonEnabled();

			return Result;
		}

		private async Task<Control> Layout(Panel Container, LayoutElement Element, DataForm Form)
		{
			if (Element is FieldReference FieldReference)
				return await this.Layout(Container, FieldReference, Form);
			else if (Element is Networking.XMPP.DataForms.Layout.TextElement TextElement)
				this.Layout(Container, TextElement, Form);
			else if (Element is Networking.XMPP.DataForms.Layout.Section Section)
				return await this.Layout(Container, Section, Form);
			else if (Element is ReportedReference ReportedReference)
				this.Layout(Container, ReportedReference, Form);

			return null;
		}

		private async Task<Control> Layout(Panel Container, Networking.XMPP.DataForms.Layout.Section Section, DataForm Form)
		{
			GroupBox GroupBox = new GroupBox();
			Container.Children.Add(GroupBox);
			GroupBox.Header = Section.Label;
			GroupBox.Margin = new Thickness(5, 5, 5, 5);

			StackPanel StackPanel = new StackPanel();
			GroupBox.Content = StackPanel;
			StackPanel.Margin = new Thickness(5, 5, 5, 5);

			Control First = null;
			Control Control;

			foreach (LayoutElement Element in Section.Elements)
			{
				Control = await this.Layout(StackPanel, Element, Form);
				if (First is null)
					First = Control;
			}

			return First;
		}

		private void Layout(Panel Container, Networking.XMPP.DataForms.Layout.TextElement TextElement, DataForm _)
		{
			TextBlock TextBlock = new TextBlock()
			{
				TextWrapping = TextWrapping.Wrap,
				Margin = new Thickness(0, 0, 0, 5),
				Text = TextElement.Text
			};

			Container.Children.Add(TextBlock);
		}

		private async Task<Control> Layout(Panel Container, FieldReference FieldReference, DataForm Form)
		{
			Field Field = Form[FieldReference.Var];
			if (Field is null)
				return null;

			Control Result = null;
			bool MakeVisible = false;

			if (Field.HasError)
				MakeVisible = true;
			else
				Field.Validate(Field.ValueStrings);

			if (Field is TextSingleField TextSingleField)
				Result = this.Layout(Container, TextSingleField, Form);
			else if (Field is TextMultiField TextMultiField)
				Result = this.Layout(Container, TextMultiField, Form);
			else if (Field is TextPrivateField TextPrivateField)
				Result = this.Layout(Container, TextPrivateField, Form);
			else if (Field is BooleanField BooleanField)
				Result = this.Layout(Container, BooleanField, Form);
			else if (Field is ListSingleField ListSingleField)
				Result = this.Layout(Container, ListSingleField, Form);
			else if (Field is ListMultiField ListMultiField)
				Result = this.Layout(Container, ListMultiField, Form);
			else if (Field is FixedField FixedField)
				this.Layout(Container, FixedField, Form);
			else if (Field is JidMultiField JidMultiField)
				Result = this.Layout(Container, JidMultiField, Form);
			else if (Field is JidSingleField JidSingleField)
				Result = this.Layout(Container, JidSingleField, Form);
			else if (Field is MediaField MediaField)
				await this.Layout(Container, MediaField, Form);

			if (MakeVisible && this.makeVisible is null)
				this.makeVisible = Result;

			return Result;
		}

		private Control Layout(Panel Container, BooleanField Field, DataForm _)
		{
			TextBlock TextBlock = new TextBlock()
			{
				TextWrapping = TextWrapping.Wrap,
				Text = Field.Label
			};

			if (Field.Required)
			{
				Run Run = new Run("*");
				TextBlock.Inlines.Add(Run);
				Run.Foreground = new SolidColorBrush(Colors.Red);
			}

			CheckBox CheckBox;

			CheckBox = new CheckBox()
			{
				Name = VarToName(Field.Var),
				Content = TextBlock,
				Margin = new Thickness(0, 3, 0, 3),
				IsEnabled = !Field.ReadOnly,
				ToolTip = Field.Description
			};

			if (!CommonTypes.TryParse(Field.ValueString, out bool IsChecked))
				CheckBox.IsChecked = null;
			else
				CheckBox.IsChecked = IsChecked;

			if (Field.HasError)
				CheckBox.Background = new SolidColorBrush(Colors.PeachPuff);
			else if (Field.NotSame)
				CheckBox.Background = new SolidColorBrush(Colors.LightGray);

			CheckBox.Click += this.CheckBox_Click;

			Container.Children.Add(CheckBox);

			return CheckBox;
		}

		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is CheckBox CheckBox))
				return;

			string Var = NameToVar(CheckBox.Name);
			Field Field = this.form[Var];
			if (Field is null)
				return;

			if (CheckBox.IsChecked.HasValue)
			{
				CheckBox.Background = null;
				Field.SetValue(CommonTypes.Encode(CheckBox.IsChecked.Value));
				this.CheckOkButtonEnabled();
			}
			else
			{
				CheckBox.Background = new SolidColorBrush(Colors.LightGray);
				Field.SetValue(string.Empty);
			}
		}

		private void CheckOkButtonEnabled()
		{
			foreach (Field Field in this.form.Fields)
			{
				if (!Field.ReadOnly && Field.HasError)
				{
					this.OkButton.IsEnabled = false;
					return;
				}

				if (Field.Required && string.IsNullOrEmpty(Field.ValueString))
				{
					this.OkButton.IsEnabled = false;
					return;
				}
			}

			this.OkButton.IsEnabled = true;
		}

		private void Layout(Panel Container, FixedField Field, DataForm _)
		{
			TextBlock TextBlock = new TextBlock()
			{
				TextWrapping = TextWrapping.Wrap,
				Margin = new Thickness(0, 5, 0, 5),
				Text = Field.ValueString
			};

			Container.Children.Add(TextBlock);
		}

		private Control Layout(Panel Container, JidMultiField Field, DataForm _)
		{
			TextBox TextBox = this.LayoutTextBox(Container, Field);
			TextBox.TextChanged += this.TextBox_TextChanged;
			TextBox.AcceptsReturn = true;
			TextBox.AcceptsTab = true;
			TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

			return TextBox;
		}

		private Control Layout(Panel Container, JidSingleField Field, DataForm _)
		{
			TextBox TextBox = this.LayoutTextBox(Container, Field);
			TextBox.TextChanged += this.TextBox_TextChanged;

			return TextBox;
		}

		private Control Layout(Panel Container, ListMultiField Field, DataForm _)
		{
			TextBlock TextBlock = new TextBlock()
			{
				TextWrapping = TextWrapping.Wrap,
				Text = Field.Label
			};

			if (Field.Required)
			{
				Run Run = new Run("*");
				TextBlock.Inlines.Add(Run);
				Run.Foreground = new SolidColorBrush(Colors.Red);
			}

			GroupBox GroupBox = new GroupBox();
			Container.Children.Add(GroupBox);
			GroupBox.Name = VarToName(Field.Var);
			GroupBox.Header = TextBlock;
			GroupBox.ToolTip = Field.Description;
			GroupBox.Margin = new Thickness(5, 5, 5, 5);

			StackPanel StackPanel = new StackPanel();
			GroupBox.Content = StackPanel;
			StackPanel.Margin = new Thickness(5, 5, 5, 5);

			string[] Values = Field.ValueStrings;
			CheckBox CheckBox;

			foreach (KeyValuePair<string, string> Option in Field.Options)
			{
				CheckBox = new CheckBox()
				{
					Content = Option.Key,
					Tag = Option.Value,
					Margin = new Thickness(0, 3, 0, 3),
					IsEnabled = !Field.ReadOnly,
					IsChecked = Array.IndexOf<string>(Values, Option.Value) >= 0
				};

				if (Field.HasError)
					CheckBox.Background = new SolidColorBrush(Colors.PeachPuff);
				else if (Field.NotSame)
					CheckBox.Background = new SolidColorBrush(Colors.LightGray);

				CheckBox.Click += this.MultiListCheckBox_Click;

				StackPanel.Children.Add(CheckBox);
			}

			GroupBox.Tag = this.LayoutErrorLabel(StackPanel, Field);

			return GroupBox;
		}

		private void MultiListCheckBox_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is CheckBox CheckBox))
				return;

			if (!(CheckBox.Parent is StackPanel StackPanel))
				return;

			if (!(StackPanel.Parent is GroupBox GroupBox))
				return;

			string Var = NameToVar(GroupBox.Name);
			Field Field = this.form[Var];
			if (Field is null)
				return;

			List<string> Values = new List<string>();

			foreach (UIElement Element in StackPanel.Children)
			{
				CheckBox = Element as CheckBox;
				if (CheckBox is null)
					continue;

				if (CheckBox.IsChecked.HasValue && CheckBox.IsChecked.Value)
					Values.Add((string)CheckBox.Tag);
			}

			Field.SetValue(Values.ToArray());

			TextBlock ErrorLabel = (TextBlock)GroupBox.Tag;
			Brush Background;

			if (Field.HasError)
			{
				Background = new SolidColorBrush(Colors.PeachPuff);
				this.OkButton.IsEnabled = false;

				if (!(ErrorLabel is null))
				{
					ErrorLabel.Text = Field.Error;
					ErrorLabel.Visibility = Visibility.Visible;
				}
			}
			else
			{
				Background = null;
				this.CheckOkButtonEnabled();

				if (!(ErrorLabel is null))
					ErrorLabel.Visibility = Visibility.Collapsed;
			}

			foreach (UIElement Element in StackPanel.Children)
			{
				CheckBox = Element as CheckBox;
				if (CheckBox is null)
					continue;

				CheckBox.Background = Background;
			}
		}

		private Control Layout(Panel Container, ListSingleField Field, DataForm _)
		{
			this.LayoutControlLabel(Container, Field);

			ComboBox ComboBox = new ComboBox()
			{
				Name = VarToName(Field.Var),
				IsReadOnly = Field.ReadOnly,
				ToolTip = Field.Description,
				Margin = new Thickness(0, 0, 0, 5)
			};

			if (Field.HasError)
				ComboBox.Background = new SolidColorBrush(Colors.PeachPuff);
			else if (Field.NotSame)
				ComboBox.Background = new SolidColorBrush(Colors.LightGray);

			ComboBoxItem Item;

			if (!(Field.Options is null))
			{
				foreach (KeyValuePair<string, string> P in Field.Options)
				{
					Item = new ComboBoxItem()
					{
						Content = P.Key,
						Tag = P.Value
					};

					ComboBox.Items.Add(Item);
				}
			}

			if (Field.ValidationMethod is Networking.XMPP.DataForms.ValidationMethods.OpenValidation)
			{
				ComboBox.IsEditable = true;
				ComboBox.Text = Field.ValueString;
				ComboBox.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
					new System.Windows.Controls.TextChangedEventHandler(ComboBox_TextChanged));
			}
			else
			{
				string s = Field.ValueString;

				ComboBox.IsEditable = false;
				ComboBox.SelectionChanged += this.ComboBox_SelectionChanged;

				if (!(Field.Options is null))
					ComboBox.SelectedIndex = Array.FindIndex<KeyValuePair<string, string>>(Field.Options, (P) => P.Value.Equals(s));
			}

			Container.Children.Add(ComboBox);
			ComboBox.Tag = this.LayoutErrorLabel(Container, Field);

			return ComboBox;
		}

		private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!(sender is ComboBox ComboBox))
				return;

			string Var = NameToVar(ComboBox.Name);
			Field Field = this.form[Var];
			if (Field is null)
				return;

			TextBlock ErrorLabel = (TextBlock)ComboBox.Tag;
			string s = ComboBox.Text;

			if (ComboBox.SelectedItem is ComboBoxItem ComboBoxItem && ((string)ComboBoxItem.Content) == s)
				s = (string)ComboBoxItem.Tag;

			Field.SetValue(s);

			if (Field.HasError)
			{
				ComboBox.Background = new SolidColorBrush(Colors.PeachPuff);
				this.OkButton.IsEnabled = false;

				if (!(ErrorLabel is null))
				{
					ErrorLabel.Text = Field.Error;
					ErrorLabel.Visibility = Visibility.Visible;
				}
			}
			else
			{
				ComboBox.Background = null;
				
				if (!(ErrorLabel is null))
					ErrorLabel.Visibility = Visibility.Collapsed;
				
				this.CheckOkButtonEnabled();
			}
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!(sender is ComboBox ComboBox))
				return;

			string Var = NameToVar(ComboBox.Name);
			Field Field = this.form[Var];
			if (Field is null)
				return;

			TextBlock ErrorLabel = (TextBlock)ComboBox.Tag;
			string Value;

			if (!(ComboBox.SelectedItem is ComboBoxItem Item))
				Value = string.Empty;
			else
				Value = (string)Item.Tag;

			Field.SetValue(Value);

			if (Field.HasError)
			{
				ComboBox.Background = new SolidColorBrush(Colors.PeachPuff);
				this.OkButton.IsEnabled = false;

				if (!(ErrorLabel is null))
				{
					ErrorLabel.Text = Field.Error;
					ErrorLabel.Visibility = Visibility.Visible;
				}
				return;
			}
			else
			{
				ComboBox.Background = null;
				
				if (!(ErrorLabel is null))
					ErrorLabel.Visibility = Visibility.Collapsed;
				
				this.CheckOkButtonEnabled();
			}
		}

		private async Task Layout(Panel Container, MediaField Field, DataForm _)
		{
			MediaElement MediaElement;
			Uri Uri = null;
			Grade Best = Runtime.Inventory.Grade.NotAtAll;
			Grade Grade;
			bool IsImage = false;
			bool IsVideo = false;
			bool IsAudio = false;

			bool TopMarginLaidOut = this.LayoutControlLabel(Container, Field);

			foreach (KeyValuePair<string, Uri> P in Field.Media.URIs)
			{
				if (P.Key.StartsWith("image/"))
				{
					IsImage = true;
					Uri = P.Value;
					break;
				}
				else if (P.Key.StartsWith("video/"))
				{
					switch (P.Key.ToLower())
					{
						case "video/x-ms-asf":
						case "video/x-ms-wvx":
						case "video/x-ms-wm":
						case "video/x-ms-wmx":
							Grade = Grade.Perfect;
							break;

						case "video/mp4":
							Grade = Grade.Excellent;
							break;

						case "video/3gp":
						case "video/3gpp ":
						case "video/3gpp2 ":
						case "video/3gpp-tt":
						case "video/h263":
						case "video/h263-1998":
						case "video/h263-2000":
						case "video/h264":
						case "video/h264-rcdo":
						case "video/h264-svc":
							Grade = Grade.Ok;
							break;

						default:
							Grade = Grade.Barely;
							break;
					}

					if (Grade > Best)
					{
						Best = Grade;
						Uri = P.Value;
						IsVideo = true;
					}
				}
				else if (P.Key.StartsWith("audio/"))
				{
					switch (P.Key.ToLower())
					{
						case "audio/x-ms-wma":
						case "audio/x-ms-wax":
						case "audio/x-ms-wmv":
							Grade = Grade.Perfect;
							break;

						case "audio/mp4":
						case "audio/mpeg":
							Grade = Grade.Excellent;
							break;

						case "audio/amr":
						case "audio/amr-wb":
						case "audio/amr-wb+":
						case "audio/pcma":
						case "audio/pcma-wb":
						case "audio/pcmu":
						case "audio/pcmu-wb":
							Grade = Grade.Ok;
							break;

						default:
							Grade = Grade.Barely;
							break;
					}

					if (Grade > Best)
					{
						Best = Grade;
						Uri = P.Value;
						IsAudio = true;
					}
				}
			}

			if (IsImage)
			{
				BitmapImage BitmapImage = new System.Windows.Media.Imaging.BitmapImage();
				BitmapImage.BeginInit();
				try
				{
					if (Field.Media.Binary != null)
						BitmapImage.UriSource = new Uri(await Waher.Content.Markdown.Model.Multimedia.ImageContent.GetTemporaryFile(Field.Media.Binary));
					else if (Uri != null)
						BitmapImage.UriSource = Uri;
					else if (!string.IsNullOrEmpty(Field.Media.URL))
						BitmapImage.UriSource = new Uri(Field.Media.URL);
				}
				finally
				{
					BitmapImage.EndInit();
				}

				Image Image = new Image()
				{
					Source = BitmapImage,
					ToolTip = Field.Description,
					Margin = new Thickness(0, TopMarginLaidOut ? 0 : 5, 0, 5)
				};

				if (Field.Media.Width.HasValue)
					Image.Width = Field.Media.Width.Value;

				if (Field.Media.Height.HasValue)
					Image.Height = Field.Media.Height.Value;

				Container.Children.Add(Image);
			}
			else if (IsVideo || IsAudio)
			{
				MediaElement = new MediaElement()
				{
					Source = Uri,
					LoadedBehavior = MediaState.Manual,
					ToolTip = Field.Description
				};

				Container.Children.Add(MediaElement);

				if (IsVideo)
				{
					MediaElement.Margin = new Thickness(0, TopMarginLaidOut ? 0 : 5, 0, 5);

					if (Field.Media.Width.HasValue)
						MediaElement.Width = Field.Media.Width.Value;

					if (Field.Media.Height.HasValue)
						MediaElement.Height = Field.Media.Height.Value;
				}

				DockPanel ControlPanel = new DockPanel()
				{
					Width = 290
				};

				Container.Children.Add(ControlPanel);

				Button Button = new Button()
				{
					Width = 50,
					Height = 23,
					Margin = new Thickness(0, 0, 5, 0),
					Content = "<<",
					Tag = MediaElement
				};

				ControlPanel.Children.Add(Button);
				Button.Click += this.Rewind_Click;

				Button = new Button()
				{
					Width = 50,
					Height = 23,
					Margin = new Thickness(5, 0, 5, 0),
					Content = "Play",
					Tag = MediaElement
				};

				ControlPanel.Children.Add(Button);
				Button.Click += this.Play_Click;

				Button = new Button()
				{
					Width = 50,
					Height = 23,
					Margin = new Thickness(5, 0, 5, 0),
					Content = "Pause",
					Tag = MediaElement
				};

				ControlPanel.Children.Add(Button);
				Button.Click += this.Pause_Click;

				Button = new Button()
				{
					Width = 50,
					Height = 23,
					Margin = new Thickness(5, 0, 5, 0),
					Content = "Stop",
					Tag = MediaElement
				};

				ControlPanel.Children.Add(Button);
				Button.Click += this.Stop_Click;

				Button = new Button()
				{
					Width = 50,
					Height = 23,
					Margin = new Thickness(5, 0, 0, 0),
					Content = ">>",
					Tag = MediaElement
				};

				ControlPanel.Children.Add(Button);
				Button.Click += this.Forward_Click;

				MediaElement.Play();
			}
		}

		private void Rewind_Click(object sender, RoutedEventArgs e)
		{
			Button Button = (Button)sender;
			MediaElement MediaElement = (MediaElement)Button.Tag;

			if (!(MediaElement is null))
			{
				if (MediaElement.SpeedRatio >= 0)
					MediaElement.SpeedRatio = -1;
				else if (MediaElement.SpeedRatio > -32)
					MediaElement.SpeedRatio *= 2;
			}
		}

		private void Play_Click(object sender, RoutedEventArgs e)
		{
			Button Button = (Button)sender;
			MediaElement MediaElement = (MediaElement)Button.Tag;

			if (!(MediaElement is null))
			{
				if (MediaElement.Position >= MediaElement.NaturalDuration.TimeSpan)
				{
					MediaElement.Stop();
					MediaElement.Position = TimeSpan.Zero;
				}

				MediaElement.Play();
			}
		}

		private void Pause_Click(object sender, RoutedEventArgs e)
		{
			Button Button = (Button)sender;
			MediaElement MediaElement = (MediaElement)Button.Tag;
			MediaElement?.Pause();
		}

		private void Stop_Click(object sender, RoutedEventArgs e)
		{
			Button Button = (Button)sender;
			MediaElement MediaElement = (MediaElement)Button.Tag;
			MediaElement?.Stop();
		}

		private void Forward_Click(object sender, RoutedEventArgs e)
		{
			Button Button = (Button)sender;
			MediaElement MediaElement = (MediaElement)Button.Tag;

			if (!(MediaElement is null))
			{
				if (MediaElement.SpeedRatio <= 0)
					MediaElement.SpeedRatio = 1;
				else if (MediaElement.SpeedRatio < 32)
					MediaElement.SpeedRatio *= 2;
			}
		}

		private Control Layout(Panel Container, TextMultiField Field, DataForm _)
		{
			TextBox TextBox = this.LayoutTextBox(Container, Field);
			TextBox.TextChanged += this.TextBox_TextChanged;
			TextBox.AcceptsReturn = true;
			TextBox.AcceptsTab = true;
			TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			TextBox.TextWrapping = TextWrapping.Wrap;

			return TextBox;
		}

		private Control Layout(Panel Container, TextPrivateField Field, DataForm _)
		{
			this.LayoutControlLabel(Container, Field);

			PasswordBox PasswordBox = new PasswordBox()
			{
				Name = VarToName(Field.Var),
				Password = Field.ValueString,
				IsEnabled = !Field.ReadOnly,
				ToolTip = Field.Description,
				Margin = new Thickness(0, 0, 0, 5)
			};

			if (Field.HasError)
				PasswordBox.Background = new SolidColorBrush(Colors.PeachPuff);
			else if (Field.NotSame)
				PasswordBox.Background = new SolidColorBrush(Colors.LightGray);

			PasswordBox.PasswordChanged += this.PasswordBox_PasswordChanged;

			Container.Children.Add(PasswordBox);
			PasswordBox.Tag = this.LayoutErrorLabel(Container, Field);

			return PasswordBox;
		}

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (!(sender is PasswordBox PasswordBox))
				return;

			string Var = NameToVar(PasswordBox.Name);
			Field Field = this.form[Var];
			if (Field is null)
				return;

			Field.SetValue(PasswordBox.Password);

			TextBlock ErrorLabel = (TextBlock)PasswordBox.Tag;

			if (Field.HasError)
			{
				PasswordBox.Background = new SolidColorBrush(Colors.PeachPuff);
				this.OkButton.IsEnabled = false;

				if (!(ErrorLabel is null))
				{
					ErrorLabel.Text = Field.Error;
					ErrorLabel.Visibility = Visibility.Visible;
				}
			}
			else
			{
				PasswordBox.Background = null;
				
				if (!(ErrorLabel is null))
					ErrorLabel.Visibility = Visibility.Collapsed;
				
				this.CheckOkButtonEnabled();
			}
		}

		private Control Layout(Panel Container, TextSingleField Field, DataForm _)
		{
			TextBox TextBox = this.LayoutTextBox(Container, Field);
			TextBox.TextChanged += this.TextBox_TextChanged;

			return TextBox;
		}

		private TextBox LayoutTextBox(Panel Container, Field Field)
		{
			this.LayoutControlLabel(Container, Field);

			TextBox TextBox = new TextBox()
			{
				Name = VarToName(Field.Var),
				Text = Field.ValueString,
				IsReadOnly = Field.ReadOnly,
				ToolTip = Field.Description,
				Margin = new Thickness(0, 0, 0, 5)
			};

			if (Field.HasError)
				TextBox.Background = new SolidColorBrush(Colors.PeachPuff);
			else if (Field.NotSame)
				TextBox.Background = new SolidColorBrush(Colors.LightGray);

			Container.Children.Add(TextBox);
			TextBox.Tag = this.LayoutErrorLabel(Container, Field);

			return TextBox;
		}

		private TextBlock LayoutErrorLabel(Panel Container, Field Field)
		{
			TextBlock ErrorLabel = new TextBlock()
			{
				TextWrapping = TextWrapping.Wrap,
				Margin = new Thickness(0, 0, 0, 5),
				Text = Field.Error,
				Foreground = new SolidColorBrush(Colors.Red),
				FontWeight = FontWeights.Bold,
				Visibility = Field.HasError ? Visibility.Visible : Visibility.Collapsed
			};

			Container.Children.Add(ErrorLabel);

			return ErrorLabel;
		}

		private bool LayoutControlLabel(Panel Container, Field Field)
		{
			if (string.IsNullOrEmpty(Field.Label) && !Field.Required)
				return false;
			else
			{
				TextBlock TextBlock = new TextBlock()
				{
					TextWrapping = TextWrapping.Wrap,
					Text = Field.Label,
					Margin = new Thickness(0, 5, 0, 0)
				};

				Container.Children.Add(TextBlock);

				if (Field.Required)
				{
					Run Run = new Run("*");
					TextBlock.Inlines.Add(Run);
					Run.Foreground = new SolidColorBrush(Colors.Red);
				}

				return true;
			}
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!(sender is TextBox TextBox))
				return;

			string Var = NameToVar(TextBox.Name);
			Field Field = this.form[Var];
			if (Field is null)
				return;

			TextBlock ErrorLabel = (TextBlock)TextBox.Tag;

			Field.SetValue(TextBox.Text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n'));

			if (Field.HasError)
			{
				TextBox.Background = new SolidColorBrush(Colors.PeachPuff);
				this.OkButton.IsEnabled = false;
				if (!(ErrorLabel is null))
				{
					ErrorLabel.Text = Field.Error;
					ErrorLabel.Visibility = Visibility.Visible;
				}
			}
			else
			{
				TextBox.Background = null;
				
				if (!(ErrorLabel is null))
					ErrorLabel.Visibility = Visibility.Collapsed;
				
				this.CheckOkButtonEnabled();
			}
		}

		private void Layout(Panel Container, ReportedReference _, DataForm Form)
		{
			if (Form.Records.Length == 0 || Form.Header.Length == 0)
				return;

			Dictionary<string, int> VarIndex = new Dictionary<string, int>();
			ColumnDefinition ColumnDefinition;
			RowDefinition RowDefinition;
			TextBlock TextBlock;
			int i, j;

			Brush BorderBrush = new SolidColorBrush(Colors.Gray);
			Brush Bg1 = new SolidColorBrush(Color.FromArgb(0x20, 0x40, 0x40, 0x40));
			Brush Bg2 = new SolidColorBrush(Color.FromArgb(0x10, 0x80, 0x80, 0x80));
			Border Border;
			Grid Grid = new Grid();
			Container.Children.Add(Grid);

			i = 0;
			foreach (Field Field in Form.Header)
			{
				ColumnDefinition = new ColumnDefinition()
				{
					Width = GridLength.Auto
				};

				Grid.ColumnDefinitions.Add(ColumnDefinition);

				VarIndex[Field.Var] = i++;
			}

			RowDefinition = new System.Windows.Controls.RowDefinition()
			{
				Height = GridLength.Auto
			};

			Grid.RowDefinitions.Add(RowDefinition);

			foreach (Field[] Row in Form.Records)
			{
				RowDefinition = new RowDefinition()
				{
					Height = GridLength.Auto
				};

				Grid.RowDefinitions.Add(RowDefinition);
			}

			foreach (Field Field in Form.Header)
			{
				if (!VarIndex.TryGetValue(Field.Var, out i))
					continue;

				Border = new Border();
				Grid.Children.Add(Border);

				Grid.SetColumn(Border, i);
				Grid.SetRow(Border, 0);

				Border.BorderBrush = BorderBrush;
				Border.BorderThickness = new Thickness(1);
				Border.Padding = new Thickness(5, 1, 5, 1);
				Border.Background = Bg1;

				TextBlock = new TextBlock()
				{
					FontWeight = FontWeights.Bold,
					Text = Field.Label
				};

				Border.Child = TextBlock;
			}

			j = 0;
			foreach (Field[] Row in Form.Records)
			{
				j++;

				foreach (Field Field in Row)
				{
					if (!VarIndex.TryGetValue(Field.Var, out i))
						continue;

					Border = new Border();
					Grid.Children.Add(Border);

					Grid.SetColumn(Border, i);
					Grid.SetRow(Border, j);

					Border.BorderBrush = BorderBrush;
					Border.BorderThickness = new Thickness(1);
					Border.Padding = new Thickness(5, 1, 5, 1);

					if ((j & 1) == 1)
						Border.Background = Bg2;
					else
						Border.Background = Bg1;

					TextBlock = new TextBlock()
					{
						Text = Field.ValueString
					};

					Border.Child = TextBlock;
				}
			}
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			this.form.Submit();

			this.DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.form.Cancel();

			this.DialogResult = false;
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			if (this.makeVisible != null)
			{
				LinkedList<FrameworkElement> List = new LinkedList<FrameworkElement>();

				while (this.makeVisible != null)
				{
					List.AddFirst(this.makeVisible);
					this.makeVisible = this.makeVisible.Parent as FrameworkElement;
				}

				foreach (FrameworkElement E in List)
				{
					if (E.Focusable)
						E.Focus();
					else
						E.BringIntoView();
				}
			}
		}

		private static string VarToName(string Var)
		{
			return "Form_" + Var.Replace("#", "__GATO__");
		}

		private static string NameToVar(string Name)
		{
			return Name.Substring(5).Replace("__GATO__", "#");
		}

		// TODO: Color picker.
		// TODO: Dynamic forms & post back

	}
}
