using Labb_3.API;
using Labb_3.Command;
using Labb_3.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Labb_3.ViewModel
{
    class APIViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public ObservableCollection<FetchCategories> Categories { get; set; }
        [JsonPropertyName("trivia_categories")]
        public List<FetchCategories> TriviaCategories { get; set; }

        static readonly HttpClient client = new HttpClient();        
        public DelegateCommand ImportCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private int _activeCategory;
        public int ActiveCategory
        {
            get => _activeCategory;
            set
            {
                _activeCategory = value;
                RaisePropertyChanged();
            }
        }
        private Difficulty _difficulty;
        public Difficulty Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                RaisePropertyChanged();
            }
        }

        private int _amountOfQuestions;
        private string response;

        public int AmountOfQuestions
        {
            get => _amountOfQuestions;
            set
            {
                _amountOfQuestions = value;
                RaisePropertyChanged();
            }
        }


        public APIViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            Categories = new ObservableCollection<FetchCategories>();
            ImportCommand = new DelegateCommand(Import);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel(object obj)
        {
            mainWindowViewModel._importQuestionsDialog?.CloseDialog();
        }

        private async void Import(object obj)
        {
            using (var client = new HttpClient())
            {
                int category = ActiveCategory + 9;
                if (AmountOfQuestions == 0) AmountOfQuestions = 1;

                var url = "https://opentdb.com/api.php?amount=10&category=24&difficulty=easy&type=multiple";

                url = url.Replace("10", AmountOfQuestions.ToString());
                url = url.Replace("24", category.ToString());
                url = url.Replace("easy", Difficulty.ToString().ToLower());

                try
                {
                    MessageBox.Show("Getting Questions");
                    response = await client.GetStringAsync(url);                    
                }
                catch
                {
                    MessageBox.Show("Wait for client to answer", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                var result = JsonConvert.DeserializeObject<QuestionTriviaResponse>(response);

                if (result.response_code == 0)
                {
                    for (int i = 0; i < result.results.Length; i++)
                    {
                        WebUtility.HtmlDecode(result.results[i].question);
                        WebUtility.HtmlDecode(result.results[i].correct_answer);
                        WebUtility.HtmlDecode(result.results[i].incorrect_answers[0]);
                        WebUtility.HtmlDecode(result.results[i].incorrect_answers[1]);
                        WebUtility.HtmlDecode(result.results[i].incorrect_answers[2]);

                        mainWindowViewModel.ActivePack.Questions.Add(new Question(
                            result.results[i].question, 
                            result.results[i].correct_answer, 
                            result.results[i].incorrect_answers));

                        mainWindowViewModel.SaveJson();
                    }
                }
                else
                {
                    MessageBox.Show("There was a problem loading Questions from Open Trivia", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                mainWindowViewModel._importQuestionsDialog?.CloseDialog();
            }
        }

        public async Task FetchCategories()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = "https://opentdb.com/api_category.php";
                    var response = await client.GetStringAsync(url);

                    var result = JsonConvert.DeserializeObject<CategoryTriviaResponse>(response);

                    foreach (var category in result.trivia_categories)
                    {
                        Categories.Add(category);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
}
