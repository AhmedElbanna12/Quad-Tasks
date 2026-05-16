import { RouterLink } from '@angular/router';
import { Component, OnInit, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class TaskListComponent implements OnInit {

  tasks: Task[] = [];
  isLoading = false;
  searchQuery = '';
  currentPage = 1;
  pageSize = 5;

  private donutChart: Chart | null = null;
  private barChart: Chart | null = null;

  constructor(
    private taskService: TaskService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.isLoading = true;
    this.tasks = [];

    this.taskService.getTasks().subscribe({
      next: (res) => {
        this.tasks = res;
        this.isLoading = false;
        this.cdr.detectChanges();
        this.renderCharts();
      },
      error: (err) => {
        console.error(err);
        this.tasks = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // Search
  filteredTasks(): Task[] {
    if (!this.searchQuery.trim()) return this.tasks;
    const q = this.searchQuery.toLowerCase();
    return this.tasks.filter(t =>
      t.title.toLowerCase().includes(q) ||
      t.description.toLowerCase().includes(q)
    );
  }

  onSearch(): void {
    this.currentPage = 1; // رجّع للصفحة الأولى عند البحث
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.currentPage = 1;
  }

  // Pagination
  totalPages(): number {
    return Math.ceil(this.filteredTasks().length / this.pageSize);
  }

  pagedTasks(): Task[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredTasks().slice(start, start + this.pageSize);
  }

  pageNumbers(): number[] {
    return Array.from({ length: this.totalPages() }, (_, i) => i + 1);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages()) return;
    this.currentPage = page;
  }

  // Stats
  getCount(status: number): number {
    return this.tasks.filter(t => t.status === status).length;
  }

  deleteTask(id: number): void {
    if (!confirm('Are you sure?')) return;
    this.taskService.deleteTask(id).subscribe({
      next: () => this.loadTasks(),
      error: () => alert('Failed to delete task')
    });
  }

  renderCharts(): void {
    const pending    = this.getCount(0);
    const inProgress = this.getCount(1);
    const done       = this.getCount(2);

    this.donutChart?.destroy();
    this.barChart?.destroy();

    const donutEl = document.getElementById('donutChart') as HTMLCanvasElement;
    const barEl   = document.getElementById('barChart')   as HTMLCanvasElement;

    if (donutEl) {
      this.donutChart = new Chart(donutEl, {
        type: 'doughnut',
        data: {
          labels: ['Pending', 'In progress', 'Done'],
          datasets: [{
            data: [pending, inProgress, done],
            backgroundColor: ['#EF9F27', '#378ADD', '#639922'],
            borderWidth: 0,
            hoverOffset: 4
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          cutout: '65%',
          plugins: { legend: { display: false } }
        }
      });
    }

    if (barEl) {
      this.barChart = new Chart(barEl, {
        type: 'bar',
        data: {
          labels: ['Pending', 'In progress', 'Done'],
          datasets: [{
            data: [pending, inProgress, done],
            backgroundColor: ['#FAEEDA', '#E6F1FB', '#EAF3DE'],
            borderColor: ['#EF9F27', '#378ADD', '#639922'],
            borderWidth: 1.5,
            borderRadius: 6
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: { legend: { display: false } },
          scales: {
            x: { grid: { display: false }, ticks: { font: { size: 12 } } },
            y: { grid: { color: 'rgba(0,0,0,0.05)' }, ticks: { stepSize: 1, font: { size: 12 } } }
          }
        }
      });
    }
  }
}