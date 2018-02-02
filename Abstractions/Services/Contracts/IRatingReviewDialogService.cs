using System.Threading.Tasks;

namespace Services.Core
{
    public interface IRatingReviewDialogService
    {
        string FeedbackEmail { get; set; }
        string StoreId { get; set; }
        Task<bool> ShowNativeRatingReviewAsync();
        Task<bool> ShowRatingReviewAsync();
        Task<bool> FeedbackAsync(string emailSubject = "", string emailBody = "");
        Task ShowFullRateFeebackDialogsync();
    }
}