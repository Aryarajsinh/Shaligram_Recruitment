using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaligram_Recruitment.Common.Helpers
{
    public class PdfTemplate
    {
        public const string pdftemplete = @"
            <!DOCTYPE html>
<html>

<head>
  <title>Shaligram Sample Codes</title>
   <style>
body, h1, p, table {
  margin: 0;
  padding: 0;
letter-spacing:1px;
}
h1{
    text-align:center;
    padding:20px;
}
.table-container {
  max-width: 80%;
  margin: 20px auto;
}
table {
  width: 100%;
  border-collapse: collapse;
  border: 1px solid #000; 
}
thead th {
  background-color: #333; 
  color: #fff; 
  padding: 10px;
  text-align: center; 
  border-bottom: 1px solid #000;
  border-right: 1px solid #000; 
}
.text-align-center{
    text-align:center;
}
tbody td {
  padding: 10px;
  border-bottom: 1px solid #000; 
  border-right: 1px solid #000;
}
tbody tr:nth-child(even) {
  background-color: #f2f2f2;
}
thead th:last-child {
  border-right: none;
}
tbody td:last-child {
  border-right: none;
}
  </style>
</head>

<body>
  <h1>{PDFHeading}</h1>
  <table>
    <thead>
      <tr>
      <th>Employee Id</th>
      <th>Employee Name</th>
      <th>Birthdate</th>
      <th>Gender</th>
      <th>Phone Number</th>
      <th>Country</th>
      <th>State</th>
      <th>City</th>
      <th>Skills</th>
      </tr>
    </thead>
    <tbody>
		{Data}
    </tbody>
  </table>
<h1>{PDFHeading2}</h1>
<table>
    <thead>
      <tr>
      <th>Employee Id</th>
      <th>Employee Name</th>
      <th>Birthdate</th>
      <th>Gender</th>
      <th>Phone Number</th>
      <th>Country</th>
      <th>State</th>
      <th>City</th>
      <th>Skills</th>
      </tr>
    </thead>
    <tbody>
		{Data2}
    </tbody>
  </table>
</body>

</html>";
    }
}
