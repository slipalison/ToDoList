import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ITask, Task } from './todos/entities/task';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public form: FormGroup;
  tasks: Task[] = [];
  filterValue: string = 'All';
  today = new Date();

  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.form = this.fb.group({
      name: ['', Validators.compose([
        Validators.minLength(3),
        Validators.maxLength(60),
        Validators.required
      ])],
      deadline: ['']
    });
  }

  ngOnInit(): void {
    this.listTodo();
  }

  private url: string = 'http://localhost:8082'
  listTodo() {
    var data = this.http.get<Task[]>(`${this.url}/ToDo`);
    data.subscribe((result: Task[]) => {
      this.tasks = result;
    });
  }

  addTodo(result: Task) {
    var data = this.http.post(`${this.url}/ToDo`, result);
    data.subscribe((result: any) => {
      console.log("Adicionado a fila");
    });
  }

  updateTodo(result: Task, status: string) {

    result.status = status;
    var data = this.http.patch(`${this.url}/ToDo/${result.id}`, result);
    data.subscribe((result: any) => {
      this.listTodo();
      console.log("Atualizado ", result);
    });
  }


  deleteTodo(result: Task) {
    var data = this.http.delete(`${this.url}/ToDo/${result.id}`);

    data.subscribe((result: any) => {
      this.listTodo();
      console.log("Deletado", result);
    });
  }

  filterList() {
    if (this.filterValue == 'All')
      return this.tasks
    else
      return this.tasks.filter(x => x.status == this.filterValue)
  }

  addTask() {
    let name = this.form.controls["name"];
    let deadline = this.form.controls["deadline"];

    if (!name.valid) return;

    let task: ITask = new Task();
    task.name = name.value;

    if (deadline.value != null && deadline.value != '') {
      let data = new Date(deadline.value.toString() + ':00z');
      task.deadline = data;
    }



    var data = this.http.post(`${this.url}/ToDo`, task);
    data.subscribe((result: any) => {
      this.listTodo();
      this.form.reset();
    });
  }

  removeTask(task: Task) {
    const index = this.tasks.indexOf(task);
    if (index >= 0) {
      this.tasks.splice(index, 1);
    }
  }
}
