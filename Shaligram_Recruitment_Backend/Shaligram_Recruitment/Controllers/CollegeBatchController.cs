using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Model.ViewModels.Student;
using Shaligram_Recruitment.Services.StudentProfile;

namespace Shaligram_Recruitment.Controllers
{
    public class CollegeBatchController : Controller
    {
        private readonly IStudentProfileService _studentProfileService;

        public CollegeBatchController(IStudentProfileService studentProfileService)
        {
            _studentProfileService = studentProfileService;
        }
        // GET: StudentController
        public async Task<ActionResult> CollegeBatchList(string search = "", int page = 1, int pageSize = 10, string sortColumn = "batchId", string sortDirection = "asc")
        {
            sortDirection = (sortDirection.ToLower() == "desc") ? "desc" : "asc";
            //var pagedResult = await _studentProfileService.StudentList();
            var pagedResult = await _studentProfileService.GetPageCollegeBatch(search, page, pageSize, sortColumn, sortDirection);
            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = pagedResult.TotalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)pagedResult.TotalRecords / pageSize);
            ViewBag.CollegeName = await _studentProfileService.GetCollegeName();
            return View(pagedResult);
        }
        [HttpPost]
        public async Task<ActionResult> CollegeBatchList(DataTableAjaxPostModel model)
        {
            var pagedResult = await _studentProfileService.GetPageCollegeBatch(
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
                data = pagedResult.CollegeBatch
            };

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewBatch(CollegeBatchModel db)
        {

            int data = await _studentProfileService.CollegeBatchAdd(db);
            if (data == 1)
            {
                TempData["Msg"] = "Student Added SuccessFully";
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "This Data Is Already Added" });
            }
        }



        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentProfileService.BatchDetails(id); // Use async method
            if (student == null)
            {
                return NotFound();
            }
            return PartialView("_BatchDetails", student);
        } 
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentProfileService.BatchDetails(id); // Use async method
            ViewBag.CollegeName = new SelectList(await _studentProfileService.GetCollegeName(), "CollegeName", "CollegeName");
            return PartialView("_UpdateBatch", student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CollegeBatchModel model)
        {
           
            if (await _studentProfileService.BatchEdit(model) == 1)
            {
                TempData["Msg"] = "College Batch SuccessFully Updated...";
                return RedirectToAction("CollegeBatchList");
            }
            else {
                var student = await _studentProfileService.BatchDetails(model.BatchId);
                TempData["Error"] = "SomeThing Wrong Try Again..";
                return PartialView("_UpdateBatch", student);
            }
        }


        // GET: StudentController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            int data = await _studentProfileService.DeleteBatch(id);
            if (data == 1)
            {
                TempData["Msg"] = "Success Fully Deleted...";
                return RedirectToAction("CollegeBatchList");
            }
            else
            {
                TempData["Error"] = "Delete Not Deleted Something Wrong....";
                return RedirectToAction("CollegeBatchList");
            }
        }
    }
}
