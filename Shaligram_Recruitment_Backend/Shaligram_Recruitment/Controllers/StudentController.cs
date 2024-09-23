using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shaligram_Recruitment.Common.Messages;
using Shaligram_Recruitment.Fillter;
using Shaligram_Recruitment.Model.ViewModels.CollegeBatch;
using Shaligram_Recruitment.Model.ViewModels.Pagination;
using Shaligram_Recruitment.Model.ViewModels.Student;
using Shaligram_Recruitment.Services.StudentProfile;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shaligram_Recruitment.Controllers
{
    //[TypeFilter(typeof(CustomAuthorizationFilter))]
    public class StudentController : Controller
    {
        private readonly IStudentProfileService _studentProfileService;

        public StudentController(IStudentProfileService studentProfileService)
        {
            _studentProfileService = studentProfileService;
        }
        // GET: StudentController
        public async Task<ActionResult> StudentList(string search = "", int page = 1, int pageSize = 10, string sortColumn = "studentId", string sortDirection = "asc")
        {
            sortDirection = (sortDirection.ToLower() == "desc") ? "desc" : "asc";
            //var pagedResult = await _studentProfileService.StudentList();
            var pagedResult = await _studentProfileService.GetPagedUsers(search, page, pageSize, sortColumn, sortDirection);
            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = pagedResult.TotalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)pagedResult.TotalRecords / pageSize);
            ViewBag.CollegeName = await _studentProfileService.GetCollegeName();
            ViewBag.Years = await _studentProfileService.GetBetchYear();
            return View(pagedResult);
        }
        [HttpPost]
        public async Task<ActionResult> StudentList(DataTableAjaxPostModel model)
        {
            var pagedResult = await _studentProfileService.GetPagedUsers(
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
                data = pagedResult.Users
            };

            return Json(result);
        }
        [HttpPost]
         public async Task<IActionResult> AddStudent(StudentModel db)
        {
          
                int data = await _studentProfileService.StudentAdd(db);
                if (data == 1)
                {
                    TempData["Msg"] = "Student Added SuccessFully";
                    return Json(new { success = true });
                }
                else
                {                   
                    return Json(new { success = false, message = "Email Is Already Exist" });
                }
            } 
        [HttpPost]
         public async Task<IActionResult> UploadFile(CollegeBatchModel db)
        {
            try
            {
                // Check if model state is valid
                if (ModelState.IsValid)
                {
                    // Validate if a file has been uploaded
                    if (db.UploadFile == null || db.UploadFile.Length == 0)
                    {
                        // Return a validation error if no file is uploaded
                        return Json(new { success = false, message = "No file uploaded. Please select a valid CSV file." });
                    }

                    // Attempt to parse the uploaded CSV file
                    var records = _studentProfileService.ParseCsvFile(db.UploadFile);

                    // Validate if records have been parsed successfully
                    if (records == null || !records.Any())
                    {
                        return Json(new { success = false, message = "The CSV file is empty or not formatted correctly." });
                    }

                    // Call the service method to upload the parsed data to the database
                    //whenever run this project than check this field
                    List<CsvFileModel> csvFile = new List<CsvFileModel>();
                    await _studentProfileService.uploadFileData(csvFile,db);

                    // If successful, send a success message
                    TempData["Msg"] = "File successfully uploaded!";
                    return Json(new { success = true });
                }
                else
                {
                    // Model state validation failed
                    return Json(new { success = false, message = "Invalid file or form data. Please upload the correct CSV file." });
                }
            }
            catch (CsvHelperException ex)
            {
                // Handle CSV parsing exceptions
                return Json(new { success = false, message = "Error in parsing the CSV file. Please check the file format." });
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                return Json(new { success = false, message = "An error occurred while uploading the file. Please try again." });
            }


        }



        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentProfileService.StudentDetails(id); // Use async method
            if (student == null)
            {
                return NotFound();
            }
            return PartialView("_StudentDetails", student);
        }


        //// GET: StudentController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
           int data = await _studentProfileService.DeleteStudent(id);
            if(data == 1) {
                TempData["Msg"] = "Success Fully Deleted...";
                return RedirectToAction("StudentList");
            }
            else
            {
                TempData["Error"] = "Delete Not Deleted Something Wrong....";
                return RedirectToAction("StudentList");
            }
        }      
    }
}
