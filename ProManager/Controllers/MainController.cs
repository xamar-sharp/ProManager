using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProManager.Models;
using ProManager.Services;
using ProManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace ProManager.Controllers
{
    public sealed class MainController : Controller
    {
        private readonly Repository _repos;
        private readonly IModelValidator<TaskCommentDto> _commentValidator;
        private readonly IModelValidator<TaskDto> _taskValidator;
        private readonly IModelValidator<SortDto> _sortValidator;
        private readonly ISortManager _sortManager;
        private readonly ITaskNavigator _navigator;
        private readonly ConfigurationMap _config;
        public MainController(Repository repos, IModelValidator<TaskCommentDto> commentValidator, IModelValidator<TaskDto> taskValidator, IModelValidator<SortDto> sortValidator, ISortManager sortManager, ITaskNavigator navigator, IOptions<ConfigurationMap> config)
        {
            _repos = repos;
            _commentValidator = commentValidator;
            _taskValidator = taskValidator;
            _sortValidator = sortValidator;
            _sortManager = sortManager;
            _navigator = navigator;
            _config = config.Value;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int skipTasks = 0, bool increase = false)
        {
            if (increase && await _repos.TasksMoreThan(skipTasks))
            {
                _navigator.IncreaseLoadedCount(HttpContext, _config.TakeTaskCount);
                ViewBag.SkippedTasks = _navigator.TasksLoaded;
            }
            else
            {
                ViewBag.SkippedTasks = skipTasks;
            }
            return View();
        }
        [HttpGet]
        public IActionResult TaskCreating()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Sort()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string taskName)
        {
            return View(model: await _repos.FindTaskByName(taskName));
        }
        [HttpGet]
        public async Task<IActionResult> ReadTask(string taskName)
        {
            return View(model: await _repos.FindTaskByName(taskName));
        }
        [HttpGet]
        public async Task<IActionResult> ReadComment(Guid id)
        {
            return View(model: await _repos.FindCommentById(id));
        }
        [HttpGet]
        public async Task<IActionResult> DropComment(string taskName, Guid id)
        {
            if (await _repos.DeleteComment(id))
            {
                await _repos.UpdateTaskState(taskName);
            }
            return RedirectToAction("Edit", new { taskName = taskName });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProjectOrTask([FromForm] TaskDto dto)
        {
            if (ModelState.IsValid)
            {
                if (_taskValidator.IsValid(dto))
                {
                    if (dto.IsProject)
                    {
                        if (!await _repos.CreateNewProject(dto.TargetName))
                        {
                            ModelState.AddModelError("", "Database Error!");
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        if (!await _repos.CreateNewTask(dto.ProjectName, dto.TargetName, dto.StartDate, dto.CancelDate))
                        {
                            ModelState.AddModelError("", "Database Error!");
                        }
                        else
                        {
                            await _repos.UpdateProjectState(dto.TargetName);
                            return RedirectToAction("Index");
                        }
                    }
                }
                ModelState.AddModelError("", "Task has invalid struct!");
            }
            return View("TaskCreating", dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTaskComment([FromForm] TaskCommentDto dto)
        {
            if (ModelState.IsValid)
            {
                if (_commentValidator.IsValid(dto))
                {
                    if (!await _repos.CreateNewTaskComment(dto.TaskName, dto.IsFile ? dto.File : dto.Text))
                    {
                        ModelState.AddModelError("", "Database Error!");
                    }
                    else
                    {
                        await _repos.UpdateTaskState(dto.TaskName);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Comment has invalid struct!");
                }
            }
            return RedirectToAction("Edit", new { taskName = dto.TaskName });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask([FromForm] TaskModel model)
        {
            if (ModelState.IsValid)
            {
                if (_taskValidator.IsValidModel(model))
                {
                    if (await _repos.UpdateTask(model.TaskName, model.StartDate, model.CancelDate))
                    {
                        await _repos.UpdateProjectState(model.TaskName);
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Database Error!");
                }
                ModelState.AddModelError("", "Invalid time range!");
            }
            return View("Edit", model: model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SortTasks([FromForm] SortDto dto)
        {
            if (_sortValidator.IsValid(dto))
            {
                List<TaskModel> results = new List<TaskModel>(2);
                if (dto.SortByDate)
                {
                    results.AddRange(_sortManager.SortByCreateDate(await _repos.GetAllTasks()));
                }
                if (dto.SortByProjectName)
                {
                    results.AddRange(_sortManager.SortByProject(await _repos.GetAllTasks()));
                }
                ViewBag.Sorted = results;
            }
            return View("Index");
        }
    }
}
