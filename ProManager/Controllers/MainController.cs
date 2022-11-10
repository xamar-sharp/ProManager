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
        private readonly TaskCommentDtoValidator _commentValidator;
        private readonly TaskDtoValidator _taskValidator;
        private readonly SortDtoValidator _sortValidator;
        private readonly ISortManager _sortManager;
        private readonly ITaskNavigator _navigator;
        private readonly ConfigurationMap _config;
        public MainController(Repository repos, TaskCommentDtoValidator commentValidator, TaskDtoValidator taskValidator, SortDtoValidator sortValidator, ISortManager sortManager,ITaskNavigator navigator,IOptions<ConfigurationMap> config)
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
        public IActionResult Index(int skipTasks = 0,bool increase = false)
        {
            if (increase)
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
        public async Task<IActionResult> Edit(string taskName)//НА ЭТОЙ ЖЕ СТРАНИЦЕ СПИСОК С КОММЕНТАРИЯМИ И ФОРМА ДЛЯ СОЗДАНИЯ КОММЕНТА!
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
            }
            return RedirectToAction("Edit", new { taskName = dto.TaskName });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask([FromForm] TaskDto dto)//TargetName is IMMUTABLE
        {
            if (ModelState.IsValid)
            {
                if (_taskValidator.IsValid(dto))
                {
                    if (await _repos.UpdateTask(dto.TargetName, dto.StartDate, dto.CancelDate))
                    {
                        await _repos.UpdateProjectState(dto.TargetName);
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Database Error!");
                }
                ModelState.AddModelError("", "Task has invalid struct!");
            }
            return View("Edit", model: dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SortTasks([FromForm] SortDto dto)
        {
            if (_sortValidator.IsValid(dto))
            {
                IEnumerable<TaskModel> results = new List<TaskModel>(2);
                if (dto.SortByDate)
                {
                    results.ToList().AddRange(_sortManager.SortByCreateDate(await _repos.GetAllTasks()));
                }
                if (dto.SortByProjectName)
                {
                    results.ToList().AddRange(_sortManager.SortByProject(await _repos.GetAllTasks()));
                }
                ViewBag.Sorted = results;
            }
            return View("Index");
        }
    }
}
