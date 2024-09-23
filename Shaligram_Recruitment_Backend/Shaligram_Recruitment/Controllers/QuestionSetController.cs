using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Model.ViewModels.QuestionSet;
using Shaligram_Recruitment.Model.ViewModels.Student;
using Shaligram_Recruitment.Services.StudentProfile;

namespace Shaligram_Recruitment.Controllers
{
    public class QuestionSetController : Controller
    {
        private readonly IStudentProfileService _studentProfileService;

        public QuestionSetController(IStudentProfileService studentProfileService)
        {
            _studentProfileService = studentProfileService;
        }
        // GET: StudentController
        public async Task<ActionResult> QuestionSetList(string search = "", int page = 1, int pageSize = 10, string sortColumn = "QuestionSetId", string sortDirection = "asc")
        {
            sortDirection = (sortDirection.ToLower() == "desc") ? "desc" : "asc";          
            var pagedResult = await _studentProfileService.GetPageQuestionSet(search, page, pageSize, sortColumn, sortDirection);
            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = pagedResult.TotalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)pagedResult.TotalRecords / pageSize);
            ViewBag.QuestionSet = await _studentProfileService.GetQuestionSet();
         
            return View(pagedResult);
        }
        [HttpPost]
        public async Task<ActionResult> QuestionSetList(DataTableAjaxPostModel model)
        {
            var pagedResult = await _studentProfileService.GetPageQuestionSet(
                model.Search?.Value ?? string.Empty,
                model.Start / model.Length + 1, // Calculate the current page
                model.Length,                   // Page size
                 model.SortColumn, // Add these
                model.SortDirection);

            var result = new
            {
                draw = model.Draw,
                recordsTotal = pagedResult.TotalRecords,
                recordsFiltered = pagedResult.TotalRecords,
                data = pagedResult.QuestionSet
            };

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestionSet(QuestionSetModel db)
        {

            int data = await _studentProfileService.QuestionSetAdd(db);
            if (data == 1)
            {
                TempData["Msg"] = "QuestionSet Added SuccessFully";
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "QuestionSet Is Already Exist" });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var QuestionSet = await _studentProfileService.QuestionSetDetails(id); // Use async method
           
            return PartialView("_UpdateQuestionSet", QuestionSet);
        }
       
        [HttpPost]
        public async Task<IActionResult> Edit(QuestionSetModel model)
        {

            if (await _studentProfileService.QuestionSetEdit(model) == 1)
            {
                TempData["Msg"] = "QuestionSet SuccessFully Updated...";
                return RedirectToAction("QuestionSetList");
            }
            else
            {
                TempData["Error"] = "This QuestionSet Already Exist...";
                return RedirectToAction("QuestionSetList");
            }
        }



        //// GET: StudentController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            int data = await _studentProfileService.DeleteQuestionSet(id);
            if (data == 1)
            {
                TempData["Msg"] = "Success Fully Deleted...";
                return RedirectToAction("QuestionSetList");
            }
            else
            {
                TempData["Error"] = "Delete Not Deleted Something Wrong....";
                return RedirectToAction("QuestionSetList");
            }
        }
    }
}
