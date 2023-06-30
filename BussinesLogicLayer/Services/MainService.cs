using UP.Core.Contracts;
using UP.Core.Contracts.Services;
using UP.Core.Models;

namespace UP.Services.Services;

public class MainService : IMainService
{
    private readonly IUnitOfWork unitOfWork;

    public MainService(IUnitOfWork unitOfWork)
    => this.unitOfWork = unitOfWork;

    public async Task<IEnumerable<MatchModel>> GetAllMatches()
    {
        DateTime now = DateTime.Now;
        DateTime twentyFourHoursLater = now.AddHours(24);

        var rawMatchDataList = await unitOfWork.Matches.GetAllAsync();
        var rawBetsDataList = await unitOfWork.Bets.GetAllAsync();
        List<MatchModel> tempModelList = new List<MatchModel>();
        List<MatchModel> readyModelList = new List<MatchModel>();

        foreach (var match in rawMatchDataList)
        {
            var betEntitiesList = await unitOfWork.Main.GetAllBetsByMatchIdAsync(match.Id);
            var betModelsList = new List<BetModel>();

            foreach (var bet in betEntitiesList)
            {
                var oddEntitiesList = await unitOfWork.Main.GetAllOddsByBetIdAsync(bet.Id);
                var oddModelsList = new List<OddModel>();

                foreach (var odd in oddEntitiesList)
                {
                    oddModelsList.Add(new OddModel
                    {
                        Id = odd.Id,
                        Name = odd.Name,
                        Value = odd.Value,
                        SpecialBetValue = odd.SpecialBetValue,
                        BetModelId = bet.Id,
                        IsActive = bet.IsActive,
                    });
                }

                betModelsList.Add(new BetModel
                {
                    Id = bet.Id,
                    Name = bet.Name,
                    IsLive = bet.IsLive,
                    MatchModelId = bet.MatchEntityId,
                    OddModels = oddModelsList,
                    IsActive = bet.IsActive
                });
            }

            tempModelList.Add(new MatchModel
            {
                Id = match.Id,
                Name = match.Name,
                EventModelId = match.EventEntityId,
                MatchType = match.MatchType,
                StartDate = match.StartDate,
                BetModels = betModelsList,
                IsActive = match.IsActive,
            });
        }


        readyModelList = tempModelList.Where(a => a.StartDate >= now && a.StartDate <= twentyFourHoursLater).ToList();

        return readyModelList;
    }

    public async Task<SingleMatchModel> GetSingleMatchByIdAsync(int id)
    {
        var matchEntity = await unitOfWork.Main.GetSingleMatchByIdAsync(id);

        if (matchEntity != null)
        {
            if (matchEntity.IsActive)
            {
                var activeBetsEntities = matchEntity.Bets.Where(bet => bet.IsActive).ToList();
                var activeOddsEntities = activeBetsEntities.SelectMany(bet => bet.Odds).Where(odd => odd.IsActive).ToList();

                var activeBetsModels = new List<BetModel>();
                var activeOddsModels = new List<OddModel>();

                foreach (var bet in activeBetsEntities)
                {
                    activeBetsModels.Add(new BetModel
                    {
                        Id = bet.Id,
                        Name = bet.Name,
                        IsActive = bet.IsActive,
                        IsLive = bet.IsLive
                    });
                }

                foreach (var bet in activeOddsEntities)
                {
                    activeOddsModels.Add(new OddModel
                    {
                        Id = bet.Id,
                        Name = bet.Name,
                        Value = bet.Value,
                        SpecialBetValue = bet.SpecialBetValue,
                        IsActive = bet.IsActive,
                    });
                }

                var matchModel = new SingleMatchModel
                {
                    Id = matchEntity.Id,
                    Name = matchEntity.Name,
                    MatchType = matchEntity.MatchType,
                    StartDate = matchEntity.StartDate,
                    BetModels = activeBetsModels,
                    OddModels = activeOddsModels,
                };

                return matchModel;
            }
            else
            {
                var activeBetsEntities = matchEntity.Bets.ToList();
                var activeOddsEntities = activeBetsEntities.SelectMany(bet => bet.Odds).ToList();

                var activeBetsModels = new List<BetModel>();
                var activeOddsModels = new List<OddModel>();

                foreach (var bet in activeBetsEntities)
                {
                    activeBetsModels.Add(new BetModel
                    {
                        Id = bet.Id,
                        Name = bet.Name,
                        IsActive = bet.IsActive,
                        IsLive = bet.IsLive
                    });
                }

                foreach (var bet in activeOddsEntities)
                {
                    activeOddsModels.Add(new OddModel
                    {
                        Id = bet.Id,
                        Name = bet.Name,
                        Value = bet.Value,
                        SpecialBetValue = bet.SpecialBetValue,
                        IsActive = bet.IsActive,
                    });
                }

                var matchModel = new SingleMatchModel
                {
                    Id = matchEntity.Id,
                    Name = matchEntity.Name,
                    MatchType = matchEntity.MatchType,
                    StartDate = matchEntity.StartDate,
                    BetModels = activeBetsModels,
                    OddModels = activeOddsModels,
                };

                return matchModel;
            }
        }

        return new SingleMatchModel {
            Id = 0,
        };
    }
}
