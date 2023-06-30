using Microsoft.AspNetCore.Mvc;
using UP.Core.Contracts.Services;
using UP.Core.Helpers;
using UP.Core.Models;

namespace UP.Api.Controllers;

[ApiController]
[Route(ControllerConstants.Route)]
public class MainController : ControllerBase
{
    private readonly IMainService mainService;

    public MainController(IMainService mainService)
    => this.mainService = mainService;

    [HttpGet(ControllerConstants.Action)]
    public async Task<IEnumerable<MatchModel>> GetAllActiveMatches()
        => await mainService.GetAllMatches();

    [HttpGet(ControllerConstants.ActionId)]
    public async Task<SingleMatchModel> GetAllActiveMatches(int id)
        => await mainService.GetSingleMatchByIdAsync(id);
}