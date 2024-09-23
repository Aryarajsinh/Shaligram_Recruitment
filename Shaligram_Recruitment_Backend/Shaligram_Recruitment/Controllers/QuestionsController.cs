using Microsoft.AspNetCore.Mvc;
using Shaligram_Recruitment.Model.ViewModels.Questions;
using Shaligram_Recruitment.Services.Questions;
using Shaligram_Recruitment.Services.StudentProfile;

namespace Shaligram_Recruitment.Controllers
{
    public class QuestionsController : Controller
    {
        // Assuming you have a service or repository to handle database operations
        private readonly IQuestionServices _questionService;

        public IStudentProfileService _studentService { get; }

        public QuestionsController(IQuestionServices questionService,IStudentProfileService studentService)
        {
            _questionService = questionService;
            _studentService = studentService;
        }

        public async Task<IActionResult> QuestionList()
        {
            ViewBag.QuestionSet =await _studentService.GetQuestionSet();
            return View();
        }

        [HttpPost]
        public IActionResult CreateQuestion(QuestionModel model)
        {
            if (model.AnswerOptions != null && model.AnswerOptions.Count > 0)
            {
                foreach (var option in model.AnswerOptions)
                {
                    // Save each answer option and its "correct" flag to the database
                }
            }

            // Save question data to the database using stored procedures or other logic
            return RedirectToAction("SuccessPage");
        }

        public IActionResult SuccessPage()
        {
            return View(); // A success page after the question is saved
        }
    }
}
