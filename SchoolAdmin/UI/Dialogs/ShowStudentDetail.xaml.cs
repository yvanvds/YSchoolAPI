using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolAdmin.UI.Dialogs
{
	/// <summary>
	/// Interaction logic for ShowStudentDetail.xaml
	/// </summary>
	public partial class ShowStudentDetail : Window
	{
		public ShowStudentDetail(Models.Student student)
		{
			InitializeComponent();

			Title = student.Name + " " + student.FirstName;

			WisaWisaID.Content = student.WisaAccount?.WisaID;
			ADWisaID.Content = student.DirectoryAccount?.WisaID;
			SSWisaID.Content = student.SmartschoolAccount?.AccountID;

			WisaName.Content = student.WisaAccount?.Name;
			ADName.Content = student.DirectoryAccount?.LastName;
			SSName.Content = student.SmartschoolAccount?.SurName;

			WisaFirstName.Content = student.WisaAccount?.FirstName;
			ADFirstName.Content = student.DirectoryAccount?.FirstName;
			SSFirstName.Content = student.SmartschoolAccount?.GivenName;

			ADLogin.Content = student.DirectoryAccount?.UID;
			SSLogin.Content = student.SmartschoolAccount?.UID;

			WisaClass.Content = student.WisaAccount?.ClassGroup;
			ADClass.Content = student.DirectoryAccount?.ClassGroup;
			SSClass.Content = student.SmartschoolAccount?.Group;

			WisaStemID.Content = student.WisaAccount?.StemID;
			SSStemID.Content = student.SmartschoolAccount?.StemID;

			WisaStateID.Content = student.WisaAccount?.StateID;
			SSStateID.Content = student.SmartschoolAccount?.RegisterID;

			WisaDateBirth.Content = student.WisaAccount?.DateOfBirth.ToString("dd-MM-yyyy");
			SSDateBirth.Content = student.SmartschoolAccount?.Birthday.ToString("dd-MM-yyyy");

			WisaPlaceBirth.Content = student.WisaAccount?.PlaceOfBirth;
			SSPlaceBirth.Content = student.SmartschoolAccount?.BirthPlace;

			WisaNationality.Content = student.WisaAccount?.Nationality;
			//SSNationality.Content = student.SmartschoolAccount?.

			WisaGender.Content = student.WisaAccount?.Gender.ToString();
			SSGender.Content = student.SmartschoolAccount?.Gender.ToString();

			WisaStreet.Content = student.WisaAccount?.Street;
			SSStreet.Content = student.SmartschoolAccount?.Street;

			WisaHouseNumber.Content = student.WisaAccount?.HouseNumber;
			SSHouseNumber.Content = student.SmartschoolAccount?.HouseNumber;

			WisaBusNumber.Content = student.WisaAccount?.HouseNumberAdd;
			SSBusNumber.Content = student.SmartschoolAccount?.HouseNumberAdd;

			WisaCity.Content = student.WisaAccount?.City;
			SSCity.Content = student.SmartschoolAccount?.City;

			WisaPostalCode.Content = student.WisaAccount?.PostalCode;
			SSPostalCode.Content = student.SmartschoolAccount?.PostalCode;
		}
	}
}
