using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using Frontend.Resources.Components;
using System.ComponentModel;

namespace Frontend
{
    public partial class ViewMatchs : ContentPage, INotifyPropertyChanged
    {
        private bool _matchesEmpty;
        public bool MatchesEmpty
        {
            get => _matchesEmpty;
            set
            {
                if (_matchesEmpty != value)
                {
                    _matchesEmpty = value;
                    OnPropertyChanged(nameof(MatchesEmpty));
                }
            }
        }

        public ViewMatchs()
        {
            InitializeComponent();
            BindingContext = this;
            LoadMatches();
        }

        void LoadMatches()
        {
            Match_Dto[] matches = getAllMatch();
            int columns = 2;
            int row = 0, col = 0;

            if (matches != null && matches.Length > 0)
            {
                MatchesEmpty = false;

                foreach (var match in matches)
                {
                    MatchSummary matchSummary = new MatchSummary();
                    matchSummary.BindData(match);

                    Grid.SetRow(matchSummary, row);
                    Grid.SetColumn(matchSummary, col);
                    MatchesGrid.Children.Add(matchSummary);

                    col++;
                    if (col >= columns)
                    {
                        col = 0;
                        row++;
                    }
                }
            }
            else
            {
                MatchesEmpty = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
