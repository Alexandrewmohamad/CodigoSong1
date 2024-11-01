using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace PnT.SongClient.UI.Controls
{
	/// <summary>
	/// Implements a NumericUpDown that also displays unit for the displayed value.
	/// </summary>
	public partial class UnitNumericUpDown : NumericUpDown
	{
		#region Fields
		/// <summary>
		/// The displayed unit text.
		/// </summary>
		private string unit = "";
		
        /// <summary>
		/// Gets or sets the displayed unit text.
		/// </summary>
        [Localizable(true)]
		public string Unit
		{
			get { return unit; }
			set
			{
				//check value
				if (value == null)
				{
					//set empty string
					unit = string.Empty;
					return;
				}

				//trim value
				value = value.Trim();

				//check length after trim
				if (value.Length == 0)
				{
					//set empty string
					unit = string.Empty;
					return;
				}

				//unit can't have just digit chars
				foreach (char currentChar in value.ToCharArray())
				{
					if (!Char.IsDigit(currentChar) &&
						 !currentChar.Equals(' '))
					{
						//found a char that is not a number
						//set unit
						unit = value;
						return;
					}
				}

				//did not find at least one not digit char
				//add parenthesis
				unit = "(" + value + ")";
			}
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor.
		/// </summary>
		public UnitNumericUpDown()
			: base()
		{
			//init components
			InitializeComponent();

			//add unit to base class text property
			base.Text = base.Text + " " + unit;
		}
		#endregion

		#region Overriden methods
		public override string Text
		{
			get
			{
				//get text without unit
				string currentText = base.Text;

				//remove unit if needed
				if (unit.Length > 0 &&
					 currentText.IndexOf(unit) >= 0)
				{
					//remove anything starting from unit position
					currentText = currentText.Substring(
						 0, currentText.IndexOf(unit));
				}

				//return current text
				return currentText.Trim();
			}
			set
			{
				//set text with unit (if any)
				if (unit.Length > 0)
				{
					//set text with unit
					base.Text = value + " " + unit;
				}
				else
				{
					//just set text without unit
					base.Text = value;
				}
			}
		}
		#endregion
	}
}