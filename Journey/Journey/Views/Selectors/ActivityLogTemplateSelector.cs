using Journey.Models.Challenge;
using Xamarin.Forms;

namespace Journey.Views.Selectors
{
    public class ActivityLogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OtherWorkoutTemplate { get; set; }
        public DataTemplate OtherKMTemplate { get; set; }
        public DataTemplate OtherKcalTemplate { get; set; }
        public DataTemplate MeWorkoutTemplate { get; set; }
        public DataTemplate MeKMTemplate { get; set; }
        public DataTemplate MeKcalTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ChallengeWorkoutActivityLog activityWorkoutLog)
            {
                if (activityWorkoutLog.Mine)
                    return MeWorkoutTemplate;
                return OtherWorkoutTemplate;
            }

            else if (item is ChallengeKmActivityLog activityKmLog)
            {
                if (activityKmLog.Mine)
                    return MeKMTemplate;
                return OtherKMTemplate;
            }

            else if (item is ChallengeKcalActivityLog activityKcalLog)
            {
                if (activityKcalLog.Mine)
                    return MeKcalTemplate;
                return OtherKcalTemplate;
            }

            return MeWorkoutTemplate;
        }
    }
}