import { Routes } from '@angular/router';
import { DashboardComponent } from '../../../Components/dashboard/dashboard.component';
import { AdminComponent } from './admin.component';
import { QuestionsetComponent } from '../../../Pages/AdminPages/QuestionSet/Questionset/questionset.component';
import { QuestionsComponent } from '../../../Pages/AdminPages/Questions/questions/questions.component';
import { LogoutComponent } from '../../../Pages/AdminPages/LogOut/logout/logout.component';
import { ExamComponent } from '../../../Pages/AdminPages/Exam/exam/exam.component';
import { CollegebatchComponent } from '../../../Pages/AdminPages/CollegeBatch/collegebatch/collegebatch.component';
import { StudentComponent } from '../../../Pages/AdminPages/Student/student/student.component';
import { EditstudentComponent } from '../../../Pages/AdminPages/Student/Edit_Student/editstudent/editstudent.component';
import { nonAuthGuard } from '../../../Auth/non-auth.guard';
import { ErrorComponent } from '../../../Pages/error/error.component';


export const AdminRoutes: Routes = [
  {
    path: '',
    redirectTo: "",
    pathMatch: "full"
},
    {
        path:"admin",
        component:DashboardComponent,
        children: [           
          {path:"", component:AdminComponent},   
          {path:"questionSet", component:QuestionsetComponent,canActivate: [nonAuthGuard]},    
          {path:"questions", component:QuestionsComponent,canActivate: [nonAuthGuard]},     
          {path:"exam", component:ExamComponent,canActivate: [nonAuthGuard]},     
          {path:"collegeBatch", component:CollegebatchComponent,canActivate: [nonAuthGuard]},     
          {path:"student", component:StudentComponent,canActivate: [nonAuthGuard]},     
          {path:'editstudent/:id', component:EditstudentComponent,canActivate: [nonAuthGuard]},     
          {path:"logout", component:LogoutComponent,canActivate: [nonAuthGuard]},          
        ],
      },
      { path: 'error', component: ErrorComponent }, // Error route
  { path: '**', redirectTo: 'error' }, // Wildcard route to catch undefined paths
    
];