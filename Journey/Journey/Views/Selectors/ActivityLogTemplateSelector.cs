using Journey.Models.Challenge;
using Journey.Models.Post;
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
            if (item is ChallengeWorkoutActivityLog)
            {
                var activityLog = item as ChallengeWorkoutActivityLog;
                if (activityLog.Mine)
                    return MeWorkoutTemplate;
                else
                    return OtherWorkoutTemplate;
            }
            else if (item is ChallengeKmActivityLog)
            {
                var activityLog = item as ChallengeKmActivityLog;
                if (activityLog.Mine)
                    return MeKMTemplate;
                else
                    return OtherKMTemplate;
            }
            return MeWorkoutTemplate;
        }
    }
}