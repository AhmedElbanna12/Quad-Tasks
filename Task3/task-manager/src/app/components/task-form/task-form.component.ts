import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../services/task.service';
import { TaskRequest } from '../../models/task-request';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {

  taskForm!: FormGroup;
  taskId: number | null = null;
  isEdit = false;
  isLoading = false;

  // ← ده اللي كان ناقص
  statuses = [
    { key: 'todo', value: 0, label: 'Pending',     color: '#f59e0b' },
    { key: 'prog', value: 1, label: 'In Progress', color: '#3b82f6' },
    { key: 'done', value: 2, label: 'Done',        color: '#10b981' },
  ];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.taskForm = this.fb.group({
      title:       ['', Validators.required],
      description: [''],
      status:      [0],
      userId:      [1]
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit  = true;
      this.taskId  = +id;
      this.loadTask(this.taskId);
    }
  }

  loadTask(id: number): void {
    this.isLoading = true;
    this.taskService.getTaskById(id).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: (task) => this.taskForm.patchValue(task),
      error: ()     => {}
    });
  }

  onSubmit(): void {
    if (this.taskForm.invalid) return;
    this.isLoading = true;

    const formData: TaskRequest = this.taskForm.value;
    const request$ = this.isEdit && this.taskId
      ? this.taskService.updateTask(this.taskId, formData)
      : this.taskService.createTask(formData);

    request$.pipe(
      finalize(() => this.isLoading = false)
    ).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: (err) => console.log(err)
    });
  }
}