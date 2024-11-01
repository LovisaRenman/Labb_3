using Labb_3.Command;
using Labb_3.Model;
using System.Windows.Threading;

namespace Labb_3.ViewModel
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack => mainWindowViewModel.ActivePack;

        private bool _visibilityModePlayerView;
        public bool VisibilityModePlayerView
        {
            get => _visibilityModePlayerView;
            set
            {
                _visibilityModePlayerView = value;
                RaisePropertyChanged();
                RaisePropertyChanged("VisibilityModePlayerView");
            }
        }

        private bool _visibilityModePlayerEndView;
        public bool VisibilityModePlayerEndView
        {
            get => _visibilityModePlayerEndView;
            set
            {
                _visibilityModePlayerEndView = value;
                RaisePropertyChanged();
                RaisePropertyChanged("VisibilityModePlayerView");
            }
        }      

        public DelegateCommand StartPlayModeCommand { get; }
        public DelegateCommand ClickButtonCommand0 { get; }
        public DelegateCommand ClickButtonCommand1 { get; }
        public DelegateCommand ClickButtonCommand2 { get; }
        public DelegateCommand ClickButtonCommand3 { get; }


        public int CurrentQuestionIndex { get; set; }
        public int QuestionsCount { get; set; }

        private DispatcherTimer timer;
        private int timerTick;

        public int TimerTick
        {
            get => timerTick; 
            set 
            {
                timerTick = value;
                RaisePropertyChanged();
            }
        }
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            VisibilityModePlayerView = false;
            VisibilityModePlayerEndView = false;

            StartPlayModeCommand = new DelegateCommand(StartPlayMode, StartPlayModeActive);

            ClickButtonCommand0 = new DelegateCommand(ClickAnswer);
            ClickButtonCommand1 = new DelegateCommand(ClickAnswer);
            ClickButtonCommand2 = new DelegateCommand(ClickAnswer);
            ClickButtonCommand3 = new DelegateCommand(ClickAnswer);
        }

        private void ClickAnswer(object obj)
        {
            
        }

        private void StartPlayMode(object obj)
        {
            StartPlayModeCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.ConfigurationViewModel.StartEditModeCommand.RaiseCanExecuteChanged();

            VisibilityModePlayerEndView = false;
            RaisePropertyChanged("VisibilityModePlayerEndView");
            mainWindowViewModel.ConfigurationViewModel.VisibilityModeConfigurationView = false;
            mainWindowViewModel.RaisePropertyChanged("VisibilityModeConfigurationView");
            CheckMarksInvisible();
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            
            VisibilityModePlayerView = true;
            RaisePropertyChanged("VisibilityModePlayerView");

            List<Question> questions = ActivePack.Questions.ToList();
            CurrentQuestionIndex = 0;
            QuestionsCount = questions.Count();
            
            ShowNextQuestion(QuestionsCount);           
        }
        private void ShowNextQuestion(int amountOfQuestions)
        {
            if (CurrentQuestionIndex < amountOfQuestions) 
            {
                int currentQuestion = CurrentQuestionIndex;
                ProgressionOfQuestions = $"{currentQuestion + 1} of {amountOfQuestions}";
                ActiveQuestionProcessing();
                CurrentQuestionIndex++;
            }
            else EndOfQuestions();
        }
       
        private void ActiveQuestionProcessing()
        {
            Query = ActivePack.Questions[CurrentQuestionIndex].Query;
            correctAnswer = ActivePack.Questions[CurrentQuestionIndex].CorrectAnswer;

            List<string> tempList = new List<string>
            {
                ActivePack.Questions[CurrentQuestionIndex].CorrectAnswer, 
                ActivePack.Questions[CurrentQuestionIndex].IncorrectAnswers[0],
                ActivePack.Questions[CurrentQuestionIndex].IncorrectAnswers[1],
                ActivePack.Questions[CurrentQuestionIndex].IncorrectAnswers[2]
            };

            Random rnd = new Random();
            AnswerOrderByRandom = tempList.OrderBy(a => rnd.Next()).ToList();

            TimerTick = ActivePack.TimeLimitInSeconds;
            timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            TimerTick--;

            if (TimerTick == 0)
            {
                timer.Stop();
                await MarkCorrectAnswerAsync();
                ShowNextQuestion(QuestionsCount);
            }
        }

        private void EndOfQuestions()
        {
            VisibilityModePlayerView = false;
            RaisePropertyChanged("VisibilityModePlayerView");

            VisibilityModePlayerEndView = true;
            RaisePropertyChanged("VisibilityModePlayerEndView");
        }

        private async Task MarkCorrectAnswerAsync()
        {
            int IndexOfCorrectAnswer = AnswerOrderByRandom.IndexOf(correctAnswer);

            if (IndexOfCorrectAnswer == 0) 
            {
                Checkmark0 = true;
                RaisePropertyChanged("CheckMark0");
            } 
            else if (IndexOfCorrectAnswer == 1) 
            {
                Checkmark1 = true;
                RaisePropertyChanged("CheckMark1");
            }                            
            else if (IndexOfCorrectAnswer == 2)
            {
                Checkmark2 = true;
                RaisePropertyChanged("Checkmark2");
            }
            else if (IndexOfCorrectAnswer == 3)
            {
                Checkmark3 = true;
                RaisePropertyChanged("Checkmark3");
            }
            await Task.Delay(2000);
            CheckMarksInvisible();
        }

        private bool StartPlayModeActive(object? arg)
        {
            if (VisibilityModePlayerView) return false;
            else if (ActivePack.Questions.Count() == 0) return false;
            else return true;            
        }

        private void CheckMarksInvisible()
        {
            Checkmark0 = false;
            Checkmark1 = false;
            Checkmark2 = false;
            Checkmark3 = false;
            RaisePropertyChanged("Checkmark0");
            RaisePropertyChanged("Checkmark1");
            RaisePropertyChanged("Checkmark2");
            RaisePropertyChanged("Checkmark3");
        }

        private List<string> _answerOrderByRandom;
        public List<string> AnswerOrderByRandom
        {
            get => _answerOrderByRandom;
            set
            {
                _answerOrderByRandom = value;
                RaisePropertyChanged();
            }
        }

        private string _query;
        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                RaisePropertyChanged();
            }
        }

        private string _progressionOfQuestions;
        public string ProgressionOfQuestions
        {
            get => _progressionOfQuestions; 
            set 
            {
                _progressionOfQuestions = value;
                RaisePropertyChanged();
            }
        }

        string correctAnswer { get; set; }

        private bool _checkmark0;
        public bool Checkmark0
        {
            get => _checkmark0; 
            set 
            {
                _checkmark0 = value;
                RaisePropertyChanged();
            }
        }

        private bool _checkmark1;
        public bool Checkmark1
        {
            get => _checkmark1; 
            set 
            {
                _checkmark1 = value;
                RaisePropertyChanged();
            }
        }
        
        private bool _checkmark2;
        public bool Checkmark2
        {
            get => _checkmark2; 
            set 
            {
                _checkmark2 = value;
                RaisePropertyChanged();
            }
        }
        
        private bool _checkmark3;
        public bool Checkmark3
        {
            get => _checkmark3; 
            set 
            {
                _checkmark3 = value;
                RaisePropertyChanged();
            }
        }


    }
}
