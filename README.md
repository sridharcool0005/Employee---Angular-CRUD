# Employee---Angular-CRUD
Angular CRUD Operations



First Step -

Angular Project

1)Create Project
---------------------------------------------------------------------------------
 ng new AngularCRUD 
Create New Angular Project Names AngularCRUD

2)ng serve
---------------------------------------------------------------------------------

3)ng g c employees
---------------------------------------------------------------------------------
//switch to parent component directory
cd src\app\employees
//create child components
ng g c employee
ng g c employee-list

4)Add Bootsrap CDN url styles to the index.html
---------------------------------------------------------------------------------

Final index.html looks likes this.

<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>AngularCRUD</title>
  <base href="/">

  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="icon" type="image/x-icon" href="favicon.ico">
  <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
</head>
<body>
  <app-root></app-root>
  <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous"></script>
</body>
</html>

5)Update app.component.html as follows
---------------------------------------------------------------------------------

Final app.component.html looks likes this.
<div class="container">
  <app-employees></app-employees>
</div>

6)Update employees.component.html 
---------------------------------------------------------------------------------

Final employees.component.html looks likes this.
<div style="text-align:center">
  <h2 class="jumbotron bg-secondary text-white">Employee Register</h2>
</div>
<div class="row">
  <div class="col-md-6">
    <app-employee></app-employee>
  </div>
  <div class="col-md-6">
    <app-employee-list></app-employee-list>
  </div>
</div>

7)Create Service and Model Classes
---------------------------------------------------------------------------------

To create these classes let’s add a new folder shared

//switch to shared folder
cd src\app\employees\shared
//create employee model class
ng g class employee --type=model
//create employee service class
ng g s employee

8)Update your model /src/app/employees/shared/employee.model.ts
---------------------------------------------------------------------------------

Final employee.model.ts looks likes this.
export class Employee {
    EmployeeID : number;
    FirstName:string;
    LastName:string;
    EmpCode:string;
    Position:string;
    Office:string;
}

9)Final employee.service
---------------------------------------------------------------------------------

//--/src/app/employees/shared/employee.service.ts update with below code

// @Injectable({
//   providedIn: 'root'
// })
import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

import {Employee} from'./employee.model'

@Injectable()
export class EmployeeService {
  selectedEmployee : Employee;
  employeeList : Employee[];
  constructor(private http : Http) { }

  postEmployee(emp : Employee){
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({'Content-Type':'application/json'});
    var requestOptions = new RequestOptions({method : RequestMethod.Post,headers : headerOptions});
    return this.http.post('http://localhost:28750/api/Employee',body,requestOptions).map(x => x.json());
  }

  putEmployee(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put('http://localhost:28750/api/Employee/' + id,
      body,
      requestOptions).map(res => res.json());
  }

  getEmployeeList(){
    console.log("Service Call");
    this.http.get('http://localhost:28750/api/Employee')
    .map((data : Response) =>{
      console.log("map data -" + data);
      return data.json() as Employee[];
    }).toPromise().then(x => {
      this.employeeList = x;
      console.log("Service data -" + x);
    })
  }

  deleteEmployee(id: number) {
    return this.http.delete('http://localhost:28750/api/Employee/' + id).map(res => res.json());
  }
}

10)Inject Employee Service in Components
---------------------------------------------------------------------------------

Final employees.component.ts  --Root Component

import { Component, OnInit } from '@angular/core';
import { EmployeeService } from './shared/employee.service'
@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
  providers :[EmployeeService]
})
export class EmployeesComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}

---------------------------------------------------------------------------------

Final employee-list.component.ts  --Child Component

import { Component, OnInit } from '@angular/core';

import { EmployeeService } from '../shared/employee.service'
import { Employee } from '../shared/employee.model';
import { ToastrService } from 'ngx-toastr';    
@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {

  constructor(private employeeService: EmployeeService,private toastr : ToastrService) { }

  ngOnInit() {
    this.employeeService.getEmployeeList();
  }

  showForEdit(emp: Employee) {
    this.employeeService.selectedEmployee = Object.assign({}, emp);;
  }


  onDelete(id: number) {
    if (confirm('Are you sure to delete this record ?') == true) {
      this.employeeService.deleteEmployee(id)
      .subscribe(x => {
        this.employeeService.getEmployeeList();
        this.toastr.warning("Deleted Successfully","Employee Register");
      })
    }
  }
}

---------------------------------------------------------------------------------
Final employee.component.ts  --Child Component

import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'

import { EmployeeService } from '../shared/employee.service'
import { ToastrService } from 'ngx-toastr'
@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {

  constructor(private employeeService: EmployeeService, private toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.reset();
    this.employeeService.selectedEmployee = {
      EmployeeID: null,
      FirstName: '',
      LastName: '',
      EmpCode: '',
      Position: '',
      Office: ''
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.EmployeeID == null) {
      this.employeeService.postEmployee(form.value)
        .subscribe(data => {
          this.resetForm(form);
          this.employeeService.getEmployeeList();
          this.toastr.success('New Record Added Succcessfully', 'Employee Register');
        })
    }
    else {
      this.employeeService.putEmployee(form.value.EmployeeID, form.value)
      .subscribe(data => {
        this.resetForm(form);
        this.employeeService.getEmployeeList();
        this.toastr.info('Record Updated Successfully!', 'Employee Register');
      });
    }
  }
}

---------------------------------------------------------------------------------

10)Now lets see final UI
---------------------------------------------------------------------------------
employee-list.component.html - Child Component


<table class="table table-sm table-hover">
  <tr *ngFor="let employee of employeeService.employeeList">
    <td>{{employee.FirstName}} - {{employee.LastName}}</td>
    <td>{{employee.EmpCode}}</td>
    <td>
      <a class="btn" (click)="showForEdit(employee)">
        <i class="fa fa-pencil-square-o"></i>
      </a>
      <a class="btn text-danger" (click)="onDelete(employee.EmployeeID)">
        <i class="fa fa-trash-o"></i>
      </a>
    </td>
  </tr>
</table>

employee.component.html - Child Component

<form class="emp-form" #employeeForm="ngForm" (ngSubmit)="onSubmit(employeeForm)">
  <input type="hidden" name="EmployeeID" #EmployeeID="ngModel" [(ngModel)]="employeeService.selectedEmployee.EmployeeID">
  <div class="form-row">
    <div class="form-group col-md-6">
      <input class="form-control" name="FirstName" #FirstName="ngModel" [(ngModel)]="employeeService.selectedEmployee.FirstName"
        placeholder="First Name" required>
      <div class="validation-error" *ngIf="FirstName.invalid && FirstName.touched">This Field is Required.</div>
    </div>
    <div class="form-group col-md-6">
      <input class="form-control" name="LastName" #LastName="ngModel" [(ngModel)]="employeeService.selectedEmployee.LastName" placeholder="Last Name"
        required>
      <div class="validation-error" *ngIf="LastName.invalid && LastName.touched">This Field is Required.</div>
    </div>
  </div>
  <div class="form-group">
    <input class="form-control" name="Position" #Position="ngModel" [(ngModel)]="employeeService.selectedEmployee.Position" placeholder="Position">
  </div>
  <div class="form-row">
    <div class="form-group col-md-6">
      <input class="form-control" name="EmpCode" #EmpCode="ngModel" [(ngModel)]="employeeService.selectedEmployee.EmpCode" placeholder="Emp Code">
    </div>
    <div class="form-group col-md-6">
      <input class="form-control" name="Office" #Office="ngModel" [(ngModel)]="employeeService.selectedEmployee.Office" placeholder="Office">
    </div>
  </div>
  <div class="form-row">
    <div class="form-group col-md-8">
      <button [disabled]="!employeeForm.valid" type="submit" class="btn btn-lg btn-block btn-info">
        <i class="fa fa-floppy-o"></i> Submit</button>
    </div>
    <div class="form-group col-md-4">
      <button type="button" class="btn btn-lg btn-block btn-secondary" (click)="resetForm(employeeForm)">
        <i class="fa fa-repeat"></i> Reset</button>
    </div>
  </div>
</form>


employees.component.html - Root

<div style="text-align:center">
  <h2 class="jumbotron bg-secondary text-white">Employee Register</h2>
</div>
<div class="row">
  <div class="col-md-6">
    <app-employee></app-employee>
  </div>
  <div class="col-md-6">
    <app-employee-list></app-employee-list>
  </div>
</div>


11) Update Angular.json
angular.json
Add this toastr code

"styles": [
              "src/styles.css",
              "../node_modules/ngx-toastr/toastr.css"
            ],
			
			
12) Observable error

npm install rxjs-compat

restart project
