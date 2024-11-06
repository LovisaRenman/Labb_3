using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3.API
{
    public class CategoryTriviaResponse
    {
        public ObservableCollection<FetchCategories> trivia_categories { get; set; }
    }
}
