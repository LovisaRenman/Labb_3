using Labb_3.Model;
using System.Collections.ObjectModel;

namespace Labb_3.ViewModel
{
    class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack model;
        public ObservableCollection<Question> Questions { get; set; }

        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            this.Questions = new ObservableCollection<Question>(model.Questions);
        }

        public string Name 
        {
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty 
        { get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
    }
}
