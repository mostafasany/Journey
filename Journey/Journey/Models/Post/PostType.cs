using Prism.Mvvm;

namespace Journey.Models.Post
{
    public abstract class PostType : BindableBase
    {
        private int id;

        protected string PosterName;

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string PostText => GetPostText();

        public string FormatedExpPoint => $"+ {GetExpPoint()} Exp";

        public int ExpPoint => GetExpPoint();

        protected abstract int GetExpPoint();

        protected abstract string GetPostText();
    }
}
//    public class MediaPostType : PostType
//    {
//        public MediaPostType(string posterName)
//        {
//            PosterName = posterName;
//        }

//        protected override int GetExpPoint()
//        {
//            return 25;
//        }

//        protected override string GetPostText()
//        {
//            return $"{PosterName} added new story";
//        }

//    }

//    public class CheckInPostType : PostType
//    {
//        public CheckInPostType(string posterName)
//        {
//            PosterName = posterName;
//        }
//        private Location location;
//        public Location Location
//        {
//            get => location;
//            set => SetProperty(ref location, value);
//        }

//        protected override int GetExpPoint()
//        {
//            return 50;
//        }

//        protected override string GetPostText()
//        {
//            return $"{PosterName} went to {Location?.Name}";
//        }
//    }

//    public class MeasurmentPostType : PostType
//    {
//        public MeasurmentPostType(string posterName)
//        {
//            PosterName = posterName;
//        }

//        protected override int GetExpPoint()
//        {
//            return 75;
//        }

//        protected override string GetPostText()
//        {
//            return string.Format("{0} added new measurements", PosterName);
//        }
//    }

//    public class MealPostType : PostType
//    {
//        public MealPostType(string posterName)
//        {
//            PosterName = posterName;
//        }


//        private MealEnum meal;
//        public MealEnum Meal
//        {
//            get => meal;
//            set => SetProperty(ref meal, value);
//        }

//        protected override int GetExpPoint()
//        {
//            return 100;
//        }

//        protected override string GetPostText()
//        {
//            return $"{PosterName} had his {Meal}";
//        }
//    }

//}