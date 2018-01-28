using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ElderVisionFrank
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private async void CameraButton_Clicked(object sender, EventArgs e)
		{
			var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

			if (photo != null)
				PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
		}
	}
}
