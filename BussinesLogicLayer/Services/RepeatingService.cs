using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using UP.Core.Contracts.Services;
using UP.Core.Models;

namespace UP.Services.Services;

public class RepeatingService : IRepeatingService, IDisposable
{
    private const string infoLink = "https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7";

    private readonly ISportService sportService;
    private readonly IEventService eventService;
    private readonly IMatchService matchService;
    private readonly IBetService betService;
    private readonly IOddService oddService;
    private readonly ILogger<RepeatingService> logger;

    List<int> activeEventsList = new List<int>();
    List<int> activeMatchesList = new List<int>();
    List<int> activeBetsList = new List<int>();
    List<int> activeOddsList = new List<int>();

    public RepeatingService(ISportService sportService, IEventService eventService, IMatchService matchService, IBetService betService, IOddService oddService, ILogger<RepeatingService> logger)
    {
        this.sportService = sportService;
        this.eventService = eventService;
        this.matchService = matchService;
        this.betService = betService;
        this.oddService = oddService;
        this.logger = logger;
    }

    public async Task CallLink()
    {
        logger.LogInformation("Getting the FEED");
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(infoLink);
            if (response.IsSuccessStatusCode)
            {
                string xmlContent = await response.Content.ReadAsStringAsync();

                XDocument doc = XDocument.Parse(xmlContent);

                XElement root = doc.Root;

                // TODO ADD TRY PARSE
                await HandleSport(root);
                if (activeEventsList.Count > 0)
                {
                    await HandleStatusEvents();
                }

                if (activeOddsList.Count > 0)
                {
                    await HandleStatusOdds();
                }

                if (activeBetsList.Count > 0)
                {
                    await HandleStatusBets();
                }

                if (activeMatchesList.Count > 0)
                {
                    await HandleStatusMatches();
                }
            }
            else
            {
                throw new Exception("Something isn't right in CALL LINK in HangFireService");
            }

        }
    }

    private async Task HandleSport(XElement rootElement)
    {
        foreach (XElement sportElement in rootElement.Elements("Sport"))
        {
            SportModel sportModel = new SportModel()
            {
                Id = (int)sportElement.Attribute("ID"),
                Name = (string)sportElement.Attribute("Name"),
            };


            if (await sportService.GetByIdAsync(sportModel.Id) == null)
            {
                await sportService.CreateAsync(sportModel);
            }

            await HandleEvent(sportElement, sportModel);
        }
    }

    private async Task HandleEvent(XElement sportElement, SportModel sportModel)
    {
        foreach (XElement eventElement in sportElement.Elements("Event"))
        {
            EventModel eventModel = new EventModel()
            {
                Id = (int)eventElement.Attribute("ID"),
                Name = (string)eventElement.Attribute("Name"),
                IsLive = (bool)eventElement.Attribute("IsLive"),
                CategoryId = (string)eventElement.Attribute("CategoryID"),
                SportModelId = sportModel.Id,
                IsActive = true
            };

            if (await eventService.GetByIdAsync(eventModel.Id) == null)
            {
                await eventService.CreateAsync(eventModel);
            }
            else
            {
                await eventService.UpdateAsync(eventModel);
            }

            activeEventsList.Add(eventModel.Id);

            await HandleMatch(eventElement, eventModel);
        }

    }

    private async Task HandleMatch(XElement eventElement, EventModel eventModel)
    {
        foreach (XElement matchElement in eventElement.Elements("Match"))
        {
            MatchModel matchModel = new MatchModel()
            {
                Id = (int)matchElement.Attribute("ID"),
                Name = (string)matchElement.Attribute("Name"),
                StartDate = (DateTime)matchElement.Attribute("StartDate"),
                MatchType = (string)matchElement.Attribute("MatchType"),
                EventModelId = eventModel.Id,
                IsActive = true
            };

            if (await matchService.GetByIdAsync(matchModel.Id) == null)
            {
                await matchService.CreateAsync(matchModel);
            }
            else
            {
                await matchService.UpdateAsync(matchModel);
            }

            activeMatchesList.Add(matchModel.Id);

            await HandleBets(matchElement, matchModel);
        }

    }

    private async Task HandleBets(XElement matchElement, MatchModel matchModel)
    {
        foreach (XElement betElement in matchElement.Elements("Bet"))
        {
            BetModel betModel = new BetModel()
            {
                Id = (int)betElement.Attribute("ID"),
                Name = (string)betElement.Attribute("Name"),
                IsLive = (bool)betElement.Attribute("IsLive"),
                MatchModelId = matchModel.Id,
                IsActive = true
            };

            if (await betService.GetByIdAsync(betModel.Id) == null)
            {
                await betService.CreateAsync(betModel);
            }
            else
            {
                await betService.UpdateAsync(betModel);
            }

            activeBetsList.Add(betModel.Id);

            await HandleOdds(betElement, betModel);
        }

    }

    private async Task HandleOdds(XElement betElement, BetModel betModel)
    {
        foreach (XElement oddElement in betElement.Elements("Odd"))
        {
            OddModel oddModel = new OddModel
            {
                Id = (int)oddElement.Attribute("ID"),
                Name = (string)oddElement.Attribute("Name"),
                Value = (decimal)oddElement.Attribute("Value"),
                SpecialBetValue = (decimal?)oddElement.Attribute("SpecialBetValue"),
                BetModelId = betModel.Id,
                IsActive = true
            };


            if (await oddService.GetByIdAsync(oddModel.Id) == null)
            {
                await oddService.CreateAsync(oddModel);
            }
            else
            {
                await oddService.UpdateAsync(oddModel);
            }

            activeOddsList.Add(oddModel.Id);


        }

    }

    private async Task HandleStatusEvents()
    {
        var dbEvents = await eventService.GetAllAsync();
        foreach (var dbEvent in dbEvents)
        {
            if (!activeEventsList.Contains(dbEvent.Id))
            {
                await eventService.UpdateAsync(new EventModel
                {
                    Id = dbEvent.Id,
                    Name = dbEvent.Name,
                    CategoryId = dbEvent.CategoryId,
                    SportModelId = dbEvent.SportModelId,
                    IsLive = dbEvent.IsLive,
                    IsActive = false
                });
            }
        }
    }
    private async Task HandleStatusMatches()
    {

        var dbMatches = await matchService.GetAllAsync();
        foreach (var dbMatch in dbMatches)
        {
            if (!activeMatchesList.Contains(dbMatch.Id))
            {
                await matchService.UpdateAsync(new MatchModel
                {
                    Id = dbMatch.Id,
                    Name = dbMatch.Name,
                    BetModels = dbMatch.BetModels,
                    EventModelId = dbMatch.EventModelId,
                    MatchType = dbMatch.MatchType,
                    StartDate = dbMatch.StartDate,
                    IsActive = false
                });
            }
        }
    }


    private async Task HandleStatusBets()
    {
        var dbBets = await betService.GetAllAsync();
        foreach (var dbBet in dbBets)
        {
            if (!activeBetsList.Contains(dbBet.Id))
            {
                await betService.UpdateAsync(new BetModel
                {
                    Id = dbBet.Id,
                    Name = dbBet.Name,
                    IsLive = dbBet.IsLive,
                    MatchModelId = dbBet.MatchModelId,
                    OddModels = dbBet.OddModels,
                    IsActive = false
                });
            }
        }
    }
    private async Task HandleStatusOdds()
    {
        var dbOdds = await oddService.GetAllAsync();
        foreach (var dbOdd in dbOdds)
        {
            if (!activeBetsList.Contains(dbOdd.Id))
            {
                await oddService.UpdateAsync(new OddModel
                {
                    Id = dbOdd.Id,
                    Name = dbOdd.Name,
                    BetModelId = dbOdd.BetModelId,
                    Value = dbOdd.Value,
                    SpecialBetValue = dbOdd.SpecialBetValue,
                    IsActive = false
                });
            }
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
