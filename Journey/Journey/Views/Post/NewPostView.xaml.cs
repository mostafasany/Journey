using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journey.Views.Post
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPostView : ContentView
	{
		public NewPostView ()
		{
			InitializeComponent ();
		}
	}
}