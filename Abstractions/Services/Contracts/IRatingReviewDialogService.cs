using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IRatingReviewDialogService
    {
        string FeedbackEmail { get; set; }
        string StoreId { get; set; }
        Task<bool> FeedbackAsync(string emailSubject = "", string emailBody = "");
        Task ShowFullRateFeebackDialogsync();
        Task<bool> ShowNativeRatingReviewAsync();
        Task<bool> ShowRatingReviewAsync();
    }
}