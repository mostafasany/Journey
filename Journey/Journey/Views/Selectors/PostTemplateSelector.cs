using Xamarin.Forms;

namespace Journey.Views.Selectors
{
    public class PostTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PostTemplate { get; set; }
        public DataTemplate AdsTemplate { get; set; }
        public DataTemplate CampaignTemplate { get; set; }

        public DataTemplate PeopleYouKnowTemplate { get; set; }

        // public DataTemplate ScaleMeasurmentsTemplate { get; set; }
        public DataTemplate WeeklyTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) => PostTemplate;
    }
}