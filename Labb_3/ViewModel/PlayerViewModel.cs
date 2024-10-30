using Labb_3.Command;
using Labb_3.Model;
using System.Windows;
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

        public bool VisibilityModeConfigurationView
        {
            get => mainWindowViewModel.ConfigurationViewModel.VisibilityModeConfigurationView;
            set
            {
                mainWindowViewModel.ConfigurationViewModel.VisibilityModeConfigurationView = value;
                RaisePropertyChanged();
                mainWindowViewModel.ConfigurationViewModel.RaisePropertyChanged("VisibilityModePlayerView");
            }
        }

        public DelegateCommand StartPlayModeCommand { get; }
        public DelegateCommand ClickButtonCommand0 { get; }
        public DelegateCommand ClickButtonCommand1 { get; }
        public DelegateCommand ClickButtonCommand2 { get; }
        public DelegateCommand ClickButtonCommand3 { get; }

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

            VisibilityModeConfigurationView = false;
            mainWindowViewModel.RaisePropertyChanged("VisibilityModeConfigurationView");
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            if (mainWindowViewModel.ActivePack.Questions.Count > 0) 
            {
                VisibilityModePlayerView = true;
                RaisePropertyChanged("VisibilityModePlayerView");

                for (int i = 0; i < ActivePack.Questions.Count(); i++)                
                {
                    QuestionSlide(i);
                }
            }
            else
            {
                VisibilityModePlayerView = false;
                RaisePropertyChanged("VisibilityModePlayerView");
            }
        }
       
        private void QuestionSlide(int i)
        {
            Query = ActivePack.Questions[i].Query;

            List<string> tempList = new List<string>
            {
                ActivePack.Questions[i].CorrectAnswer, 
                ActivePack.Questions[i].IncorrectAnswers[0],
                ActivePack.Questions[i].IncorrectAnswers[1],
                ActivePack.Questions[i].IncorrectAnswers[2]
            };

            Random rnd = new Random();
            AnswerOrderByRandom = tempList.OrderBy(a => rnd.Next()).ToList();

            TimerTick = ActivePack.TimeLimitInSeconds;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerTick--;

            if (TimerTick == 0) timer.Stop();
        }
        private bool StartPlayModeActive(object? arg)
        {
            if (VisibilityModePlayerView) return false;
            else return true;            
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
    }
}
