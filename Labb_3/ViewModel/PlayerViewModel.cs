using Labb_3.Command;
using Labb_3.Model;
using System.Diagnostics;
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

        public int CorrectAnswers { get; set; }
        public int CurrentQuestionIndex { get; set; }
        public int QuestionsCount { get; set; }

        public DispatcherTimer Timer;
        
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
            CorrectAnswers = 0;

            StartPlayModeCommand = new DelegateCommand(StartPlayMode, StartPlayModeActive);

            ClickButtonCommand0 = new DelegateCommand(ClickAnswer0);
            ClickButtonCommand1 = new DelegateCommand(ClickAnswer1);
            ClickButtonCommand2 = new DelegateCommand(ClickAnswer2);
            ClickButtonCommand3 = new DelegateCommand(ClickAnswer3);
        }

        private async void ClickAnswer3(object obj)
        {
            Timer.Stop();
            IndexOfCorrectAnswer = AnswerOrderByRandom.IndexOf(correctAnswer);
            if (IndexOfCorrectAnswer != 3) Xmark3 = true;
            else CorrectAnswers++;
            await MarkCorrectAnswerTask();

            Xmark3 = false;
            ShowNextQuestion(QuestionsCount);
        }

        private async void ClickAnswer2(object obj)
        {
            IndexOfCorrectAnswer = AnswerOrderByRandom.IndexOf(correctAnswer);
            Timer.Stop();
            if (IndexOfCorrectAnswer != 2) Xmark2 = true;
            else CorrectAnswers++;
            await MarkCorrectAnswerTask();

            Xmark2 = false;
            ShowNextQuestion(QuestionsCount);
        }

        private async void ClickAnswer1(object obj)
        {
            IndexOfCorrectAnswer = AnswerOrderByRandom.IndexOf(correctAnswer);
            Timer.Stop();
            if (IndexOfCorrectAnswer != 1) Xmark1 = true;
            else CorrectAnswers++;
            await MarkCorrectAnswerTask();

            Xmark1 = false;
            ShowNextQuestion(QuestionsCount);
        }

        private async void ClickAnswer0(object obj)
        {
            IndexOfCorrectAnswer = AnswerOrderByRandom.IndexOf(correctAnswer);
            Timer.Stop();
            if (IndexOfCorrectAnswer != 0) Xmark0 = true;
            else CorrectAnswers++;
            await MarkCorrectAnswerTask();

            Xmark0 = false;
            ShowNextQuestion(QuestionsCount);
        }

        private void StartPlayMode(object obj)
        {
            mainWindowViewModel.ConfigurationViewModel.AddButtonCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.ConfigurationViewModel.RemoveButtonCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.ConfigurationViewModel.BtnOptionsOpenCommand.RaiseCanExecuteChanged();

            StartPlayModeCommand.RaiseCanExecuteChanged();
            mainWindowViewModel.ConfigurationViewModel.StartEditModeCommand.RaiseCanExecuteChanged();

            VisibilityModePlayerEndView = false;
            RaisePropertyChanged("VisibilityModePlayerEndView");
            mainWindowViewModel.ConfigurationViewModel.VisibilityModeConfigurationView = false;
            mainWindowViewModel.RaisePropertyChanged("VisibilityModeConfigurationView");
            CheckMarksInvisible();
            XmarksInvisible();
            
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            
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
            Timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            TimerTick--;

            if (TimerTick == 0)
            {
                Timer.Stop();
                await MarkCorrectAnswerTask();
                ShowNextQuestion(QuestionsCount);
            }
        }

        private void EndOfQuestions()
        {
            VisibilityModePlayerView = false;

            AmountOfCorrectAnswers = $"You got {CorrectAnswers} answer correct out of {ActivePack.Questions.Count()} questions";

            VisibilityModePlayerEndView = true;
        }

        private async Task MarkCorrectAnswerTask()
        {
            IndexOfCorrectAnswer = AnswerOrderByRandom.IndexOf(correctAnswer);

            if (IndexOfCorrectAnswer == 0) Checkmark0 = true;
            else if (IndexOfCorrectAnswer == 1) Checkmark1 = true;                           
            else if (IndexOfCorrectAnswer == 2) Checkmark2 = true;
            else if (IndexOfCorrectAnswer == 3) Checkmark3 = true;
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
        }

        private void XmarksInvisible()
        {
            Xmark0 = false;
            Xmark1 = false;
            Xmark2 = false;
            Xmark3 = false;
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

        private string _amountOfCorrectAnswers;

        public string AmountOfCorrectAnswers
        {
            get => _amountOfCorrectAnswers; 
            set 
            {
                _amountOfCorrectAnswers = value;
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

        private bool _xmark0;
        public bool Xmark0
        {
            get => _xmark0;
            set 
            {
                _xmark0 = value;
                RaisePropertyChanged("Xmark0");
            }
        }

        private bool _xmark1;
        public bool Xmark1
        {
            get => _xmark1;
            set
            {
                _xmark1 = value;
                RaisePropertyChanged("Xmark1");
            }
        }

        private bool _xmark2;
        public bool Xmark2
        {
            get => _xmark2;
            set
            {
                _xmark2 = value;
                RaisePropertyChanged("Xmark2");
            }
        }

        private bool _xmark3;
        public bool Xmark3
        {
            get => _xmark3;
            set
            {
                _xmark3 = value;
                RaisePropertyChanged("Xmark3");
            }
        }
        public int IndexOfCorrectAnswer { get; set; }
    }
}
