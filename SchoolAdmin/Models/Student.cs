using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
	public class Student
	{
		public Wisa.Student WisaAccount = null;
		public AD.Account DirectoryAccount = null;
		public Smartschool.Account SmartschoolAccount = null;
		public List<string> Changes = new List<string>();

		public bool DoADDelete { get; set; } = false;
		public bool DoSSDelete { get; set; } = false;
		public bool DoADCreate { get; set; } = false;
		public bool DoSSCreate { get; set; } = false;
		public bool DoADUpdate { get; set; } = false;
		public bool DoSSUpdate { get; set; } = false;

		public string Name
		{
			get
			{
				if (WisaAccount != null) return WisaAccount.Name;
				if (DirectoryAccount != null) return DirectoryAccount.LastName;
				if (SmartschoolAccount != null) return SmartschoolAccount.SurName;
				return "";
			}
		}

		public string FirstName
		{
			get
			{
				if (WisaAccount != null) return WisaAccount.FirstName;
				if (DirectoryAccount != null) return DirectoryAccount.FirstName;
				if (SmartschoolAccount != null) return SmartschoolAccount.GivenName;
				return "";
			}
		}

		public string AccountName
		{
			get
			{
				if (DirectoryAccount != null) return DirectoryAccount.UID;
				if (SmartschoolAccount != null) return SmartschoolAccount.UID;
				return "";
			}
		}

		public string WisaID
		{
			get
			{
				if (WisaAccount != null) return WisaAccount.WisaID;
				if (DirectoryAccount != null) return DirectoryAccount.WisaID;
				if (SmartschoolAccount != null) return SmartschoolAccount.AccountID;
				return "";
			}
		}

		public string ClassGroup
		{
			get
			{
				if (WisaAccount != null) return WisaAccount.ClassGroup;
				return "";
			}
		}

		public bool HasWisaAccount
		{
			get
			{
				return WisaAccount != null;
			}
		}

		public bool HasSmartschoolAccount
		{
			get
			{
				return SmartschoolAccount != null;
			}
		}

		public bool HasDirectoryAccount
		{
			get
			{
				return DirectoryAccount != null;
			}
		}

		public string ListOfChanges
		{
			get
			{
				string result = "";
				for(int i = 0; i < Changes.Count; i++)
				{
					if (i > 0) result += ", ";
					result += Changes[i];
				}
				return result;
			}
		}
	}
}
