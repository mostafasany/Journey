using Journey.Models.Post;
using Xamarin.Forms;

namespace Journey.Views.Selectors
{
    public class CommentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MeCommentTemplate { get; set; }
        public DataTemplate OtherCommentTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is Comment comment) || comment.Mine)
                return MeCommentTemplate;
            return OtherCommentTemplate;
        }
    }
}