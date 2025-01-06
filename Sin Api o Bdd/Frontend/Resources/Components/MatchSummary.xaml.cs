namespace Frontend.Resources.Components;
using Frontend.Resources.Entities;

public partial class MatchSummary : ContentView
{
	public MatchSummary()
	{
		//InitializeComponent();
	}

    public void BindData(Match_Dto match)
    {
        var tournament = getTournament(match.IdTournament);
        var teamA = getTeam(match.IdTeamA);
        var teamB = getTeam(match.IdTeamB);

        BindingContext = new
        {
            TeamAName = teamA.Name,
            TeamBName = teamB.Name,
            TeamAScore = match.GoalsTeamA,
            TeamBScore = match.GoalsTeamB,
            MatchDate = match.Date,
            TournamentName = tournament.Name
        };
    }
}