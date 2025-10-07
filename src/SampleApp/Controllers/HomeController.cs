using Microsoft.AspNetCore.Mvc;
using SampleApp.Jobs;
using SampleApp.Models;
using SampleApp.Service;

namespace SampleApp.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IMessageService messageService;
	private readonly IJobManager jobManager;

	public HomeController(ILogger<HomeController> logger, IMessageService messageService, IJobManager jobManager)
	{
		_logger = logger;
		this.messageService = messageService;
		this.jobManager = jobManager;
	}

	[HttpGet]
	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public IActionResult Message()
	{
		return View();
	}

	[HttpGet]
	public IActionResult Settings()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> PostMessage(MessageInputModel inputModel)
	{
		if (ModelState.IsValid)
		{
			// Here you would typically process the inputModel, e.g., save it to a database or send it to a service.
			_logger.LogInformation("Controller | Message received: {Message}", inputModel.Value);

			await messageService.PublishAsync(inputModel);
			await jobManager.EnqueueMessageJob(inputModel);

			return RedirectToAction("Message");
		}

		return View("Message", inputModel);
	}
}
