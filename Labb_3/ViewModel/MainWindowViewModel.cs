using Labb_3.Command;
using Labb_3.Dialogs;
using Labb_3.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Labb_3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        public CreateNewPackDialog CreatePackDialog { get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public APIViewModel APIViewModel { get; }
        public DelegateCommand NewQuestionPackCommand { get; }
        public DelegateCommand BtnCreateCommand { get; }
        public DelegateCommand BtnCancelCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand RemoveQuestionPackCommand { get; }
        public DelegateCommand FullscreenCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand ImportQuestionsCommand { get; }
        public DelegateCommand SaveJsonCommand { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack; 
            set 
            { 
                _activePack = value;
                RaisePropertyChanged("ActivePack");
                ConfigurationViewModel?.RaisePropertyChanged();
                PlayerViewModel?.RaisePropertyChanged();
            }
        }

        private QuestionPackViewModel? _newQuestionPack;
        public QuestionPackViewModel? NewQuestionPack
        {
            get => _newQuestionPack;
            set 
            {
                _newQuestionPack = value;
                RaisePropertyChanged();
            }
        }

        public event EventHandler RequestShowDialogImportQuestions;

        public event EventHandler RequestShowDialogCreateNewPack;
        public event EventHandler RequestCloseDialogCreateNewPack;

        public event EventHandler RequestShowMessageBoxRemove;
        public event EventHandler RequestShowMessageBoxCloseApplication;


        private bool _windowState;

        public bool IsWindowNormal
        {
            get => _windowState; 
            set 
            {
                _windowState = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            StartPacks();

            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
            APIViewModel = new APIViewModel(this);
            APIViewModel.FetchCategories();

            NewQuestionPackCommand = new DelegateCommand(CreateNewQuestionPack);
            BtnCreateCommand = new DelegateCommand(Create);
            BtnCancelCommand = new DelegateCommand(Cancel);
            SetActivePackCommand = new DelegateCommand(SelectActivePackCommand);
            RemoveQuestionPackCommand = new DelegateCommand(RemoveQuestionPack, RemoveQuestionPackActive);
            FullscreenCommand = new DelegateCommand(Fullscreen);
            ExitCommand = new DelegateCommand(Exit);
            ImportQuestionsCommand = new DelegateCommand(ImportQuestions, ImportQuestionsActive);
            SaveJsonCommand = new DelegateCommand(ActionSaveJson);

            IsWindowNormal = true;
        }

        private bool ImportQuestionsActive(object? arg)
        {
            return APIViewModel.hasInternet;
        }

        private void ActionSaveJson(object obj)
        {
            SaveJson();
        }

        private void ImportQuestions(object obj)
        {
            RequestShowDialogImportQuestions.Invoke(this, EventArgs.Empty);
        }

        private async void StartPacks()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "Labb_3");
            path = Path.Combine(path, "Labb_3.json");
            Packs = new ObservableCollection<QuestionPackViewModel>();

            if (Path.Exists(path))
            {
                await LoadJson(path);
                ActivePack = Packs.FirstOrDefault();
            }
            else
            {
                ActivePack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));
                Packs.Add(ActivePack);
            }
        }

        private async Task LoadJson(string path)
        {
            using FileStream openStream = File.OpenRead(path);
            var QuestionPack = JsonSerializer.Deserialize<QuestionPack[]>(openStream, JsonSerializerOptions());

            foreach (var pack in QuestionPack)
            {
                if (pack != null) Packs.Add(new QuestionPackViewModel(pack));
            }
        }

        private async void Exit(object obj)
        {
            await SaveJson();
            RequestShowMessageBoxCloseApplication.Invoke(this, EventArgs.Empty);
        }

        private JsonSerializerOptions JsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                IncludeFields = true,
                IgnoreReadOnlyProperties = false
            };
            return jsonOptions;
        }

        public async Task SaveJson()
        {
            string jsonPacks = JsonSerializer.Serialize(Packs, JsonSerializerOptions());
            
            File.WriteAllText(SavePath(), jsonPacks);
        }

        public string SavePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            path = Path.Combine(path, "Labb_3");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, "Labb_3.json");

            return path;
        }

        private void Fullscreen(object obj)
        {
            if (IsWindowNormal == true) IsWindowNormal = false;
            else if (IsWindowNormal == false) IsWindowNormal = true;
        }

        private bool RemoveQuestionPackActive(object? arg)
        {
            if (Packs.Count > 1) return true;
            else return false;
        }

        private void RemoveQuestionPack(object obj)
        {
            RequestShowMessageBoxRemove.Invoke(this, EventArgs.Empty);            

            if (Packs.Count > 0) ActivePack = Packs.FirstOrDefault();

            SaveJson();
        }

        private void SelectActivePackCommand(object obj)
        {
            ActivePack = (QuestionPackViewModel)obj;
        }

        private void Cancel(object obj)
        {            
            RequestCloseDialogCreateNewPack.Invoke(this, EventArgs.Empty);
        }

        private void Create(object obj)
        {
            RemoveQuestionPackCommand.RaiseCanExecuteChanged();

            Packs.Add(NewQuestionPack);
            ActivePack = NewQuestionPack;
            RequestCloseDialogCreateNewPack.Invoke(this, EventArgs.Empty);
            SaveJson();
        }

        private void CreateNewQuestionPack(object obj)
        {
            NewQuestionPack = new QuestionPackViewModel(new QuestionPack("Default Question Pack"));

            RequestShowDialogCreateNewPack.Invoke(this, EventArgs.Empty);
        }        
    }
}
