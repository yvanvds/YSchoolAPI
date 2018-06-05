using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolAdmin.Properties;

namespace SchoolAdmin.Models
{
	public class Connector
	{
		private bool connected = false;
		public bool Connected { get => connected; }

		public bool ValidateConfig()
		{
			if (!Settings.Default.UseWisa)
			{
				Global.Log.Add("Sync Met Wisa is Noodzakelijk");
				return false;
			}

			int count = 0;
			if (Settings.Default.UseAD) count++;
			if (Settings.Default.UseSmartschool) count++;
			if (Settings.Default.UseGoogle) count++;
			if (Settings.Default.UseOpenLdap) count++;

			if(count == 0)
			{
				Global.Log.Add("Sync met Wisa en minstens één andere bron");
				return false;
			}

			if(Settings.Default.WisaURL.Length == 0)
			{
				Global.Log.Add("Wisa URL ontbreekt");
				return false;
			}

			if(Settings.Default.WisaPort == 0)
			{
				Global.Log.Add("Wisa port is 0");
				return false;
			}

			if(Settings.Default.WisaDatabase.Length == 0)
			{
				Global.Log.Add("Wisa database ontbreekt");
				return false;
			}

			if(Settings.Default.WisaAccount.Length == 0)
			{
				Global.Log.Add("Wisa account ontbreekt");
				return false;
			}

			if(Settings.Default.WisaPassword.Length == 0)
			{
				Global.Log.Add("Wisa wachtwoord ontbreekt");
				return false;
			}

			if(Settings.Default.WisaActiveSchools.Length == 0)
			{
				Global.Log.Add("Er zijn geen active scholen aangeduid in Wisa");
				return false;
			}

			if (Settings.Default.UseAD)
			{
				if (Settings.Default.ADDomain.Length == 0)
				{
					Global.Log.Add("Active Directory domain is noodzakelijk");
					return false;
				}

				if (Settings.Default.ADAccountPath.Length == 0)
				{
					Global.Log.Add("Active Directory account path is leeg");
					return false;
				}

				if (Settings.Default.ADGroupPath.Length == 0) {
					Global.Log.Add("Active Directory group path is leeg");
					return false;
				}

				if(Settings.Default.ADStudentPath.Length == 0)
				{
					Global.Log.Add("Active Directory path voor leerlingen is leeg");
					return false;
				}

				if(Settings.Default.ADStaffPath.Length == 0)
				{
					Global.Log.Add("Active Directory path voor personeel is leeg");
					return false;
				}
			}

			if(Settings.Default.UseSmartschool)
			{
				if(Settings.Default.SmartschoolURL.Length == 0)
				{
					Global.Log.Add("Smartschool URL ontbreekt");
					return false;
				}

				if(Settings.Default.SmartschoolKey.Length == 0)
				{
					Global.Log.Add("Smartschool API Key ontbreekt");
					return false;
				}

				if(Settings.Default.SmartschoolStudents.Length == 0)
				{
					Global.Log.Add("Smartschool categorie voor leerlingen is leeg");
					return false;
				}

				if(Settings.Default.SmartschoolStaff.Length == 0)
				{
					Global.Log.Add("Smartschool categorie voor personeel is leeg");
					return false;
				}
			}

			if(Settings.Default.UseGoogle)
			{
				if(Settings.Default.GoogleAdminUser.Length == 0)
				{
					Global.Log.Add("Google Admin user ontbreekt");
					return false;
				}

				if(Settings.Default.GoogleAppName.Length == 0)
				{
					Global.Log.Add("Google App Name ontbreekt");
					return false;
				}

				if(Settings.Default.GoogleDomain.Length == 0)
				{
					Global.Log.Add("Google Domein ontbreekt");
					return false;
				}

				if(Settings.Default.GoogleKey.Length == 0
					|| Settings.Default.GoogleSecretID.Length == 0 
					|| Settings.Default.GoogleTokenURI.Length == 0)
				{
					Global.Log.Add("Google Secret ontbreekt");
					return false;
				}
			}

			if (Settings.Default.UseOpenLdap)
			{
				if (Settings.Default.LdapHost.Length == 0)
				{
					Global.Log.Add("Ldap Host ontbreekt");
					return false;
				}

				if (Settings.Default.LdapPort == 0)
				{
					Global.Log.Add("Ldap poort ontbreekt");
					return false;
				}

				if (Settings.Default.LdapAdminDN.Length == 0)
				{
					Global.Log.Add("Ldap Admin DN ontbreekt");
					return false;
				}

				if (Settings.Default.LdapPassword.Length == 0)
				{
					Global.Log.Add("Ldap Password is niet ingesteld");
					return false;
				}

				if (Settings.Default.LdapBaseDN.Length == 0)
				{
					Global.Log.Add("Ldap basis DN is niet ingesteld");
					return false;
				}

				if (Settings.Default.LdapAccountOU.Length == 0) {
					Global.Log.Add("Ldap Account OU is leeg");
					return false;
				}

			}

			return true;
		}

		public async Task<bool> ConnectToWisa()
		{
			Wisa.Connector.Init(
				Properties.Settings.Default.WisaURL,
				Properties.Settings.Default.WisaPort,
				Properties.Settings.Default.WisaAccount,
				Properties.Settings.Default.WisaPassword,
				Properties.Settings.Default.WisaDatabase,
				Global.Log
			);

			bool result = await Wisa.Schools.Load();
			if (!result) return false;

			Wisa.Schools.ActiveSchoolsFromString(Properties.Settings.Default.WisaActiveSchools);
			bool schoolFound = false;
			foreach (var school in Wisa.Schools.All)
			{
				if (school.IsActive)
				{
					schoolFound = true;
				}
			}

			if (!schoolFound)
			{
				Global.Log.Add("Geen actieve scholen gevonden");
				return false;
			}

			return true;
		}

		public void ConnecToSmartschool()
		{
			Smartschool.Connector.DiscardSubgroups = Properties.Settings.Default.SmartschoolDiscardableSubgroups.Split(',');
			Smartschool.Connector.Disconnect();
			Smartschool.Connector.Init(
				Properties.Settings.Default.SmartschoolURL,
				Properties.Settings.Default.SmartschoolKey,
				Global.Log
			);
		}

		public void ConnectToDirectory()
		{
			AD.Connector.AzureDomain = Properties.Settings.Default.ADAzureDomain;
			AD.Connector.TeacherOU = Properties.Settings.Default.ADTeacherOU;
			AD.Connector.SupportOU = Properties.Settings.Default.ADSupportOU;
			AD.Connector.DirectorOU = Properties.Settings.Default.ADDirectorsOU;
			AD.Connector.AdminOU = Properties.Settings.Default.ADAdminsOU;

			if (Properties.Settings.Default.ADGroupGrades)
			{
				AD.Connector.StudentGrade = new string[] {
					Properties.Settings.Default.ADNameGrade1,
					Properties.Settings.Default.ADNameGrade2,
					Properties.Settings.Default.ADNameGrade3
				};
			}

			if (Properties.Settings.Default.ADGroupYear)
			{
				AD.Connector.StudentYear = new string[] {
					Properties.Settings.Default.ADNameYear1,
					Properties.Settings.Default.ADNameYear2,
					Properties.Settings.Default.ADNameYear3,
					Properties.Settings.Default.ADNameYear4,
					Properties.Settings.Default.ADNameYear5,
					Properties.Settings.Default.ADNameYear6,
					Properties.Settings.Default.ADNameYear7
				};
			}

			AD.Connector.Init(
				Properties.Settings.Default.ADDomain,
				Properties.Settings.Default.ADAccountPath,
				Properties.Settings.Default.ADGroupPath,
				Properties.Settings.Default.ADStudentPath,
				Properties.Settings.Default.ADStaffPath,
				Global.Log
				);
		}

		public void ConnectToGoogle()
		{
			Google.Connector.Init(
				Properties.Settings.Default.GoogleAppName,
				Properties.Settings.Default.GoogleAdminUser,
				Properties.Settings.Default.GoogleDomain,
				Properties.Settings.Default.GoogleKey,
				Properties.Settings.Default.GoogleSecretID,
				Properties.Settings.Default.GoogleTokenURI,
				Global.Log
				);
		}

		public void ConnectToLdap()
		{
			OpenLdap.Connector.BaseDN = Properties.Settings.Default.LdapBaseDN;
			OpenLdap.Connector.AccountOU = Properties.Settings.Default.LdapAccountOU;

			OpenLdap.Connector.Init(
				Properties.Settings.Default.LdapHost,
				Properties.Settings.Default.LdapPort,
				Properties.Settings.Default.LdapAdminDN,
				Properties.Settings.Default.LdapPassword,
				Global.Log
				);
		}

		public async Task<bool> ConnectAll()
		{
			connected = false;
			bool result = await ConnectToWisa();
			if (!result) return false;

			if(Settings.Default.UseSmartschool)
			{
				ConnecToSmartschool();
			}

			if(Settings.Default.UseAD)
			{
				ConnectToDirectory();
			}

			if(Settings.Default.UseGoogle)
			{
				ConnectToGoogle();
			}

			if(Settings.Default.UseOpenLdap)
			{
				ConnectToLdap();
			}

			connected = true;
			return true;
		}
	}
}
