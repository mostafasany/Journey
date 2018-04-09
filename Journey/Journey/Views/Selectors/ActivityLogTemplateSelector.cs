using Journey.Models.Challenge;
using Xamarin.Forms;

namespace Journey.Views.Selectors
{
    public class ActivityLogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OtherWorkoutTemplate { get; set; }
        public DataTemplate OtherKMTemplate { get; set; }
        public DataTemplate MeWorkoutTemplate { get; set; }
        public DataTemplate MeKMTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ChallengeWorkoutActivityLog activityLog)
            {
                if (activityLog.Mine)
                    return MeWorkoutTemplate;
                return OtherWorkoutTemplate;
            }

            if (item is ChallengeKmActivityLog activityLog1)
            {
                if (activityLog1.Mine)
                    return MeKMTemplate;
                return OtherKMTemplate;
            }

            return MeWorkoutTemplate;
        }
    }
}