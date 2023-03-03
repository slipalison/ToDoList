import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ITask, Task } from '../Entities/task';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {


  public form: FormGroup;
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

  listTodo() {
    var data = this.http.get<ITask[]>('http://localhost:8080/ToDo');
    data.subscribe((result: ITask[]) => {
      this.tasks = result;
    });
  }

  addTodo(result: ITask) {
    var data = this.http.post('http://localhost:8080/ToDo', result);

    data.subscribe((result: any) => {
      console.log("Adicionado a fila");

      this.listTodo();
    });
  }

  updateTodo(result: ITask, status: string) {
    result.status = status;
    var data = this.http.patch(`http://localhost:8080/ToDo/${result.id}`, result);

    data.subscribe((result: any) => {
      console.log("Atualizado ", result);
    });
  }


  deleteTodo(result: ITask) {
    var data = this.http.delete(`http://localhost:8080/ToDo/${result.id}`);

    data.subscribe((result: any) => {
      console.log("Deletado", result);
      this.listTodo();
    });
  }

  newTask: string = '';
  tasks: ITask[] = [];


  addTask() {

    console.log(this.form.value);

    let task: ITask = new Task();


    task.name = this.form.controls["name"].value;
    if (this.form.controls["deadline"].value !== '')
      task.deadline = this.form.controls["deadline"].value;

    console.log(this.form.controls["name"].valid)
    if (this.form.controls["name"].valid) {
      this.addTodo(task);
    }

    this.form.reset()
  }

  removeTask(task: ITask) {
    const index = this.tasks.indexOf(task);
    if (index >= 0) {
      this.tasks.splice(index, 1);
    }
  }
}

