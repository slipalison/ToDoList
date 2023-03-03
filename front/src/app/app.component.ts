import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Task } from '../Entities/task';


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
    var data = this.http.get<Task[]>('http://localhost:8080/ToDo');
    data.subscribe((result: Task[]) => {
      this.tasks = result;
    });
  }

  addTodo(result: Task) {
    var data = this.http.post('http://localhost:8080/ToDo', result);

    data.subscribe((result: any) => {
      console.log("Adicionado a fila");
    });
  }

  updateTodo(result: Task) {
    var data = this.http.patch(`http://localhost:8080/ToDo/${result.id}`, result);

    data.subscribe((result: any) => {
      console.log("Atualizado ", result);
    });
  }


  deleteTodo(result: Task) {
    var data = this.http.delete(`http://localhost:8080/ToDo/${result.id}`);

    data.subscribe((result: any) => {
      console.log("Deletado", result);
    });
  }

  newTask: string = '';
  tasks: Task[] = [];


  addTask() {

    console.log(this.form.value);

    if (this.newTask.length > 0) {
      //this.tasks.push(this.newTask);
      this.newTask = '';
    }

    this.form.reset()
  }

  removeTask(task: Task) {
    const index = this.tasks.indexOf(task);
    if (index >= 0) {
      this.tasks.splice(index, 1);
    }
  }
}
