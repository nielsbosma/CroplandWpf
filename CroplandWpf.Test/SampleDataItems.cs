﻿using CroplandWpf.MVVM;
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

		public override string ToString()
		{
			return String.Format("{0} ({1})", Name, InternalNumber);
		}
	}

	public class FileItem : ViewModelBase
	{
		public string Name
		{
			get
			{
				return _name;
			}
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

		public double Size_Mb
		{
			get
			{
				return _size_Mb;
			}
			set
			{
				if(_size_Mb != value)
				{
					_size_Mb = value;
					OnPropertyChanged("Size_Mb");
				}
			}
		}
		private double _size_Mb = 0;

		public string Path { get; set; }

		public FileItem() { }

		public override string ToString()
		{
			return String.Format("{0} ({1})", Name, Size_Mb);
		}
	}

	public class CustomSearchItem : ViewModelBase
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public bool HasBeenFoundBefore { get; set; }

		public CustomSearchItem() { }
	}
}