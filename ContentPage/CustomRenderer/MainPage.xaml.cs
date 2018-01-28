using System;
using Xamarin.Forms;

namespace CustomRenderer
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			OnTakePhotoButtonClicked(null, null);
		}

		async void OnTakePhotoButtonClicked (object sender, EventArgs e)
		{
			await Navigation.PushAsync (new CameraPage ());
		}
	}
}

