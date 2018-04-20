using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CroplandWpf.Exceptions
{
	public class TemplatePartNotFoundException : Exception
	{
		public TemplatePartNotFoundException(string templatePartName, Type exceptionSourceType) : 
			base(String.Format("Template part '{0}' not found within {1}", templatePartName, exceptionSourceType.Name))
		{

		}
	}
}