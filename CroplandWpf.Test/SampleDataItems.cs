using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CroplandWpf.Test
{
	public enum PersonalityType
	{
		NA,
		Introvert,
		Extrovert,
		AnnoyingCryBaby
	}

	public class Person : ViewModelBase
	{
		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged("Name");
				}
			}
		}
		private string _name;

		public int InternalNumber
		{
			get { return _internalNumber; }
			set
			{
				if (_internalNumber != value)
				{
					_internalNumber = value;
					OnPropertyChanged("InternalNumber");
				}
			}
		}
		private int _internalNumber;

		public bool IsMedic
		{
			get
			{
				return _isMedic;
			}
			set
			{
				if (_isMedic != value)
				{
					_isMedic = value;
					OnPropertyChanged("IsMedic");
				}
			}
		}
		private bool _isMedic;

		public PersonalityType PersonalityType
		{
			get
			{
				return _personalityType;
			}
			set
			{
				if (_personalityType != value)
				{
					_personalityType = value;
					OnPropertyChanged("PersonalityType");
				}
			}
		}
		private PersonalityType _personalityType;

		public Person()
		{

		}
	}

	public class FileItem : ViewModelBase
	{
		public string Name { get; set; }

		public double Size_Mb { get; set; }

		public string Path { get; set; }

		public FileItem() { }
	}
}
