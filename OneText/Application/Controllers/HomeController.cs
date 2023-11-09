﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneText.Application.Database;
using ILogger = Spark.Library.Logging.ILogger;

namespace OneText.Application.Controllers;
public class HomeController : Controller
{
    private readonly ILogger _logger;
    private readonly DatabaseContext _db;

    public HomeController(ILogger logger, DatabaseContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        return Ok("Welcome to Spark Api");
    }

    [HttpGet, Authorize]
    [Route("dashboard")]
    public IActionResult Dashboard()
    {
        return Ok("Dashboard api");
    }
}